using CustomLoggerHelper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecretsKeyVault;
using System.Data;

namespace SqlConnect
{
    public class SqlHelper: ISqlHelper
    {
        private string _connectionString;
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;
        private Dictionary<SqlDbEnum, string> _connectionStrings;
        public SqlHelper(ILoggerHelper logger, IConfiguration configRoot
            , IKeyVaultManagedIdentityHelper secretsHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _connectionStrings = new Dictionary<SqlDbEnum, string>();
            _secretsHelper = secretsHelper;
        }
        public async Task InitializeConnectionStringsAsync()
        {
            try
            {
                // Loop through all values of SqlDbEnum and dynamically load connection strings
                foreach (SqlDbEnum dbEnum in Enum.GetValues(typeof(SqlDbEnum)))
                {
                    string secretKey = dbEnum.ToString();  // Using enum name as the secret key
                    string secretValue = await _secretsHelper.GetSecretAsync(secretKey);

                    if (!string.IsNullOrEmpty(secretValue))
                    {
                        _connectionStrings[dbEnum] = secretValue;
                    }
                    else
                    {
                        _logger.LogWarning($"No secret found for {dbEnum}.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing connection strings.");
            }
        }

        // Retrieve connection string by enum
        public async Task<string> GetConnectionString(SqlDbEnum dbEnum)
        {
            if (_connectionStrings.ContainsKey(dbEnum))
            {
                return _connectionStrings[dbEnum];
            }
            else
            {
                _logger.LogWarning($"Connection string not found for {dbEnum}.");
                return string.Empty;
            }
        }
        // CRUD: Insert data into a table
        public async Task<bool> InsertAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum)
        {
            try
            {
                string connStr = await GetConnectionString(dbEnum);
                if (string.IsNullOrEmpty(connStr)) return false;

                using (var connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing INSERT query.");
                return false;
            }
        }

        // CRUD: Update data in a table
        public async Task<bool> UpdateAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum)
        {
            try
            {
                string connStr = await GetConnectionString(dbEnum);
                if (string.IsNullOrEmpty(connStr)) return false;

                using (var connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing UPDATE query.");
                return false;
            }
        }

        // CRUD: Delete data from a table
        public async Task<bool> DeleteAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum)
        {
            try
            {
                string connStr = await GetConnectionString(dbEnum);
                if (string.IsNullOrEmpty(connStr)) return false;

                using (var connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing DELETE query.");
                return false;
            }
        }

        // CRUD: Select data from a table (returns a list of dictionaries with column names and values)
        public async Task<List<Dictionary<string, object>>> SelectAsync(string query, SqlParameter[] parameters, SqlDbEnum dbEnum)
        {
            try
            {
                string connStr = await GetConnectionString(dbEnum);
                if (string.IsNullOrEmpty(connStr)) return new List<Dictionary<string, object>>();

                var result = new List<Dictionary<string, object>>();

                using (var connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var row = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[reader.GetName(i)] = reader.GetValue(i);
                                }
                                result.Add(row);
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SELECT query.");
                return new List<Dictionary<string, object>>();
            }
        }

        // Execute Stored Procedure
        public async Task<bool> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters, SqlDbEnum dbEnum)
        {
            try
            {
                string connStr = await GetConnectionString(dbEnum);
                if (string.IsNullOrEmpty(connStr)) return false;

                using (var connection = new SqlConnection(connStr))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(parameters);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing stored procedure.");
                return false;
            }
        }
    }
}
