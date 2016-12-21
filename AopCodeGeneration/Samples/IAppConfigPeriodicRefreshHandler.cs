using System;
using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface IAppConfigPeriodicRefreshHandler : IDisposable
    {
        void Start();
    }

    public sealed class AppConfigPeriodicRefreshHandlerAopProxy : IAppConfigPeriodicRefreshHandler
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IAppConfigPeriodicRefreshHandler _realInstance;

        public AppConfigPeriodicRefreshHandlerAopProxy(
            IAppConfigPeriodicRefreshHandler realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public void Start()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(AppConfigPeriodicRefreshHandlerAopProxy),
                nameof(Start),
                Start);
            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo Start(InvocationInfo invocationInfo)
        {
            _realInstance.Start();
            return invocationInfo;
        }

        public void Dispose()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(AppConfigPeriodicRefreshHandlerAopProxy),
                nameof(Dispose),
                Dispose);
            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo Dispose(InvocationInfo invocationInfo)
        {
            _realInstance.Dispose();
            return invocationInfo;
        }
    }
}