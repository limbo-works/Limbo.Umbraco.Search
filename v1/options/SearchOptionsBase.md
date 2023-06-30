# SearchOptionsBase

## Interfaces

The `SearchOptionsBase` class implements the following interfaces:

- ISearchOptions
- IGetSearcherOptions
- IDebugSearchOptions





## Properties

### Text

Declares a text that the returned results should match. 

Which fields that are searched are controlled by the [GetTextFields](#GetTextFields) methods. By default, the `nodeName`, `title` and `teaser` fields are searched.





## Methods

### GetTextFields

The method is responsible for returning a list of the fields that should be searched in when performing a text based search. The list of fields to search may be dependant on other pproperties, which is why this is a method and not a property. The method is virtual meaning that you can override it to change the behavior:
