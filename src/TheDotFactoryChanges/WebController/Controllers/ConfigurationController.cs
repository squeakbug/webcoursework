using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

using WebControllers.Models;
using DataAccessInterface;
using Presenter;
using AuthService;

namespace WebControllers.Controllers
{
    [Route("api/{basePath}")]
    public class ConfigurationController : ControllerBase
    {
        private IConverterService _service;
        private IAuthService _authService;
        private ILogger _logger;

        public ConfigurationController(IConverterService service, IAuthService authService,
            ILogger<ConfigurationController> logger)
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
        [HttpGet, Route("configurationNames")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<ConfigurationNameDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfigurationNames()
        {
            ICollection<ConfigurationNameDTO> configNames = new List<ConfigurationNameDTO>();
            IEnumerable<Configuration> configs;
            try
            {
                configs = _service.GetConfigurations();
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500);
            }
            foreach (var item in configs)
                configNames.Add(new ConfigurationNameDTO { Name = item.displayName });
            return Ok(configNames);
        }

        [Authorize]
        [HttpGet, Route("configurations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<ConfigurationDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfigurationsAll()
        {
            ICollection<ConfigurationDTO> configsDTO = new List<ConfigurationDTO>();
            IEnumerable<Configuration> configs;
            try
            {
                configs = _service.GetConfigurations();
            }
            catch (ApplicationException)
            {
                return StatusCode(500);
            }
            foreach (var item in configs)
            {
                try
                {
                    ConfigurationDTO dto = ConfigToConfigDTO(item);
                    configsDTO.Add(dto);
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    return StatusCode(500);
                }
            }
            return Ok(configsDTO);
        }

        [Authorize]
        [HttpPost, Route("configurations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfigurationsPOST([FromBody] IEnumerable<ConfigurationDTO> body)
        {
            foreach (var item in body)
            {
                try
                {
                    _service.CreateConfig(ConfigFromConfigDTO(item));
                }
                catch (ApplicationException ex)
                {
                    _logger.Log(LogLevel.Error, ex.Message);
                    return StatusCode(500);
                }
            }
            return Ok();
        }

        [Authorize]
        [HttpGet, Route("configurations/{configurationId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConfigurationDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfigurationsGET(long configurationId)
        {
            return Ok(ConfigToConfigDTO(_service.GetConfigById((int)configurationId)));
        }

        [Authorize]
        [HttpPut, Route("configurations/{configurationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfigurationsPUT(long configurationId,
            [FromBody] ConfigurationDTO body)
        {
            try
            {
                _service.UpdateConfig(ConfigFromConfigDTO(body));
            }
            catch (ApplicationException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return StatusCode(500);
            }

            return Ok();
        }

        /// <summary>
        /// Delete configuration by id
        /// </summary>
        /// <remarks>
        /// Delete configuration by id
        /// </remarks>
        /// <returns>Successful operation</returns>
        [Authorize]
        [HttpDelete, Route("configurations/{configurationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfigurationsDELETE(long configurationId)
        {
            try
            {
                _service.DeleteConfig((int)configurationId);
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

        private ConfigurationPaddingRemovalVertical PaddingRemovalVertToDTO(PaddingRemoval paddingRemoval)
        {
            switch (paddingRemoval)
            {
                case PaddingRemoval.Fixed:
                    return ConfigurationPaddingRemovalVertical.Fixed;
                case PaddingRemoval.None:
                    return ConfigurationPaddingRemovalVertical.None;
                case PaddingRemoval.Tighest:
                    return ConfigurationPaddingRemovalVertical.Tighest;
            }
            throw new ApplicationException("no such padding removal");
        }

        private ConfigurationRotation RotationToDTO(Rotation rotation)
        {
            switch (rotation)
            {
                case Rotation.RotateZero:
                    return ConfigurationRotation.RotateZero;
                case Rotation.RotateNinety:
                    return ConfigurationRotation.RotateNinety;
                case Rotation.RotateOneEighty:
                    return ConfigurationRotation.RotateOneEighty;
                case Rotation.RotateTwoSeventy:
                    return ConfigurationRotation.RotateTwoSeventy;
            }
            throw new ApplicationException("no such padding removal");
        }

        private ConfigurationDTO ConfigToConfigDTO(Configuration config)
        {
            return new ConfigurationDTO
            {
                BitLayout = config.bitLayout == BitLayout.ColumnMajor ? ConfigurationBitLayout.ColumnMajor : ConfigurationBitLayout.RowMajor,
                BmpVisualizerChar = config.bmpVisualizerChar,
                ByteFormat = config.byteFormat == ByteFormat.Binary ? ConfigurationByteFormat.Binary : ConfigurationByteFormat.Hex,
                ByteOrder = config.byteOrder == ByteOrder.LsbFirst ? ConfigurationByteOrder.LsbFirst : ConfigurationByteOrder.MsbFirst,
                CommentBlockEndString = config.commentBlockEndString,
                CommentBlockMiddleString = config.commentBlockMiddleString,
                CommentCharDescriptor = config.commentCharDescriptor,
                CommentCharVisualizer = config.commentCharVisualizer,
                CommentEndString = config.commentEndString,
                CommentStartString = config.commentStartString,
                CommentStyle = config.commentStyle == CommentStyle.C ? ConfigurationCommentStyle.C : ConfigurationCommentStyle.Cpp,
                CommentVariableName = config.commentVariableName,
                DisplayName = config.displayName,
                FlipHorizontal = config.flipHorizontal,
                FlipVertical = config.flipVertical,
                GenerateLookupArray = config.generateLookupArray,
                GenerateSpaceCharacterBitmap = config.generateSpaceCharacterBitmap,
                Id = config.Id,
                MinHeight = config.minHeight,
                PaddingRemovalHorizontal = PaddingRemovalHorizToDTO(config.paddingRemovalHorizontal),
                PaddingRemovalVertical = PaddingRemovalVertToDTO(config.paddingRemovalVertical),
                Rotation = RotationToDTO(config.rotation),
                SpaceGenerationPixels = config.spaceGenerationPixels,
                UserId = config.UserId,
                VarNfBitmaps = config.varNfBitmaps,
                VarNfCharInfo = config.varNfCharInfo,
                VarNfFontInfo = config.varNfFontInfo,
                VarNfHeight = config.varNfHeight,
                VarNfWidth = config.varNfWidth,
            };
        }

        private PaddingRemoval PaddingRemovalHorizFromDTO(ConfigurationPaddingRemovalHorizontal paddingRemoval)
        {
            switch (paddingRemoval)
            {
                case ConfigurationPaddingRemovalHorizontal.Fixed:
                    return PaddingRemoval.Fixed;
                case ConfigurationPaddingRemovalHorizontal.None:
                    return PaddingRemoval.None;
                case ConfigurationPaddingRemovalHorizontal.Tighest:
                    return PaddingRemoval.Tighest;
            }
            throw new ApplicationException("no such padding removal");
        }

        private PaddingRemoval PaddingRemovalVertFromDTO(ConfigurationPaddingRemovalVertical paddingRemoval)
        {
            switch (paddingRemoval)
            {
                case ConfigurationPaddingRemovalVertical.Fixed:
                    return PaddingRemoval.Fixed;
                case ConfigurationPaddingRemovalVertical.None:
                    return PaddingRemoval.None;
                case ConfigurationPaddingRemovalVertical.Tighest:
                    return PaddingRemoval.Tighest;
            }
            throw new ApplicationException("no such padding removal");
        }

        private Rotation RotationFromDTO(ConfigurationRotation rotation)
        {
            switch (rotation)
            {
                case ConfigurationRotation.RotateZero:
                    return Rotation.RotateZero;
                case ConfigurationRotation.RotateNinety:
                    return Rotation.RotateNinety;
                case ConfigurationRotation.RotateOneEighty:
                    return Rotation.RotateOneEighty;
                case ConfigurationRotation.RotateTwoSeventy:
                    return Rotation.RotateTwoSeventy;
            }
            throw new ApplicationException("no such padding removal");
        }

        private Configuration ConfigFromConfigDTO(ConfigurationDTO config)
        {
            return new Configuration
            {
                bitLayout = config.BitLayout == ConfigurationBitLayout.ColumnMajor ? BitLayout.ColumnMajor : BitLayout.RowMajor,
                bmpVisualizerChar = config.BmpVisualizerChar,
                byteFormat = config.ByteFormat == ConfigurationByteFormat.Binary ? ByteFormat.Binary : ByteFormat.Hex,
                byteOrder = config.ByteOrder == ConfigurationByteOrder.LsbFirst ? ByteOrder.LsbFirst : ByteOrder.MsbFirst,
                commentBlockEndString = config.CommentBlockEndString,
                commentBlockMiddleString = config.CommentBlockMiddleString,
                commentCharDescriptor = config.CommentCharDescriptor,
                commentCharVisualizer = config.CommentCharVisualizer,
                commentEndString = config.CommentEndString,
                commentStartString = config.CommentStartString,
                commentStyle = config.CommentStyle == ConfigurationCommentStyle.C ? CommentStyle.C : CommentStyle.Cpp,
                commentVariableName = config.CommentVariableName,
                displayName = config.DisplayName,
                flipHorizontal = config.FlipHorizontal,
                flipVertical = config.FlipVertical,
                generateLookupArray = config.GenerateLookupArray,
                generateSpaceCharacterBitmap = config.GenerateSpaceCharacterBitmap,
                Id = (int)config.Id,
                minHeight = (int)config.MinHeight,
                paddingRemovalHorizontal = PaddingRemovalHorizFromDTO(config.PaddingRemovalHorizontal),
                paddingRemovalVertical = PaddingRemovalVertFromDTO(config.PaddingRemovalVertical),
                rotation = RotationFromDTO(config.Rotation),
                spaceGenerationPixels = (int)config.SpaceGenerationPixels,
                UserId = (int)config.UserId,
                varNfBitmaps = config.VarNfBitmaps,
                varNfCharInfo = config.VarNfCharInfo,
                varNfFontInfo = config.VarNfFontInfo,
                varNfHeight = config.VarNfHeight,
                varNfWidth = config.VarNfWidth,
            };
        }

        private ConfigurationPaddingRemovalHorizontal PaddingRemovalHorizToDTO(PaddingRemoval paddingRemoval)
        {
            switch (paddingRemoval)
            {
                case PaddingRemoval.Fixed:
                    return ConfigurationPaddingRemovalHorizontal.Fixed;
                case PaddingRemoval.None:
                    return ConfigurationPaddingRemovalHorizontal.None;
                case PaddingRemoval.Tighest:
                    return ConfigurationPaddingRemovalHorizontal.Tighest;
            }
            throw new ApplicationException("no such padding removal");
        }
    }
}
