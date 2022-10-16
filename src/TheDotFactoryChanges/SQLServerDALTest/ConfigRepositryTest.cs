using DataAccessInterface;
using DataAccessSQLServer;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace SqlServerDALTest
{
    public class ConfigConverterTest
    {
        [Test]
        public void CreateConfigTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cfgDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserConfig>>();
            var cfgSetMock = new Mock<Context>(options);
            cfgSetMock.Setup(c => c.UserConfig)
                      .Returns(cfgDbSetMock.Object);
            cfgSetMock.Setup(set =>
                set.UserConfig.Add(It.IsAny<DataAccessSQLServer.UserConfig>()));
            cfgSetMock.Setup(set =>
                set.SaveChanges());
            var sut = new ConfigRepository(cfgSetMock.Object);
            var cfg = GetUserConfigSample()[0];

            sut.Create(cfg);

            cfgSetMock.Verify(set =>
                set.UserConfig.Add(It.Is<DataAccessSQLServer.UserConfig>(cfg =>
                    cfg.DisplayName == cfg.DisplayName)));
            cfgSetMock.Verify(set =>
                set.SaveChanges());
        }

        [Test]
        public void GetConfigByIdTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cfgDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserConfig>>();
            var cfgSetMock = new Mock<Context>(options);
            var cfg = GetDALUserConfigSample()[0];
            cfgSetMock.Setup(c => c.UserConfig)
                      .Returns(cfgDbSetMock.Object);
            cfgSetMock.Setup(set =>
                set.UserConfig.Find(It.IsAny<int>()))
                      .Returns(cfg);
            var sut = new ConfigRepository(cfgSetMock.Object);

            var retcfg = sut.GetConfigurationById(cfg.Id);

            cfgSetMock.Verify(set =>
                set.UserConfig.Find(It.Is<int>(id =>
                    id == cfg.Id)));
        }

        [Test]
        public void GetConfigsTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cfgDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserConfig>>();
            var cfgSetMock = new Mock<Context>(options);
            cfgSetMock.Setup(c => c.UserConfig)
                       .Returns(cfgDbSetMock.Object);
            var cfgs = GetDALUserConfigSample();
            cfgSetMock.Setup(c => c.GetUserConfigSet())
                       .Returns(cfgs.AsQueryable());
            var sut = new ConfigRepository(cfgSetMock.Object);

            var retcfgs = sut.GetConfigurations();

            cfgSetMock.Verify(set => set.GetUserConfigSet());
            Assert.NotNull(retcfgs);
            Assert.AreEqual(3, retcfgs.Count());
        }

        [Test]
        public void UpdateConfigTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cfgDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserConfig>>();
            var cfgSetMock = new Mock<Context>(options);
            cfgSetMock.Setup(c => c.UserConfig)
                       .Returns(cfgDbSetMock.Object);
            var cfg = GetUserConfigSample()[0];
            var sut = new ConfigRepository(cfgSetMock.Object);

            sut.Update(cfg);

            cfgSetMock.Verify(set => set.UserConfig);
        }

        [Test]
        public void DeleteConfigTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cfgDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserConfig>>();
            var cfgSetMock = new Mock<Context>(options);
            var cfg = GetDALUserConfigSample()[0];
            cfgSetMock.Setup(c => c.UserConfig)
                      .Returns(cfgDbSetMock.Object);
            cfgSetMock.Setup(set =>
                set.UserConfig.Find(It.IsAny<int>()))
                      .Returns(cfg);
            cfgSetMock.Setup(set =>
                set.UserConfig.Remove(It.IsAny<DataAccessSQLServer.UserConfig>()));
            var sut = new ConfigRepository(cfgSetMock.Object);

            sut.Delete(cfg.Id);

            cfgSetMock.Verify(set =>
                set.UserConfig.Remove(It.IsAny<DataAccessSQLServer.UserConfig>()));
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

        private List<DataAccessInterface.Configuration> GetUserConfigSample()
        {
            return new List<DataAccessInterface.Configuration>()
            {
                new DataAccessInterface.Configuration
                {
                    Id = 1,
                    displayName = "name_001",
                },
                new DataAccessInterface.Configuration
                {
                    Id = 2,
                    displayName = "name_002",
                },
                new DataAccessInterface.Configuration
                {
                    Id = 3,
                    displayName = "name_003",
                }
            };
        }
    }
}