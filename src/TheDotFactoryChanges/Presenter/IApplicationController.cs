using System;
using System.Collections.Generic;
using System.Text;

namespace Presenter
{
    public interface IApplicationController
    {
        IApplicationController RegisterView<TView, TImplementation>()
            where TImplementation : class, TView
            where TView : class, IView;
        IApplicationController RegisterInstance<TInstance>(TInstance instance)
            where TInstance : class;
        IApplicationController RegisterService<TService, TImplementation>()
            where TImplementation : class, TService
            where TService : class, IService;
        void Run<TPresenter>()
            where TPresenter : class, IPresenter;
        void Run<TPresenter, TArgumnent>(TArgumnent argumnent)
           where TPresenter : class, IPresenter<TArgumnent>;
    }
}
