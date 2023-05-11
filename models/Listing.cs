
using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class Listing
{
     public ObjectId Id { get; set; }  
     public string title { get; set; }
    public string description { get; set; }
    public string imageSrc { get; set; }

    public DateTimeOffset createdAt { get; set; }
    public string category { get; set; }
    public int bathroomCount { get; set; }
    public int roomCount { get; set; }

    public int guestCount { get; set; }
    public string locationValue { get; set; }
    public ObjectId userId { get; set; }

    public int price { get; set; }


    // public Listing()

    // {

    //   Id = ObjectId.GenerateNewId();
    //     createdAt = DateTime.UtcNow;
    //     title = string.Empty;
    //     description = string.Empty;
    //     imageSrc = string.Empty;
    //     category = string.Empty;
    //     bathroomCount = 0;
    //     guestCount = 0;
    //     locationValue = string.Empty;
    //     userId = ObjectId.Empty;
    //     price = 0;
    // }
}

