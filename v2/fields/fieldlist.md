# FieldList

The `FieldList` is a list for custom list of [**`Field`**](./field.md) items.

Like other lists, you can add a new item through the `Add` method:

```csharp
fields.Add(new Field("content"));
```

But there are a few method overloads as well - eg. for adding a field just by it's name:

```csharp
fields.Add("content");
```

or by it's name while also setting the boost to `50` and the fuzzyness to `0.75`:

```csharp
fields.Add("content", 50, 0.75f);
```