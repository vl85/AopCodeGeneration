using System;

namespace AopCodeGeneration.Metadata
{
    public sealed class InvocationInfo
    {
        //TODO interface or real implementation?
        public Type ContainingType { get; }

        public string MethodName { get; }

        public InvocationSpecialType InvocationSpecialType { get; }

        public ArgumentInfo[] Arguments { get; }

        public ResultInfo Result { get; }

        public bool IsMethodGeneric => MethodGenericArguments?.Length > 0;

        public GenericArgumentInfo[] MethodGenericArguments { get; }

        public GenericArgumentInfo[] ClassGenericArguments { get; }

        public Func<InvocationInfo, InvocationInfo> RealFunc { get; }

        public InvocationInfo(
            Type containingType,
            string methodName,
            ArgumentInfo[] arguments,
            ResultInfo result,
            GenericArgumentInfo[] methodGenericArguments,
            GenericArgumentInfo[] classGenericArguments,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            ContainingType = containingType;
            MethodName = methodName;
            Arguments = arguments;
            Result = result;
            MethodGenericArguments = methodGenericArguments;
            ClassGenericArguments = classGenericArguments;
            RealFunc = realFunc;
            InvocationSpecialType = invocationSpecialType;
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                new ArgumentInfo[0],
                ResultInfo.Create(typeof (void)),
                GenericArgumentInfo.EmptyArray,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            ResultInfo result,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                new ArgumentInfo[0],
                result,
                GenericArgumentInfo.EmptyArray,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            ArgumentInfo[] arguments,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                arguments,
                ResultInfo.Create(typeof(void)),
                GenericArgumentInfo.EmptyArray,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            ArgumentInfo[] arguments,
            ResultInfo result,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                arguments,
                result,
                GenericArgumentInfo.EmptyArray,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            ArgumentInfo[] arguments,
            GenericArgumentInfo[] methodGenericArguments,
            ResultInfo result,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                arguments,
                result,
                methodGenericArguments,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            ArgumentInfo[] arguments,
            GenericArgumentInfo[] methodGenericArguments,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                arguments,
                ResultInfo.Create(typeof(void)),
                methodGenericArguments,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            GenericArgumentInfo[] methodGenericArguments,
            ResultInfo result,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                new ArgumentInfo[0],
                result,
                methodGenericArguments,
                GenericArgumentInfo.EmptyArray,
                realFunc,
                invocationSpecialType);
        }

        public static InvocationInfo Create(
            Type type,
            string name,
            GenericArgumentInfo[] methodGenericArguments,
            GenericArgumentInfo[] classGenericArguments,
            ResultInfo result,
            Func<InvocationInfo, InvocationInfo> realFunc,
            InvocationSpecialType invocationSpecialType = InvocationSpecialType.None)
        {
            return new InvocationInfo(
                type,
                name,
                new ArgumentInfo[0],
                result,
                methodGenericArguments,
                classGenericArguments,
                realFunc,
                invocationSpecialType);
        }
    }
}