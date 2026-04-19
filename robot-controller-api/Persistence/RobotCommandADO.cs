using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class RobotCommandADO : IRobotCommandDataAccess
{
    public List<RobotCommand> GetRobotCommands()
    {
        return new List<RobotCommand>();
    }

    public RobotCommand GetRobotCommandById(int id)
    {
        return new RobotCommand();
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