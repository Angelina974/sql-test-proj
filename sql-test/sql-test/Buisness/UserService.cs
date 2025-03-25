using Microsoft.Identity.Client;
using sql_test.DataAccesses;
using System.Security.Claims;
namespace sql_test.Buisness
{
    public class UserService(IUserDataAccess _dataAccess) : IUserService
    {
        public async Task HandleSuccessfulSignin(IEnumerable<Claim> claims)
        {
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

            if (emailClaim != null && nameClaim != null)
            {
                var user = await _dataAccess.FindByEmail(emailClaim.Value);

                if (user != null)
                {
                    user = new User
                    {
                        Email = emailClaim.Value,
                        Name = nameClaim.Value
                    };

                    await _dataAccess.Save(user);
                }
            }

            
        }

        public async Task<User?> GetCurrent(IEnumerable<Claim> claims)
        {
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (email != null)
            {
                return await _dataAccess.FindByEmail(email.Value);
            }
            return null;
        }
    }
}