using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using WebControllers.Controllers;
using DataAccessInterface;
using Presenter;
using AuthService;
using WebControllers.Models;

namespace WebControllers.Controllers
{
    [Route("api/{basePath}")]
    public class FontController : ControllerBase
    {
        private IConverterService _service;
        private IAuthService _authService;
        private ILogger _logger;

        public FontController(IConverterService service, IAuthService authService,
            ILogger<FontController> logger)
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

        [Authorize]
        [HttpGet, Route("fontNames")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<FontNameDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontNames()
        {
            ICollection<FontNameDTO> fontNames = new List<FontNameDTO>();
            IEnumerable<string> names;
            try
            {
                names = _service.GetFontNames();
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }
            foreach (var item in names)
                fontNames.Add(new FontNameDTO { Name = item });
            return Ok(fontNames);
        }

        [Authorize]
        [HttpGet, Route("fonts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<FontDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontsAll()
        {
            ICollection<FontDTO> fonts = new List<FontDTO>();
            IEnumerable<Font> modelFonts;
            try
            {
                modelFonts = _service.GetFonts();
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }
            foreach (var item in modelFonts)
                fonts.Add(new FontDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Size = item.Size,
                });
            return Ok(fonts);
        }

        [Authorize]
        [HttpGet, Route("fonts/{fontId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FontDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontGET(long fontId)
        {
            var modelFont = _service.GetFontById((int)fontId);
            return Ok(new Font
            {
                Id = modelFont.Id,
                Name = modelFont.Name,
                Size = modelFont.Size,
            });
        }

        [Authorize]
        [HttpPost, Route("fonts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontPOST([FromBody] FontDTO font)
        {
            int id;
            try
            {
                id = _service.AddFont(new Font
                {
                    Id = (int)font.Id,
                    Name = font.Name,
                    Size = (int)font.Size,
                });
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }
            return Ok(id.ToString());
        }

        [Authorize]
        [HttpPut, Route("fonts/{fontId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontPUT(long fontId, [FromBody] FontDTO font)
        {
            try
            {
                _service.UpdateFont(new Font
                {
                    Id = (int)fontId,
                    Name = font.Name,
                    Size = (int)font.Size,
                });
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            return Ok();
        }

        [Authorize]
        [HttpDelete, Route("fonts/{fontId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontsDELETE(long fontId)
        {
            try
            {
                _service.DeleteFont((int)fontId);
            }
            catch (NotFoundException)
            {
                return StatusCode(404);
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(401);
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
    }
}
