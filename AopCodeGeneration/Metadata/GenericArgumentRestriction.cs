using System;

namespace AopCodeGeneration.Metadata
{
    public sealed class GenericArgumentRestriction
    {
        public GenericArgumentRestrictionType RestrictionType { get; }

        public Type Type { get; }

        public GenericArgumentRestriction(GenericArgumentRestrictionType restrictionType, Type type)
        {
            RestrictionType = restrictionType;
            Type = type;
        }

        public static GenericArgumentRestriction Create(GenericArgumentRestrictionType restrictionType)
        {
            return new GenericArgumentRestriction(restrictionType, null);
        }

        public static GenericArgumentRestriction Create(Type type)
        {
            return new GenericArgumentRestriction(GenericArgumentRestrictionType.Type, type);
        }

        public static GenericArgumentRestriction[] EmptyArray { get; } = new GenericArgumentRestriction[0];
    }
}
