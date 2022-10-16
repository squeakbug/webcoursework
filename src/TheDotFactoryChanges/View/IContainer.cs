using System;
using System.Linq.Expressions;

namespace View
{
    public interface IContainer
    {
        void Register<TService, TImplementation>()
            where TImplementation : TService
            where TService : class;
        void Register<TService>()
            where TService : class;
        void RegisterInstance<T>(T instance)
            where T : class;
        TService Resolve<TService>();
        bool IsRegistered<TService>();
    }
}
