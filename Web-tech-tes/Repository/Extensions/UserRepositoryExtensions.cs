using Entities.Models;
using Entities.RequestFeatures.Role;
using Entities.RequestFeatures.User;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

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

            // Получаем параметры для user из query
            var orderByRoleQueryStr = string.Join(",", orderByQueryString
                .Split(",").Select(q => q.Trim())
                .Where(q => !q.StartsWith("Roles", StringComparison.CurrentCultureIgnoreCase)));

            var orderQuery = OrderQueryBuilderExtensions.CreateOrderQuery<User>(orderByRoleQueryStr);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return users.OrderBy(e => e.Name);

            return users.OrderBy(orderQuery);
        }
        public static IQueryable<User> SortUsersThenByRoles(this IQueryable<User> users, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return users.OrderBy(e => e.Name).ThenBy(e => e.Roles.FirstOrDefault().Name);

            var queryRaw = orderByQueryString
                .Split(",").Select(q => q.Trim());

            // Получаем параметры для user из query
            var orderByRoleQueryStrForUsers = string.Join(",", queryRaw
                .Where(q => !q.StartsWith("Roles", StringComparison.CurrentCultureIgnoreCase)));

            // Получаем параметры для role из query
            var orderByRoleQueryStrForRoles = string.Join(",", queryRaw
                .Where(q => q
                    .StartsWith("Roles", StringComparison.CurrentCultureIgnoreCase))
                .Select(q => string.Join(" ", q.Split(" ").Skip(1))).ToArray());

            var orderQueryForUsers = OrderQueryBuilderExtensions
                .CreateOrderQuery<User>(orderByRoleQueryStrForUsers);

            if(string.IsNullOrWhiteSpace(orderByRoleQueryStrForRoles) && 
                !string.IsNullOrWhiteSpace(orderQueryForUsers))
            {
                return users.OrderBy(orderQueryForUsers);
            }

            if(string.IsNullOrWhiteSpace(orderQueryForUsers) &&
                string.IsNullOrWhiteSpace(orderByRoleQueryStrForRoles))
            {
                return users.OrderBy(e => e.Name)
                    .ThenBy(e => e.Roles.FirstOrDefault().Name);
            }

            var rolesOrderQuery = orderByRoleQueryStrForRoles.Split(",");
            foreach (var q in rolesOrderQuery)
            {
                var roleQuery = q.Split(" ");
                var roleAtt = roleQuery[0];
                var isDesc = roleQuery[1].EndsWith("desc");

                switch (roleAtt)
                {
                    case "Id":
                        return users.OrderBy(orderQueryForUsers).ThenByWithDirection(e => e.Roles.FirstOrDefault().Id, isDesc);
                    case "Name":
                        return users.OrderBy(orderQueryForUsers).ThenByWithDirection(e => e.Roles.FirstOrDefault().Name, isDesc);
                    default:
                        return users.OrderBy(orderQueryForUsers).ThenByWithDirection(e => e.Roles.FirstOrDefault().Name, isDesc);
                }
            }

            return users.OrderBy(orderQueryForUsers).ThenBy(e => e.Roles.FirstOrDefault().Name);
        }
        private static IOrderedQueryable<TSource> ThenByWithDirection<TSource, TKey>(
                      this IOrderedQueryable<TSource> source,
                      Expression<Func<TSource, TKey>> keySelector,
                      bool descending = false)
        {
            if (descending) return source.ThenByDescending(keySelector);
            return source.ThenBy(keySelector);
        }
    }
}