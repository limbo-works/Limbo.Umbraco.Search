# Block List

The `IIndexingHelper` interface describes a number of methods with the `Write{...}` prefix. Their purpose is to take some kind of value and append the values textual representation to a `TextWriter`.

For the block list, there is the `WriteBlockList` method that takes  an instance of Umbraco's `BlockListModel` class.

```method
{
  "name": "WriteBlockList",
  "parameters": [
    {
        "name": "writer",
        "type": "TextWriter"
    },
    {
        "name": "blockList",
        "type": "BlockListModel"
    },
    {
        "name": "culture",
        "type": "string",
        "nullable": true,
        "defaultValue": null
    },
    {
        "name": "segment",
        "type": "string",
        "nullable": true,
        "defaultValue": null
    }
  ]
}
```

The block list it self doesn't really hold any data on it's own, the method's purpose is to pass on the individual block items to the `WriteBlockListItem` method:

```method
{
  "name": "WriteBlockListItem",
  "parameters": [
    {
        "name": "writer",
        "type": "TextWriter"
    },
    {
        "name": "blockListItem",
        "type": "BlockListItem"
    },
    {
        "name": "culture",
        "type": "string",
        "nullable": true,
        "defaultValue": null
    },
    {
        "name": "segment",
        "type": "string",
        "nullable": true,
        "defaultValue": null
    }
  ]
}
```

Each block item consists of a content part, and optionally also a settings part depending on your block list configuration. Both of those are instances of `IPublishedElement`.

If you create a custom class that implements the `IIndexingHelper` interface, you are free to return a textual representation directly from the `WriteBlockListItem` method, or pass content and settings parts further on to the `WriteElement` method.

```method
{
  "name": "WriteElement",
  "parameters": [
    {
        "name": "writer",
        "type": "TextWriter"
    },
    {
        "name": "IPublishedElement",
        "type": "element"
    },
    {
        "name": "culture",
        "type": "string",
        "nullable": true,
        "defaultValue": null
    },
    {
        "name": "segment",
        "type": "string",
        "nullable": true,
        "defaultValue": null
    }
  ]
}
```