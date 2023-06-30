# Options

A part of Limbo Search uses a concept around options classes that may be used to build up search queries. The general idea is than an options class can wrap all the Examine specific search logic such that other parts of your code can be implemented without requiring to interact directly with Examine.

For instance for a hypotetical site, you could have an `EmployeeSearchOptions` class that wraps up the logic for searching specifically for employees, and then another `NewsSearchOptions` class that wraps up the logic for searching for news articles.

Limbo Search comes with a bunch of options related interfaces and base classes that you can implement and extend in order to control how the search is performed.

## ISearchOptions

At the center is the `ISearchOptions` interface. It does nothing more than requiring you to implement a `GetBooleanOperation` method, which returns an instance of Examine's `IBooleanOperation`.

To start out simple, an `EmployeeSearchOptions` class could limit the search to all nodes where the node type alias is `employee`. Eg. such as:

```csharp
public class EmployeeSearchOptions : ISearchOptions {

    public IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query) {

        return query.NativeQuery("__NodeTypeAlias:employee");

    }

}
```

Since the options class implements the `ISearchOptions` interface, it can be passed on to `ISearchHelper.Search` method, which will then return an instance of `SearchResultList` representing the overall result.

Each item is an instance of Examine's `ISearchResult`.

```cshtml
@inject ISearchHelper SearchHelper

@{

    // Make the search via the search helper
    SearchResultList  result = SearchHelper.Search(new EmployeeSearchOptions())

    <pre>Total: @result.Total</pre>

    foreach (ISearchResult item in result.Items) {

        <pre>@item.Id</pre>

    }

}
```



## ISearcherOptions

If your options class both implements `ISearchOptions` and `ISearcherOptions`, the `ISearchHelper.Search` method will ask the `ISearcherOptions.Searcher` property for which `ISearcher` to be used for the search (opposed to the external index, which is default).

The property being a property doesn't really allow for advanced logic, so usage may be limited. But other code may set the property:

```csharp
public class EmployeeSearchOptions : ISearchOptions, IGetSearcherOptions {

    public ISearcher? Searcher { get; set; }

    public IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query) {
        return query.NativeQuery("__NodeTypeAlias:employee");
    }

}
```



## IGetSearcherOptions 

The `IGetSearcherOptions.GetSearcher` method works in the same way as the `ISearcherOptions.Searcher` property, but since it's a method, a few useful dependencies are passed along via the methods's parameter.

For instance, the `IExamineManager` instance as shown in the example below can be used to get a reference to the members index, which we then can use to get the searcher for the index.

The `GetSearcher` return value is nullable, meaning if a `null` is returned, the `ISearchHelper.Search` will default back to the external index.

```csharp
public class EmployeeSearchOptions : ISearchOptions, IGetSearcherOptions {

    public IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query) {
        return query.NativeQuery("__NodeTypeAlias:employee");
    }

    public ISearcher? GetSearcher(IExamineManager examineManager, ISearchHelper searchHelper) {
        if (examineManager.TryGetIndex("MemberIndex", out IIndex? index)) return index.Searcher;
        return null;
    }

}
```



## ILeadingWildcardSearchOptions

Eg. when extended a class like `SearchOptionsBase`, the `Text` property onyl supports trailing wildcards - a wildcard at the end of each work. This means that when searching for a **bike**, your search will also match words like **bikes** and **biker**.

Leading wildcards - a wildcard at the start of each word - are not supported by default. Among other things, this may require a bit more performance as Lucene indexes aren't optimized for this. But by having your search options class implement the `ILeadingWildcardSearchOptions` interface, you can set it's `AllowLeadingWildcard` to `true`, and a search for **ike** will match **bike** as well as **bikes** and **biker**. But also **Mike** and **hike** - so be careful that your search isn't search too broad.

```csharp
public class EmployeeSearchOptions : SearchOptionsBase {

    public bool? AllowLeadingWildcard { get; set; }

    public EmployeeSearchOptions() {
        AllowLeadingWildcard = true;
    }

}
```

When using `SearchOptionsBase` and enabling leading wildcards, this will apply to all text fields returned by the `SearchOptionsBase.GetTextFields` method.




## SearchOptionsBase

This package very much works by creating an options class that then implements the interfaces for the specific features that you need. In a new solution, this can be a bit cumbersome, so to get going more quickly, the package also features the `SearchOptionsBase` class, that you can extend to get the most basic logic right away.

<a href="./SearchOptionsBase.md" class="cta">Read more about the <strong>SearchOptionsBase</strong> class</a>
