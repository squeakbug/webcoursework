using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Presenter
{
    public interface IConverterView : IView
    {
        // Common
        void SetNewlineChar(string ch);
        event Action FormLoad;
        event Action<int> tcInputIndexChanged;

        // Font
        void SetSaveFileDialogFileName(string filename);
        void SetInputFontEntryText(string str);
        void SetInputFontEntryFont(Font font);
        event Action<Font> FontSelected;
        event Action<string> txtBoxPEGFontNameChanged;

        // InputText
        void SetInputTextEntryText(string str);
        void SetInputTextEntryFont(Font font);
        event Action<string> txtInputTextChanged;

        // TextInsertion
        void SetCurrentTextInsertedIndx(int indx);
        void SetTextInsertions(List<string> insertions);
        event Action TextInsertedBtnClicked;
        event Action<int> TextInsertionChanged;

        // OutputText
        void SetSourceFieldText(string str);
        void SetHeaderFieldText(string str);
        event Action GenPEGBtnClicked;
        event Action GenerateBtnClicked;

        // Config
        void SetConfigurations(List<string> configurations);
        void SetCurrentConfiguration(int indx);
        void SetApplicationVersion(string version);
        event Action OutputConfigBtnClicked;
        event Action<int> CurrentConfigChanged;

        // BitmapLoading
        void SetLoadedBitmapImage(Bitmap bmp);
        event Action<string> BitmapLoadBtnClicked;
        event Action<string> ImageNameChanged;

        // Menu
        event Action SaveAsClicked;
        event Action<string> DialogSaveClicked;
    }
}
