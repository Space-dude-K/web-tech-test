using AutoMapper;
using Entities.DTO;
using Entities.RequestFeatures.User;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;
using WebApi.ActionFilters;

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
        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
        {
            _logger.LogInformation($"Get users request with params");

            var usersFromDb = await _repository.Users
                .GetUsersWithRolesAsync(userParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(usersFromDb.MetaData));

            var userDto = _mapper.Map<IEnumerable<UserDTO>>(usersFromDb);

            return Ok(userDto);
        }
    }
}
