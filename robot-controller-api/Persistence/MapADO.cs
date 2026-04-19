using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public class MapADO : IMapDataAccess
{
    public List<Map> GetMaps()
    {
        return new List<Map>();
    }

    public Map GetMapById(int id)
    {
        return new Map();
    }

    public Map AddMap(Map map)
    {
        return map;
    }

    public Map UpdateMap(Map map)
    {
        return map;
    }

    public bool DeleteMap(int id)
    {
        return true;
    }
}