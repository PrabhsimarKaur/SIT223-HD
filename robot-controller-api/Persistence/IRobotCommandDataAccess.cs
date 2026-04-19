using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public interface IRobotCommandDataAccess
{
    List<RobotCommand> GetRobotCommands();

    RobotCommand GetRobotCommandById(int id);

    RobotCommand AddRobotCommand(RobotCommand command);

    RobotCommand UpdateRobotCommand(RobotCommand command);

    bool DeleteRobotCommand(int id);
}