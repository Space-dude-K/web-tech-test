using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private WebApiDbContext _webApiDbContext;
        private IUserRepository _userRepository;
        private IUserRepository _roleRepository;

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
    }
}
