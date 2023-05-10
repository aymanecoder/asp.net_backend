
using MongoDB.Driver;
using MongoDB.Bson;
public class ListingService
{
    private readonly IMongoCollection<Listing> _listings;
    private readonly DbContext _dbContext;
    public ListingService(DbContext dbContext)

    {

        _dbContext = dbContext;
       
    }

    public async Task<List<Listing>> GetListings()
    {
        return await _dbContext.Listing.Find(listing => true).ToListAsync();

    }
    public async Task CreateListing(Listing listing)
{
   
    await _dbContext.Listing.InsertOneAsync(listing);
}
        
}

