using System;
using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface IInterfaceWithGenericMethod
    {
        T GetFoo<T>() where T : class;

        T GetBar<T>() where T : struct;

        void SetBoo<T, TA>(T t, TA a) 
            where T : class, IDisposable, new()
            where TA : new();
    }

    public sealed class InterfaceWithGenericMethodAopProxy : IInterfaceWithGenericMethod
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IInterfaceWithGenericMethod _realInstance;

        public InterfaceWithGenericMethodAopProxy(
            IInterfaceWithGenericMethod realInstance,
            InterceptionHandler interceptionHandler)
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        public T GetFoo<T>() where T : class
        {
            var genericArgument1 = GenericArgumentInfo.Create(
                typeof (T),
                nameof(T),
                new[] {GenericArgumentRestriction.Create(GenericArgumentRestrictionType.Class)});

            var invocationInfo = InvocationInfo.Create(
                typeof(InterfaceWithGenericMethodAopProxy),
                nameof(GetFoo),
                new[]
                {
                    genericArgument1
                },
                ResultInfo.Create(typeof(T), genericArgument1),
                GetFoo<T>);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (T)result.Result.Value;
        }

        private InvocationInfo GetFoo<T>(InvocationInfo invocationInfo) where T : class
        {
            invocationInfo.Result.Value = _realInstance.GetFoo<T>();

            return invocationInfo;
        }

        public T GetBar<T>() where T : struct
        {
            var genericArgumentT = GenericArgumentInfo.Create(
                typeof(T),
                nameof(T),
                new[] { GenericArgumentRestriction.Create(GenericArgumentRestrictionType.Struct) });

            var invocationInfo = InvocationInfo.Create(
                typeof(InterfaceWithGenericMethodAopProxy),
                nameof(GetBar),
                new[]
                {
                    genericArgumentT
                },
                ResultInfo.Create(typeof(T), genericArgumentT),
                GetBar<T>);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (T)result.Result.Value;
        }

        private InvocationInfo GetBar<T>(InvocationInfo invocationInfo) where T : struct
        {
            invocationInfo.Result.Value = _realInstance.GetBar<T>();

            return invocationInfo;
        }

        public void SetBoo<T, TA>(T t, TA a)
            where T : class, IDisposable, new()
            where TA : new()
        {
            var genericArgumentT = GenericArgumentInfo.Create(
                typeof (T),
                nameof(T),
                new[]
                {
                    GenericArgumentRestriction.Create(GenericArgumentRestrictionType.Class),
                    GenericArgumentRestriction.Create(typeof (IDisposable)),
                    GenericArgumentRestriction.Create(GenericArgumentRestrictionType.New)
                });

            var genericArgumentTa = GenericArgumentInfo.Create(
                typeof (TA),
                nameof(TA),
                new[] {GenericArgumentRestriction.Create(GenericArgumentRestrictionType.New)});

            var invocationInfo = InvocationInfo.Create(
                typeof (InterfaceWithGenericMethodAopProxy),
                nameof(GetBar),
                new[]
                {
                    ArgumentInfo.Create(typeof (T), nameof(t), t, genericArgumentT),
                    ArgumentInfo.Create(typeof (TA), nameof(a), a, genericArgumentTa),
                },
                new[]
                {
                    genericArgumentT,
                    genericArgumentTa
                },
                SetBoo<T, TA>);

            _interceptionHandler.Invoke(invocationInfo);
        }

        private InvocationInfo SetBoo<T, TA>(InvocationInfo invocationInfo)
            where T : class, IDisposable, new()
            where TA : new()
        {
            const int tIndex = 0;
            const int aIndex = 0;
            _realInstance.SetBoo(
                (T)invocationInfo.Arguments[tIndex].Value,
                (TA)invocationInfo.Arguments[aIndex].Value);

            return invocationInfo;
        }
    }
}