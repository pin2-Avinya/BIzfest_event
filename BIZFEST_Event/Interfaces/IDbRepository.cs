using System.Data;

namespace BIZFEST_Event.Interfaces
{
    public interface IDbRepository
    {
        IDbConnection CreateConnection();
        IDbConnection CreateConnection(string connectionString);
    }
}
