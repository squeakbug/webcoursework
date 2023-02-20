namespace Domain
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserById(Guid id);
        Task<User?> Create(NewUser newUser);
        Task<User?> Update(User user);
        Task<bool> Delete(Guid id);
    }
}
