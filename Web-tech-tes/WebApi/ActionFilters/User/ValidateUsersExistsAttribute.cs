using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Entities.RequestFeatures.Role;
using Entities.RequestFeatures.User;

namespace WebApi.ActionFilters.User
{
    public class ValidateUsersExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ValidateUsersExistsAttribute> _logger;
        public ValidateUsersExistsAttribute(IRepositoryManager repository,
            ILogger<ValidateUsersExistsAttribute> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") || context.HttpContext.Request.Method.Equals("PATCH");

            var userParameters = (UserParameters)context.ActionArguments["userParameters"];
            var roleParameters = (RoleParameters)context.ActionArguments["roleParameters"];

            var usersFromDb = await _repository.Users
            .GetUsersWithRolesAsync(userParameters, roleParameters, trackChanges);

            if (usersFromDb == null || usersFromDb.Count == 0)
            {
                var msg = $"No users in the database.";
                _logger.LogInformation(msg);
                context.Result = new NotFoundObjectResult(msg);
            }
            else
            {
                var msg = $"{usersFromDb.Count} users was found in the database.";
                _logger.LogInformation(msg);
                context.HttpContext.Items.Add("users", usersFromDb);
                await next();
            }
        }
    }
}