using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace View
{
    public partial class OutputConfigurationForm : Form, Presenter.ISettingsView
    {
        public event Action FormLoad;
        public event Action Applied;

        // Config managing
        public event Action<int> cbxOutputConfigurationsIndexChanged;
        public event Action<string> NewConfigSaveBtnClicked;
        public event Action RemoveConfigBtnClicked;

        // Flip/Flap Rotate
        public event Action<Presenter.CheckState> cbxFlipHorizCheckedChanged;
        public event Action<Presenter.CheckState> cbxFlipVertCheckedChanged;
        public event Action<int> cbxRotationIndexChanged;

        // Padding removal
        public event Action<int> cbxPaddingHorizIndexChanged;
        public event Action<int> cbxPaddingVertIndexChanged;

        // LineWrap
        public event Action rbnLineWrapAtBitmapClicked;
        public event Action rbnLineWrapAtColumnClicked;

        // Descriptors
        public event Action<Presenter.CheckState> cbxGenerateLookupArrayCheckedChanged;
        public event Action<int> cbxCharWidthFormatIndexChanged;
        public event Action<int> cbxCharHeightFormatIndexChanged;
        public event Action<int> cbxFontHeightFormatIndexChanged;
        public event Action<Presenter.CheckState> cbxGenerateLookupBlocksCheckedChanged;
        public event Action<string> txtLookupBlocksNewAfterCharCountChanged;
        public event Action<int> cbxImgWidthFormatIndexChanged;
        public event Action<int> cbxImgHeightFormatIndexChanged;

        // Space char generation
        public event Action<Presenter.CheckState> cbxGenerateSpaceBitmapCheckedChanged;
        public event Action<string> txtSpacePixelsChanged;
        public event Action<string> txtMinHeighthanged;

        // Comments
        public event Action<Presenter.CheckState> cbxCommentVarNameCheckedChanged;
        public event Action<Presenter.CheckState> cbxCommentCharVisualCheckedChanged;
        public event Action<Presenter.CheckState> cbxCommentCharDescCheckedChanged;
        public event Action<string> txtBmpVisualizerCharTextChanged;
        public event Action<int> cbxCommentStyleIndexChanged;

        // Variable name format
        public event Action<string> txtVarNfBitmapsTextChanged;
        public event Action<string> txtVarNfCharInfoTextChanged;
        public event Action<string> txtVarNfFontInfoTextChanged;
        public event Action<string> txtVarNfWidthTextChanged;
        public event Action<string> txtVarNfHeightTextChanged;

        // Byte
        public event Action<int> cbxBitLayoutIndexChanged;
        public event Action<int> cbxByteOrderIndexChanged;
        public event Action<int> cbxByteFormatIndexChanged;
        public event Action<int> cbxByteLeadingCharIndexChanged;

        // === Constructors ===

        public OutputConfigurationForm()
        {
            InitializeComponent();

            // set tooltips
            setControlTooltip(btnSaveNewConfig, "Save as new preset");
            setControlTooltip(btnDeleteConfig, "Delete preset");
        }

        // === Handlers ===

        private void OutputConfigurationForm_Load_1(object sender, EventArgs e)
        {
            FormLoad.Invoke();
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            Applied.Invoke();
        }

        // Config managing
        private void btnDeleteConfig_Click(object sender, EventArgs e)
        {
            gbxPadding.Focus();
            RemoveConfigBtnClicked.Invoke();
        }
        private void btnSaveNewConfig_Click(object sender, EventArgs e)
        {
            gbxPadding.Focus();
            InputBoxDialog ib = new InputBoxDialog();
            ib.FormPrompt = "Enter preset name";
            ib.FormCaption = "New preset configuration";
            ib.DefaultValue = "";

            if (ib.ShowDialog() == DialogResult.OK)
            {
                ib.Close();
                NewConfigSaveBtnClicked.Invoke(ib.InputResponse);
            }
        }
        private void cbxOutputConfigurations_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxOutputConfigurationsIndexChanged.Invoke(cbxOutputConfigurations.SelectedIndex);
        }

        // Flip/Rotate
        private void cbxFlipHoriz_CheckedChanged(object sender, EventArgs e)
        {
            cbxFlipHorizCheckedChanged.Invoke(GetCheckState(cbxFlipHoriz.CheckState));
        }
        private void cbxFlipVert_CheckedChanged(object sender, EventArgs e)
        {
            cbxFlipVertCheckedChanged.Invoke(GetCheckState(cbxFlipVert.CheckState));
        }
        private void cbxRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxRotationIndexChanged.Invoke(cbxRotation.SelectedIndex);
        }

        // Padding Removal
        private void cbxPaddingHoriz_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxPaddingHorizIndexChanged.Invoke(cbxPaddingHoriz.SelectedIndex);
        }
        private void cbxPaddingVert_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxPaddingVertIndexChanged.Invoke(cbxPaddingVert.SelectedIndex);
        }

        // Line wrap
        private void rbnLineWrapAtBitmap_Click(object sender, EventArgs e)
        {
            rbnLineWrapAtBitmapClicked.Invoke();
        }
        private void rbnLineWrapAtColumn_Click(object sender, EventArgs e)
        {
            rbnLineWrapAtColumnClicked.Invoke();
        }

        // Descriptors
        private void cbxGenerateLookupArray_CheckedChanged(object sender, EventArgs e)
        {
            cbxGenerateLookupArrayCheckedChanged.Invoke(GetCheckState(cbxGenerateLookupArray.CheckState));
        }
        private void cbxCharWidthFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCharWidthFormatIndexChanged.Invoke(cbxCharWidthFormat.SelectedIndex);
        }
        private void cbxCharHeightFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCharHeightFormatIndexChanged.Invoke(cbxCharHeightFormat.SelectedIndex);
        }
        private void cbxFontHeightFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxFontHeightFormatIndexChanged.Invoke(cbxFontHeightFormat.SelectedIndex);
        }
        private void cbxGenerateLookupBlocks_CheckedChanged(object sender, EventArgs e)
        {
            cbxGenerateLookupBlocksCheckedChanged.Invoke(GetCheckState(cbxGenerateLookupBlocks.CheckState));
        }
        private void txtLookupBlocksNewAfterCharCount_TextChanged(object sender, EventArgs e)
        {
            txtLookupBlocksNewAfterCharCountChanged.Invoke(txtLookupBlocksNewAfterCharCount.Text);
        }
        private void cbxImgWidthFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxImgWidthFormatIndexChanged.Invoke(cbxImgWidthFormat.SelectedIndex);
        }
        private void cbxImgHeightFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxImgHeightFormatIndexChanged.Invoke(cbxImgHeightFormat.SelectedIndex);
        }

        // Space char generation
        private void cbxGenerateSpaceBitmap_CheckedChanged(object sender, EventArgs e)
        {
            cbxGenerateSpaceBitmapCheckedChanged.Invoke(GetCheckState(cbxGenerateSpaceBitmap.CheckState));
        }
        private void txtSpacePixels_TextChanged(object sender, EventArgs e)
        {
            txtSpacePixelsChanged.Invoke(txtSpacePixels.Text);
        }
        private void txtMinHeight_TextChanged(object sender, EventArgs e)
        {
            txtMinHeighthanged.Invoke(txtMinHeight.Text);
        }

        // Comments
        private void cbxCommentVarName_CheckedChanged(object sender, EventArgs e)
        {
            cbxCommentVarNameCheckedChanged.Invoke(GetCheckState(cbxCommentVarName.CheckState));
        }
        private void cbxCommentCharVisual_CheckedChanged(object sender, EventArgs e)
        {
            cbxCommentCharVisualCheckedChanged.Invoke(GetCheckState(cbxCommentCharVisual.CheckState));
        }
        private void cbxCommentCharDesc_CheckedChanged(object sender, EventArgs e)
        {
            cbxCommentCharDescCheckedChanged.Invoke(GetCheckState(cbxCommentCharDesc.CheckState));
        }
        private void txtBmpVisualizerChar_TextChanged(object sender, EventArgs e)
        {
            txtBmpVisualizerCharTextChanged.Invoke(txtBmpVisualizerChar.Text);
        }
        private void cbxCommentStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCommentStyleIndexChanged.Invoke(cbxCommentStyle.SelectedIndex);
        }

        // Variable name format
        private void txtVarNfBitmaps_TextChanged(object sender, EventArgs e)
        {
            txtVarNfBitmapsTextChanged.Invoke(txtVarNfBitmaps.Text);
        }
        private void txtVarNfCharInfo_TextChanged(object sender, EventArgs e)
        {
            txtVarNfCharInfoTextChanged.Invoke(txtVarNfCharInfo.Text);
        }
        private void txtVarNfFontInfo_TextChanged(object sender, EventArgs e)
        {
            txtVarNfFontInfoTextChanged.Invoke(txtVarNfFontInfo.Text);
        }
        private void txtVarNfWidth_TextChanged(object sender, EventArgs e)
        {
            txtVarNfWidthTextChanged.Invoke(txtVarNfWidth.Text);
        }
        private void txtVarNfHeight_TextChanged(object sender, EventArgs e)
        {
            txtVarNfHeightTextChanged.Invoke(txtVarNfHeight.Text);
        }
        
        // Byte
        private void cbxBitLayout_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxBitLayoutIndexChanged.Invoke(cbxBitLayout.SelectedIndex);
        }
        private void cbxByteOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxByteOrderIndexChanged.Invoke(cbxByteOrder.SelectedIndex);
        }
        private void cbxByteFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxByteFormatIndexChanged.Invoke(cbxByteFormat.SelectedIndex);
        }
        private void cbxByteLeadingChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxByteLeadingCharIndexChanged.Invoke(cbxByteLeadingChar.SelectedIndex);
        }

        // === public ===

        public void SetCurrentConfiguration(int indx)
        {
            cbxOutputConfigurations.SelectedIndex = indx;
        }
        public void SetConfigurations(List<string> configurations)
        {
            cbxOutputConfigurations.Items.Clear();
            foreach (var item in configurations)
            {
                cbxOutputConfigurations.Items.Add(item);
            }
        }
        public void ShowErrorMessage(string topic, string content)
        {
            MessageBox.Show(content, topic);
        }

        // Flip / Rotation
        public void SetCbxRotationDataSource(List<string> dataSource)
        {
            cbxRotation.DataSource = dataSource;
        }
        public void SetCbxRotationSelectedIndex(int indx)
        {
            cbxRotation.SelectedIndex = indx;
        }
        public void SetCbxFlipHoriz(bool cs)
        {
            cbxFlipHoriz.CheckState = cs 
                ? System.Windows.Forms.CheckState.Checked 
                : System.Windows.Forms.CheckState.Unchecked;
        }
        public void SetCbxFlipVert(bool cs)
        {
            cbxFlipVert.CheckState = cs
                ? System.Windows.Forms.CheckState.Checked
                : System.Windows.Forms.CheckState.Unchecked;
        }
        
        // Padding
        public void SetCbxPaddingHorizDataSource(List<string> dataSource)
        {
            cbxPaddingHoriz.DataSource = dataSource;
        }
        public void SetCbxPaddingHorizSelectedIndex(int indx)
        {
            cbxPaddingHoriz.SelectedIndex = indx;
        }
        public void SetCbxPaddingVertDataSource(List<string> dataSource)
        {
            cbxPaddingVert.DataSource = dataSource;
        }
        public void SetCbxPaddingVertSelectedIndex(int indx)
        {
            cbxPaddingVert.SelectedIndex = indx;
        }

        // LineWrap
        public void SetRbnLineWrapAtBitmapChecked(bool cs)
        {
            rbnLineWrapAtBitmap.Checked = cs;
        }
        public void SetRbnLineWrapAtColumnChecked(bool cs)
        {
            rbnLineWrapAtColumn.Checked = cs;
        }

        // Descriptors
        public void SetCbxCharWidthFormatDataSource(List<string> dataSource)
        {
            cbxCharWidthFormat.DataSource = dataSource;
        }
        public void SetCbxCharWidthFormatSelectedIndex(int indx)
        {
            cbxCharWidthFormat.SelectedIndex = indx;
        }
        public void SetCbxCharHeightFormatDataSource(List<string> dataSource)
        {
            cbxCharHeightFormat.DataSource = dataSource;
        }
        public void SetCbxCharHeightFormatSelectedIndex(int indx)
        {
            cbxCharHeightFormat.SelectedIndex = indx;
        }
        public void SetCbxFontHeightFormatDataSource(List<string> dataSource)
        {
            cbxFontHeightFormat.DataSource = dataSource;
        }
        public void SetCbxFontHeightFormatSelectedIndex(int indx)
        {
            cbxFontHeightFormat.SelectedIndex = indx;
        }
        public void SetCbxImgWidthFormatDataSource(List<string> dataSource)
        {
            cbxImgWidthFormat.DataSource = dataSource;
        }
        public void SetCbxImgWidthFormatSelectedIndex(int indx)
        {
            cbxImgWidthFormat.SelectedIndex = indx;
        }
        public void SetCbxImgHeightFormatDataSource(List<string> dataSource)
        {
            cbxImgHeightFormat.DataSource = dataSource;
        }
        public void SetCbxImgHeightFormatSelectedIndex(int indx)
        {
            cbxImgHeightFormat.SelectedIndex = indx;
        }
        public void SetTxtLookupBlocksNewAfterCharCountText(string str)
        {
            txtLookupBlocksNewAfterCharCount.Text = str;
        }
        public void SetCbxGenerateLookupArrayChecked(bool cs)
        {
            cbxGenerateLookupArray.Checked = cs;
        }
        public void SetCbxGenerateLookupBlocksChecked(bool cs)
        {
            cbxGenerateLookupBlocks.Checked = cs;
        }

        // Byte
        public void SetCbxBitLayoutDataSource(List<string> dataSource)
        {
            cbxBitLayout.DataSource = dataSource;
        }
        public void SetCbxBitLayoutSelectedIndex(int indx)
        {
            cbxBitLayout.SelectedIndex = indx;
        }
        public void SetCbxByteOrderDataSource(List<string> dataSource)
        {
            cbxByteOrder.DataSource = dataSource;
        }
        public void SetCbxByteOrderSelectedIndex(int indx)
        {
            cbxByteOrder.SelectedIndex = indx;
        }
        public void SetCbxByteFormatDataSource(List<string> dataSource)
        {
            cbxByteFormat.DataSource = dataSource;
        }
        public void SetCbxByteFormatSelectedIndex(int indx)
        {
            cbxByteFormat.SelectedIndex = indx;
        }
        public void SetCbxByteLeadingCharDataSource(List<string> dataSource)
        {
            cbxByteLeadingChar.DataSource = dataSource;
        }
        public void SetCbxByteLeadingCharSelectedIndx(int indx)
        {
            cbxByteLeadingChar.SelectedIndex = indx;
        }

        // Comments
        public void SetTxtBmpVisualizerCharText(string str)
        {
            txtBmpVisualizerChar.Text = str;
        }
        public void SetCbxCommentVarNameCheckedChanged(bool cs)
        {
            cbxCommentVarName.Checked = cs;
        }
        public void SetCbxCommentCharVisualChecked(bool cs)
        {
            cbxCommentCharVisual.Checked = cs;
        }
        public void SetCbxCommentCharDescChecked(bool cs)
        {
            cbxCommentCharDesc.Checked = cs;
        }
        public void SetCbxCommentStyleDataSource(List<string> dataSource)
        {
            cbxCommentStyle.DataSource = dataSource;
        }
        public void SetCbxCommentStyleSelectedIndex(int indx)
        {
            cbxCommentStyle.SelectedIndex = indx;
        }

        // Variable name format
        public void SetTxtVarNfBitmapsText(string str)
        {
            txtVarNfBitmaps.Text = str;
        }
        public void SetTxtVarNfCharInfoText(string str)
        {
            txtVarNfCharInfo.Text = str;
        }
        public void SetTxtVarNfFontInfoText(string str)
        {
            txtVarNfFontInfo.Text = str;
        }
        public void SetTxtVarNfWidthText(string str)
        {
            txtVarNfWidth.Text = str;
        }
        public void SetTxtVarNfHeightText(string str)
        {
            txtVarNfHeight.Text = str;
        }

        // Space char generation
        public void SetTxtSpacePixelsText(string str)
        {
            txtSpacePixels.Text = str;
        }
        public void SetMingHeightText(string str)
        {
            txtMinHeight.Text = str;
        }

        // === private ===

        private void setControlTooltip(Control control, string tooltipString)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(control, tooltipString);
        }
        private Presenter.CheckState GetCheckState(System.Windows.Forms.CheckState cs)
        {
            switch (cs)
            {
                case System.Windows.Forms.CheckState.Checked:
                    return Presenter.CheckState.Checked;
                case System.Windows.Forms.CheckState.Unchecked:
                    return Presenter.CheckState.Unchecked;
                case System.Windows.Forms.CheckState.Indeterminate:
                    return Presenter.CheckState.Indeterminate;
                default:
                    throw new Exception("Bad check state");
            }
        }
        private System.Windows.Forms.CheckState FromCheckState(Presenter.CheckState cs)
        {
            switch (cs)
            {
                case Presenter.CheckState.Checked:
                    return System.Windows.Forms.CheckState.Checked;
                case Presenter.CheckState.Unchecked:
                    return System.Windows.Forms.CheckState.Unchecked;
                case Presenter.CheckState.Indeterminate:
                    return System.Windows.Forms.CheckState.Indeterminate;
                default:
                    throw new Exception("Bad check state");
            }
        }
    }
}
