using Moq;
using NUnit.Framework;

using Service;
using DataAccessInterface;
using NUnit.Allure.Core;

namespace ConverterServiceTest
{
    [AllureNUnit]
    public class ConverterServiceTest
    {
        public void UpdateBitmapNullArgumentTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            TestDelegate del = () => sut.UpdateBitmap(null);

            Assert.Throws<ArgumentNullException>(del);
        }

        [Test]
        public void SetInputTextNullArgumentTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            TestDelegate del = () => sut.SetInputText(null);

            Assert.Throws<ArgumentNullException>(del);
        }

        [Test]
        public void SetPegFontTextNullArgumentTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            TestDelegate del = () => sut.SetPEGFontName(null);

            Assert.Throws<ArgumentNullException>(del);
        }

        [Test]
        public void ConvertFontNotSelectedFontTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetConfigurationById(cfg.Id))
                   .Returns(cfg);
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            TestDelegate del = () => sut.ConvertFont(false);

            Assert.Throws<ClientErrorException>(del);
        }

        [Test]
        public void ConvertFontNotSelectedConfigurationTest()
        {
            var font = GetFontSample()[0];
            var fontRepo = new Mock<IFontRepository>();
            fontRepo.Setup(r => r.GetFontById(font.Id))
                    .Returns(font);
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(new List<Configuration>());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetConfigurationById(cfg.Id))
                   .Returns((Configuration)null);
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns((Configuration)null);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            repoFactory.Setup(r => r.CreateFontRepository())
                       .Returns(fontRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);
            sut.SetCurrentFont(font.Id);
            sut.SetInputText("1234567890");

            TestDelegate del = () => sut.ConvertFont(false);

            fontRepo.Verify(r => r.GetFontById(font.Id));
            Assert.Throws<ClientErrorException>(del);
        }

        [Test]
        public void ConvertFontImageTest()
        {

        }

        [Test]
        public void ConvertFontPegFontTest()
        {

        }

        [Test]
        public void ConvertFontNotPegFontTest()
        {

        }

        [Test]
        public void CreateConfigTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            cfgRepo.Setup(r => r.Create(cfg))
                   .Returns(cfg.Id);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);
            sut.ConfigAdded += (c) => { };

            var id = sut.CreateConfig(cfg);

            Assert.AreEqual(cfg.Id, id);
            cfgRepo.Verify(r => r.Create(cfg));
        }

        [Test]
        public void DeleteConfigTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            cfgRepo.Setup(r => r.GetConfigurationById(cfg.Id))
                   .Returns(cfg);
            cfgRepo.Setup(r => r.Delete(cfg.Id));
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);
            sut.ConfigRemoved += (c) => { };

            sut.DeleteConfig(cfg.Id);

            cfgRepo.Verify(r => r.Delete(cfg.Id));
        }

        [Test]
        public void GetConfigByIdTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            cfgRepo.Setup(r => r.GetConfigurationById(cfg.Id))
                   .Returns(cfg);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resCfg = sut.GetConfigById(cfg.Id);

            Assert.AreEqual(cfg, resCfg);
        }

        [Test]
        public void SetCurrentConfigTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            cfgRepo.Setup(r => r.GetConfigurationById(cfg.Id))
                   .Returns(cfg);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            TestDelegate del = () => sut.SetCurrentConfig(-100);

            Assert.Throws<NotFoundException>(del);
        }

        [Test]
        public void GetConfigurationsTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var cfgs = sut.GetConfigurations();

            cfgRepo.Verify(r => r.GetConfigurations());
        }

        [Test]
        public void UpdateConfigurationsNoWithIdTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetConfigurationById(cfg.Id))
                    .Returns(cfg);
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            cfgRepo.Setup(r => r.Update(cfg));
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);
            sut.ConfigUpdated += (c) => { };
            cfg.Id = -100;

            TestDelegate del = () => sut.UpdateConfig(cfg);

            Assert.Throws<NotFoundException>(del);
        }

        [Test]
        public void GetFontNamesTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var fonts = GetFontSample();
            fontRepo.Setup(r => r.GetFonts())
                    .Returns(fonts);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resFontNames = new List<string>(sut.GetFontNames());

            Assert.AreEqual("name_001", resFontNames[0]);
            Assert.AreEqual("name_002", resFontNames[1]);
            Assert.AreEqual("name_003", resFontNames[2]);
            fontRepo.Verify(r => r.GetFonts(), Times.Once());
        }

        [Test]
        public void GetConvertionsTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var cvtRepo = new Mock<IConvertionRepository>();
            var cvts = GetConvertionSample();
            cvtRepo.Setup(r => r.GetConvertions())
                   .Returns(cvts);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateConvertionRepository())
                       .Returns(cvtRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resCvts = sut.GetConvertions();

            cvtRepo.Verify(r => r.GetConvertions());
        }

        [Test]
        public void GetConvertionByIdTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var cvtRepo = new Mock<IConvertionRepository>();
            var cvt = GetConvertionSample()[0];
            cvtRepo.Setup(r => r.GetConvertionById(cvt.Id))
                   .Returns(cvt);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateConvertionRepository())
                       .Returns(cvtRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resCvt = sut.GetConvertionById(cvt.Id);

            cvtRepo.Verify(r => r.GetConvertionById(cvt.Id));
            Assert.AreEqual(cvt.Id, resCvt.Id);
        }

        [Test]
        public void AddConvertionTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var cvtRepo = new Mock<IConvertionRepository>();
            var cvt = GetConvertionSample()[0];
            cvtRepo.Setup(r => r.Create(cvt))
                   .Returns(cvt.Id);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateConvertionRepository())
                       .Returns(cvtRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            int id = sut.AddConvertion(cvt);

            cvtRepo.Verify(r => r.Create(cvt));
            Assert.AreEqual(cvt.Id, id);
        }

        [Test]
        public void SetCurrentFontTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var font = GetFontSample()[0];
            fontRepo.Setup(r => r.GetFontById(font.Id))
                    .Returns(font);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resFont = sut.GetFontById(font.Id);

            Assert.AreEqual(resFont, font);
            fontRepo.Verify(r => r.GetFontById(font.Id), Times.Once());
        }

        [Test]
        public void AddFontInWindowsTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var font = GetFontSample()[0];
            font.Name = "Arial";
            fontRepo.Setup(r => r.Create(font));
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            sut.AddFont(font);

            fontRepo.Setup(r => r.Create(font));
        }

        [Test]
        public void AddFontNoInWindowsTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var font = GetFontSample()[0];
            font.Name = "ha-ha look at this dude";
            fontRepo.Setup(r => r.Create(font));
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            TestDelegate del = () => sut.AddFont(font);

            Assert.Throws<ApplicationException>(del);
        }

        [Test]
        public void GetFontsTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var fonts = GetFontSample();
            fontRepo.Setup(r => r.GetFonts())
                    .Returns(fonts);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resFonts = sut.GetFonts();

            Assert.AreEqual(3, resFonts.Count());
            fontRepo.Verify(r => r.GetFonts(), Times.Once());
        }

        [Test]
        public void GetFontByIdTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var font = GetFontSample()[0];
            fontRepo.Setup(r => r.GetFontById(font.Id))
                    .Returns(font);
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            var resFont = sut.GetFontById(font.Id);

            Assert.AreEqual(resFont, font);
            fontRepo.Verify(r => r.GetFontById(font.Id), Times.Once());
        }

        [Test]
        public void UpdateFontNoUserWithIdTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var font = GetFontSample()[0];
            fontRepo.Setup(r => r.GetFontById(font.Id))
                    .Returns(font);
            fontRepo.Setup(r => r.Update(font));
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);
            font.Id = -100;

            TestDelegate del = () => sut.UpdateFont(font);

            Assert.Throws<NotFoundException>(del);
        }

        [Test]
        public void DeleteFontTest()
        {
            var cfgRepo = new Mock<IConfigRepository>();
            cfgRepo.Setup(r => r.GetConfigurations())
                   .Returns(GetUserConfigSample());
            var cfg = GetUserConfigSample()[0];
            cfgRepo.Setup(r => r.GetFirstOrDefaultConfig())
                   .Returns(cfg);
            var fontRepo = new Mock<IFontRepository>();
            var font = GetFontSample()[0];
            fontRepo.Setup(r => r.GetFontById(font.Id))
                    .Returns(font);
            fontRepo.Setup(r => r.Delete(font.Id));
            var repoFactory = new Mock<IRepositoryFactory>();
            repoFactory.Setup(f => f.CreateFontRepository())
                       .Returns(fontRepo.Object);
            repoFactory.Setup(r => r.CreateConfigRepository())
                       .Returns(cfgRepo.Object);
            var textRenderer = new Mock<ITextRenderer>();
            var sut = new ConverterService(repoFactory.Object, textRenderer.Object);

            sut.DeleteFont(font.Id);

            fontRepo.Verify(r => r.GetFontById(font.Id));
            fontRepo.Verify(r => r.Delete(font.Id));
        }



        private List<DataAccessInterface.Convertion> GetConvertionSample()
        {
            return new List<DataAccessInterface.Convertion>()
            {
                new DataAccessInterface.Convertion
                {
                    Id = 1,
                    Body = "body_001",
                    Head = "head_001",
                    Name = "name_001",
                },
                new DataAccessInterface.Convertion
                {
                    Id = 2,
                    Body = "body_002",
                    Head = "head_002",
                    Name = "name_002",
                },
                new DataAccessInterface.Convertion
                {
                    Id = 3,
                    Body = "body_003",
                    Head = "head_003",
                    Name = "name_003",
                }
            };
        }
        private List<DataAccessInterface.Font> GetFontSample()
        {
            return new List<DataAccessInterface.Font>()
            {
                new DataAccessInterface.Font
                {
                    Id = 1,
                    Name = "name_001",
                    Size = 10,
                },
                new DataAccessInterface.Font
                {
                    Id = 2,
                    Name = "name_002",
                    Size = 10,
                },
                new DataAccessInterface.Font
                {
                    Id = 3,
                    Name = "name_003",
                    Size = 10,
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