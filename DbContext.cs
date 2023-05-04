using MongoDB.Driver;

public class DbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public DbContext()
    {
        var connectionString = "mongodb+srv://aymane:aymane@cluster0.qgk357r.mongodb.net/test?retryWrites=true&w=majority";
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase("test");
    }

    public IMongoCollection<Listing> Listing => _database.GetCollection<MyModel>("Listing");
}