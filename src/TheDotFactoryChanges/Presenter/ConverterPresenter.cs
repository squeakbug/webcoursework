using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

using DataAccessInterface;

namespace Presenter
{
    public class ConverterPresenter : BasePresenter<IConverterView>
    {
        private IConverterService _service;

        private string _inputText = "";
        private string _fontName = "";
        private string _PEGfontName = "";

        private string _txtImagePath = "";
        private string _txtImageName = "";

        private string _outputSourceText = "";
        private string _outputHeaderText = "";

        private int _curTabNameIndx = 0;

        private int _curCfgIndx = 0;
        private List<int> _configurations = new List<int>();

        private int _curInputVariantIndx = 0;
        private List<string> _inputTextVariants = new List<string>();

        private Bitmap _currentLoadedBitmap;
        private string _imageName;

        public string NewLineCh { get; set; } = "\n";

        // === Constructors ===

        public ConverterPresenter(IApplicationController controller, IConverterView view, IConverterService service)
            : base(controller, view)
        {
            _service = service;

            View.FontSelected += UpdateSelectedFont;
            View.GenPEGBtnClicked += GenPEGBtnClickedHandler;
            View.GenerateBtnClicked += GenerateBtnClickedHandler;
            View.OutputConfigBtnClicked += OutputBtnCickedHandler;
            View.CurrentConfigChanged += _view_CurrentConfigChanged;
            View.TextInsertedBtnClicked += InsertTextToInputText;
            View.BitmapLoadBtnClicked += LoadBitmap;
            View.SaveAsClicked += SaveAsBtnClickedHandler;
            View.DialogSaveClicked += DialogSaveClickedHandler;
            View.FormLoad += ConfigureForm;
            View.TextInsertionChanged += _view_TextInsertionChanged;
            View.tcInputIndexChanged += _view_tcInputIndexChanged;
            View.txtInputTextChanged += _view_txtInputText_TextChanged;
            View.ImageNameChanged += _view_ImageNameChanged;
            View.txtBoxPEGFontNameChanged += _view_txtBoxPEGFontNameChanged;

            _service.FontChanged += FontChangedHandler;
            _service.OutputHeaderTextChanged += OutputHeaderTextChangedHandler;
            _service.OutputSourceTextChanged += OutputSourceTextChangedHandler;
            _service.ConfigsUpdated += _service_ConfigsUpdated;
        }

        // === Event handlers ===

        public async void ConfigureForm()
        {
            IEnumerable<Configuration> configurations = await _service.GetConfigurations();
            _configurations = new List<int>();
            var display_names = new List<string>();
            foreach (var item in configurations)
            {
                _configurations.Add(item.Id);
                display_names.Add(item.displayName);
            }
            View.SetConfigurations(display_names);
            View.SetCurrentConfiguration(0);

            Configuration cfg = await _service.GetCurrentConfig();
            string appVer = String.Format("The Dot Factory v.{0}", Configuration.ApplicationVersion);
            View.SetApplicationVersion(appVer);
            View.SetNewlineChar("\n");

            UpdateTextInsertions();
        }
        public void FontChangedHandler(System.Drawing.Font font)
        {
            UpdateInputFont(font);
        }
        public void OutputBtnCickedHandler()
        {
            Controller.Run<SettingsPresenter>();
        }
        public void SaveAsBtnClickedHandler()
        {
            View.SetSaveFileDialogFileName(String.Format("Font{0}", _PEGfontName));
        }
        public void DialogSaveClickedHandler(string filename)
        {
            File.WriteAllText(_outputSourceText, String.Format("{0}.c", filename));
            File.WriteAllText(_outputHeaderText, String.Format("{0}.h", filename));
        }
        public void GenerateBtnClickedHandler()
        {
            try
            {
                _service.ConvertFont(false);
            }
            catch (Exception ex)
            {
                View.ShowErrorMessage("Ошибка", ex.Message);
            }
        }
        private void GenPEGBtnClickedHandler()
        {
            try
            {
                _service.ConvertFont(true);
            }
            catch (Exception ex)
            {
                View.ShowErrorMessage("Ошибка", ex.Message);
            }
        }
        private void OutputHeaderTextChangedHandler(string text)
        {
            _outputHeaderText = text;
            View.SetHeaderFieldText(text);
        }
        private void OutputSourceTextChangedHandler(string text)
        {
            _outputSourceText = text;
            View.SetSourceFieldText(text);
        }
        private void _view_CurrentConfigChanged(int indx)
        {
            int cfg_id = _configurations[indx];
            _service.SetCurrentConfig(cfg_id);
        }
        private void InsertTextToInputText()
        {
            _inputText += _service.GetTextInsertion(_inputTextVariants[_curInputVariantIndx]);
            _service.SetInputText(_inputText);
            View.SetInputTextEntryText(_inputText);
        }
        private void _view_TextInsertionChanged(int newInsertionIndx)
        {
            _curInputVariantIndx = newInsertionIndx;
        }
        private void _view_tcInputIndexChanged(int indx)
        {
            _curTabNameIndx = indx;
            if (indx == 0)
            {
                _service.UpdateTabState(TabState.Text);
            }
            else
            {
                _service.UpdateTabState(TabState.Image);
            }
        }
        private void _view_txtInputText_TextChanged(string str)
        {
            _inputText = str;
            _service.SetInputText(_inputText);
        }
        private void _view_ImageNameChanged(string str)
        {
            _imageName = str;
        }
        private async void _service_ConfigsUpdated()
        {
            IEnumerable<Configuration> configs = await _service.GetConfigurations();
            var displayNames = new List<string>();
            _configurations.Clear();
            foreach (var item in configs)
            {
                displayNames.Add(item.displayName);
                _configurations.Add(item.Id);
            }
            View.SetConfigurations(displayNames);
            View.SetCurrentConfiguration(_curCfgIndx);
        }
        private void _view_txtBoxPEGFontNameChanged(string str)
        {
            _PEGfontName = str;
            _service.SetPEGFontName(str);
        }

        // === public ===

        // === private ===

        private void UpdateTextInsertions()
        {
            _inputTextVariants = _service.GetTextInsertions();
            View.SetTextInsertions(_inputTextVariants);
        }
        private void UpdateInputFont(System.Drawing.Font font)
        {
            string inputFontText = "";

            inputFontText += font.Name;
            inputFontText += " " + Math.Round(font.Size) + "pts";

            if (font.Bold)
                inputFontText += " / Bold";
            if (font.Italic)
                inputFontText += " / Italic";

            _fontName = inputFontText;
            View.SetInputFontEntryText(inputFontText);
            View.SetInputTextEntryFont(font);
        }
        private void LoadBitmap(string filename)
        {
            var bmp = new Bitmap(filename);
            _currentLoadedBitmap = bmp;
            View.SetLoadedBitmapImage(bmp);
            _txtImagePath = filename;
            _txtImageName = Path.GetFileNameWithoutExtension(filename);
            _service.UpdateBitmap(bmp);
        }
        private void UpdateSelectedFont(DataAccessInterface.Font fnt)
        {
            _service.UpdateFont(fnt);
        }
    }
}
