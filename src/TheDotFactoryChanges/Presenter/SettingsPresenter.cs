using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccessInterface;

namespace Presenter
{
    public class SettingsPresenter : BasePresenter<ISettingsView>
    {
        private IConverterService _service;

        private int _curCfgIndx;
        private List<Configuration> _configurations;

        private List<Rotation> _rotations;

        private List<PaddingRemoval> _paddingHorizValues;
        private List<PaddingRemoval> _paddingVertValues;

        private List<DescriptorFormat> _charWidthValues;
        private List<DescriptorFormat> _charHeightValues;
        private List<DescriptorFormat> _fontHeightValues;
        private List<DescriptorFormat> _imageWidthValues;
        private List<DescriptorFormat> _imageHeightValues;

        private List<CommentStyle> _commentStyles = new List<CommentStyle>();

        private List<BitLayout> _bitLayouts = new List<BitLayout>();
        private List<string> _byteLeadingChars = new List<string>();
        private List<ByteOrder> _byteOrders = new List<ByteOrder>();
        private List<ByteFormat> _byteFormats = new List<ByteFormat>();

        // === Constructors ===

        public SettingsPresenter(IApplicationController controller, ISettingsView view, IConverterService service)
            : base(controller, view)
        {
            _service = service;
            _configurations = new List<Configuration>();
            _rotations = new List<Rotation>();
            _paddingHorizValues = new List<PaddingRemoval>();
            _paddingVertValues = new List<PaddingRemoval>();
            _charWidthValues = new List<DescriptorFormat>();
            _charHeightValues = new List<DescriptorFormat>();
            _fontHeightValues = new List<DescriptorFormat>();
            _imageWidthValues = new List<DescriptorFormat>();
            _imageHeightValues = new List<DescriptorFormat>();

            View.Applied += _view_Applied;
            View.RemoveConfigBtnClicked += RemoveConfig;
            View.NewConfigSaveBtnClicked += SaveNewConfig;
            View.FormLoad += ConfigureForm;
            View.cbxOutputConfigurationsIndexChanged += ConfigurationChangedHandler;

            View.cbxFlipHorizCheckedChanged += _view_cbxFlipHorizCheckedChanged;
            View.cbxFlipVertCheckedChanged += _view_cbxFlipVertCheckedChanged;
            View.cbxRotationIndexChanged += _view_cbxRotationIndexChanged;

            View.cbxPaddingHorizIndexChanged += _view_cbxPaddingHorizIndexChanged;
            View.cbxPaddingVertIndexChanged += _view_cbxPaddingVertIndexChanged;

            View.rbnLineWrapAtBitmapClicked += _view_rbnLineWrapAtBitmapClicked;
            View.rbnLineWrapAtColumnClicked += _view_rbnLineWrapAtColumnClicked;

            View.cbxGenerateLookupArrayCheckedChanged += _view_cbxGenerateLookupArrayCheckedChanged;
            View.cbxCharWidthFormatIndexChanged += _view_cbxCharWidthFormatIndexChanged;
            View.cbxCharHeightFormatIndexChanged += _view_cbxCharHeightFormatIndexChanged;
            View.cbxFontHeightFormatIndexChanged += _view_cbxFontHeightFormatIndexChanged;
            View.cbxGenerateLookupBlocksCheckedChanged += _view_cbxGenerateLookupBlocksCheckedChanged;
            View.txtLookupBlocksNewAfterCharCountChanged += _view_txtLookupBlocksNewAfterCharCountChanged;
            View.cbxImgWidthFormatIndexChanged += _view_cbxImgWidthFormatIndexChanged;
            View.cbxImgHeightFormatIndexChanged += _view_cbxImgHeightFormatIndexChanged;

            View.cbxGenerateSpaceBitmapCheckedChanged += _view_cbxGenerateSpaceBitmapCheckedChanged;
            View.txtSpacePixelsChanged += _view_txtSpacePixelsChanged;
            View.txtMinHeighthanged += _view_txtMinHeighthanged;

            View.cbxCommentVarNameCheckedChanged += _view_cbxCommentVarNameCheckedChanged;
            View.cbxCommentCharVisualCheckedChanged += _view_cbxCommentCharVisualCheckedChanged;
            View.cbxCommentCharDescCheckedChanged += _view_cbxCommentCharDescCheckedChanged;
            View.txtBmpVisualizerCharTextChanged += _view_txtBmpVisualizerCharTextChanged;
            View.cbxCommentStyleIndexChanged += _view_cbxCommentStyleIndexChanged;

            View.txtVarNfBitmapsTextChanged += _view_txtVarNfBitmapsTextChanged;
            View.txtVarNfCharInfoTextChanged += _view_txtVarNfCharInfoTextChanged;
            View.txtVarNfFontInfoTextChanged += _view_txtVarNfFontInfoTextChanged;
            View.txtVarNfWidthTextChanged += _view_txtVarNfWidthTextChanged;
            View.txtVarNfHeightTextChanged += _view_txtVarNfHeightTextChanged;

            View.cbxBitLayoutIndexChanged += _view_cbxBitLayoutIndexChanged;
            View.cbxByteOrderIndexChanged += _view_cbxByteOrderIndexChanged;
            View.cbxByteFormatIndexChanged += _view_cbxByteFormatIndexChanged;
            View.cbxByteLeadingCharIndexChanged += _view_cbxByteLeadingCharIndexChanged;

            _service.ConfigRemoved += _service_ConfigRemoved;
            _service.ConfigAdded += _service_ConfigAdded;
            _service.ConfigUpdated += _service_ConfigUpdated;

            Setup();
        }

