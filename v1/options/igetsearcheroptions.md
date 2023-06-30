# IGetSearcherOptions

Inherits: [**`ISearchOptions`**](./isearchoptions.md)

The package looks for either the `IGetSearcherOptions` interface or the [**`ISearcherOptions**`][./isearcheroptions.md] interface for determining which searcher in Examine to use for the search. If neither interface are used, the `ExternalIndex` index is used by default.

The `IGetSearcherOptions` interface describes a `GetSearcher` method for returning the desisered searcher. This could look like in the example below:

```csharp
public class MySearchOptions : IGetSearcherOptions {

    public virtual ISearcher GetSearcher(IExamineManager examineManager, ISearchHelper searchHelper) {
        return GetSearcherByIndexName(examineManager, searchHelper, ExamineIndexes.ExternalIndexName);
    }

    // other methods

}
```