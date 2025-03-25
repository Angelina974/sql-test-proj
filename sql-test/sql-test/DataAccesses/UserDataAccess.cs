using Microsoft.EntityFrameworkCore;

namespace sql_test.DataAccesses
{
    public class UserDataAccess(ApplicationDbContext _dbContext) : IUserDataAccess
    {
        public async Task<User?> FindByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task Save(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
    
}
