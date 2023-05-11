using Microsoft.AspNetCore.Mvc;
namespace appLogement.Controllers;
using MongoDB.Bson;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ListingsController : ControllerBase
{
    private readonly ListingService _listingService;
    private readonly LoginService _loginService;
    private readonly IConfiguration _configuration;

    public ListingsController(ListingService listingService, LoginService loginService, IConfiguration configuration)
    {
        _listingService = listingService;
        _loginService = loginService;
         _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<List<Listing>>> Get()
    {
        return await _listingService.GetListings();
    }


    [HttpPost]
    public async Task<IActionResult>  CreateListing([FromBody] Listing listing)
    {
        // Get the user ID from the authorization token
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return Unauthorized();
        }

        var accessToken = authHeader.ToString().Replace("Bearer ", "");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        // Parse the token and extract the user ID claim
        var token = tokenHandler.ReadJwtToken(accessToken);
     var userId = token.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
        Console.WriteLine($"User ID: {token.Claims}");
            var claims = token.Claims.ToList();

    // Display the claims in the console output
    Console.WriteLine($"Token claims:");
    foreach (var claim in claims)
    {
        Console.WriteLine($"{claim.Type}: {claim.Value}");
    }
        if (userId == null)
        {
            return BadRequest("Invalid user ID.");
        }

        // Verify that the user ID is valid
        var user = await _loginService.GetUserById(new ObjectId(userId));
        if (user == null)
        {
            return BadRequest("Invalid user ID.");
        }

        // Set the user ID and creation date for the new listing
        listing.userId = new ObjectId(userId);
        listing.createdAt = DateTimeOffset.UtcNow;

        // Create the new listing
         await _listingService.CreateListing(listing);
         return Ok();
    }
     [HttpGet("{id:length(24)}", Name = "GetListingById")]
    public async Task<ActionResult<Listing>> GetListingById(string id)
    {
        var listing = await _listingService.GetListingById(new ObjectId(id));
        if (listing == null)
        {
            return NotFound();
        }
        return Ok(listing);
    }
    
[HttpDelete("{id:length(24)}")]
// [Authorize]
public async Task<IActionResult> DeleteListing(string id)
{
    // Get the user ID from the authorization token
 if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return Unauthorized();
        }

        var accessToken = authHeader.ToString().Replace("Bearer ", "");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        // Parse the token and extract the user ID claim
        var token = tokenHandler.ReadJwtToken(accessToken);
     var userId = token.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

    var userIdv = new ObjectId(userId);

    // Call the DeleteListing method of the ListingService class
    var result = await _listingService.DeleteListing(new ObjectId(id), userIdv);

    if (!result)
    {
        return NotFound("Listing not found or user does not have permission to delete it.");
    }

    return Ok();
}

[HttpPut("{id:length(24)}")]
public async Task<IActionResult> UpdateListing(string id, [FromBody] Listing listing)
{
    // Get the user ID from the authorization token
    if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
    {
        return Unauthorized();
    }

    var accessToken = authHeader.ToString().Replace("Bearer ", "");

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
    // Parse the token and extract the user ID claim
    var token = tokenHandler.ReadJwtToken(accessToken);
    var userId = token.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;

    if (userId == null)
    {
        return BadRequest("Invalid user ID.");
    }

    // Verify that the user ID is valid
    var user = await _loginService.GetUserById(new ObjectId(userId));
    if (user == null)
    {
        return BadRequest("Invalid user ID.");
    }

    // Check if the listing exists
    var existingListing = await _listingService.GetListingById(new ObjectId(id));
    if (existingListing == null)
    {
        return NotFound("Listing not found.");
    }

    // Check if the user is authorized to update the listing
    if (existingListing.userId != new ObjectId(userId))
    {
        return Forbid("User is not authorized to update this listing.");
    }

    // Update the existing listing
    existingListing.title = listing.title;
    existingListing.description = listing.description;
    existingListing.price = listing.price;

    await _listingService.UpdateListing(existingListing);

    return Ok(existingListing);
}

}