using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace WebApi.ActionFilters
{
    public class ValidateUserExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ValidateUserExistsAttribute> _logger;
        public ValidateUserExistsAttribute(IRepositoryManager repository, 
            ILogger<ValidateUserExistsAttribute> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") || context.HttpContext.Request.Method.Equals("PATCH");
            var id = (int)context.ActionArguments["userId"];
            var user = await _repository.Users.GetUserAsync(id, trackChanges);

            if (user == null)
            {
                var msg = $"User with id: {id} doesn't exist in the database.";
                _logger.LogInformation(msg);
                context.Result = new NotFoundObjectResult(msg);
            }
            else
            {
                var msg = $"Requested user with id: {id} exist in the database.";
                _logger.LogInformation(msg);
                context.HttpContext.Items.Add("user", user);
                await next();
            }
        }
    }
}