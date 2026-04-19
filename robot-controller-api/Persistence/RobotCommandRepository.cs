using Npgsql;
using robot_controller_api.Models;
using System.Linq;

namespace robot_controller_api.Persistence;

public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
{
    private IRepository _repo => this;

    public List<RobotCommand> GetRobotCommands()
    {
        return _repo.ExecuteReader<RobotCommand>(
            "SELECT * FROM robotcommand");
    }

    public RobotCommand GetRobotCommandById(int id)
    {
        return _repo.ExecuteReader<RobotCommand>(
            "SELECT * FROM robotcommand WHERE id=@id",
            new[] { new NpgsqlParameter("id", id) }
        ).Single();
    }

    public RobotCommand AddRobotCommand(RobotCommand command)
    {
        return command;
    }

    public RobotCommand UpdateRobotCommand(RobotCommand command)
    {
        return command;
    }

    public bool DeleteRobotCommand(int id)
    {
        return true;
    }
}