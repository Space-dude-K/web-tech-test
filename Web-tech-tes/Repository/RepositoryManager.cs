using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private WebApiDbContext _webApiDbContext;

        private IUserRepository _userRepository;
        private IRoleRepository _roleRepository;

        public RepositoryManager(WebApiDbContext webApiDbContext)
        {
            _webApiDbContext = webApiDbContext;
        }
        public IUserRepository Users
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_webApiDbContext);

                return _userRepository;
            }
        }
        public IRoleRepository Roles
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_webApiDbContext);

                return _roleRepository;
            }
        }
        public Task SaveAsync()
        {
            if (_webApiDbContext.ChangeTracker.HasChanges())
                return _webApiDbContext.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}