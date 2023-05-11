
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
public async Task<Listing> GetListingById(ObjectId id)
{
    var filter = Builders<Listing>.Filter.Eq("_id", id);
    var listing = await _dbContext.Listing.Find(filter).FirstOrDefaultAsync();
    return listing;
}
public async Task<bool> DeleteListing(ObjectId listingId, ObjectId userId)
{
    // Check if the user owns the listing
    var listing = await GetListingById(listingId);
    if (listing == null)
    {
        return false;
    }

    if (listing.userId != userId)
    {
        return false;
    }

    // Delete the listing from the database
    var result = await _dbContext.Listing.DeleteOneAsync(
        Builders<Listing>.Filter.Eq(l => l.Id, listingId)
    );

    return result.DeletedCount > 0;
}

public async Task<bool> UpdateListing(Listing listing)
{
    try
    {
        var filter = Builders<Listing>.Filter.Eq(l => l.Id, listing.Id);
        var update = Builders<Listing>.Update
            .Set(l => l.title, listing.title)
            .Set(l => l.description, listing.description)
            .Set(l => l.price, listing.price);

        var result = await _listings.ReplaceOneAsync(filter, listing);

        return result.ModifiedCount > 0;
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine(ex.ToString());
        return false;
    }
}
        
}

