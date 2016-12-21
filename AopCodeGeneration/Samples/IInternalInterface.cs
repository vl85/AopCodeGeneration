using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    internal interface IInternalInterface
    {
        void DoSomethingInternal();
    }

    internal sealed class InternalInterfaceAopProxy : IInternalInterface
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IInternalInterface _realInstance;

        internal InternalInterfaceAopProxy(
            IInternalInterface realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public void DoSomethingInternal()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(InternalInterfaceAopProxy),
                nameof(DoSomethingInternal),
                DoSomethingInternal);
            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo DoSomethingInternal(InvocationInfo invocationInfo)
        {
            _realInstance.DoSomethingInternal();
            return invocationInfo;
        }
    }
}