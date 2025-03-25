using System.Security.Claims;

namespace sql_test.Buisness
{
    
    public interface IUserService
    {

        Task HandleSuccessfulSignin(IEnumerable<Claim> claims);

        Task<User?> GetCurrent(IEnumerable<Claim> claims);
    }
}

