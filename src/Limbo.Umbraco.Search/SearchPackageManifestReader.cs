using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

namespace Limbo.Umbraco.Search;

// [CHANGE: Umbraco 17 upgrade - IManifestFilter removed in Umbraco 14+, replaced by IPackageManifestReader] Related: Composers/SearchComposer.cs, Limbo.Umbraco.Search.csproj
/// <summary>
/// Package manifest reader used to identify the package (e.g. for telemetry). This replaces the
/// <c>IManifestFilter</c> approach used prior to Umbraco 14, which was removed alongside the new backoffice.
/// </summary>
public class SearchPackageManifestReader : IPackageManifestReader {

    /// <inheritdoc />
    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        // Initialize a new manifest for this package
        PackageManifest manifest = new() {
            Id = SearchPackage.Alias,
            Name = SearchPackage.Name,
            Version = SearchPackage.InformationalVersion,
            AllowTelemetry = true,
            Extensions = []
        };

        return Task.FromResult<IEnumerable<PackageManifest>>(new[] { manifest });

    }

}
