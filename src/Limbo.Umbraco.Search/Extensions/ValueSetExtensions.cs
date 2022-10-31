using System;
using System.Collections.Generic;
using Examine;

namespace Limbo.Umbraco.Search.Extensions {

    /// <summary>
    /// Static class with various extension methods for <see cref="ValueSet"/>.
    /// </summary>
    public static class ValueSetExtensions {
        
        /// <summary>
        /// Adds a value to the keyed item, if it doesn't exist the key will be created.
        /// </summary>
        /// <param name="valueSet">The value set.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns>The number of items stored for the key.</returns>
        public static int Add(this ValueSet valueSet, string key, object value) {

            if (valueSet.Values is not IDictionary<string, IReadOnlyList<object>> dictionary) {
                throw new Exception($"'{nameof(valueSet.Values)}' is not an instance of '{typeof(IDictionary<string, IReadOnlyList<object>>)}'");
            }

            if (!dictionary.TryGetValue(key, out IReadOnlyList<object>? values)) {
                dictionary.Add(key, values = new List<object>());
            }

            if (values is not List<object> list) {
                dictionary[key] = list = new List<object>(values);
            }
            
            list.Add(value);
            
            return values.Count;

        }

        /// <summary>
        /// Sets a value to the keyed item, if it doesn't exist the key will be created.
        /// </summary>
        /// <param name="valueSet">The value set.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to be added.</param>
        public static void Set(this ValueSet valueSet, string key, object value) {

            if (valueSet.Values is not IDictionary<string, IReadOnlyList<object>> dictionary) {
                throw new Exception($"'{nameof(valueSet.Values)}' is not an instance of '{typeof(IDictionary<string, IReadOnlyList<object>>)}'");
            }
            
            dictionary[key] = new List<object> { value };

        }


        /// <summary>
        /// Attempts to add the specified <paramref name="value"/> to the item with <paramref name="key"/>, if the item does not already exist.
        /// </summary>
        /// <param name="valueSet">The value set.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to be added.</param>
        /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>.</returns>
        public static bool TryAdd(this ValueSet valueSet, string key, object value) {
            
            if (valueSet.Values.ContainsKey(key)) return false;

            if (valueSet.Values is not IDictionary<string, IReadOnlyList<object>> dictionary) {
                throw new Exception($"'{nameof(valueSet.Values)}' is not an instance of '{typeof(IDictionary<string, IReadOnlyList<object>>)}'");
            }

            dictionary.Add(key, new List<object> { value });
            return true;

        }

    }

}