using CustomLoggerHelper;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using SecretsKeyVault;
using System.Runtime.CompilerServices;

namespace MongoConnect
{
    public class MongoDbHelper<T>: IMongoDbHelper<T> where T:class, new()
    {
        private IConfigurationRoot _configRoot;
        private readonly ILoggerHelper _logger;
        private readonly IMongoCollection<T> _collection;
        private readonly string _mongoConStr;
        private readonly IMongoClient _mongoClient;
        private IMongoDatabase _database;
        private readonly IKeyVaultManagedIdentityHelper _secretsHelper;

        public MongoDbHelper(ILoggerHelper logger, IConfiguration configRoot
            , IKeyVaultManagedIdentityHelper secretsHelper)
        {
            _logger = logger;
            _configRoot = (IConfigurationRoot)configRoot;
            _secretsHelper = secretsHelper;
            _mongoConStr = GetMongoConstr().Result;
            var settings = MongoClientSettings.FromConnectionString(_mongoConStr);
            // Set the ServerApi field of the settings object to set the version of the Stable API on the client
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            // Create a new client and connect to the server
            _mongoClient = new MongoClient(settings);
            _database = _mongoClient.GetDatabase("biplabhomeMongo");
            
        }

        public async Task<string> GetMongoConstr()
        {
            return await _secretsHelper.GetSecretAsync("MongoDbConstr");
        }
        public async Task<bool> TestPing()
        {
            var isSuccess = false;
            // Send a ping to confirm a successful connection
            try
            {
                var result = _mongoClient.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                isSuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error TestPing");
            }
            return isSuccess;
        }
        public async Task<bool> InsertAsync(T entity)
        {
            try
            {
                var collection = _database.GetCollection<T>(typeof(T).Name); // Use the type name as collection name
                await collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting document.");
                return false;
            }
        }

        // Update an existing document of type T in MongoDB by its id
        public async Task<bool> UpdateAsync(ObjectId id, T updatedEntity)
        {
            try
            {
                var collection = _database.GetCollection<T>(typeof(T).Name);
                var filter = Builders<T>.Filter.Eq("Id", id);  // Assuming every entity has an "Id" field
                var update = Builders<T>.Update
                    .Set("Name", updatedEntity.GetType().GetProperty("Name")?.GetValue(updatedEntity)) // For example, update "Name"
                    .Set("EmailAddress", updatedEntity.GetType().GetProperty("EmailAddress")?.GetValue(updatedEntity)); // Update "EmailAddress" as an example

                var result = await collection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;  // Return true if one document was modified
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating document.");
                return false;
            }
        }

        // Delete a document of type T from MongoDB by its id
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            try
            {
                var collection = _database.GetCollection<T>(typeof(T).Name);
                var filter = Builders<T>.Filter.Eq("Id", id);  // Assuming every entity has an "Id" field

                var result = await collection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;  // Return true if one document was deleted
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting document.");
                return false;
            }
        }
        public async Task<long> DeleteAllExceptionAsync(string collectionName)
        {
            var collection = _database.GetCollection<Exception>(collectionName);

            // Delete all documents in the collection
            var result = await collection.DeleteManyAsync(FilterDefinition<Exception>.Empty);
            return result.DeletedCount;
        }

        // Get a document of type T by its id
        public async Task<T?> GetByIdAsync(ObjectId id)
        {
            try
            {
                var collection = _database.GetCollection<T>(typeof(T).Name);
                var filter = Builders<T>.Filter.Eq("Id", id);
                var entity = await collection.Find(filter).FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving document.");
                return null;
            }
        }

        // Get a document of type T by its id
        public async Task<T?> GetByCustomParamAsync(T request, bool isAnd = true)
        {
            try
            {
                var filter = Builders<T>.Filter.Empty;
                // Dynamically build a filter based on the properties of 'request'
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    if (property.PropertyType == typeof(ObjectId))
                    //property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    var value = property.GetValue(request);
                    if (value != null)
                    {
                        if(isAnd)
                            // Dynamically add the filter based on property name and value
                            filter &= Builders<T>.Filter.Eq(property.Name, value);
                        else
                            filter |= Builders<T>.Filter.Eq(property.Name, value);
                    }
                }
                var collection = _database.GetCollection<T>(typeof(T).Name);
                var entity = await collection.Find(filter).FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving document.");
                return null;
            }
        }

        // Get all documents of type T
        public async IAsyncEnumerable<T> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            // Create an empty filter to retrieve all documents
            var filter = Builders<T>.Filter.Empty;

            // Access the MongoDB collection using the type of the class
            var collection = _database.GetCollection<T>(typeof(T).Name);

            // Execute the query asynchronously and retrieve the cursor
            using (var cursor = await collection.FindAsync(filter, cancellationToken: cancellationToken))
            {
                // Manually iterate over the cursor with MoveNextAsync
                while (await cursor.MoveNextAsync(cancellationToken))
                {
                    foreach (var entity in cursor.Current)
                    {
                        yield return entity;  // Yield each entity one by one
                    }
                }
            }
        }
    }
}
