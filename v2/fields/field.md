# Field

The `Field` class describes a field to be searched, primarily the name or key of the field.

Eg. the example below represents the `nodeName` field, which may be added to a [**`FieldList`**](./field.md) instance:

```csharp
Field field = new Field("nodeName");
```

The `Field` class also holds information about the field's boost level and fuzziness - both not set by default:

```csharp
Field field = new Field("nodeName", 50);
```

```csharp
Field field = new Field("nodeName", fuzz: 0.75f);
```