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


[Route("api/login")]
public class LoginController : Controller
{
    private readonly DbContext _dbContext;
    private readonly IConfiguration _configuration;

    public LoginController(DbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        // Validate input
        if (user == null || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
        {
            return BadRequest("Invalid user object.");
        }

        // Authenticate user against database
        var dbUser = await _dbContext.User.Find(u => u.email == user.email).FirstOrDefaultAsync();
        if (dbUser == null || !new PasswordHasher<User>().VerifyHashedPassword(dbUser, dbUser.password, user.password).Equals(PasswordVerificationResult.Success))
        {
            return Unauthorized("Invalid email or password.");
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, dbUser.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}