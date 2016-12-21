using System;

namespace AopCodeGeneration.Metadata
{
    public sealed class GenericArgumentInfo
    {
        public Type Type { get; }

        public string Name { get; }

        public int Index { get; }

        public Variance Variance { get; }

        public GenericSource GenericSource { get; }

        public GenericArgumentRestriction[] Restrictions { get; }

        public GenericArgumentInfo(
            Type type,
            string name,
            int index,
            Variance variance,
            GenericSource genericSource,
            GenericArgumentRestriction[] restrictions)
        {
            Type = type;
            Name = name;
            Index = index;
            Variance = variance;
            GenericSource = genericSource;
            Restrictions = restrictions;
        }

        public static GenericArgumentInfo Create(Type type, string name)
        {
            return new GenericArgumentInfo(type, name, 0, Variance.Invariant, GenericSource.Method, GenericArgumentRestriction.EmptyArray);
        }

        public static GenericArgumentInfo Create(Type type, string name, GenericArgumentRestriction[] restrictions)
        {
            return new GenericArgumentInfo(type, name, 0, Variance.Invariant, GenericSource.Method, restrictions);
        }

        public static GenericArgumentInfo Create(Type type, string name, int index)
        {
            return new GenericArgumentInfo(type, name, index, Variance.Invariant, GenericSource.Method, GenericArgumentRestriction.EmptyArray);
        }

        public static GenericArgumentInfo Create(Type type, string name, int index, GenericArgumentRestriction[] restrictions)
        {
            return new GenericArgumentInfo(type, name, index, Variance.Invariant, GenericSource.Method, restrictions);
        }

        public static GenericArgumentInfo Create(Type type, string name, Variance variance, GenericSource genericSource)
        {
            return new GenericArgumentInfo(type, name, 0, variance, genericSource, GenericArgumentRestriction.EmptyArray);
        }

        public static GenericArgumentInfo Create(Type type, string name, Variance variance, GenericSource genericSource, GenericArgumentRestriction[] restrictions)
        {
            return new GenericArgumentInfo(type, name, 0, variance, genericSource, restrictions);
        }

        public static GenericArgumentInfo Create(Type type, string name, int index, Variance variance, GenericSource genericSource)
        {
            return new GenericArgumentInfo(type, name, index, variance, genericSource, GenericArgumentRestriction.EmptyArray);
        }

        public static GenericArgumentInfo Create(Type type, string name, int index, Variance variance, GenericSource genericSource, GenericArgumentRestriction[] restrictions)
        {
            return new GenericArgumentInfo(type, name, index, variance, genericSource, restrictions);
        }

        public static GenericArgumentInfo[] EmptyArray { get; } = new GenericArgumentInfo[0];
    }
}