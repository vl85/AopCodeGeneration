using System.Diagnostics;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Behaviour
{
    public sealed class InterceptionHandler
    {
        private Interceptor[] Interceptors { get; }

        public InterceptionHandler(Interceptor[] interceptors)
        {
            Interceptors = interceptors;
        }

        public InvocationInfo Invoke(InvocationInfo invocationInfo)
        {
            var result = Interceptors.Length == 0
                ? invocationInfo
                : Invoke(invocationInfo, 0);

            //return result.Result.Exception == null
            //    ? invocationInfo.RealFunc(result)
            //    : result;
            return invocationInfo.RealFunc(result);
        }

        private InvocationInfo Invoke(InvocationInfo invocationInfo, int index)
        {
            Debug.Assert(index < Interceptors.Length);

            //InvocationInfo result;
            //try
            //{
            //    result = Interceptors[index].Invoke(invocationInfo);
            //}
            //catch (Exception ex)
            //{
            //    invocationInfo.Result.Exception = ex;
            //    return invocationInfo;
            //}
            var result = Interceptors[index].Invoke(invocationInfo);

            return Interceptors.Length > index + 1
                ? Invoke(result, index + 1)
                : result;
        }
    }
}