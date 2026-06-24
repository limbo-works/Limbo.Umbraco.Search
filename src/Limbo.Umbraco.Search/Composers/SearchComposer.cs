using Limbo.Umbraco.Search.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable 1591

namespace Limbo.Umbraco.Search.Composers;

public class SearchComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {
        builder.Services.AddSingleton<IPackageManifestReader, SearchPackageManifestReader>();
        builder.Services.AddSingleton<ISearchHelper, SearchHelper>();
        builder.Services.AddSingleton<IIndexingHelper, IndexingHelper>();
    }

}