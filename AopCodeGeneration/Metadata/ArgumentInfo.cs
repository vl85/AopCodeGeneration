using System;

namespace AopCodeGeneration.Metadata
{
    public sealed class ArgumentInfo
    {
        public Type Type { get; }

        public string Name { get; }

        public object Value { get; set; }

        public ArgumentSpecialType ArgumentSpecialType { get; }
        
        public GenericArgumentInfo GenericArgumentInfo { get; }

        public ArgumentInfo(
            Type type,
            string name,
            object value,
            ArgumentSpecialType argumentSpecialType,
            GenericArgumentInfo genericArgumentInfo)
        {
            Type = type;
            Name = name;
            Value = value;
            ArgumentSpecialType = argumentSpecialType;
            GenericArgumentInfo = genericArgumentInfo;
        }

        public static ArgumentInfo Create(Type type, string name, object value)
        {
            return new ArgumentInfo(type, name, value, ArgumentSpecialType.None, null);
        }

        public static ArgumentInfo Create(Type type, string name, object value, ArgumentSpecialType argumentSpecialType)
        {
            return new ArgumentInfo(type, name, value, argumentSpecialType, null);
        }

        public static ArgumentInfo Create(Type type, string name, object value, GenericArgumentInfo genericArgumentInfo)
        {
            return new ArgumentInfo(
                type,
                name,
                value,
                ArgumentSpecialType.None,
                genericArgumentInfo);
        }

        public static ArgumentInfo Create(
            Type type,
            string name,
            object value,
            ArgumentSpecialType argumentSpecialType,
            GenericArgumentInfo genericArgumentInfo)
        {
            return new ArgumentInfo(
                type,
                name,
                value,
                argumentSpecialType,
                genericArgumentInfo);
        }
    }
}