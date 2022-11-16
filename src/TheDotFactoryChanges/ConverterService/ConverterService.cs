using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

using Presenter;
using DataAccessInterface;

namespace Service
{
    public class ConverterService : IConverterService
    {
        private Bitmap _bmp;
        private System.Drawing.Font _font;
        private TabState _tabState;
        private string _inputText;
        private string _PEGFontName;

        private string _curTextInsertion;
        private Dictionary<string, string> _textInsertions;

        private int _curConfigId = 0;

        private string _outputSourceText;
        private string _outputHeaderText;

        private IRepositoryFactory _repositoryFactory;

        public event Action<System.Drawing.Font> FontChanged;
        public event Action<string> OutputSourceTextChanged;
        public event Action<string> OutputHeaderTextChanged;
        public event Action<int> ConfigRemoved;
        public event Action<int> ConfigAdded;
        public event Action<int> ConfigUpdated;
        public event Action ConfigsUpdated;

        private string _imageName;

        private ITextRenderer _textRenderer;

        // === Constructors ===

        public ConverterService(IRepositoryFactory repositoryFactory, ITextRenderer textRenderer)
        {
            _bmp = null;
            _font = null;
            _tabState = TabState.Text;
            _inputText = "";
            _PEGFontName = "";

            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException("repository factory");
            _textRenderer = textRenderer ?? throw new ArgumentNullException("text renderer");

            SetupTextInsertions();
            SetupConfiguration();
        }

        // === public ====

        public void UpdateBitmap(Bitmap bmp)
        {
            if (bmp == null)
                throw new ArgumentNullException("bitmap");

            _bmp = bmp;
        }
        public void UpdateFont(System.Drawing.Font font)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            _font = font;
            FontChanged?.Invoke(_font);
        }
        public void UpdateTabState(TabState state)
        {
            _tabState = state;
        }

        public void SetInputText(string str)
        {
            if (str == null)
                throw new ArgumentNullException("input text");

            _inputText = str;
        }
        public void SetPEGFontName(string str)
        {
            if (str == null)
                throw new ArgumentNullException("null string");

            _PEGFontName = str;
        }
        public async Task ConvertFont(bool isPeg)
        {
            if (_font == null)
                throw new ClientErrorException("font not selected");

            if (_inputText.Length == 0) return;

            Configuration cfg = await GetCurrentConfig();
            if (cfg == null)
                throw new ClientErrorException("no current configuration");

            if (_tabState == TabState.Image)
            {
                var visualizer = new ImageVisualizer(cfg);
                visualizer.GetDump(_bmp, _imageName, cfg.spaceGenerationPixels,
                    cfg.minHeight, out _outputSourceText, out _outputHeaderText);
            }
            else
            {
                if (isPeg)
                {
                    IFontVisualiser visualizer = new PEGFontVisualizer(cfg, _PEGFontName);
                    visualizer.GetDump(_font, _inputText, out _outputSourceText, out _outputHeaderText, _textRenderer);
                }
                else
                {
                    IFontVisualiser visualizer = new FontVisualizer(cfg);
                    visualizer.GetDump(_font, _inputText, out _outputSourceText, out _outputHeaderText, _textRenderer);
                }

                OutputSourceTextChanged?.Invoke(_outputSourceText);
                OutputHeaderTextChanged?.Invoke(_outputHeaderText);
            }
        }

        public void AddTextInsertion(string key, string charSet)
        {
            if (key == null || charSet == null)
                throw new ArgumentNullException("text insertion");

            _textInsertions.Add(key, charSet);
        }
        public void RemoveTextInsertion(string key)
        {
            _textInsertions.Remove(key);
        }
        public string GetTextInsertion(string key)
        {
            if (!_textInsertions.ContainsKey(key))
                throw new Exception("No such text insertion");

            return _textInsertions[key];
        }
        public List<string> GetTextInsertions()
        {
            var insertions = new List<string>();
            foreach (var item in _textInsertions)
                insertions.Add(item.Key);
            return insertions;
        }

        public void SetImageName(string str)
        {
            _imageName = str;
        }

        public async Task<int> CreateConfig(Configuration cfg)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            int newId = await cfgRepo.Create(cfg);

            ConfigAdded?.Invoke(newId);

