namespace Repository
{
    public interface IRepositoryManager
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }

        Task SaveAsync();
    }
}