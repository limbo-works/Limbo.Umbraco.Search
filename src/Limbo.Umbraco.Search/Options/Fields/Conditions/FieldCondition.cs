using System;
using System.Linq;
using Examine;

namespace Limbo.Umbraco.Search.Options.Fields.Conditions {

    /// <summary>
    /// Class representing a condition for a field.
    /// </summary>
    public class FieldCondition : IFieldCondition {

        #region Properties

        /// <summary>
        /// Gets or sets the alias of the field to match.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the value to check for.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// gets or sets the type of the condition - eg. <see cref="FieldConditionType.Equals"/>.
        /// </summary>
        public FieldConditionType Type { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new field condition where the field with the <paramref name="alias"/> should be equal to <paramref name="value"/>.
        /// </summary>
        /// <param name="alias">The alias of the field.</param>
        /// <param name="value">The value to match.</param>
        public FieldCondition(string alias, string value) {
            Alias = alias;
            Value = value;
            Type = FieldConditionType.Equals;
        }

        /// <summary>
        /// Initializes a new field condition where the field with the <paramref name="alias"/> should match <paramref name="value"/>.
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="value"></param>
        /// <param name="type">The type of the condition - eg. <see cref="FieldConditionType.Equals"/> or <see cref="FieldConditionType.Contains"/>.</param>
        public FieldCondition(string alias, string value, FieldConditionType type) {
            Alias = alias;
            Value = value;
            Type = type;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns whether the specified <paramref name="result"/> matches this condition.
        /// </summary>
        /// <param name="result">The result to check.</param>
        /// <returns><c>true</c> if <paramref name="result"/> matches this condition; otherwise, <c>false</c>.</returns>
        public bool IsMatch(SearchResult result) {

            string[] values = result.GetValues(Alias).ToArray();

            if (values.Length == 0) return false;

            switch (Type) {

                case FieldConditionType.Equals:
                    return values.Any(x => x.Equals(Value));

                case FieldConditionType.Contains:
                    return values.Any(x => x.Contains(Value));

                default:
                    throw new Exception("Unknown condition type: " + Type);

            }

        }

        #endregion

    }

}