using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("api/logout")]
public class LogoutController : Controller
{
    private readonly IConfiguration _configuration;
     public LogoutController( IConfiguration configuration)
    {
        _configuration = configuration;
    }

[HttpPost]
public IActionResult Logout()
{
    // Extract the current token from the Authorization header
    var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    if (token == null)
    {
        return BadRequest("No token found in Authorization header.");
    }

    // Parse the token to get its claims
    var tokenHandler = new JwtSecurityTokenHandler();
    var tokenClaims = tokenHandler.ReadJwtToken(token);

    // Create a new ClaimsIdentity from the claims of the original token
    var claimsIdentity = new ClaimsIdentity(tokenClaims.Claims);

    // Modify the token's expiration time to a time in the past
    var newTokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = claimsIdentity,
        Expires = DateTime.UtcNow.AddMinutes(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"])), SecurityAlgorithms.HmacSha256Signature)
    };
    var newToken = tokenHandler.CreateToken(newTokenDescriptor);
    var newTokenString = tokenHandler.WriteToken(newToken);

    // Set the modified token in the response header
    Response.Headers.Add("Authorization", "Bearer " + newTokenString);

    return Ok();
}

}