using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
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
            if (user == null || string.IsNullOrEmpty(user.name) || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password))
            {
                return BadRequest("Invalid user object.");
            }

             var filter = Builders<User>.Filter.Eq(u => u.email, user.email);
            var existingUser = _dbContext.User.Find(filter).FirstOrDefault();
            if (existingUser != null)
            {
                return BadRequest("User already exists.");
            }

            // Hash password and save user to database
            var passwordHasher = new PasswordHasher<User>();
            user.password = passwordHasher.HashPassword(user, user.password);
            _dbContext.User.InsertOne(user);

            return Ok("User created successfully.");
        }
    }
}