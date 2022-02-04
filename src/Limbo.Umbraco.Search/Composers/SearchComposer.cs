using Limbo.Umbraco.Search.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

#pragma warning disable 1591

namespace Limbo.Umbraco.Search.Composers {

    public class SearchComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<ISearchHelper, SearchHelper>();
            builder.Services.AddTransient<IIndexingHelper, IndexingHelper>();
        }
    }

}