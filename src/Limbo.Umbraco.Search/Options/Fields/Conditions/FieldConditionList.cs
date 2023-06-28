﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Limbo.Umbraco.Search.Options.Fields.Conditions {

    /// <summary>
    /// Class representing a list of field conditions.
    /// </summary>
    public class FieldConditionList : IEnumerable<FieldCondition> {

        private readonly List<FieldCondition> _list;

        #region Properties

        /// <summary>
        /// Gets the amount of field conditions that have been added to the list.
        /// </summary>
        public int Count => _list.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new empty list.
        /// </summary>
        public FieldConditionList() {
            _list = new List<FieldCondition>();
        }

        /// <summary>
        /// Initializes a new list populated with the specified <paramref name="conditions"/>.
        /// </summary>
        /// <param name="conditions">A collection of conditions.</param>
        public FieldConditionList(IEnumerable<FieldCondition> conditions) {
            _list = conditions.ToList();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Adds the specified <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The condition to be added.</param>
        public void Add(FieldCondition condition) {
            _list.Add(condition);
        }

        /// <summary>
        /// Adds the specified <paramref name="conditions"/>.
        /// </summary>
        /// <param name="conditions">The conditions to be added.</param>
        public void AddRange(IEnumerable<FieldCondition> conditions) {
            _list.AddRange(conditions);
        }

        /// <inheritdoc />
        public IEnumerator<FieldCondition> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Operator overloading

        /// <summary>
        /// Initializes a new list populated with the specified <paramref name="conditions"/>.
        /// </summary>
        /// <param name="conditions">A collection of conditions.</param>
        public static implicit operator FieldConditionList(List<FieldCondition> conditions) {
            return new FieldConditionList(conditions);
        }

        /// <summary>
        /// Initializes a new list populated with the specified <paramref name="conditions"/>.
        /// </summary>
        /// <param name="conditions">A collection of conditions.</param>
        public static implicit operator FieldConditionList(FieldCondition[] conditions) {
            return new FieldConditionList(conditions);
        }

        #endregion

    }

}