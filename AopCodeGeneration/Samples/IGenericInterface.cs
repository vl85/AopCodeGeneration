using System;
using AopCodeGeneration.Behaviour;
using AopCodeGeneration.Metadata;

namespace AopCodeGeneration.Samples
{
    public interface IGenericInterface<out T, out TA>
        where T : class, IDisposable, new()
        where TA : struct
    {
        T GetX();

        TA GetY();
    }

    //TODO what about variant generic parameters in implementation?
    public sealed class GenericInterfaceAopProxy<T, TA> : IGenericInterface<T, TA>
        where T : class, IDisposable, new()
        where TA : struct
    {
        private readonly InterceptionHandler _interceptionHandler;
        private readonly IGenericInterface<T, TA> _realInstance;

        public GenericInterfaceAopProxy(
            IGenericInterface<T, TA> realInstance,
            InterceptionHandler interceptionHandler) : this()
        {
            _realInstance = realInstance;
            _interceptionHandler = interceptionHandler;
        }

        private const int ClassGenericArgumentTIndex = 0;
        private const int ClassGenericArgumentTaIndex = 1; //TODO not to forget about naming or generated code marks
        private readonly GenericArgumentInfo _classGenericArgumentT;
        private readonly GenericArgumentInfo _classGenericArgumentTa;
        private readonly GenericArgumentInfo[] _classGenericArguments;

        public GenericInterfaceAopProxy()
        {
            _classGenericArgumentT = GenericArgumentInfo.Create(
                typeof (T),
                nameof(T),
                ClassGenericArgumentTIndex,
                Variance.Covariant,
                GenericSource.Class,
                new[]
                {
                    GenericArgumentRestriction.Create(GenericArgumentRestrictionType.Class),
                    GenericArgumentRestriction.Create(typeof (IDisposable)),
                    GenericArgumentRestriction.Create(GenericArgumentRestrictionType.New)
                });

            _classGenericArgumentTa = GenericArgumentInfo.Create(
                typeof (TA),
                nameof(TA),
                ClassGenericArgumentTaIndex,
                Variance.Covariant,
                GenericSource.Class,
                new[]
                {
                    GenericArgumentRestriction.Create(GenericArgumentRestrictionType.Struct)
                });

            _classGenericArguments = new[]
            {
                _classGenericArgumentT,
                _classGenericArgumentTa
            };
        }

        public T GetX()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(ContainerAopProxy),
                nameof(GetX),
                GenericArgumentInfo.EmptyArray,
                _classGenericArguments,
                ResultInfo.Create(typeof(T), _classGenericArgumentT),
                GetX);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (T)result.Result.Value;
        }

        private InvocationInfo GetX(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.GetX();

            return invocationInfo;
        }

        public TA GetY()
        {
            var invocationInfo = InvocationInfo.Create(
                typeof(ContainerAopProxy),
                nameof(GetY),
                GenericArgumentInfo.EmptyArray,
                _classGenericArguments,
                ResultInfo.Create(typeof(TA), _classGenericArgumentTa),
                GetY);

            var result = _interceptionHandler.Invoke(invocationInfo);

            return (TA)result.Result.Value;
        }

        private InvocationInfo GetY(InvocationInfo invocationInfo)
        {
            invocationInfo.Result.Value = _realInstance.GetY();

            return invocationInfo;
        }
    }
}