            return newId;
        }
        public async Task DeleteConfig(int id)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            var cfg = await cfgRepo.GetConfigurationById(id);
            if (cfg == null)
                throw new NotFoundException($"nod config with id = {id}");

            cfgRepo = _repositoryFactory.CreateConfigRepository();
            await cfgRepo.Delete(id);

            ConfigRemoved?.Invoke(id);
        }
        public async Task UpdateConfig(Configuration cfg)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            var dbCfg = await cfgRepo.GetConfigurationById(cfg.Id);
            if (dbCfg == null)
                throw new NotFoundException("no cfg with such id");

            cfgRepo = _repositoryFactory.CreateConfigRepository();
            await cfgRepo.Update(cfg);

            ConfigUpdated?.Invoke(cfg.Id);
        }
        public async Task<Configuration> GetConfigById(int id)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            return await cfgRepo.GetConfigurationById(id);
        }
        public async Task<Configuration> GetCurrentConfig()
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            return await cfgRepo.GetConfigurationById(_curConfigId);
        }
        public async Task SetCurrentConfig(int id)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            var cfg = await cfgRepo.GetConfigurationById(id);
            if (cfg == null)
                throw new NotFoundException("current config");

            _curConfigId = id;
        }
        public async Task<IEnumerable<Configuration>> GetConfigurations()
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            return await cfgRepo.GetConfigurations();
        }
        public async Task UpdateConfigurations(IEnumerable<Configuration> cfgs)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            foreach (var cfg in cfgs)
            {
                await cfgRepo.Update(cfg);
            }

            ConfigsUpdated?.Invoke();
        }
        public Bitmap GetCurrentBitmap()
        {
            return _bmp;
        }
        public System.Drawing.Font GetCurrentFont()
        {
            return _font;
        }

        public async Task<IEnumerable<string>> GetFontNames()
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var fonts = await fontRepo.GetFonts();
            var fontNames = new List<string>();
            foreach (var item in fonts)
                fontNames.Add(item.Name);
            return fontNames;
        }

        public async Task<IEnumerable<Convertion>> GetConvertions()
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return await cvtRepo.GetConvertions();
        }

        public async Task<Convertion> GetConvertionById(int id)
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return await cvtRepo.GetConvertionById(id);
        }

        public async Task<int> AddConvertion(Convertion cvt)
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return await cvtRepo.Create(cvt);
        }

        public string GetOutputSourceText()
        {
            return _outputSourceText;
        }

        public string GetOutputHeaderText()
        {
            return _outputHeaderText;
        }

        public async Task SetCurrentFont(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var font = await fontRepo.GetFontById(id);
            if (font == null)
                throw new NotFoundException("no font with such id");

            _font = new System.Drawing.Font(font.Name, font.Size);
        }

        public async Task<int> AddFont(DataAccessInterface.Font font)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var sysFont = new System.Drawing.Font(font.Name, font.Size);
            if ((sysFont.Name == "Microsoft Sans Serif" && font.Name != "Microsoft Sans Serif")
                || (sysFont.Name == "DejaVu Sans" && font.Name != "DejaVu Sans"))
            {
                throw new ApplicationException("no such font in windows catalog");
            }

            return await fontRepo.Create(font);
        }

        public async Task<IEnumerable<DataAccessInterface.Font>> GetFonts()
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return await fontRepo.GetFonts();
        }

        public async Task<DataAccessInterface.Font> GetFontById(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return await fontRepo.GetFontById(id);
        }

        public async Task UpdateFont(DataAccessInterface.Font font)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var tmp = await fontRepo.GetFontById(font.Id);
            if (tmp == null)
                throw new NotFoundException("no font with such id");

            fontRepo = _repositoryFactory.CreateFontRepository();
            await fontRepo.Update(font);
        }

        public async Task DeleteFont(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var font = await fontRepo.GetFontById(id);
            if (font == null)
                throw new NotFoundException("no font with such id");

            fontRepo = _repositoryFactory.CreateFontRepository();
            await fontRepo.Delete(id);
        }

        // === private ===

        private void SetupTextInsertions()
        {
            _textInsertions = new Dictionary<string, string>();
            string all = "", numbers = "", letters = "", uppercaseLetters = "", lowercaseLetters = "", symbols = "";

            for (char character = ' '; character < 127; ++character)
            {
                all += character;

                if (Char.IsNumber(character))
                {
                    numbers += character;
                }
                else if (Char.IsSymbol(character))
                {
                    symbols += character;
                }
                else if (Char.IsLetter(character) && Char.IsLower(character))
                {
                    letters += character;
                    lowercaseLetters += character;
                }
                else if (Char.IsLetter(character) && !Char.IsLower(character))
                {
                    letters += character;
                    uppercaseLetters += character;
                }
            }

            _textInsertions.Add("All", all);
            _textInsertions.Add("Numbers (0-9)", numbers);
            _textInsertions.Add("Letters (A-z)", letters);
            _textInsertions.Add("Lowercase letters (a-z)", lowercaseLetters);
            _textInsertions.Add("Uppercase letters (A-Z)", uppercaseLetters);

            _curTextInsertion = "All";
        }
        private async Task SetupConfiguration()
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            var configs = new List<Configuration>(await cfgRepo.GetConfigurations());
            _curConfigId = -1;
            if (configs.Count == 0)
            {
                _curConfigId = await cfgRepo.Create(new Configuration());
            }
            else
            {
                _curConfigId = cfgRepo.GetFirstOrDefaultConfig().Id;
            }
        }
    }
}
