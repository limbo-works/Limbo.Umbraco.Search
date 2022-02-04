using Limbo.Umbraco.Search.Models.Groups;

namespace Limbo.Umbraco.Search.Options.Groups {

    /// <summary>
    /// Class representing the options for a grouped search.
    /// </summary>
    public class GroupedSearchOptionsBase {

        /// <summary>
        /// Gets the limit of the specified <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="result">When this method returns, holds the limit of <paramref name="group"/> if specified; otherwise <c>0</c>.</param>
        /// <returns><c>true</c> if a limit was found; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetLimit(SearchGroup group, out int result) {
            result = 0;
            return false;
        }
        
        /// <summary>
        /// Gets the offset of the specified <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="result">When this method returns, holds the offset of <paramref name="group"/> if specified; otherwise <c>0</c>.</param>
        /// <returns><c>true</c> if a offset was found; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetOffset(SearchGroup group, out int result) {
            result = 0;
            return false;
        }

    }

}