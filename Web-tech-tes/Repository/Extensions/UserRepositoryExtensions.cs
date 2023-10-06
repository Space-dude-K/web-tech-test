using Entities.Models;
using Entities.RequestFeatures.User;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> FilterUsers(this IQueryable<User> users, UserParameters userParameters)
        {
            return users.Where(u => 
            u.Id >= userParameters.MinId && u.Id <= userParameters.MaxId &&
            u.Name.Length >= userParameters.MinNameLength && u.Name.Length <= userParameters.MaxNameLength &&
            u.Email.Length >= userParameters.MinEmailLength && u.Email.Length <= userParameters.MaxEmailLength &&
            u.Age >= userParameters.MinAge && u.Age <= userParameters.MaxAge);
        }
        public static IQueryable<User> Sort(this IQueryable<User> users, string orderByQueryString)
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