        // === Handlers ===

        // Flip Rotate
        private void _view_cbxFlipHorizCheckedChanged(CheckState cs)
        {
            if (_curCfgIndx == -1)
                return;

            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].flipHorizontal = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].flipHorizontal = false;
        }
        private void _view_cbxFlipVertCheckedChanged(CheckState cs)
        {
            if (_curCfgIndx == -1)
                return;

            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].flipVertical = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].flipVertical = false;
        }
        private void _view_cbxRotationIndexChanged(int indx)
        {
            if (indx < 0 || indx > _rotations.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].rotation = _rotations[indx];
        }

        // Padding removal
        private void _view_cbxPaddingHorizIndexChanged(int indx)
        {
            if (indx < 0 || indx > _paddingHorizValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].paddingRemovalHorizontal = _paddingHorizValues[indx];
        }
        private void _view_cbxPaddingVertIndexChanged(int indx)
        {
            if (indx < 0 || indx > _paddingVertValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].paddingRemovalVertical = _paddingVertValues[indx];
        }

        // Line wrap
        private void _view_rbnLineWrapAtColumnClicked()
        {
            _configurations[_curCfgIndx].lineWrap = LineWrap.AtColumn;
        }
        private void _view_rbnLineWrapAtBitmapClicked()
        {
            _configurations[_curCfgIndx].lineWrap = LineWrap.AtBitmap;
        }

        // Descriptors
        private void _view_cbxGenerateLookupBlocksCheckedChanged(CheckState cs)
        {
            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].generateLookupBlocks = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].generateLookupBlocks = false;
        }
        private void _view_cbxCharWidthFormatIndexChanged(int indx)
        {
            if (indx < 0 || indx > _charWidthValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].descCharWidth = _charWidthValues[indx];
        }
        private void _view_cbxCharHeightFormatIndexChanged(int indx)
        {
            if (indx < 0 || indx > _charHeightValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].descCharHeight = _charHeightValues[indx];
        }
        private void _view_cbxFontHeightFormatIndexChanged(int indx)
        {
            if (indx < 0 || indx > _fontHeightValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].descFontHeight = _fontHeightValues[indx];
        }
        private void _view_cbxGenerateLookupArrayCheckedChanged(CheckState cs)
        {
            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].generateLookupArray = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].generateLookupArray = false;
        }
        private void _view_txtLookupBlocksNewAfterCharCountChanged(string str)
        {
            int charCnt = 0;
            try
            {
                charCnt = int.Parse(str);
            }
            catch (Exception ex)
            {
                View.ShowErrorMessage("Error", ex.Message);
                return ;
            }

            _configurations[_curCfgIndx].lookupBlocksNewAfterCharCount = charCnt;
        }
        private void _view_cbxImgWidthFormatIndexChanged(int indx)
        {
            if (indx < 0 || indx > _imageWidthValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].descImgWidth = _imageWidthValues[indx];
        }
        private void _view_cbxImgHeightFormatIndexChanged(int indx)
        {
            if (indx < 0 || indx > _imageHeightValues.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].descImgHeight = _imageHeightValues[indx];
        }

        // Space char generation
        private void _view_cbxGenerateSpaceBitmapCheckedChanged(CheckState cs)
        {
            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].generateSpaceCharacterBitmap = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].generateSpaceCharacterBitmap = false;
        }
        private void _view_txtSpacePixelsChanged(string str)
        {
            int spacePixCnt = 0;
            try
            {
                spacePixCnt = int.Parse(str);
            }
            catch (Exception ex)
            {
                View.ShowErrorMessage("Error", ex.Message);
                return;
            }

            _configurations[_curCfgIndx].spaceGenerationPixels = spacePixCnt;
        }
        private void _view_txtMinHeighthanged(string str)
        {
            int minHeight = 0;
            try
            {
                minHeight = int.Parse(str);
            }
            catch (Exception ex)
            {
                View.ShowErrorMessage("Error", ex.Message);
                return;
            }

            _configurations[_curCfgIndx].minHeight = minHeight;
        }

        // Comments
        private void _view_cbxCommentVarNameCheckedChanged(CheckState cs)
        {
            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].commentVariableName = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].commentVariableName = false;
        }
        private void _view_cbxCommentCharVisualCheckedChanged(CheckState cs)
        {
            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].commentCharVisualizer = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].commentCharVisualizer = false;
        }
        private void _view_cbxCommentCharDescCheckedChanged(CheckState cs)
        {
            if (cs == CheckState.Checked)
                _configurations[_curCfgIndx].commentCharDescriptor = true;
            else if (cs == CheckState.Unchecked)
                _configurations[_curCfgIndx].commentCharDescriptor = false;
        }
        private void _view_txtBmpVisualizerCharTextChanged(string str)
        {
            _configurations[_curCfgIndx].bmpVisualizerChar = str;
        }
        private void _view_cbxCommentStyleIndexChanged(int indx)
        {
            if (indx < 0 || indx > _commentStyles.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].commentStyle = _commentStyles[indx];
        }

        // Variable name format
        private void _view_txtVarNfBitmapsTextChanged(string str)
        {
            _configurations[_curCfgIndx].varNfBitmaps = str;
        }
        private void _view_txtVarNfCharInfoTextChanged(string str)
        {
            _configurations[_curCfgIndx].varNfCharInfo = str;
        }
        private void _view_txtVarNfFontInfoTextChanged(string str)
        {
            _configurations[_curCfgIndx].varNfFontInfo = str;
        }
        private void _view_txtVarNfWidthTextChanged(string str)
        {
            _configurations[_curCfgIndx].varNfWidth = str;
        }
        private void _view_txtVarNfHeightTextChanged(string str)
        {
            _configurations[_curCfgIndx].varNfHeight = str;
        }

        // Byte
        private void _view_cbxByteLeadingCharIndexChanged(int indx)
        {
            if (indx < 0 || indx > _byteLeadingChars.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");
            _configurations[_curCfgIndx].byteLeadingString = _byteLeadingChars[indx];

            /*
            if (_byteLeadingChars[indx] == ByteLeadingStringHex)
            {
                if (_configurations[_curCfgIndx].byteFormat != ByteFormat.Hex)
                {
                    int newByteFormatIndx = _byteFormats.FindIndex((x) => x == ByteFormat.Hex);
                    View.SetByteFormatsSelectedIndx(newByteFormatIndx);
                }
            }
            else if (_byteLeadingChars[indx] == ByteLeadingStringBinary)
            {
                if (_configurations[_curCfgIndx].byteFormat != ByteFormat.Binary)
                {
                    int newByteFormatIndx = _byteFormats.FindIndex((x) => x == ByteFormat.Binary);
                    View.SetByteFormatsSelectedIndx(newByteFormatIndx);
                }
            }
            */
        }
        private void _view_cbxByteFormatIndexChanged(int indx)
        {
            if (indx < 0 || indx > _byteFormats.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].byteFormat = _byteFormats[indx];

            /*
            if (_byteFormats[indx] == ByteFormat.Hex)
            {
                if (_configurations[_curCfgIndx].byteLeadingString != ByteLeadingStringHex)
                {
                    int newByteLeadingCharsIndx = _byteLeadingChars.FindIndex((x) => x == ByteLeadingStringHex);
                    View.SetCbxByteLeadingCharSelectedIndx(newByteLeadingCharsIndx);
                }
            }
            else if (_byteFormats[indx] == ByteFormat.Binary)
            {
                if (_configurations[_curCfgIndx].byteLeadingString != ByteLeadingStringBinary)
                {
                    int newByteLeadingCharsIndx = _byteLeadingChars.FindIndex((x) => x == ByteLeadingStringBinary);
                    View.SetCbxByteLeadingCharSelectedIndx(newByteLeadingCharsIndx);
                }
            }
            */
        }
        private void _view_cbxByteOrderIndexChanged(int indx)
        {
            if (indx < 0 || indx > _byteFormats.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].byteFormat = _byteFormats[indx];
        }
        private void _view_cbxBitLayoutIndexChanged(int indx)
        {
            if (indx < 0 || indx > _bitLayouts.Count)
                View.ShowErrorMessage("Error", "Некорректный индекс");

            _configurations[_curCfgIndx].bitLayout = _bitLayouts[indx];
        }

        private void _view_Applied()
        {
            _service.UpdateConfigurations(_configurations);
            View.Close();
        }

        private void _service_ConfigRemoved(int id)
        {
            SyncConfigurations();
        }
        private void _service_ConfigAdded(int id)
        {
            SyncConfigurations();
        }
        private void _service_ConfigUpdated(int id)
        {
            SyncConfigurations();
        }

        public void ConfigurationChangedHandler(int indx)
        {
            if (indx != -1)
            {
                _curCfgIndx = indx;
                SyncViewWithConfig((Configuration)_configurations[_curCfgIndx].Clone());
            }
        }
        public void SaveNewConfig(string configName)
        {
            Configuration oc = (Configuration)_configurations[_curCfgIndx].Clone();
            oc.displayName = configName;
            _configurations.Add(oc);
            _curCfgIndx = _configurations.Count - 1;

            SyncWithView();
        }
        public void RemoveConfig()
        {
            _configurations.RemoveAt(_curCfgIndx);
            _curCfgIndx = _configurations.Count - 1;

            // TODO: "выключить" все виджеты

            SyncWithView();
        }
        public void ConfigureForm()
        {
            SyncConfigurations();
        }
        
        // === public ===

        // === private ===

        private void SetupComboBox<T>(List<T> collection, T val, Action<List<string>> setDataSourceFunc, Action<int> setIndexFunc)
        {
            var strCollection = new List<string>();
            foreach (var item in collection) strCollection.Add(item.ToString());
            setDataSourceFunc(strCollection);
            setIndexFunc(collection.FindIndex((x) => val.Equals(x)));
        }
        private void SyncViewWithConfig(Configuration cfg)
        {
            SetupComboBox(_rotations, cfg.rotation,
                View.SetCbxRotationDataSource, View.SetCbxRotationSelectedIndex);
            View.SetCbxFlipHoriz(cfg.flipHorizontal);
            View.SetCbxFlipVert(cfg.flipVertical);

            SetupComboBox(_paddingHorizValues, cfg.paddingRemovalHorizontal,
                View.SetCbxPaddingHorizDataSource, View.SetCbxPaddingHorizSelectedIndex);
            SetupComboBox(_paddingVertValues, cfg.paddingRemovalVertical,
                View.SetCbxPaddingVertDataSource, View.SetCbxPaddingVertSelectedIndex);

            if (cfg.lineWrap == LineWrap.AtBitmap)
                View.SetRbnLineWrapAtBitmapChecked(true);
            else if (cfg.lineWrap == LineWrap.AtColumn)
                View.SetRbnLineWrapAtColumnChecked(true);

            SetupComboBox(_commentStyles, cfg.commentStyle,
                View.SetCbxCommentStyleDataSource, View.SetCbxCommentStyleSelectedIndex);

            SetupComboBox(_bitLayouts, cfg.bitLayout,
                View.SetCbxBitLayoutDataSource, View.SetCbxBitLayoutSelectedIndex);
            SetupComboBox(_byteOrders, cfg.byteOrder,
                View.SetCbxByteOrderDataSource, View.SetCbxByteOrderSelectedIndex);
            SetupComboBox(_byteFormats, cfg.byteFormat,
                View.SetCbxByteFormatDataSource, View.SetCbxByteFormatSelectedIndex);
            SetupComboBox(_byteLeadingChars, cfg.byteLeadingString,
                View.SetCbxByteLeadingCharDataSource, View.SetCbxByteLeadingCharSelectedIndx);

            SetupComboBox(_charWidthValues, cfg.descCharWidth,
                View.SetCbxCharWidthFormatDataSource, View.SetCbxCharWidthFormatSelectedIndex);
            SetupComboBox(_charHeightValues, cfg.descCharHeight,
                View.SetCbxCharHeightFormatDataSource, View.SetCbxCharHeightFormatSelectedIndex);
            SetupComboBox(_fontHeightValues, cfg.descFontHeight,
                View.SetCbxFontHeightFormatDataSource, View.SetCbxFontHeightFormatSelectedIndex);
            SetupComboBox(_imageWidthValues, cfg.descImgWidth,
                View.SetCbxImgWidthFormatDataSource, View.SetCbxImgWidthFormatSelectedIndex);
            SetupComboBox(_imageHeightValues, cfg.descImgHeight,
                View.SetCbxImgHeightFormatDataSource, View.SetCbxImgHeightFormatSelectedIndex);
            View.SetTxtLookupBlocksNewAfterCharCountText(cfg.lookupBlocksNewAfterCharCount.ToString());
            View.SetCbxGenerateLookupArrayChecked(cfg.generateLookupArray);
            View.SetCbxGenerateLookupBlocksChecked(cfg.generateLookupBlocks);

            View.SetTxtBmpVisualizerCharText(cfg.bmpVisualizerChar);
            View.SetCbxCommentVarNameCheckedChanged(cfg.commentVariableName);
            View.SetCbxCommentCharVisualChecked(cfg.commentCharVisualizer);
            View.SetCbxCommentCharDescChecked(cfg.commentCharDescriptor);

            View.SetTxtVarNfBitmapsText(cfg.varNfBitmaps);
            View.SetTxtVarNfCharInfoText(cfg.varNfCharInfo);
            View.SetTxtVarNfFontInfoText(cfg.varNfFontInfo);
            View.SetTxtVarNfWidthText(cfg.varNfWidth);
            View.SetTxtVarNfHeightText(cfg.varNfHeight);

            View.SetTxtSpacePixelsText(cfg.spaceGenerationPixels.ToString());
            View.SetMingHeightText(cfg.minHeight.ToString());
       }

        private void SyncWithModel()
        {
            _configurations = new List<Configuration>(_service.GetConfigurations());
            int curCfgId = _service.GetCurrentConfig().Id;
            _curCfgIndx = _configurations.FindIndex((x) => x.Id == curCfgId);
        }
        private void SyncWithView()
        {
            var displayNames = new List<string>();
            foreach (var item in _configurations)
            {
                displayNames.Add(item.displayName);
            }
            View.SetConfigurations(displayNames);
            View.SetCurrentConfiguration(_curCfgIndx);
        }
        private void SyncConfigurations()
        {
            SyncWithModel();
            SyncWithView();            
        }
        private void Setup()
        {
            foreach (Rotation item in Enum.GetValues(typeof(Rotation)))
                _rotations.Add(item);

            foreach (PaddingRemoval item in Enum.GetValues(typeof(PaddingRemoval)))
            {
                _paddingHorizValues.Add(item);
                _paddingVertValues.Add(item);
            }

            foreach (DescriptorFormat item in Enum.GetValues(typeof(DescriptorFormat)))
            {
                _charWidthValues.Add(item);
                _charHeightValues.Add(item);
                _fontHeightValues.Add(item);
                _imageWidthValues.Add(item);
                _imageHeightValues.Add(item);
            }

            foreach (CommentStyle item in Enum.GetValues(typeof(CommentStyle)))
            {
                _commentStyles.Add(item);
            }

            foreach (BitLayout item in Enum.GetValues(typeof(BitLayout)))
            {
                _bitLayouts.Add(item);
            }

            _byteLeadingChars.Add(Configuration.ByteLeadingStringBinary);
            _byteLeadingChars.Add(Configuration.ByteLeadingStringHex);

            foreach (ByteOrder item in Enum.GetValues(typeof(ByteOrder)))
            {
                _byteOrders.Add(item);
            }

            foreach (ByteFormat item in Enum.GetValues(typeof(ByteFormat)))
            {
                _byteFormats.Add(item);
            }

            SyncConfigurations();
        }
    }
}
