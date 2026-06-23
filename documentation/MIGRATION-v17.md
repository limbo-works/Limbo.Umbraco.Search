# Migration to Umbraco 17

This document describes the changes made to migrate **Limbo.Umbraco.Search** from Umbraco 10–12 (.NET 6) to **Umbraco 17 (.NET 10)**.

## Summary

| Aspect | Before | After |
| --- | --- | --- |
| Target framework | `net6` | `net10.0` |
| C# language version | `11.0` | `13.0` |
| Umbraco | `[10.0.0,12.999)` | `[17.0.0,18)` |
| Package version | `2.0.0-alpha009` | `17.0.0-alpha001` |

The bulk of the package (Examine integration, indexing extensions, search helpers, query/options model) compiled unchanged — those APIs are stable across Umbraco 10 → 17. Only the package-manifest registration required a code change.

## Changes

### 1. Project file — `src/Limbo.Umbraco.Search/Limbo.Umbraco.Search.csproj`

- `TargetFramework`: `net6` → `net10.0`
- `LangVersion`: `11.0` → `13.0`
- `Umbraco.Cms.Core`: `[10.0.0,12.999)` → `[17.0.0,18)`
- `Umbraco.Cms.Web.Website`: `[10.0.0,12.999)` → `[17.0.0,18)`
- `Skybrud.Essentials.AspNetCore`: `1.0.0` → `1.0.2`
- `VersionPrefix`: `2.0.0-alpha009` → `17.0.0-alpha001`
- `Description`: "Search package for Umbraco 10+." → "Search package for Umbraco 17+."

### 2. Breaking change — `IManifestFilter` removed in Umbraco 14+

The old backoffice manifest system was removed when the new (Bellissima) backoffice landed in Umbraco 14. `IManifestFilter`, `PackageManifest` filtering, and `IUmbracoBuilder.ManifestFilters()` no longer exist. The replacement for server-side / backend-only packages is the `IPackageManifestReader` interface (`Umbraco.Cms.Infrastructure.Manifest`), whose implementations are collected by `PackageManifestService`.

**Removed:** `SearchManifestFilter.cs`

It implemented `IManifestFilter` and used reflection to set `PackageId` (which did not exist before Umbraco 12).

**Added:** `SearchPackageManifestReader.cs`

Implements `IPackageManifestReader` and returns a single `PackageManifest`. `PackageId`/`Id` is now a first-class property, so the reflection workaround was dropped.

```csharp
public class SearchPackageManifestReader : IPackageManifestReader {
    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {
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
```

> Note: `AllowTelemetry` is used instead of `AllowPackageTelemetry`, which is now obsolete and scheduled for removal in Umbraco 18.

### 3. Composer — `Composers/SearchComposer.cs`

The manifest filter registration was replaced with a DI registration of the new reader:

```diff
- builder.ManifestFilters().Append<SearchManifestFilter>();
+ builder.Services.AddTransient<IPackageManifestReader, SearchPackageManifestReader>();
```

(Added `using Umbraco.Cms.Infrastructure.Manifest;`.)

### 4. Documentation — `README.md`

Updated Umbraco version (10 → 17), target framework (.NET 6 → .NET 10), and installation instructions.

## What did **not** change

These Umbraco/Examine APIs are stable across 10 → 17 and required no edits:

- `IExamineManager`, `IIndex`, `ISearcher`, `IBooleanOperation`, `IQuery`, `QueryOptions`
- `IndexingItemEventArgs` / `ValueSet` (Examine indexing pipeline)
- `Examine.Lucene` providers (`BaseLuceneSearcher.LuceneAnalyzer`, `SortableField`)
- `IUmbracoContextAccessor`, `IPublishedContent`, `IPublishedElement`
- `BlockListModel` / `BlockListItem`
- `UdiParser`, `Constants.UmbracoIndexes.*`

## Build status

`dotnet build -c Release` and `dotnet pack -c Release` both succeed against Umbraco 17 / .NET 10 with **0 errors and 0 code warnings**.

The only remaining build warnings are `NU1902` / `NU1903` transitive-dependency vulnerability advisories (MailKit, MessagePack, MimeKit, System.Security.Cryptography.Xml) inherited from Umbraco 17's own dependency tree — they are not referenced directly by this package and are resolved by Umbraco patch releases.

## Notes for consumers

- Requires the .NET 10 SDK/runtime and an Umbraco 17 host.
- This is a backend-only package (no `App_Plugins` / client-side manifest), so no `umbraco-package.json` or backoffice JavaScript migration was needed.
- No public API surface of the package changed — search/indexing usage in consuming projects continues to work as before.
