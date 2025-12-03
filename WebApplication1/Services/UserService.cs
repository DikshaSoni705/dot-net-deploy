using MongoDB.Driver;
using Microsoft.Extensions.Options;
using UserApi.Models;

namespace UserApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;

            // Safety check
            if (string.IsNullOrEmpty(settings.ConnectionString))
                throw new Exception("MongoDB ConnectionString is missing in appsettings.json");

            if (string.IsNullOrEmpty(settings.DatabaseName))
                throw new Exception("MongoDB DatabaseName is missing in appsettings.json");

            if (string.IsNullOrEmpty(settings.CollectionName))
                throw new Exception("MongoDB CollectionName is missing in appsettings.json");

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.CollectionName);
        }

        public async Task<List<User>> GetAll() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> GetById(string id) =>
            await _users.Find(u => u.Id == id).FirstOrDefaultAsync();

        public async Task Create(User user) =>
            await _users.InsertOneAsync(user);

        public async Task Update(string id, User newUser) =>
            await _users.ReplaceOneAsync(u => u.Id == id, newUser);

        public async Task Delete(string id) =>
            await _users.DeleteOneAsync(u => u.Id == id);
    }
}
