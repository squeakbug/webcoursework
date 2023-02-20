using Microsoft.AspNetCore.Mvc;

using Domain;
using Application;
using WebControllers.Models;
using Infrastructure;

namespace WebControllers
{
    [Route("api/v1")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(IUserService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPut, Route("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPresenter))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid userId, [FromBody] UserPresenter user)
        {
            UserPresenter newUser;

            if (user == null)
            {
                return BadRequest();
            }

            try
            {
                user.Id = userId;
                var userDomain = UserConverter.MapToBusinessEntity(user);
                var newUserDomain = await _userService.Update(userDomain);
                if (newUserDomain == null)
                {
                    return NotFound();
                }
                newUser = UserConverter.MapFromBusinessEntity(newUserDomain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok(newUser);
        }

        [HttpPost, Route("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPresenter))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser([FromBody] UserPayload userPayload)
        {
            UserPresenter newUser;
            if (userPayload == null)
            {
                return BadRequest();
            }
            try
            {
                var userDomain = UserConverter.MapToBusinessEntity(userPayload);
                var newUserDomain = await _userService.Create(userDomain);
                if (newUserDomain == null)
                {
                    return StatusCode(500);
                }
                newUser = UserConverter.MapFromBusinessEntity(newUserDomain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok(newUser);
        }

        [HttpGet, Route("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserPresenter))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid userId)
        {
            UserPresenter newUser;
            try
            {
                var userDomain = await _userService.GetUserById(userId);
                if (userDomain == null)
                {
                    return NotFound();
                }
                newUser = UserConverter.MapFromBusinessEntity(userDomain);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok(newUser);
        }

        [HttpGet, Route("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserPresenter>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = new List<UserPresenter>();
            try
            {
                var usersDomain = await _userService.GetUsers();
                foreach (var user in usersDomain)
                {
                    var newUser = UserConverter.MapFromBusinessEntity(user);
                    users.Add(newUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            return Ok(users);
        }
    }
}