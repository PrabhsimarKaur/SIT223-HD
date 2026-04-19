using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserDataAccess _userDataAccess;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserDataAccess userDataAccess)
            : base(options, logger, encoder, clock)
        {
            _userDataAccess = userDataAccess;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");

            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header."));
            }

            try
            {
                var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                var credentialBytes = Convert.FromBase64String(encodedCredentials);
                var credentials = Encoding.UTF8.GetString(credentialBytes);

                var parts = credentials.Split(':');
                if (parts.Length != 2)
                {
                    Response.StatusCode = 401;
                    return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format."));
                }

                var email = parts[0];
                var password = parts[1];

                var user = _userDataAccess.GetUserByEmail(email);
                if (user == null)
                {
                    Response.StatusCode = 401;
                    return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
                }

                var hasher = new PasswordHasher<UserModel>();
                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result == PasswordVerificationResult.Failed)
                {
                    Response.StatusCode = 401;
                    return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
                }

                var claims = new[]
                {
                    new Claim("name", $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role ?? "User"),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, "Basic");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch
            {
                Response.StatusCode = 401;
                return Task.FromResult(AuthenticateResult.Fail("Authentication failed."));
            }
        }
    }
}