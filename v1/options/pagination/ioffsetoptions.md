# IOffsetOptions

Inherits: [**`ISearchOptions`**](./isearchoptions.md)

If you've created a custom options class that only implements the `ISearchOptions` interface, the search will return all matching results, which may not always be ideal.

Therefore the `IOffsetOptions` interface allows you set both an `Offset` and a `Limit`, which then can be used to control which sub set of the matched results that should be returned.

```csharp
public class MySearchOptions : IOffsetOptions {

    public int Offset { get; set; }

    public int Limit { get; set; }

    // other properties and methods

}
```