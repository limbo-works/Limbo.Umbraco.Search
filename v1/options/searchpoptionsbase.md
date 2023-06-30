# SearchOptionsBase

## Interfaces

The `SearchOptionsBase` class implements the following interfaces:

- ISearchOptions
- IGetSearcherOptions
- IDebugSearchOptions





## Properties

### Text

Declares a text that the returned results should match. 

Which fields that are searched are controlled by the [GetTextFields](#gettextfields) methods. By default, the `nodeName`, `title` and `teaser` fields are searched.





## Methods

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

Read more about the [**`FieldList`**](./../fields/field.md) and [**`FieldList`**](./../fields/field.md) classes to see additional options - eg. fuzzy search and boosting specific fields.



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
