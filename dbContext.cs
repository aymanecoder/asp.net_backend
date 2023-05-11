using MongoDB.Driver;

public class DbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;

    public DbContext()
    {
        var connectionString = "mongodb+srv://aymane:aymane@cluster0.qgk357r.mongodb.net/test?retryWrites=true&w=majority";
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase("test2");
    }

    public IMongoCollection<Listing> Listing => _database.GetCollection<Listing>("Listing");
    public IMongoCollection<User> User => _database.GetCollection<User>("User");
        public IMongoCollection<Reservation> Reservation => _database.GetCollection<Reservation>("Reservation");

}
