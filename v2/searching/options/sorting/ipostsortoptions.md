# IPostSortOptions

Inherits: [**`ISearchOptions`**](./isearchoptions.md)

By default Examine will return matching results sorted on a score calculated on from which fields are match, boost values similar. Examine supports sorting the results by a specific field before the results are returned to you, and ideally you should strive to use this when sorting results.

But in some cases - eg. with complex values - it's better suitable to sort teh results after you've retrieved them from Examine. This part can be accomplished by implementing the `IPostSortOptions` interface as it then helps you perform a *post sort* of the results.

In the example below, the `NewsSearchOptions` class describes an options class for searching through news pages. At Limbo we're typically adding a custom `contentDate` property, so that editors can control the visible date instead of jsut using the page's creation time. In this case we can use the `OrderByContentDate` from this package to sort the news articles by their content date, in descending order:

```csharp
public class NewsSearchOptions : IPostSortOptions {

    public SortOrder SortOrder { get; set;} = SortOrder.Descending;

    public override IEnumerable<ISearchResult> Sort(IEnumerable<ISearchResult> results, ILogger logger) {
        return results.OrderByContentDate(SortOrder);
    }

    // other properties and methods

}
```