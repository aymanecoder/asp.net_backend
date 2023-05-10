using Microsoft.AspNetCore.Mvc;
namespace appLogement.Controllers;
using MongoDB.Bson;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("[controller]")]
public class ListingsController : ControllerBase
{
    private readonly ListingService _listingService;
    private readonly LoginService _loginService;


    public ListingsController(ListingService listingService,LoginService loginService)
    {
        _listingService = listingService;
        _loginService = loginService;



    }

    [HttpGet]
    public async Task<ActionResult<List<Listing>>> Get()
    {
        return await _listingService.GetListings();
    }

    [HttpPost]
    [Authorize]
public async Task<ActionResult<Listing>> CreateListing([FromBody] Listing listing)
{
    var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
    if (userIdClaim == null)
    {
        return Unauthorized();
    }

    var userId = new ObjectId(userIdClaim.Value);

    var user = await _loginService.GetUserById(userId);
    if (user == null)
    {
        return BadRequest("Invalid user ID.");
    }

    listing.userId = userId;
    listing.createdAt = DateTime.Now;

    await _listingService.CreateListing(listing);

    return CreatedAtRoute("GetListingById", new { id = listing.Id }, listing);
}


}
