namespace sql_test.DataAccesses
{
    public interface IUserDataAccess
    {
        Task<User?> FindByEmail(string email);
        Task Save(User user);
    }
}
