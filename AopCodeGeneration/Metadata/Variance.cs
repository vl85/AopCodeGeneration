namespace AopCodeGeneration.Metadata
{
    public enum Variance
    {
        /// <summary>
        /// no modifier
        /// </summary>
        Invariant,

        /// <summary>
        /// out, for return types
        /// i.e. IEnumerable&lt;Base&gt; bIEnum = new List&lt;Derived&gt;();
        /// </summary>
        Covariant,

        /// <summary>
        /// in, for input types
        /// i.e. using IComparer&lt;Base&gt; for Derived
        /// </summary>
        Contravariant
    }
}