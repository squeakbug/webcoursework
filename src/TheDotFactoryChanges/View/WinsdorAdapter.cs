using System.Linq.Expressions;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Presenter;

namespace View
{
    public class WinsdorAdapter : IContainer
    {
        private readonly WindsorContainer _container = new WindsorContainer();

        public void Register<TService, TImplementation>() 
            where TImplementation : TService
            where TService : class
        {
            _container.Register(Component.For<TService>().ImplementedBy<TImplementation>());
        }

        public void Register<TService>()
            where TService : class
        {
            _container.Register(Component.For<TService>());
        }

        public void RegisterInstance<T>(T instance)
            where T : class
        {
            _container.Register(Component.For<T>().Instance(instance));
        }
        public TService Resolve<TService>()
        {
            return _container.Resolve<TService>();
        }

        public bool IsRegistered<TService>()
        {
            return _container.Kernel.HasComponent(typeof(TService));
        }
    }
}
