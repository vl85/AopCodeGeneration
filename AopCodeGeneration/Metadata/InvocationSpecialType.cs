namespace AopCodeGeneration.Metadata
{
    public enum InvocationSpecialType
    {
        None,
        PropertyGet,
        PropertySet,
        EventAdd,
        EventRemove
    }

    public static class InvocationSpecialTypeExtensions
    {
        public static bool IsProperty(this InvocationSpecialType invocationSpecialType)
        {
            return invocationSpecialType == InvocationSpecialType.PropertyGet
                   || invocationSpecialType == InvocationSpecialType.PropertySet;
        }

        public static bool IsEvent(this InvocationSpecialType invocationSpecialType)
        {
            return invocationSpecialType == InvocationSpecialType.EventAdd
                   || invocationSpecialType == InvocationSpecialType.EventRemove;
        }

        public static bool IsSpecial(this InvocationSpecialType invocationSpecialType)
        {
            return invocationSpecialType != InvocationSpecialType.None;
        }
    }
}
