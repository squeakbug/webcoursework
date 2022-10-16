﻿using System;
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
            _bmp = bmp;
        }
        public void UpdateFont(System.Drawing.Font font)
        {
            _font = font;
            FontChanged.Invoke(_font);
        }
        public void UpdateTabState(TabState state)
        {
            _tabState = state;
        }

        public void SetInputText(string str)
        {
            _inputText = str;
        }
        public void SetPEGFontName(string str)
        {
            _PEGFontName = str;
        }
        public void ConvertFont(bool isPeg)
        {
            if (_font == null)
                throw new ApplicationException("Не выбран шрифт");

            if (_inputText.Length == 0) return;

            Configuration cfg = GetCurrentConfig();
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

                OutputSourceTextChanged.Invoke(_outputSourceText);
                OutputHeaderTextChanged.Invoke(_outputHeaderText);
            }
        }

        public void AddTextInsertion(string key, string charSet)
        {
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

        public void CreateConfig(Configuration cfg)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            int newId = cfgRepo.Create(cfg);

            ConfigAdded.Invoke(newId);
        }
        public void DeleteConfig(int id)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            cfgRepo.Delete(id);

            ConfigRemoved.Invoke(id);
        }
        public void UpdateConfig(Configuration cfg)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            cfgRepo.Update(cfg);

            ConfigUpdated.Invoke(cfg.Id);
        }
        public Configuration GetConfigById(int id)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            return cfgRepo.GetConfigurationById(id);
        }
        public Configuration GetCurrentConfig()
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            return cfgRepo.GetConfigurationById(_curConfigId);
        }
        public void SetCurrentConfig(int id)
        {
            _curConfigId = id;
        }
        public IEnumerable<Configuration> GetConfigurations()
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            return cfgRepo.GetConfigurations();
        }
        public void UpdateConfigurations(IEnumerable<Configuration> cfgs)
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            foreach (var cfg in cfgs)
            {
                cfgRepo.Update(cfg);
            }

            ConfigsUpdated.Invoke();
        }
        public Bitmap GetCurrentBitmap()
        {
            return _bmp;
        }
        public System.Drawing.Font GetCurrentFont()
        {
            return _font;
        }

        public IEnumerable<string> GetFontNames()
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var fonts = fontRepo.GetFonts();
            var fontNames = new List<string>();
            foreach (var item in fonts)
                fontNames.Add(item.Name);
            return fontNames;
        }

        public IEnumerable<Convertion> GetConvertions()
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return cvtRepo.GetConvertions();
        }

        public Convertion GetConvertionById(int id)
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return cvtRepo.GetConvertionById(id);
        }

        public int AddConvertion(Convertion cvt)
        {
            var cvtRepo = _repositoryFactory.CreateConvertionRepository();
            return cvtRepo.Create(cvt);
        }

        public string GetOutputSourceText()
        {
            return _outputSourceText;
        }

        public string GetOutputHeaderText()
        {
            return _outputHeaderText;
        }

        public void SetCurrentFont(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            var font = fontRepo.GetFontById(id);
            _font = new System.Drawing.Font(font.Name, font.Size);
        }

        public int AddFont(DataAccessInterface.Font font)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return fontRepo.Create(font);
        }

        public IEnumerable<DataAccessInterface.Font> GetFonts()
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return fontRepo.GetFonts();
        }

        public DataAccessInterface.Font GetFontById(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            return fontRepo.GetFontById(id);
        }
        public void UpdateFont(DataAccessInterface.Font font)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            fontRepo.Update(font);
        }

        public void DeleteFont(int id)
        {
            var fontRepo = _repositoryFactory.CreateFontRepository();
            fontRepo.Delete(id);
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
        private void SetupConfiguration()
        {
            var cfgRepo = _repositoryFactory.CreateConfigRepository();
            var configs = new List<Configuration>(cfgRepo.GetConfigurations());
            _curConfigId = -1;
            if (configs.Count == 0)
            {
                _curConfigId = cfgRepo.Create(new Configuration());
            }
        }
    }
}
