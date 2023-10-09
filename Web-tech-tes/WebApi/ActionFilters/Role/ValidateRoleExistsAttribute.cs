using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace WebApi.ActionFilters.Role
{
    public class ValidateRoleExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ValidateRoleExistsAttribute> _logger;
        public ValidateRoleExistsAttribute(IRepositoryManager repository,
            ILogger<ValidateRoleExistsAttribute> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") || context.HttpContext.Request.Method.Equals("PATCH");
            var id = (int)context.ActionArguments["roleId"];
            var role = await _repository.Roles.GetRoleAsync(id, trackChanges);

            if (role == null)
            {
                var msg = $"Role with id: {id} doesn't exist in the database.";
                _logger.LogInformation(msg);
                context.Result = new NotFoundObjectResult(msg);
                return;
            }
            else
            {
                var msg = $"Requested user with id: {id} exist in the database.";
                _logger.LogInformation(msg);
                context.HttpContext.Items.Add("role", role);
                await next();
            }
        }
    }
}