
using MongoDB.Driver;
using MongoDB.Bson;
public class ReservationService
{
    private readonly IMongoCollection<Listing> _listings;
    private readonly DbContext _dbContext;
    public ReservationService(DbContext dbContext)

    {

        _dbContext = dbContext;
       
    }

    public async Task CreateReservation(Reservation reservation)
{
    try
    {
        await _dbContext.Reservation.InsertOneAsync(reservation);
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine(ex.Message);
        throw;
    }
    
}
    public async Task<Reservation> GetReservationById(string id)
{
    try
    {
        var filter = Builders<Reservation>.Filter.Eq(r => r.Id, id);
        var reservation = await _dbContext.Reservation.Find(filter).FirstOrDefaultAsync();
        return reservation;
    }
    catch (Exception ex)
    {
        // Log the exception
         Console.WriteLine(ex.Message);
        throw;
    }
}


public async Task<bool> UpdateReservation(Reservation reservation)
{
    try
    {
        var filter = Builders<Reservation>.Filter.Eq(r => r.Id, reservation.Id);
        var update = Builders<Reservation>.Update
            .Set(r => r.StartDate, reservation.StartDate)
            .Set(r => r.EndDate, reservation.EndDate)
            .Set(r => r.TotalPrice, reservation.TotalPrice);

        var result = await _dbContext.Reservation.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }
    catch (Exception ex)
    {
        // Log the exception
       Console.WriteLine(ex.Message);
        throw;
    }
}

public async Task<bool> DeleteReservation(string id)
{
    try
    {
        var filter = Builders<Reservation>.Filter.Eq(r => r.Id, id);
        var result = await _dbContext.Reservation.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }
    catch (Exception ex)
    {
        // Log the exception
         Console.WriteLine(ex.Message);
        throw;
    }
}
}
        


