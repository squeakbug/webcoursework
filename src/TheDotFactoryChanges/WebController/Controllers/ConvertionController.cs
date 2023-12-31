﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using WebControllers.Controllers;
using WebControllers.Models;
using Domain.Services;
using Domain.Entities;
using Domain.Errors;

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
                convertions = await Task.Run(() => _service.GetConvertions());
            }
            catch (NotAuthorizedException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(401);
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
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
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    return StatusCode(500);
                }
            }

            return Ok(convertionsDTO);
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
                convertions = await Task.Run(() => _service.GetConvertions());
            }
            catch (NotAuthorizedException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(401);
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
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
                        Name = item.Name,
                        Id = item.Id,
                    });
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    return StatusCode(500);
                }
            }

            return Ok(convertionsDTO);
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
                convertion = await Task.Run(() => _service.GetConvertionById(convertionId));
            }
            catch (NotAuthorizedException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(401);
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500);
            }

            if (convertion == null)
            {
                _logger.Log(LogLevel.Error, "resourse not found");
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
        [HttpGet, Route("convertions/from-template")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoConvertionResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FromTemplate([FromQuery] int fontId, [FromQuery] int configId,
            [FromQuery] string template)
        {
            string source, header;
            try
            {
                source = await Task.Run(() => _service.ConvertFont(fontId, template));
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

            var response = new DoConvertionResponse
            {
                Body = source,
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost, Route("convertions")]
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
                newId = await Task.Run(() => _service.AddConvertion(newConvertion));
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

            var response = new SaveConvertionResponse
            {
                Id = newId,
            };

            return Ok(response);
        }
    }
}
