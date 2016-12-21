using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public sealed class IgnoredInterfaceAopProxy : IIgnoredInterface
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IIgnoredInterface _realInstance;

        public IgnoredInterfaceAopProxy(
            IIgnoredInterface realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public void Dispose()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(IgnoredInterfaceAopProxy),
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