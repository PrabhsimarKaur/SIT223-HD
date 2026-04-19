using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class MapEF : IMapDataAccess
{
    private readonly RobotContext _context;

    public MapEF(RobotContext context)
    {
        _context = context;
    }

    public List<Map> GetMaps()
    {
        return _context.Maps.ToList();
    }

    public Map GetMapById(int id)
    {
        return _context.Maps.Find(id)!; // ! removes null warning
    }

    public Map AddMap(Map map)
    {
        _context.Maps.Add(map);
        _context.SaveChanges();
        return map;
    }

    public Map UpdateMap(Map map)
    {
        var existing = _context.Maps.Find(map.Id);

        if (existing == null)
            return null!; // quick fix for assignment

        existing.Name = map.Name;
        existing.Description = map.Description;
        existing.Rows = map.Rows;
        existing.Columns = map.Columns;
        existing.ModifiedDate = DateTime.Now;

        _context.SaveChanges();
        return existing;
    }

    public bool DeleteMap(int id)
    {
        var map = _context.Maps.Find(id);
        if (map == null) return false;

        _context.Maps.Remove(map);
        _context.SaveChanges();
        return true;
    }
}