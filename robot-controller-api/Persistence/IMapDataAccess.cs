using robot_controller_api.Models;

namespace robot_controller_api.Persistence;

public interface IMapDataAccess
{
    List<Map> GetMaps();

    Map GetMapById(int id);

    Map AddMap(Map map);

    Map UpdateMap(Map map);

    bool DeleteMap(int id);
}