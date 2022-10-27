using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using DataAccessInterface;

namespace Presenter
{
    public interface IConverterService : IService
    {
        void UpdateBitmap(Bitmap bmp);
        Bitmap GetCurrentBitmap();

        void UpdateFont(System.Drawing.Font font);
        System.Drawing.Font GetCurrentFont();
        event Action<System.Drawing.Font> FontChanged;

        void UpdateTabState(TabState state);
        void SetInputText(string str);

        void ConvertFont(bool isPeg);
        event Action<string> OutputSourceTextChanged;
        event Action<string> OutputHeaderTextChanged;

        void AddTextInsertion(string key, string charSet);
        void RemoveTextInsertion(string key);
        string GetTextInsertion(string key);
        List<string> GetTextInsertions();

        void SetImageName(string str);
        void SetPEGFontName(string str);

        int CreateConfig(Configuration cfg);
        void DeleteConfig(int id);
        void UpdateConfig(Configuration cfg);
        Configuration GetConfigById(int id);
        Configuration GetCurrentConfig();
        void SetCurrentConfig(int id);
        IEnumerable<Configuration> GetConfigurations();
        void UpdateConfigurations(IEnumerable<Configuration> cfgs);
        event Action<int> ConfigRemoved;
        event Action<int> ConfigAdded;
        event Action<int> ConfigUpdated;
        event Action ConfigsUpdated;

        IEnumerable<string> GetFontNames();
        IEnumerable<Convertion> GetConvertions();
        Convertion GetConvertionById(int id);
        int AddConvertion(Convertion cvt);
        string GetOutputSourceText();
        string GetOutputHeaderText();

        void SetCurrentFont(int id);
        int AddFont(DataAccessInterface.Font font);
        IEnumerable<DataAccessInterface.Font> GetFonts();
        DataAccessInterface.Font GetFontById(int id);
        void UpdateFont(DataAccessInterface.Font font);
        void DeleteFont(int id);
    }
}
