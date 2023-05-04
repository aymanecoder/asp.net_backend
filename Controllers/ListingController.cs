using Microsoft.AspNetCore.Mvc;
namespace appLogement.Controllers;
using MongoDB.Bson;
[ApiController]
[Route("[controller]")]
public class ListingsController : ControllerBase
{
    private readonly ListingService _listingService;

    public ListingsController(ListingService listingService)
    {
        _listingService = listingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Listing>>> Get()
    {
        return await _listingService.GetListings();
    }
}
