using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.Search;

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

        return Task.FromResult<IEnumerable<PackageManifest>>([manifest]);

    }

}
