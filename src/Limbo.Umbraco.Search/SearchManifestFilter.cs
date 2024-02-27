using System.Collections.Generic;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.Search;

/// <inheritdoc />
public class SearchManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageId = SearchPackage.Alias,
            PackageName = SearchPackage.Name,
            Version = SearchPackage.InformationalVersion
        };

        // Append the manifest
        manifests.Add(manifest);

    }

}