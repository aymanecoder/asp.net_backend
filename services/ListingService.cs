
using MongoDB.Driver;
using MongoDB.Bson;
public class ListingService
{
    private readonly IMongoCollection<Listing> _listings;

    public ListingService(IConfiguration config)
    {
        var client = new MongoClient("mongodb+srv://aymane:aymane@cluster0.qgk357r.mongodb.net");
        var database = client.GetDatabase("test");
        _listings = database.GetCollection<Listing>("Listing");
    }

    public async Task<List<Listing>> GetListings()
    {
        return await _listings.Find(listing => true).ToListAsync();
    }
}

