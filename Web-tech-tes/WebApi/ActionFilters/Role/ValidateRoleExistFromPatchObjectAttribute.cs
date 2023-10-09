using Entities.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;
using Entities.DTO.Update;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApi.ActionFilters.Role
{
    public class ValidateRoleExistFromPatchObjectAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ValidateRoleExistFromPatchObjectAttribute> _logger;
        public ValidateRoleExistFromPatchObjectAttribute(IRepositoryManager repository,
            ILogger<ValidateRoleExistFromPatchObjectAttribute> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var patchDoc = (JsonPatchDocument<UserUpdateDTO>)context.ActionArguments["patchDoc"];

            if (patchDoc == null)
            {
                var msg = $"PatchDoc for role object sent from client is null";
                _logger.LogInformation(msg);
                context.Result = new BadRequestObjectResult(msg);
                return;
            }

            var elementsWithPath = patchDoc.Operations
                .Where(o => o.path.Equals("/Roles/-")).FirstOrDefault();

            var docRole = (RoleDTO)JsonConvert
                .DeserializeObject(elementsWithPath.value.ToString(), typeof(RoleDTO));
            var roleFromDb = await _repository.Roles.GetRoleAsync(docRole.Id, false);

            if (roleFromDb == null)
            {
                var msg = $"Role with id: {docRole.Id} doesn't exist in the database.";
                _logger.LogInformation(msg);
                context.Result = new NotFoundObjectResult(msg);
                return;
            }
            else if(!roleFromDb.Name.Equals(docRole.Name))
            {
                var msg = $"Role with name: {docRole.Name} doesn't exist in the database.";
                _logger.LogInformation(msg);
                context.Result = new NotFoundObjectResult(msg);
                return;
            }
            else
            {
                var msg = $"Requested user with id: {docRole.Id} exist in the database.";
                _logger.LogInformation(msg);

                await next();
            }
        }
    }
}