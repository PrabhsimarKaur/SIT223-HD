using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class RobotCommandEF : IRobotCommandDataAccess
{
    private readonly RobotContext _context;

    public RobotCommandEF(RobotContext context)
    {
        _context = context;
    }

    public List<RobotCommand> GetRobotCommands()
    {
        return _context.RobotCommands.ToList();
    }

    public RobotCommand GetRobotCommandById(int id)
    {
        return _context.RobotCommands.Find(id)!;
    }

    public RobotCommand AddRobotCommand(RobotCommand command)
    {
        _context.RobotCommands.Add(command);
        _context.SaveChanges();
        return command;
    }

    public RobotCommand UpdateRobotCommand(RobotCommand command)
    {
        var existing = _context.RobotCommands.Find(command.Id);

        if (existing == null)
            return null!;

        existing.Name = command.Name;
        existing.Description = command.Description;
        existing.IsMoveCommand = command.IsMoveCommand;
        existing.ModifiedDate = DateTime.Now;

        _context.SaveChanges();
        return existing;
    }

    public bool DeleteRobotCommand(int id)
    {
        var cmd = _context.RobotCommands.Find(id);
        if (cmd == null) return false;

        _context.RobotCommands.Remove(cmd);
        _context.SaveChanges();
        return true;
    }
}