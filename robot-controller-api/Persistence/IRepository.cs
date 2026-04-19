using Npgsql;
using System.Data;

namespace robot_controller_api.Persistence;

public interface IRepository
{
    string ConnectionString => 
        "Host=localhost;Username=postgres;Password=postgres;Database=sit331";

    public List<T> ExecuteReader<T>(string sql, NpgsqlParameter[]? parameters = null)
        where T : new()
    {
        List<T> result = new();

        using var connection = new NpgsqlConnection(ConnectionString);
        connection.Open();

        using var command = new NpgsqlCommand(sql, connection);

        if (parameters != null)
            command.Parameters.AddRange(parameters);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            result.Add(reader.MapToModel<T>());
        }

        return result;
    }
}