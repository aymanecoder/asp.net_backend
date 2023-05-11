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
[Route("[controller]")]
public class LogoutController : Controller
{
[HttpPost]

public IActionResult Logout()
{
    // Get the token from the authorization header
    var header = Request.Headers["Authorization"].FirstOrDefault();
    if (header != null && header.StartsWith("Bearer "))
    {
        var tokenString = header.Substring("Bearer ".Length);

        // Invalidate the token by creating a new token descriptor with an expiration time in the past
        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = token.Subject,
            Expires = DateTime.UtcNow.AddMinutes(-1),
            SigningCredentials = token.SigningCredentials
        };
        var invalidToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);
        var invalidTokenString = new JwtSecurityTokenHandler().WriteToken(invalidToken);

        return Ok(new { Token = invalidTokenString });
    }

    return BadRequest("Invalid or missing authorization header.");
}
}