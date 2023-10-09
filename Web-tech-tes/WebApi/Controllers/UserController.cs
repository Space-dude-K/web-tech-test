using AutoMapper;
using Entities.DTO;
using Entities.DTO.Update;
using Entities.Models;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Role;
using Entities.RequestFeatures.User;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;
using System.Dynamic;
using System.Text.RegularExpressions;
using WebApi.ActionFilters;
using WebApi.ActionFilters.Role;
using WebApi.ActionFilters.User;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IRepositoryManager repository, ILogger<UserController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Получаем список всех пользователей
        /// </summary>
        /// <returns>Список пользователей</returns>
        /// <response code="200">Возращает пользователей</response>
        /// <response code="400">Если отсутствует Accept header</response>
        /// <response code="401">Если неавторизован</response>
        /// <response code="404">Если нет пользователей в БД</response>
        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateUsersExistsAttribute))]
        public async Task<IActionResult> GetUsers(
            [FromQuery] UserParameters userParameters, 
            [FromQuery] RoleParameters roleParameters)
        {
            var usersFromDb = HttpContext.Items["users"] as PagedList<User>;

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(usersFromDb.MetaData));

            var userDto = _mapper.Map<IEnumerable<UserDTO>>(usersFromDb);

            return Ok(userDto);
        }
        /// <summary>
        /// Получаем пользователя по Id
        /// </summary>
        /// <returns>Пользователь</returns>
        /// <response code="200">Возращает пользователя</response>
        /// <response code="400">Если отсутствует Accept header</response>
        /// <response code="401">Если неавторизован</response>
        /// <response code="404">Если пользователя нет в БД</response>
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> GetUser(int userId)
        {
            var userFromDb = HttpContext.Items["user"] as User;
            var userDto = _mapper.Map<UserDTO>(userFromDb);

            return Ok(userDto);
        }
        /// <summary>
        /// Creates a newly created category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>A newly created category</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPatch("{userId}", Name = "AddRoleToUser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        //[ServiceFilter(typeof(ValidateRoleExistsAttribute))]
        //[ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        [ServiceFilter(typeof(ValidateRoleExistFromPatchObjectAttribute))]
        public async Task<IActionResult> AddRoleToUser(int userId, 
            [FromBody] JsonPatchDocument<UserUpdateDTO> patchDoc)
        {
            var userFromDb = HttpContext.Items["user"] as User;

            var userToPatch = _mapper.Map<UserUpdateDTO>(userFromDb);

            patchDoc.ApplyTo(userToPatch, ModelState);
            TryValidateModel(userToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(userToPatch, userFromDb);

            await _repository.SaveAsync();

            return NoContent();
        }
        [HttpPut("{userId}", Name = "AddRoleToUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDTO user)
        {
            var userFromDb = HttpContext.Items["user"] as User;

            TryValidateModel(user);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(user, userFromDb);

            await _repository.SaveAsync();

            return NoContent();
        }
        /// <summary>
        /// Deletes existing user
        /// </summary>
        /// <param name="categoryId"></param>
        /// <response code="204">If user was successfully deleted</response>
        /// <response code="400">If Accept header is missing</response>
        /// <response code="401">If unauthorized</response>
        /// <response code="404">If user doesn't exist</response>
        [HttpDelete("{userId}", Name = "DeleteUserById")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [ServiceFilter(typeof(ValidateUserExistsAttribute))]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            _logger.LogDebug($"Delete user: {userId} request");

            var userFromDb = HttpContext.Items["user"] as User;

            _repository.Users.DeleteUser(userFromDb);

            await _repository.SaveAsync();

            return NoContent();
        }
    }
}