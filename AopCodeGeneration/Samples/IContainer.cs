using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface IContainer
    {
        T GetElement<T>(int index);
        void SetElement<T>(int index, T value);

        bool GetElement(int index, out object value);
    }

    public sealed class ContainerAopProxy : IContainer
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IContainer _realInstance;

        public ContainerAopProxy(
            IContainer realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public T GetElement<T>(int index)
        {
            var genericArgumentT = GenericArgumentInfo.Create(typeof (T), nameof(T));
            var invocationInfo = InvocationInfo.Create(
                typeof (ContainerAopProxy),
                nameof(GetElement),
                new[]
                {
                    ArgumentInfo.Create(typeof (int), nameof(index), index, genericArgumentT)
                },
                new[]
                {
                    genericArgumentT
                },
                ResultInfo.Create(typeof (T), genericArgumentT),
                GetElement<T>);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (T) result.Result.Value;
        }

        private InvocationInfo GetElement<T>(InvocationInfo invocationInfo)
        {
            const int indexIndex = 0;
            var result = _realInstance.GetElement<T>(
                (int)invocationInfo.Arguments[indexIndex].Value);

            invocationInfo.Result.Value = result;

            return invocationInfo;
        }

        public void SetElement<T>(int index, T value)
        {
            var genericArgumentT = GenericArgumentInfo.Create(typeof (T), nameof(T));
            var invocationInfo = InvocationInfo.Create(
                typeof (ContainerAopProxy),
                nameof(SetElement),
                new[]
                {
                    ArgumentInfo.Create(typeof (int), nameof(index), index),
                    ArgumentInfo.Create(typeof (T), nameof(value), value, genericArgumentT)
                },
                new[]
                {
                    genericArgumentT
                },
                SetElement<T>);

            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo SetElement<T>(InvocationInfo invocationInfo)
        {
            const int indexIndex = 0;
            const int valueIndex = 0;
            _realInstance.SetElement(
                (int) invocationInfo.Arguments[indexIndex].Value,
                (T) invocationInfo.Arguments[valueIndex].Value);

            return invocationInfo;
        }

        public bool GetElement(int index, out object value)
        {
            var invocationInfo = InvocationInfo.Create(
                typeof (ContainerAopProxy),
                nameof(GetElement),
                new[]
                {
                    ArgumentInfo.Create(typeof (int), nameof(index), index),
                    ArgumentInfo.Create(typeof (object), nameof(value), default(object), ArgumentSpecialType.Out)
                },
                ResultInfo.Create(typeof (bool)),
                GetElement);

            var result = _interceptionHandler.Invoke(invocationInfo);
            const int valueIndex = 1;
            value = result.Arguments[valueIndex].Value;

            return (bool) result.Result.Value;
        }

        private InvocationInfo GetElement(InvocationInfo invocationInfo)
        {
            const int indexIndex = 0;
            const int valueIndex = 1;

            object valueParam;
            var result = _realInstance.GetElement(
                (int)invocationInfo.Arguments[indexIndex].Value,
                out valueParam
                );
            invocationInfo.Arguments[valueIndex].Value = valueParam;
            invocationInfo.Result.Value = result;

            return invocationInfo;
        }
    }
}