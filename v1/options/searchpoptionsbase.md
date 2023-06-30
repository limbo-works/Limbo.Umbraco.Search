# SearchOptionsBase

The `SearchOptionsBase` class is a base class that implements most of the common search functionality that we typically use at Limbo. The class contains a number of properties and method that may be used to customize the search.

It's recommended not to create instances of this class directly, but instead create your own base class that extends the `SearchOptionsBase` class.





## Interfaces

The `SearchOptionsBase` class implements the following interfaces:

- [**`ISearchOptions`**](./isearchoptions.md)
- [**`IGetSearcherOptions`**](./igetsearcheroptions.md)
- [**`IDebugSearchOptions`**](./idebugsearchoptions.md)







## Properties

### Text

Declares a text that the returned results should match. 

Which fields that are searched are controlled by the [GetTextFields](#gettextfields) methods. By default, the `nodeName`, `title` and `teaser` fields are searched.

```csharp
SearchOptionsBase options = new() {
    Text = "Hello there"
};
```

### RootIds

Declares a list of IDs that should be in the path of the returned results. If more than one ID is specified, at least one of the IDs should
be in the path of the result to be a match.

The property is used by the [**`SearchPath`**](#searchpath) method.

```csharp
SearchOptionsBase options = new() {
    new List<int> { 1234 }
};
```

### DisableHideFromSearch

At Limbo we're typically adding a `hideFromSearch` property to pages in Umbraco, which if enabled, will result in a corresponding `hideFromSearch` field in Examine with the value set to `1`.

Pages without this setting or where the setting hasn't been enabled, the field may not exist in Umbraco, in which case searching for `hideFromSearch:0` wont get you the expected result.

Therefore we are updating the index with a default value, but this isn't done automatically, so you can disable this part of the search:

```csharp
SearchOptionsBase options = new() {
    DisableHideFromSearch = true
};
```








## Methods


### GetBooleanOperation

*Empty placeholder for now*



### GetTextFields

The method is responsible for returning a list of the fields that should be searched in when performing a text based search. The list of fields to search may be dependant on other pproperties, which is why this is a method and not a property. The method is virtual meaning that you can override it to change the behavior.

Either return a new list:

```csharp
protected override FieldList GetTextFields(ISearchHelper helper) {
    return new FieldList {
        new Field("nodeName", 50),
        new Field("title", 40),
        new Field("teaser", 20),
        new Field("content")
    };
}
```

or or add to the default list:

```csharp
protected override FieldList GetTextFields(ISearchHelper helper) {

    // Get the list of fields from the base class
    FieldList fields = base.GetTextFields(helper);

    // Append the "content" field to the list
    fields.Add("content");

    // Return the updated list
    return fields;

}
```

Read more about the [**`FieldList`**](./../fields/field.md) and [**`Field`**](./../fields/field.md) classes to see additional options - eg. fuzzy search and boosting specific fields.



### GetQueryList

Overall method for building the `QueryList` representing the Examine search query. By default, this method will call the following methods:

- [`SearchType`](#searchtype)
- [`SearchText`](#searchtext)
- [`SearchPath`](#searchpath)
- [`SearchHideFromSearch`](#searchhidefromsearch)

The method may be overridden in case you need to add additional parts to the query:

```csharp
protected override QueryList GetQueryList(ISearchHelper searchHelper) {

    QueryList query = base.GetQueryList(searchHelper);

    query.Add("hello:there");

    return query;

}
```




### GetSearcher

From: [**`IGetSearcherOptions`**](./igetsearcheroptions.md)

The `GetSearcher` method is used for returning the `ISearcher` to be used for the search. The method's default behavior is to use the searcher of Umbraco's `ExternalIndex`.

The method can be overridden to use another searcher:

```csharp
public virtual ISearcher GetSearcher(IExamineManager examineManager, ISearchHelper searchHelper) {
    return GetSearcherByIndexName(examineManager, searchHelper, "MyCustomIndex");
}
```

The `GetSearcherByIndexName` method is a protected method in the `SearchOptionsBase` class.







### SearchType

Virtual method for limiting the search to specific content types.

The method doesn't do anything by default, but it may be overridden to search for specific content types. Eg. in the example below to limit the search to only employees.

Notice that the `AppendNodeTypeAlias` method is used for appending a single node type alias to the overall query. There is also a `AppendNodeTypeAliases` variant if you need to search for multiple node type aliases.

```csharp
protected override void SearchType(ISearchHelper searchHelper, QueryList query) {
    query.AppendNodeTypeAlias(EmployeePage.ModelTypeAlias);
}
```




### SearchText

Virtual method for configuring the text based search. The method is responsible for splitting the value of the [**`Text`**](#text) property into words, and search for these words in the fields returned by the [**`GetTextFields`**](#gettextfields)  method.

For some of our sites, we're overriding this method to use a Hunspell dictionary for improving the search experience for the user - eg. to also search for variations of of a word or finding the correct word if spelled incorrectly.





### SearchPath

Virtual method for restricting the search to results for which the path matches the criteria defined by this method.

Default behavior is an OR-based search to match at least one of the IDs specified by the **`RootIds`** should match. To change the method to be AND-based instead (all specified IDs must match), you can override the method like this:

```csharp
protected override void SearchPath(ISearchHelper searchHelper, QueryList query) {
    if (RootIds.Count == 0) return;
    query.Add("(" + string.Join(" AND ", from id in RootIds select "path_search:" + id) + ")");
}
```