// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using robot_controller_api.Models;
// using robot_controller_api.Persistence;

// namespace robot_controller_api.Controllers
// {
//     [ApiController]
//     [Route("users")]
//     [Authorize]
//     public class UsersController : ControllerBase
//     {
//         private readonly UserDataAccess _userDataAccess;

//         public UsersController(UserDataAccess userDataAccess)
//         {
//             _userDataAccess = userDataAccess;
//         }

//         [HttpGet]
//         [Authorize(Policy = "AdminOnly")]
//         public IActionResult GetAllUsers()
//         {
//             return Ok(_userDataAccess.GetAllUsers());
//         }

//         [HttpGet("admin")]
//         [Authorize(Policy = "AdminOnly")]
//         public IActionResult GetAdminUsers()
//         {
//             return Ok(_userDataAccess.GetAdminUsers());
//         }

//         [HttpGet("{id}")]
//         [Authorize(Policy = "AdminOnly")]
//         public IActionResult GetUserById(int id)
//         {
//             var user = _userDataAccess.GetUserById(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }

//             return Ok(user);
//         }

//         [AllowAnonymous]
//         [HttpPost]
//         public IActionResult AddUser(UserModel newUser)
//         {
//             if (newUser == null)
//             {
//                 return BadRequest();
//             }

//             var existingUser = _userDataAccess.GetUserByEmail(newUser.Email);
//             if (existingUser != null)
//             {
//                 return Conflict("A user with this email already exists.");
//             }

//             var plainPassword = newUser.PasswordHash;

//             var hasher = new PasswordHasher<UserModel>();
//             newUser.PasswordHash = hasher.HashPassword(newUser, plainPassword);

//             newUser.Role ??= "User";
//             newUser.CreatedDate = DateTime.Now;
//             newUser.ModifiedDate = DateTime.Now;

//             var createdUser = _userDataAccess.AddUser(newUser);
//             return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
//         }

//         [HttpPut("{id}")]
//         [Authorize(Policy = "AdminOnly")]
//         public IActionResult UpdateUser(int id, UserModel updatedUser)
//         {
//             var existingUser = _userDataAccess.GetUserById(id);
//             if (existingUser == null)
//             {
//                 return NotFound();
//             }

//             existingUser.FirstName = updatedUser.FirstName;
//             existingUser.LastName = updatedUser.LastName;
//             existingUser.Description = updatedUser.Description;
//             existingUser.Role = updatedUser.Role;
//             existingUser.ModifiedDate = DateTime.Now;

//             var success = _userDataAccess.UpdateUser(id, existingUser);

//             if (!success)
//             {
//                 return StatusCode(500);
//             }

//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         [Authorize(Policy = "AdminOnly")]
//         public IActionResult DeleteUser(int id)
//         {
//             var success = _userDataAccess.DeleteUser(id);

//             if (!success)
//             {
//                 return NotFound();
//             }

//             return NoContent();
//         }

//         [HttpPatch("{id}")]
//         [Authorize(Policy = "AdminOnly")]
//         public IActionResult UpdateLogin(int id, LoginModel loginModel)
//         {
//             var user = _userDataAccess.GetUserById(id);
//             if (user == null)
//             {
//                 return NotFound();
//             }

//             var existingEmailUser = _userDataAccess.GetUserByEmail(loginModel.Email);
//             if (existingEmailUser != null && existingEmailUser.Id != id)
//             {
//                 return Conflict("Another user already uses this email.");
//             }

//             var hasher = new PasswordHasher<UserModel>();
//             var newPasswordHash = hasher.HashPassword(user, loginModel.Password);

//             var success = _userDataAccess.UpdateLogin(id, loginModel.Email, newPasswordHash, DateTime.Now);

//             if (!success)
//             {
//                 return StatusCode(500);
//             }

//             return NoContent();
//         }
//     }
// }

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserDataAccess _userDataAccess;

        public UsersController(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userDataAccess.GetAllUsers());
        }

        [HttpGet("admin")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAdminUsers()
        {
            return Ok(_userDataAccess.GetAdminUsers());
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserById(int id)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddUser(UserModel newUser)
        {
            if (newUser == null)
            {
                return BadRequest();
            }

            var existingUser = _userDataAccess.GetUserByEmail(newUser.Email);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            var plainPassword = newUser.PasswordHash;

            var hasher = new PasswordHasher<UserModel>();
            newUser.PasswordHash = hasher.HashPassword(newUser, plainPassword);

            newUser.CreatedDate = DateTime.Now;
            newUser.ModifiedDate = DateTime.Now;

            if (string.IsNullOrWhiteSpace(newUser.Role))
            {
                newUser.Role = "User";
            }

            var createdUser = _userDataAccess.AddUser(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUser(int id, UserModel updatedUser)
        {
            var existingUser = _userDataAccess.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Description = updatedUser.Description;
            existingUser.Role = updatedUser.Role;
            existingUser.ModifiedDate = DateTime.Now;

            var success = _userDataAccess.UpdateUser(id, existingUser);

            if (!success)
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            var success = _userDataAccess.DeleteUser(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateLogin(int id, LoginModel loginModel)
        {
            var user = _userDataAccess.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var hasher = new PasswordHasher<UserModel>();
            var passwordHash = hasher.HashPassword(user, loginModel.Password);

            var success = _userDataAccess.UpdateLogin(id, loginModel.Email, passwordHash, DateTime.Now);

            if (!success)
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}