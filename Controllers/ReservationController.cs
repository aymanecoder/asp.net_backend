
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
public class ReservationController : Controller {
       private readonly ReservationService _reservationService;
    private readonly LoginService _loginService;
        private readonly ListingService _listingService;

    private readonly IConfiguration _configuration;

    public ReservationController(ReservationService reservationService, LoginService loginService, IConfiguration configuration,ListingService listingService)
    {
        _reservationService = reservationService;
        _loginService = loginService;
         _configuration = configuration;
         _listingService=listingService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Reservation>>> Get()
    {
        return await _reservationService.GetReservations();
    }

    [HttpPost]
public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
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
    listing=await _listingService.GetListingById(reservation.ListingId);
    // Set the user ID and creation date for the new reservation
    reservation.UserId = userId;
    reservation.CreatedAt = DateTime.UtcNow;
    reservation.TotalPrice = listing.price * (reservation.EndDate - reservation.StartDate).Days;

    // Create the new reservation
    await _reservationService.CreateReservation(reservation);

    return Ok(reservation);
}

[HttpGet("{id:length(24)}")]
public async Task<IActionResult> GetReservation(string id)
{
    var reservation = await _reservationService.GetReservationById(id);

    if (reservation == null)
    {
        return NotFound();
    }

    return Ok(reservation);
}


[HttpPut("{id:length(24)}")]
public async Task<IActionResult> UpdateReservation(string id, [FromBody] Reservation reservation)
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

    // Check if the reservation exists
    var existingReservation = await _reservationService.GetReservationById(id);
    if (existingReservation == null)
    {
        return NotFound();
    }

    // Check if the user is authorized to update the reservation
    if (existingReservation.UserId != userId)
    {
        return Forbid("User is not authorized to update this reservation.");
    }

    // Update the existing reservation
    existingReservation.StartDate = reservation.StartDate;
    existingReservation.EndDate = reservation.EndDate;
    existingReservation.TotalPrice = reservation.TotalPrice;

    await _reservationService.UpdateReservation(existingReservation);

    return Ok(existingReservation);
}[HttpDelete("{id:length(24)}")]
public async Task<IActionResult> DeleteReservation(string id)
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

    // Check if the reservation exists
    var existingReservation = await _reservationService.GetReservationById(id);
    if (existingReservation == null)
    {
        return NotFound();
    }

    // Check if the user is authorized to delete the reservation
    if (existingReservation.UserId != userId)
    {
        return Forbid("User is not authorized to delete this reservation.");
    }

    // Delete the existing reservation
    await _reservationService.DeleteReservation(existingReservation.Id);

    return Ok();
}

}
