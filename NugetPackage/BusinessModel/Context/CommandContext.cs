using CustomLoggerHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace BusinessModel.Context
{
    public class CommandContext<T> : ICommandContext<T> where T : DbContext
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly BaseContext<T> _baseContext;
        private readonly DbContext _dbContext;
        private readonly ILoggerHelper _logger;

        public CommandContext(BaseContext<T> baseContext, ILoggerHelper logger) 
        {
            _baseContext = baseContext;
            _dbContext = _baseContext.GetDbContext();
            _logger = logger;
        }

        // Create a new transaction
        public async Task CreateTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        // Commit the current transaction
        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress to commit.");
            }

            try
            {
                await _currentTransaction.CommitAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        // Rollback the current transaction
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
            {
                throw new InvalidOperationException("No transaction is in progress to rollback.");
            }

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
        public async Task<int> SaveChangesWithTransactionAsync()
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                try
                {
                    await CreateTransactionAsync();
                    // Log the start of the transaction
                    _logger.LogInformation("Transaction started.");

                    // Save changes to the database
                    int affectedRows = await _dbContext.SaveChangesAsync();
                    _logger.LogInformation($"SaveChangesAsync executed. Rows affected: {affectedRows}");

                    // Commit the transaction
                    await _currentTransaction.CommitAsync();
                    _logger.LogInformation("Transaction committed successfully.");

                    // Return the number of affected rows
                    return affectedRows;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during SaveChangesAsync. Rolling back transaction.");

                    await RollbackTransactionAsync();
                    return 0;
                }
            });
        }

        // Save changes
        public async Task<int> SaveChangesAsync()
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                return await _dbContext.SaveChangesAsync();
            });
        }

        public T GetDbContext()
        {
            return (T)_dbContext;
        }
        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}
