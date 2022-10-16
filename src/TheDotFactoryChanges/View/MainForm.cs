using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

using Presenter;

namespace View
{
    public partial class MainForm : Form, IConverterView
    {
        public event Action<Font> FontSelected;
        public event Action GenPEGBtnClicked;
        public event Action GenerateBtnClicked;
        public event Action OutputConfigBtnClicked;
        public event Action<int> CurrentConfigChanged;
        public event Action TextInsertedBtnClicked;
        public event Action<string> BitmapLoadBtnClicked;
        public event Action SaveAsClicked;
        public event Action<string> DialogSaveClicked;
        public event Action FormLoad;
        public event Action<int> TextInsertionChanged;
        public event Action<int> tcInputIndexChanged;
        public event Action<string> txtInputTextChanged;
        public event Action<string> ImageNameChanged;
        public event Action<string> txtBoxPEGFontNameChanged;

        private readonly int _colouredTextLengthLimit = 1500;
        private string _nl = "\n";

        // === Constructors ===

        public MainForm()
        {
            InitializeComponent();

            // set UI properties that the designer does not set correctly
            // designer sets MinSize values before initializing the splitter distance which causes an exception
            splitContainer1.SplitterDistance = 340;
            splitContainer1.Panel1MinSize = 287;
            splitContainer1.Panel2MinSize = 260;
        }

        // === IConverterView ===

        public new void Show()
        {
            Application.Run(this);
        }
        public void SetInputTextEntryFont(Font font)
        {
            txtInputText.Font = font;
        }
        public void SetInputTextEntryText(string str)
        {
            txtInputText.Text = str;
        }
        public void SetInputFontEntryFont(Font font)
        {
            txtInputFont.Font = font;
        }
        public void SetInputFontEntryText(string str)
        {
            txtInputFont.Text = str;
        }
        public void SetLoadedBitmapImage(Bitmap bmp)
        {
            pbxBitmap.Image = bmp;
        }
        public void SetSourceFieldText(string str)
        {
            txtOutputTextSource.Text = str;
            OutputSyntaxColoredString(txtOutputTextSource);
        }
        public void SetHeaderFieldText(string str)
        {
            txtOutputTextHeader.Text = str;
            OutputSyntaxColoredString(txtOutputTextHeader);
        }
        public void SetApplicationVersion(string version)
        {
            Text = version;
        }
        public void SetConfigurations(List<string> configurations)
        {
            cbxOutputConfiguration.Items.Clear();
            foreach (var item in configurations)
            {
                cbxOutputConfiguration.Items.Add(item);
            }
        }
        public void SetCurrentConfiguration(int indx)
        {
            cbxOutputConfiguration.SelectedIndex = indx;
        }
        public void SetCurrentTextInsertedIndx(int indx)
        {
            cbxTextInsert.SelectedIndex = indx;
        }
        public void SetTextInsertions(List<string> insertions)
        {
            foreach (var item in insertions)
            {
                cbxTextInsert.Items.Add(item);
            }
        }
        public void SetSaveFileDialogFileName(string filename)
        {
            dlgSaveAs.FileName = filename;
        }
        public void SetNewlineChar(string ch)
        {
            _nl = ch;
        }
        public void ShowErrorMessage(string topic, string content)
        {
            MessageBox.Show(content, topic);
        }

