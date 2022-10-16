using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public interface IView
    {
        void ShowErrorMessage(string topic, string content);
        void Show();
        void Close();
        event EventHandler Load;
    }
}
