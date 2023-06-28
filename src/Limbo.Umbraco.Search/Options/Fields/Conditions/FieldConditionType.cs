namespace Limbo.Umbraco.Search.Options.Fields.Conditions {

    /// <summary>
    /// Indicates the comparison type of a <see cref="FieldCondition"/>.
    /// </summary>
    public enum FieldConditionType {

        /// <summary>
        /// Indicates that the field value should be equal to the value of the condition.
        /// </summary>
        Equals,

        /// <summary>
        /// Indicates that the field value should contain to the value of the condition.
        /// </summary>
        Contains

    }

}