using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public enum CheckState
    {
        Checked,
        Unchecked,
        Indeterminate
    }

    public interface ISettingsView : IView
    {
        event Action Applied;
        event Action FormLoad;

        // Config managing
        void SetCurrentConfiguration(int indx);
        void SetConfigurations(List<string> configurations);
        event Action<int> cbxOutputConfigurationsIndexChanged;
        event Action<string> NewConfigSaveBtnClicked;
        event Action RemoveConfigBtnClicked;

        // Flip/Flap Rotate
        void SetCbxRotationDataSource(List<string> dataSource);
        void SetCbxRotationSelectedIndex(int indx);
        void SetCbxFlipHoriz(bool cs);
        void SetCbxFlipVert(bool cs);
        event Action<CheckState> cbxFlipHorizCheckedChanged;
        event Action<CheckState> cbxFlipVertCheckedChanged;
        event Action<int> cbxRotationIndexChanged;

        // Padding removal
        void SetCbxPaddingHorizDataSource(List<string> dataSource);
        void SetCbxPaddingHorizSelectedIndex(int indx);
        void SetCbxPaddingVertDataSource(List<string> dataSource);
        void SetCbxPaddingVertSelectedIndex(int indx);
        event Action<int> cbxPaddingHorizIndexChanged;
        event Action<int> cbxPaddingVertIndexChanged;

        // LineWrap
        void SetRbnLineWrapAtBitmapChecked(bool cs);
        void SetRbnLineWrapAtColumnChecked(bool cs);
        event Action rbnLineWrapAtBitmapClicked;
        event Action rbnLineWrapAtColumnClicked;

        // Descriptors
        void SetCbxCharWidthFormatDataSource(List<string> dataSource);
        void SetCbxCharWidthFormatSelectedIndex(int indx);
        void SetCbxCharHeightFormatDataSource(List<string> dataSource);
        void SetCbxCharHeightFormatSelectedIndex(int indx);
        void SetCbxFontHeightFormatDataSource(List<string> dataSource);
        void SetCbxFontHeightFormatSelectedIndex(int indx);
        void SetCbxImgWidthFormatDataSource(List<string> dataSource);
        void SetCbxImgWidthFormatSelectedIndex(int indx);
        void SetCbxImgHeightFormatDataSource(List<string> dataSource);
        void SetCbxImgHeightFormatSelectedIndex(int indx);
        void SetTxtLookupBlocksNewAfterCharCountText(string str);
        void SetCbxGenerateLookupArrayChecked(bool cs);
        void SetCbxGenerateLookupBlocksChecked(bool cs);
        event Action<CheckState> cbxGenerateLookupArrayCheckedChanged;
        event Action<int> cbxCharWidthFormatIndexChanged;
        event Action<int> cbxCharHeightFormatIndexChanged;
        event Action<int> cbxFontHeightFormatIndexChanged;
        event Action<CheckState> cbxGenerateLookupBlocksCheckedChanged;
        event Action<string> txtLookupBlocksNewAfterCharCountChanged;
        event Action<int> cbxImgWidthFormatIndexChanged;
        event Action<int> cbxImgHeightFormatIndexChanged;

        // Space char generation
        event Action<CheckState> cbxGenerateSpaceBitmapCheckedChanged;
        event Action<string> txtSpacePixelsChanged;
        event Action<string> txtMinHeighthanged;

        // Comments
        void SetTxtBmpVisualizerCharText(string str);
        void SetCbxCommentVarNameCheckedChanged(bool cs);
        void SetCbxCommentCharVisualChecked(bool cs);
        void SetCbxCommentCharDescChecked(bool cs);
        void SetCbxCommentStyleDataSource(List<string> dataSource);
        void SetCbxCommentStyleSelectedIndex(int indx);
        event Action<CheckState> cbxCommentVarNameCheckedChanged;
        event Action<CheckState> cbxCommentCharVisualCheckedChanged;
        event Action<CheckState> cbxCommentCharDescCheckedChanged;
        event Action<string> txtBmpVisualizerCharTextChanged;
        event Action<int> cbxCommentStyleIndexChanged;

        // Variable name format
        void SetTxtVarNfBitmapsText(string str);
        void SetTxtVarNfCharInfoText(string str);
        void SetTxtVarNfFontInfoText(string str);
        void SetTxtVarNfWidthText(string str);
        void SetTxtVarNfHeightText(string str);
        event Action<string> txtVarNfBitmapsTextChanged;
        event Action<string> txtVarNfCharInfoTextChanged;
        event Action<string> txtVarNfFontInfoTextChanged;
        event Action<string> txtVarNfWidthTextChanged;
        event Action<string> txtVarNfHeightTextChanged;

        // Byte
        void SetCbxBitLayoutDataSource(List<string> dataSource);
        void SetCbxBitLayoutSelectedIndex(int indx);
        void SetCbxByteOrderDataSource(List<string> dataSource);
        void SetCbxByteOrderSelectedIndex(int indx);
        void SetCbxByteFormatDataSource(List<string> dataSource);
        void SetCbxByteFormatSelectedIndex(int indx);
        void SetCbxByteLeadingCharDataSource(List<string> dataSource);
        void SetCbxByteLeadingCharSelectedIndx(int indx);
        event Action<int> cbxBitLayoutIndexChanged;
        event Action<int> cbxByteOrderIndexChanged;
        event Action<int> cbxByteFormatIndexChanged;
        event Action<int> cbxByteLeadingCharIndexChanged;

        // Space char generation
        void SetTxtSpacePixelsText(string str);
        void SetMingHeightText(string str);
    }
}
