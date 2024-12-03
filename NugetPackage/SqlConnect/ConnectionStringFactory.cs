using SqlConnect;

public interface IConnectionStringFactory
{
    Task<string> GetConnectionStringAsync(SqlDbEnum con);
}

public class ConnectionStringFactory : IConnectionStringFactory
{
    private readonly ISqlHelper _sqlHelper;

    public ConnectionStringFactory(ISqlHelper sqlHelper)
    {
        _sqlHelper = sqlHelper;
    }

    public async Task<string> GetConnectionStringAsync(SqlDbEnum con)
    {
        await _sqlHelper.InitializeConnectionStringsAsync();
        return await _sqlHelper.GetConnectionString(con);
    }
}
