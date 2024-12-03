using CustomLoggerHelper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecretsKeyVault;

namespace SqlConnect
{
    public interface ISqlHelper
    {
        Task InitializeConnectionStringsAsync();
        Task<string> GetConnectionString(SqlDbEnum dbEnum);
        Task<bool> InsertAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum);
        Task<bool> UpdateAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum);
        Task<bool> DeleteAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum);
        Task<List<Dictionary<string, object>>> SelectAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum);
        Task<bool> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters, SqlDbEnum dbEnum);
    }
}
