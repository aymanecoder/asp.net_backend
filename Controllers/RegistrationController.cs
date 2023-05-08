using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;

namespace appLogement.Controllers
{
    [Route("api/register")]
    public class RegisterController : Controller
    {
        private readonly DbContext _dbContext;

        public RegisterController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            // Validate input
            if (user == null || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user object.");
            }

            // Check if user already exists
            if (_dbContext.User.Any(u => u.Email == user.Email))
            {
                return BadRequest("User already exists.");
            }

            // Hash password and save user to database
            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);
            _dbContext.User.InsertOne(user);

            return Ok("User created successfully.");
        }
    }
}