        // === Handlers ===
        private void txtInputText_TextChanged(object sender, EventArgs e)
        {
            txtInputTextChanged.Invoke(txtInputText.Text);
        }
        private void tcInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            tcInputIndexChanged.Invoke(tcInput.SelectedIndex);
        }
        private void btnFontSelect_Click(object sender, EventArgs e)
        {
            label1.Focus();
            if (fontDlgInputFont.ShowDialog(this) == DialogResult.OK)
                FontSelected.Invoke(fontDlgInputFont.Font);
        }
        private void btnOutputConfig_Click(object sender, EventArgs e)
        {
            label1.Focus();
            OutputConfigBtnClicked.Invoke();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            FormLoad.Invoke();
        }
        private void btnBitmapLoad_Click(object sender, EventArgs e)
        {
            dlgOpenFile.Filter = "Image Files (*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (dlgOpenFile.ShowDialog() != DialogResult.Cancel)
            {
                BitmapLoadBtnClicked.Invoke(dlgOpenFile.FileName);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void splitContainer1_MouseUp(object sender, MouseEventArgs e)
        {
            label1.Focus();
        }
        private void btnInsertText_Click(object sender, EventArgs e)
        {
            label1.Focus();
            TextInsertedBtnClicked.Invoke();
        }
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            about.Show();
        }
        private void cbxOutputConfiguration_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentConfigChanged.Invoke(cbxOutputConfiguration.SelectedIndex);
        }
        private void tsmCopySource_Click(object sender, EventArgs e)
        {
            if (txtOutputTextSource.Text != "")
            {
                Clipboard.SetText(txtOutputTextSource.Text);
            }
        }
        private void tsmCopyHeader_Click(object sender, EventArgs e)
        {
            if (txtOutputTextHeader.Text != "")
            {
                Clipboard.SetText(txtOutputTextHeader.Text);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsClicked.Invoke();
            if (dlgSaveAs.ShowDialog() == DialogResult.OK)
            {
                DialogSaveClicked.Invoke(dlgSaveAs.FileName);
            }
        }
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            label1.Focus();
            GenerateBtnClicked.Invoke();
        }
        private void btnGenPEG_Click(object sender, EventArgs e)
        {
            label1.Focus();
            GenPEGBtnClicked.Invoke();
        }
        private void cbxTextInsert_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextInsertionChanged.Invoke(cbxTextInsert.SelectedIndex);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Refresh();
        }
        private void txtImageName_TextChanged(object sender, EventArgs e)
        {
            ImageNameChanged.Invoke(txtImageName.Text);
        }
        private void txtBoxPEGFontName_TextChanged(object sender, EventArgs e)
        {
            txtBoxPEGFontNameChanged.Invoke(txtBoxPEGFontName.Text);
        }

        // === private ===

        void OutputSyntaxColoredString(RichTextBox outputTextBox)
        {
            string outputString = outputTextBox.Text;
            outputTextBox.Text = "";

            String[] lines = outputString.Split(new string[] { _nl }, StringSplitOptions.None);

            if (lines.Length > _colouredTextLengthLimit)
            {
                outputTextBox.Text = outputString;
                return;
            }

            Font fRegular = new Font("Courier New", 10, FontStyle.Regular);
            Font fBold = new Font("Courier New", 10, FontStyle.Bold);
            String[] keywords = { "uint_8", "const", "extern", "char", "unsigned", "int", "short", "long" };
            Regex re = new Regex(@"([ \t{}();])");

            foreach (string line in lines)
            {
                String[] tokens = re.Split(line);
                foreach (string token in tokens)
                {
                    outputTextBox.SelectionColor = Color.Black;
                    outputTextBox.SelectionFont = fRegular;

                    if (token == "//" || token.StartsWith("//"))
                    {
                        int index = line.IndexOf("//");
                        string comment = line.Substring(index, line.Length - index);
                        outputTextBox.SelectionColor = Color.Green;
                        outputTextBox.SelectionFont = fRegular;
                        outputTextBox.SelectedText = comment;
                        break;
                    }

                    if (token == "/*" || token.StartsWith("/*"))
                    {
                        int index = line.IndexOf("/*");
                        string comment = line.Substring(index, line.Length - index);
                        outputTextBox.SelectionColor = Color.Green;
                        outputTextBox.SelectionFont = fRegular;
                        outputTextBox.SelectedText = comment;
                        break;
                    }

                    if (token == "**" || token.StartsWith("**"))
                    {
                        int index = line.IndexOf("**");
                        string comment = line.Substring(index, line.Length - index);
                        outputTextBox.SelectionColor = Color.Green;
                        outputTextBox.SelectionFont = fRegular;
                        outputTextBox.SelectedText = comment;
                        break;
                    }

                    if (token == "*/" || token.StartsWith("*/"))
                    {
                        int index = line.IndexOf("*/");
                        string comment = line.Substring(index, line.Length - index);
                        outputTextBox.SelectionColor = Color.Green;
                        outputTextBox.SelectionFont = fRegular;
                        outputTextBox.SelectedText = comment;
                        break;
                    }

                    for (int i = 0; i < keywords.Length; i++)
                    {
                        if (keywords[i] == token)
                        {
                            outputTextBox.SelectionColor = Color.Blue;
                            outputTextBox.SelectionFont = fBold;
                            break;
                        }
                    }
                    outputTextBox.SelectedText = token;
                }
                outputTextBox.SelectedText = _nl;
            }
        }
    }
}

