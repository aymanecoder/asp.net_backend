using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;

[ApiController]
[Route("api/listing")]
public class ListingController : ControllerBase
{
    private readonly IMongoCollection<Listing> _collection;

    public ListingController(DbContext dbContext)
    {
        _collection = dbContext.Listing;
    }

    [HttpGet]
    public IEnumerable<Listing> Get()
    {
        return _collection.Find(x => true).ToList();
    }
}