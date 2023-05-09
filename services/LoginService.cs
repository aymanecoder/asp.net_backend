

using MongoDB.Driver;
using MongoDB.Bson;
public class LoginService {

     private readonly DbContext _dbContext;
    public LoginService(DbContext dbContext)

    {

        _dbContext = dbContext;
       
    }
    public async Task<User> GetUserById(ObjectId userId)
            {
                return await _dbContext.User.Find(u => u.Id == userId).FirstOrDefaultAsync();
            }


            
}