using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Reservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string UserId { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string ListingId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int TotalPrice { get; set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    public Reservation(string userId, string listingId, DateTime startDate, DateTime endDate, int totalPrice)
    {
        UserId = userId;
        ListingId = listingId;
        StartDate = startDate;
        EndDate = endDate;
        TotalPrice = totalPrice;
        CreatedAt = DateTime.UtcNow;
    }
}