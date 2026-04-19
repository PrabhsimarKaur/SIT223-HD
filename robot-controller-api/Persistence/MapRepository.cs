using Npgsql;
using robot_controller_api.Models;
using System.Linq;

namespace robot_controller_api.Persistence;

public class MapRepository : IMapDataAccess, IRepository
{
    private IRepository _repo => this;

    public List<Map> GetMaps()
    {
        return _repo.ExecuteReader<Map>(
            "SELECT * FROM map");
    }

    public Map GetMapById(int id)
    {
        return _repo.ExecuteReader<Map>(
            "SELECT * FROM map WHERE id=@id",
            new[] { new NpgsqlParameter("id", id) }
        ).Single();
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