# ISearchOptions

The `ISearchOptions` interface is the primary interface used to describe options for an Examine based search via this package.

It's goal is serve as basioc interface that should be combined with other interfaces, and as such, it doesn't really do much on it's own. It does however require classes to implement the `GetBooleanOperation` method, which returns an instance of Examine's `IBooleanOperation`.

In the example below, the method has been implemented with an `IBooleanOperation` to match all pages where the field `hello` contains the word `there`:

```csharp
public class MySearchOptions : ISearchOptions {

    public virtual IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query) {
        return query.NativeQuery("hello:there");
    }   

}