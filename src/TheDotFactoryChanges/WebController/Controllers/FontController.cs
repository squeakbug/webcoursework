using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using WebControllers.Models;
using Domain.Services;
using Domain.Entities;
using Domain.Errors;

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
                names = await Task.Run(() => _service.GetFontNames());
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
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
                modelFonts = await Task.Run(() => _service.GetFonts());
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500);
            }
            foreach (var item in modelFonts)
            {
                fonts.Add(new FontDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                });
            }

            return Ok(fonts);
        }

        [Authorize]
        [HttpGet, Route("fonts/{fontId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FontDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FontGET(long fontId)
        {
            var modelFont = await Task.Run(() => _service.GetFontById((int)fontId));
            return Ok(new Font
            {
                Id = modelFont.Id,
                Name = modelFont.Name,
            });
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
                await Task.Run(() => _service.UpdateFont(new Font
                {
                    Id = (int)fontId,
                    Name = font.Name,
                }));
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
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
                await Task.Run(() => _service.DeleteFont((int)fontId));
            }
            catch (NotFoundException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(404);
            }
            catch (NotAuthorizedException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(401);
            }
            catch (ClientErrorException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(400);
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
