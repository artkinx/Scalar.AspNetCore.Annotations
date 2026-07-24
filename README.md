# Artkinx.ScalarAspNetCore.Annotations

`Artkinx.ScalarAspNetCore.Annotations` provides declarative attributes for configuring and enhancing [Scalar](https://github.com/scalar/scalar) OpenAPI documentation in ASP.NET Core applications. It supports both **Swashbuckle** and the **Native OpenAPI** generation built into .NET 9+.

## Overview

Scalar offers a beautiful API reference UI. This package lets you enrich your OpenAPI schema with Scalar-specific extensions (`x-` extensions) directly from your C# code using standard attributes, making it incredibly easy to configure badges, code samples, custom operation details, and more.

## Installation

Install the package via NuGet:

```bash
dotnet add package Artkinx.ScalarAspNetCore.Annotations
```

## Setup

Depending on the OpenAPI generator you are using, you need to register the annotations in your `Program.cs` or `Startup.cs`.

### With Swashbuckle

```csharp
using Artkinx.ScalarAspNetCore.Annotations.Swashbuckle;

builder.Services.AddSwaggerGen(options =>
{
    // Register the Scalar annotations filters
    options.AddScalarAnnotations();
});
```

### With .NET 9+ Native OpenAPI

```csharp
using Artkinx.ScalarAspNetCore.Annotations.Native;

builder.Services.AddOpenApi(options =>
{
    // Register the Scalar annotations transformers
    options.AddScalarAnnotations();
});
```

## Available Attributes

Here are the attributes provided by this package to enrich your endpoints:

### `[ScalarOperation]`
Enhance an operation with Scalar-specific metadata.

```csharp
[HttpGet]
[ScalarOperation(
    Summary = "Get it now",
    Description = "For getting it now",
    OperationId = "Getit",
    Tags = ["Gerit"],
    ThemeColor = "#ffff",
    DisplayName = "Getit"
    )]
public IActionResult GetIt() { ... }
```

### `[ScalarResponse]`
Annotate your API endpoint with specific response information.
You can now use the comprehensive `ScalarContentType` enum to easily specify MIME types.

```csharp
[HttpGet]
[ScalarResponse(typeof(MyModel), 200, ScalarContentType.Json)]
public IActionResult Get() { ... }
```

### `[ScalarBadge]`
Add custom badges to your operations in the Scalar UI.
You can specify the name, color, and position.

```csharp
[HttpGet]
[ScalarBadge("Beta", BadgePosition.After, "warning")]
public IActionResult GetBetaData() { ... }
```

### `[ScalarCodeSample]`
Provide custom code snippets for your endpoints to be displayed in the Scalar UI.

```csharp
[HttpPost]
[ScalarCodeSample("curl", "curl -X POST ...", "cURL")]
public IActionResult CreateItem() { ... }
```

### `[ScalarSchema]`
Provide custom schema modifications for specific types.

### `[ScalarIgnore]`
Hide an operation or a property from the generated OpenAPI documentation entirely.

### `[ScalarStability]`
Provide a value that adds a badge for the endpoint (accepted values: Experimental, Stable, Deprecated).

### `[ScalarOrder]`
Enable custom ordering of schema properties in the Scalar UI by specifying a numeric order value.

### `[ScalarEnum]`
Enrich an enum with Scalar-specific metadata.

```csharp
[ScalarEnum("MyEnum")]
public enum MyEnum
{
    [ScalarEnum("First", "First value")]
    First,
    [ScalarEnum("Second", "Second value")]
    Second
}
```

## Enums

### 'ScalarStabilityLevel'
A stringly typed enum containing the supported Stability Levels

### `ScalarContentType`
A strongly typed enum containing all standard HTTP content types (e.g., `Json`, `Xml`, `MultipartFormData`, `EventStream`, etc.). It includes an extension method `.ToMimeString()` to easily convert the enum values to their raw MIME string representation (`"application/json"`, `"text/event-stream"`, etc.).

---

By using these declarative attributes, you can significantly enhance the readability, interactivity, and visual organization of your Scalar API documentation.
