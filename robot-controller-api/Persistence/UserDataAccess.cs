using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class UserDataAccess
    {
        private readonly string _connectionString;

        public UserDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"SELECT id, email, first_name, last_name, password_hash, description, role, created_date, modified_date FROM public.""user"" ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(ReadUser(reader));
            }

            return users;
        }

        public List<UserModel> GetAdminUsers()
        {
            var users = new List<UserModel>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"SELECT id, email, first_name, last_name, password_hash, description, role, created_date, modified_date 
                                          FROM public.""user"" 
                                          WHERE role = 'Admin' 
                                          ORDER BY id", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(ReadUser(reader));
            }

            return users;
        }

        public UserModel? GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"SELECT id, email, first_name, last_name, password_hash, description, role, created_date, modified_date 
                                          FROM public.""user"" 
                                          WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return ReadUser(reader);
            }

            return null;
        }

        public UserModel? GetUserByEmail(string email)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"SELECT id, email, first_name, last_name, password_hash, description, role, created_date, modified_date 
                                          FROM public.""user"" 
                                          WHERE email = @email", conn);
            cmd.Parameters.AddWithValue("email", email);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return ReadUser(reader);
            }

            return null;
        }

        public UserModel AddUser(UserModel user)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"
                INSERT INTO public.""user""
                (email, first_name, last_name, password_hash, description, role, created_date, modified_date)
                VALUES
                (@email, @first_name, @last_name, @password_hash, @description, @role, @created_date, @modified_date)
                RETURNING id;", conn);

            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("first_name", user.FirstName);
            cmd.Parameters.AddWithValue("last_name", user.LastName);
            cmd.Parameters.AddWithValue("password_hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("description", (object?)user.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("role", (object?)user.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("created_date", user.CreatedDate);
            cmd.Parameters.AddWithValue("modified_date", user.ModifiedDate);

            user.Id = (int)cmd.ExecuteScalar()!;
            return user;
        }

        public bool UpdateUser(int id, UserModel user)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"
                UPDATE public.""user""
                SET first_name = @first_name,
                    last_name = @last_name,
                    description = @description,
                    role = @role,
                    modified_date = @modified_date
                WHERE id = @id", conn);

            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("first_name", user.FirstName);
            cmd.Parameters.AddWithValue("last_name", user.LastName);
            cmd.Parameters.AddWithValue("description", (object?)user.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("role", (object?)user.Role ?? DBNull.Value);
            cmd.Parameters.AddWithValue("modified_date", user.ModifiedDate);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateLogin(int id, string email, string passwordHash, DateTime modifiedDate)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"
                UPDATE public.""user""
                SET email = @email,
                    password_hash = @password_hash,
                    modified_date = @modified_date
                WHERE id = @id", conn);

            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password_hash", passwordHash);
            cmd.Parameters.AddWithValue("modified_date", modifiedDate);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var cmd = new NpgsqlCommand(@"DELETE FROM public.""user"" WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            return cmd.ExecuteNonQuery() > 0;
        }

        private UserModel ReadUser(NpgsqlDataReader reader)
        {
            return new UserModel
            {
                Id = Convert.ToInt32(reader["id"]),
                Email = reader["email"].ToString()!,
                FirstName = reader["first_name"].ToString()!,
                LastName = reader["last_name"].ToString()!,
                PasswordHash = reader["password_hash"].ToString()!,
                Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                Role = reader["role"] == DBNull.Value ? null : reader["role"].ToString(),
                CreatedDate = Convert.ToDateTime(reader["created_date"]),
                ModifiedDate = Convert.ToDateTime(reader["modified_date"])
            };
        }
    }
}