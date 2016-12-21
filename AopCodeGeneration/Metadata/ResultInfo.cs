using System;

namespace AopCodeGeneration.Metadata
{
    public sealed class ResultInfo
    {
        public Type Type { get; }

        public object Value { get; set; }

        //public Exception Exception { get; internal set; }

        public GenericArgumentInfo GenericArgumentInfo { get; }

        public ResultInfo(Type type, GenericArgumentInfo genericArgumentInfo)
        {
            Type = type;
            GenericArgumentInfo = genericArgumentInfo;
        }

        public static ResultInfo Create(Type type)
        {
            return new ResultInfo(type, null);
        }

        public static ResultInfo Create(Type type, GenericArgumentInfo genericArgumentInfo)
        {
            return new ResultInfo(type, genericArgumentInfo);
        }
    }
}