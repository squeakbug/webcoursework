using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Presenter;
using Service;
using DataAccessSQLServer;
using DataAccessInterface;

namespace View
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var controller = new ApplicationController(new WinsdorAdapter())
                .RegisterView<IConverterView, MainForm>()
                .RegisterService<IConverterService, ConverterService>()
                .RegisterView<ISettingsView, OutputConfigurationForm>()
                .RegisterInstance<ITextRenderer>(new WinFormsTextRendererAdapter())
                .RegisterInstance<IRepositoryFactory>(new DataAccessSQLServer.RepositoryFactory(
                    "127.0.0.1",
                    "thedotfactory_db",
                    "SA",
                    "P@ssword"));

            controller.Run<ConverterPresenter>();
        }
    }
}
