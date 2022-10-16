using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

using WebControllers.Controllers;
using DataAccessInterface;
using Presenter;
using WebControllers.Models;

namespace WebControllers.Controllers
{
    [Route("api/{basePath}")]
    public class UserController : ControllerBase
    {
        private IConverterService _service;
        private IAuthService _authService;
        private ILogger _logger;

        public UserController(IConverterService service, IAuthService authService,
            ILogger<UserController> logger)
        {
            _service = service ?? throw new ArgumentNullException("bad service");
            _authService = authService ?? throw new ArgumentNullException("bad auth service");
            _logger = logger ?? throw new ArgumentNullException("bad logger");

            _service.OutputSourceTextChanged += (text) => { };
            _service.OutputHeaderTextChanged += (text) => { };
            _service.FontChanged += (font) => { };
            _service.ConfigRemoved += (id) => { };
            _service.ConfigAdded += (id) => { };
            _service.ConfigUpdated += (id) => { };
            _service.ConfigsUpdated += () => { };
        }

        [HttpGet, Route("users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<UserDTO>))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UsersAllAsync()
        {
            ICollection<UserDTO> users = new List<UserDTO>();
            IEnumerable<UserInfo> infos;
            try
            {
                infos = _authService.GetUsers();
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(403);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            foreach (var item in infos)
            {
                var user = new UserDTO
                {
                    Login = item.Login,
                    Name = item.Name,
                    Password = item.Password,
                };
                users.Add(user);
            }

            return Ok(users);
        }

        [HttpGet, Route("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UsersGETAsync(long userId)
        {
            UserInfo info;
            try
            {
                info = _authService.GetUserById((int)userId);
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(403);
            }
            catch (ClientErrorException)
            {
                return StatusCode(400);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }
            if (info == null)
            {
                return StatusCode(404);
            }

            var user = new UserDTO
            {
                Login = info.Login,
                Name = info.Name,
                Password = info.Password,
            };

            return Ok(user);
        }

        [Authorize]
        [HttpPatch, Route("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UsersPATCHAsync(long userId,
            [FromBody] UserPatchRequest body)
        {

            try
            {
                if (body.Op == "ChangePassword")
                {
                    _authService.UpdateUserPassword((int)userId, body.Value);
                }
                else if (body.Op == "ChangeName")
                {
                    _authService.UpdateUserName((int)userId, body.Value);
                }
                else
                {
                    throw new ClientErrorException("Not supported operation");
                }
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(403);
            }
            catch (NotFoundException)
            {
                return StatusCode(404);
            }
            catch (ClientErrorException)
            {
                return StatusCode(400);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [HttpGet, Route("users/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginAsync([FromQuery] string login, [FromQuery] string password)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [Authorize]
        [HttpGet, Route("users/logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);


            return Ok();
        }

        [HttpPost, Route("users/registration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrationAsync([FromBody] NewUserDTO body)
        {
            long newId;
            try
            {
                newId = _authService.RegistrateUser(body.Login, body.Password, body.RepPassword);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            return Ok(newId);
        }
    }
}
