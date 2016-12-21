using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface IRefUtils
    {
        void Swap<T>(ref T v1, ref T v2);
    }

    public sealed class RefUtilsAopProxy : IRefUtils
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IRefUtils _realInstance;

        public RefUtilsAopProxy(
            IRefUtils realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public void Swap<T>(ref T v1, ref T v2)
        {
            var genericArgumentT = GenericArgumentInfo.Create(typeof (T), nameof(T));
            var invocationInfo = InvocationInfo.Create(
                typeof (RefUtilsAopProxy),
                nameof(Swap),
                new[]
                {
                    ArgumentInfo.Create(typeof (T), nameof(v1), v1, ArgumentSpecialType.Ref, genericArgumentT),
                    ArgumentInfo.Create(typeof (T), nameof(v2), v2, ArgumentSpecialType.Ref, genericArgumentT)
                },
                new[]
                {
                    genericArgumentT
                },
                Swap<T>);

            var result = _interceptionHandler.Invoke(invocationInfo);

            const int v1Index = 0;
            const int v2Index = 1;
            v1 = (T) result.Arguments[v1Index].Value;
            v2 = (T) result.Arguments[v2Index].Value;
        }

        private InvocationInfo Swap<T>(InvocationInfo invocationInfo)
        {
            const int v1Index = 0;
            const int v2Index = 1;

            T v1Param = (T) invocationInfo.Arguments[v1Index].Value;
            T v2Param = (T) invocationInfo.Arguments[v2Index].Value;
            _realInstance.Swap(ref v1Param, ref v2Param);
            invocationInfo.Arguments[v1Index].Value = v1Param;
            invocationInfo.Arguments[v2Index].Value = v2Param;

            return invocationInfo;
        }
    }
}