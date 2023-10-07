using Entities.Models;
using Entities.RequestFeatures.Role;
using Entities.RequestFeatures.User;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> FilterUsers(this IQueryable<User> users, UserParameters userParameters)
        {
            return users.Where(u => 
            u.Id >= userParameters.MinUserId && u.Id <= userParameters.MaxUserId &&
            u.Name.Length >= userParameters.MinUserNameLength && u.Name.Length <= userParameters.MaxUserNameLength &&
            u.Email.Length >= userParameters.MinEmailLength && u.Email.Length <= userParameters.MaxEmailLength &&
            u.Age >= userParameters.MinAge && u.Age <= userParameters.MaxAge);
        }
        public static IQueryable<User> FilterUserRoles(this IQueryable<User> users, RoleParameters roleParameters)
        {
            return users.Where(u => u.Roles.All(r => r.Id >= roleParameters.MinRoleId && r.Id <= roleParameters.MaxRoleId && 
            r.Name.Length >= roleParameters.MinRoleNameLength && r.Name.Length <= roleParameters.MaxRoleNameLength));
        }
        public static IQueryable<User> SortUsers(this IQueryable<User> users, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return users.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilderExtensions.CreateOrderQuery<User>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return users.OrderBy(e => e.Name);

            return users.OrderBy(orderQuery);
        }
        public static IQueryable<User> SortUsersAndRoles(this IQueryable<User> users, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return users.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilderExtensions.CreateOrderQuery<User>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return users.OrderBy(e => e.Name);

            return users.OrderBy(orderQuery);
        }
    }
}