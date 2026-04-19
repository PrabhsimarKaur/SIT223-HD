using System.Data;
using System.Reflection;

namespace robot_controller_api.Persistence;

public static class ExtensionMethods
{
    public static T MapToModel<T>(this IDataRecord record) where T : new()
    {
        T obj = new();

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            if (!Equals(record[prop.Name], DBNull.Value))
            {
                prop.SetValue(obj, record[prop.Name]);
            }
        }

        return obj;
    }
}