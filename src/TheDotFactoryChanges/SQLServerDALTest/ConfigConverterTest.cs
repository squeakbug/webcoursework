using DataAccessInterface;
using DataAccessSQLServer;
using NUnit.Allure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerDALTest
{
    [AllureNUnit]
    public class ConfigConverterTest
    {
        [Test]
        public void MapToBusinessEntityTest()
        {
            var configDAL = GetDALUserConfigSample()[0];

            var config = ConfigConverter.MapToBusinessEntity(configDAL);

            var targetConfig = new DataAccessInterface.Configuration
            {
                Id = 1,
            };
            Assert.NotNull(config);
            Assert.True(config.Id.Equals(targetConfig.Id));
        }

        [Test]
        public void MapFromBusinessEntityTest()
        {
            var config = new DataAccessInterface.Configuration
            {
                Id = 1,
            };

            var configDAL = ConfigConverter.MapFromBusinessEntity(config);

            var targetConfig = new DataAccessSQLServer.UserConfig
            {
                Id = 1,
            };
            Assert.NotNull(configDAL);
            Assert.True(configDAL.Id.Equals(targetConfig.Id));
        }

        private List<DataAccessSQLServer.UserConfig> GetDALUserConfigSample()
        {
            return new List<DataAccessSQLServer.UserConfig>()
            {
                new DataAccessSQLServer.UserConfig
                {
                    Id = 1,
                    DisplayName = "name_001",
                    BitLayout = BitLayout.RowMajor.ToString(),
                    BmpVisualizerChar = "#",
                    ByteFormat = ByteFormat.Binary.ToString(),
                    ByteLeadingString = "0x",
                    ByteOrder = ByteOrder.MsbFirst.ToString(),
                    CommentBlockEndString = "//",
                    CommentBlockMiddleString = "//",
                    CommentCharDescriptor = 0,
                    CommentCharVisualizer = 0,
                    CommentEndString = "//",
                    CommentStartString = "//",
                    CommentStyle = CommentStyle.C.ToString(),
                    CommentVariableName = 0,
                    DescCharHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescCharWidth = DescriptorFormat.DontDisplay.ToString(),
                    DescFontHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescImgHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescImgWidth = DescriptorFormat.DontDisplay.ToString(),
                    FlipHorizontal = 0,
                    FlipVertical = 0,
                    GenerateLookupArray = 0,
                    GenerateLookupBlocks = 0,
                    GenerateSpaceCharacterBitmap = 0,
                    LineWrap = LineWrap.AtColumn.ToString(),
                    VarNfWidth = "s",
                    VarNfHeight = "s",
                    VarNfFontInfo = "s",
                    VarNfCharInfo = "s",
                    VarNfBitmaps = "s",
                    Ui_id = 0,
                    Rotation = Rotation.RotateOneEighty.ToString(),
                    MinHeight = 10,
                    SpaceGenerationPixels = 10,
                    PaddingRemovalVertical = PaddingRemoval.Tighest.ToString(),
                    PaddingRemovalHorizontal = PaddingRemoval.Tighest.ToString(),
                    LookupBlocksNewAfterCharCount = 10,
                },
                new DataAccessSQLServer.UserConfig
                {
                    Id = 2,
                    DisplayName = "name_002",
                    BitLayout = BitLayout.RowMajor.ToString(),
                    BmpVisualizerChar = "#",
                    ByteFormat = ByteFormat.Binary.ToString(),
                    ByteLeadingString = "0x",
                    ByteOrder = ByteOrder.MsbFirst.ToString(),
                    CommentBlockEndString = "//",
                    CommentBlockMiddleString = "//",
                    CommentCharDescriptor = 0,
                    CommentCharVisualizer = 0,
                    CommentEndString = "//",
                    CommentStartString = "//",
                    CommentStyle = CommentStyle.C.ToString(),
                    CommentVariableName = 0,
                    DescCharHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescCharWidth = DescriptorFormat.DontDisplay.ToString(),
                    DescFontHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescImgHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescImgWidth = DescriptorFormat.DontDisplay.ToString(),
                    FlipHorizontal = 0,
                    FlipVertical = 0,
                    GenerateLookupArray = 0,
                    GenerateLookupBlocks = 0,
                    GenerateSpaceCharacterBitmap = 0,
                    LineWrap = LineWrap.AtColumn.ToString(),
                    VarNfWidth = "s",
                    VarNfHeight = "s",
                    VarNfFontInfo = "s",
                    VarNfCharInfo = "s",
                    VarNfBitmaps = "s",
                    Ui_id = 0,
                    Rotation = Rotation.RotateOneEighty.ToString(),
                    MinHeight = 10,
                    SpaceGenerationPixels = 10,
                    PaddingRemovalVertical = PaddingRemoval.Tighest.ToString(),
                    PaddingRemovalHorizontal = PaddingRemoval.Tighest.ToString(),
                    LookupBlocksNewAfterCharCount = 10,
                },
                new DataAccessSQLServer.UserConfig
                {
                    Id = 3,
                    DisplayName = "name_003",
                    BitLayout = BitLayout.RowMajor.ToString(),
                    BmpVisualizerChar = "#",
                    ByteFormat = ByteFormat.Binary.ToString(),
                    ByteLeadingString = "0x",
                    ByteOrder = ByteOrder.MsbFirst.ToString(),
                    CommentBlockEndString = "//",
                    CommentBlockMiddleString = "//",
                    CommentCharDescriptor = 0,
                    CommentCharVisualizer = 0,
                    CommentEndString = "//",
                    CommentStartString = "//",
                    CommentStyle = CommentStyle.C.ToString(),
                    CommentVariableName = 0,
                    DescCharHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescCharWidth = DescriptorFormat.DontDisplay.ToString(),
                    DescFontHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescImgHeight = DescriptorFormat.DontDisplay.ToString(),
                    DescImgWidth = DescriptorFormat.DontDisplay.ToString(),
                    FlipHorizontal = 0,
                    FlipVertical = 0,
                    GenerateLookupArray = 0,
                    GenerateLookupBlocks = 0,
                    GenerateSpaceCharacterBitmap = 0,
                    LineWrap = LineWrap.AtColumn.ToString(),
                    VarNfWidth = "s",
                    VarNfHeight = "s",
                    VarNfFontInfo = "s",
                    VarNfCharInfo = "s",
                    VarNfBitmaps = "s",
                    Ui_id = 0,
                    Rotation = Rotation.RotateOneEighty.ToString(),
                    MinHeight = 10,
                    SpaceGenerationPixels = 10,
                    PaddingRemovalVertical = PaddingRemoval.Tighest.ToString(),
                    PaddingRemovalHorizontal = PaddingRemoval.Tighest.ToString(),
                    LookupBlocksNewAfterCharCount = 10,
                }
            };
        }
    }
}
