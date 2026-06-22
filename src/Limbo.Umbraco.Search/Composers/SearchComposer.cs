using Limbo.Umbraco.Search.Indexing;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable 1591

namespace Limbo.Umbraco.Search.Composers;

public class SearchComposer : IComposer {

    public void Compose(IUmbracoBuilder builder) {
        // [CHANGE: Umbraco 17 upgrade - ManifestFilters() removed, register IPackageManifestReader instead] Related: SearchPackageManifestReader.cs, Limbo.Umbraco.Search.csproj
        builder.Services.AddTransient<IPackageManifestReader, SearchPackageManifestReader>();
        builder.Services.AddTransient<ISearchHelper, SearchHelper>();
        builder.Services.AddTransient<IIndexingHelper, IndexingHelper>();
    }

}