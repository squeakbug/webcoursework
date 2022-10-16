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
    public class ConvertionController : ControllerBase
    {
        private IConverterService _service;
        private IAuthService _authService;
        private ILogger _logger;

        public ConvertionController(IConverterService service, IAuthService authService,
            ILogger<ConvertionController> logger)
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
        [HttpGet, Route("convertionsNames")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<ConvertionNameDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConvertionsNames()
        {
            ICollection<ConvertionNameDTO> convertionsDTO = new List<ConvertionNameDTO>();
            IEnumerable<Convertion> convertions;
            try
            {
                convertions = _service.GetConvertions();
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(401);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            foreach (var item in convertions)
            {
                try
                {
                    convertionsDTO.Add(new ConvertionNameDTO
                    {
                        Name = item.Name
                    });
                }
                catch
                {
                    return StatusCode(500);
                }
            }

            return Ok(convertions);
        }

        [Authorize]
        [HttpGet, Route("convertions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<ConvertionDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConvertionsAll()
        {
            ICollection<ConvertionDTO> convertionsDTO = new List<ConvertionDTO>();
            IEnumerable<Convertion> convertions;
            try
            {
                convertions = _service.GetConvertions();
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(401);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            foreach (var item in convertions)
            {
                try
                {
                    convertionsDTO.Add(new ConvertionDTO
                    {
                        Body = item.Body,
                        Head = item.Head,
                        Id = item.Id,
                    });
                }
                catch
                {
                    return StatusCode(500);
                }
            }

            return Ok(convertions);
        }

        [Authorize]
        [HttpGet, Route("convertions/{convertionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConvertionDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Convertions(int convertionId)
        {
            Convertion convertion;
            try
            {
                convertion = _service.GetConvertionById(convertionId);
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(401);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            if (convertion == null)
            {
                return StatusCode(404);
            }

            var convertionDTO = new ConvertionDTO
            {
                Body = convertion.Body,
                Head = convertion.Head,
                Id = convertion.Id,
            };
            return Ok(convertionDTO);
        }

        [Authorize]
        [HttpPost, Route("convertions/doConvertion/fromTemplate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoConvertionResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FromTemplate([FromBody] DoConvertionFromTemplateRequest body)
        {
            string source, header;
            try
            {
                _service.SetCurrentConfig(body.ConfigId);
                _service.SetCurrentFont(body.FontId);
                _service.SetInputText(body.Template);
                _service.ConvertFont(true);
                source = _service.GetOutputSourceText();
                header = _service.GetOutputHeaderText();
            }
            catch (NotAuthorizedException)
            {
                return StatusCode(401);
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }

            var response = new DoConvertionResponse
            {
                Body = source,
                Head = header,
            };

            return Ok(response);
        }

        [HttpPost, Route("convertions/saveConvertion")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveConvertionResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveConvertion([FromBody] SaveConvertionRequest body)
        {
            int newId;
            try
            {
                var newConvertion = new Convertion
                {
                    Body = body.Body,
                    Head = body.Head,
                    Name = body.Name,
                };
                newId = _service.AddConvertion(newConvertion);
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

            var response = new SaveConvertionResponse
            {
                Id = newId,
            };

            return Ok(response);
        }
    }
}
