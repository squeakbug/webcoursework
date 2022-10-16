using System;
using System.Collections.Generic;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Presenter;

namespace View
{
    public class ApplicationController : IApplicationController
    {
        private IContainer _container;

        public ApplicationController(IContainer container)
        {
            _container = container ?? throw new ArgumentNullException("context");
            _container.RegisterInstance<IApplicationController>(this);
        }

        public IApplicationController RegisterView<TView, TImplementation>()
            where TImplementation : class, TView
            where TView : class, IView
        {
            _container.Register<TView, TImplementation>();
            return this;
        }
        public IApplicationController RegisterInstance<TInstance>(TInstance instance)
            where TInstance : class
        {
            _container.RegisterInstance(instance);
            return this;
        }
        public IApplicationController RegisterService<TService, TImplementation>()
            where TImplementation : class, TService
            where TService : class, IService
        {
            _container.Register<TService, TImplementation>();
            return this;
        }
        public void Run<TPresenter>()
            where TPresenter : class, IPresenter
        {
            if (!_container.IsRegistered<TPresenter>())
            {
                _container.Register<TPresenter>();
            }

            var presenter = _container.Resolve<TPresenter>();
            presenter.Run();
        }
        public void Run<TPresenter, TArgument>(TArgument argument) 
            where TPresenter : class, IPresenter<TArgument>
        {
            if (!_container.IsRegistered<TPresenter>())
            {
                _container.Register<TPresenter>();
            }

            var presenter = _container.Resolve<TPresenter>();
            presenter.Run(argument);
        }
    }
}
