# ISearcherOptions

Inherits: [**`ISearchOptions`**](./isearchoptions.md)

The package looks for either the [**`IGetSearcherOptions`**](./isgetearcheroptions.md) interface or the `ISearcherOptions` interface for determining which searcher in Examine to use for the search. If neither interface are used, the `ExternalIndex` index is used by default.

The `ISearcherOptions` interface describes a `Searcher` property for returning the desisered searcher. This could look like in the example below:

```csharp
public class MySearchOptions : ISearcherOptions {

    public ISearcher? Searcher { get; set; }

    // other properties and methods methods

}
```

Notice that the interface doesn't describe a setter for the `Searcher` property, but you are free to add this in your implementation of the `ISearcherOptions` interface.