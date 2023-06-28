using Limbo.Umbraco.Search.Models.Groups;
using Microsoft.AspNetCore.Http;
using Skybrud.Essentials.AspNetCore;

namespace Limbo.Umbraco.Search.Options.Groups {

    /// <summary>
    /// Class representing the search options of a search group.
    /// </summary>
    public class GroupSearchOptionsBase {

        /// <summary>
        /// Gets the text parameter of the group.
        /// </summary>
        public string? Text { get; }

        /// <summary>
        /// Gets the current limit of the group.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Gets the current offset of the group.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="group"/> and <paramref name="request"/>.
        /// </summary>
        /// <param name="group">The group to based the options on.</param>
        /// <param name="request">The inbound HTTP request.</param>
        public GroupSearchOptionsBase(SearchGroup group, HttpRequest request) {
            Text = request.Query.GetString("text");
            Limit = request.Query.GetInt32($"l{group.Id}", group.Limit);
            Offset = request.Query.GetInt32($"o{group.Id}");
        }

    }

}