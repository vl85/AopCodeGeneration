using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Behaviour
{
    public sealed class Interceptor
    {
        public InvocationInfo Invoke(InvocationInfo invocationInfo)
        {
            return invocationInfo;
        }
    }
}