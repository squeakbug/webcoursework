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

        Task ConvertFont(bool isPeg);
        event Action<string> OutputSourceTextChanged;
        event Action<string> OutputHeaderTextChanged;

        void AddTextInsertion(string key, string charSet);
        void RemoveTextInsertion(string key);
        string GetTextInsertion(string key);
        List<string> GetTextInsertions();

        void SetImageName(string str);
        void SetPEGFontName(string str);

        Task<int> CreateConfig(Configuration cfg);
        Task DeleteConfig(int id);
        Task UpdateConfig(Configuration cfg);
        Task<Configuration> GetConfigById(int id);
        Task<Configuration> GetCurrentConfig();
        Task SetCurrentConfig(int id);
        Task<IEnumerable<Configuration>> GetConfigurations();
        Task UpdateConfigurations(IEnumerable<Configuration> cfgs);
        event Action<int> ConfigRemoved;
        event Action<int> ConfigAdded;
        event Action<int> ConfigUpdated;
        event Action ConfigsUpdated;

        Task<IEnumerable<string>> GetFontNames();
        Task<IEnumerable<Convertion>> GetConvertions();
        Task<Convertion> GetConvertionById(int id);
        Task<int> AddConvertion(Convertion cvt);
        string GetOutputSourceText();
        string GetOutputHeaderText();

        Task SetCurrentFont(int id);
        Task<int> AddFont(DataAccessInterface.Font font);
        Task<IEnumerable<DataAccessInterface.Font>> GetFonts();
        Task<DataAccessInterface.Font> GetFontById(int id);
        Task UpdateFont(DataAccessInterface.Font font);
        Task DeleteFont(int id);
    }
}
