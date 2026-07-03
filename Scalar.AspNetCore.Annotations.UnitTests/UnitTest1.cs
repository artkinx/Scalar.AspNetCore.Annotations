using System.Net;
using System.Text.Json;
using Artkinx.ScalarAspNetCore.Annotations.Attributes;
using Artkinx.ScalarAspNetCore.Annotations.Enums;
using Artkinx.ScalarAspNetCore.Annotations.Native;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Scalar.AspNetCore.Annotations.UnitTests;

/// <summary>
/// <para>
/// Integration tests that verify the <see cref="ScalarOperationTransformer"/> correctly
/// writes x-scalar-* OpenAPI extension properties for each custom attribute.
/// </para>
/// <para>
/// Strategy: each test method builds a minimal WebApplication with a single annotated
/// minimal-API endpoint, fetches /openapi/v1.json via the in-process test server, and
/// asserts the expected extension keys/values are present in the JSON document.
/// </para>
/// </summary>
public class OpenApiExtensionsTests
{
    // -----------------------------------------------------------------------
    // Helpers
    // -----------------------------------------------------------------------

    /// <summary>
    /// Builds an in-process <see cref="HttpClient"/> whose host exposes a single
    /// GET /test endpoint decorated with <paramref name="configureEndpoint"/>.
    /// </summary>
    private static HttpClient BuildClient(Action<RouteHandlerBuilder> configureEndpoint)
    {
        var builder = WebApplication.CreateBuilder();

        builder.WebHost.UseTestServer();

        builder.Services.AddOpenApi(options => options.AddScalarAnnotations());

        var app = builder.Build();

        app.MapOpenApi();

        var endpoint = app.MapGet("/test", () => Results.Ok("hello"));
        configureEndpoint(endpoint);

        app.Start();

        return app.GetTestServer().CreateClient();
    }

    /// <summary>Fetches the OpenAPI document and parses it as a <see cref="JsonDocument"/>.</summary>
    private static async Task<JsonDocument> GetOpenApiDocumentAsync(HttpClient client)
    {
        var response = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(json);
    }

    /// <summary>
    /// Navigates to the first operation (paths./test.get) in the OpenAPI document.
    /// </summary>
    private static JsonElement GetTestOperation(JsonDocument doc)
    {
        return doc.RootElement
            .GetProperty("paths")
            .GetProperty("/test")
            .GetProperty("get");
    }

    // -----------------------------------------------------------------------
    // x-badges
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ScalarBadgeAttribute_Writes_XBadges_Extension()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarBadgeAttribute("Beta", BadgePosition.After, "#FF5733")));

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.True(operation.TryGetProperty("x-badges", out var badges),
            "Expected 'x-badges' extension to be present on the operation.");

        Assert.Equal(JsonValueKind.Array, badges.ValueKind);
        Assert.True(badges.GetArrayLength() > 0, "Expected at least one badge.");

        var badge = badges[0];
        Assert.Equal("Beta", badge.GetProperty("name").GetString());
        Assert.Equal("after", badge.GetProperty("position").GetString());
        Assert.Equal("#FF5733", badge.GetProperty("color").GetString());
    }

    [Fact]
    public async Task ScalarBadgeAttribute_Before_Position_Serialises_Correctly()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarBadgeAttribute("New", BadgePosition.Before)));

        using var doc = await GetOpenApiDocumentAsync(client);
        var badge = GetTestOperation(doc).GetProperty("x-badges")[0];

        Assert.Equal("before", badge.GetProperty("position").GetString());
    }

    // -----------------------------------------------------------------------
    // x-codeSamples
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ScalarCodeSampleAttribute_Writes_XCodeSamples_Extension()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarCodeSampleAttribute("csharp", "var x = 1;", "My C# Sample")));

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.True(operation.TryGetProperty("x-codeSamples", out var samples),
            "Expected 'x-codeSamples' extension to be present.");

        var sample = samples[0];
        Assert.Equal("csharp", sample.GetProperty("lang").GetString());
        Assert.Equal("var x = 1;", sample.GetProperty("source").GetString());
        Assert.Equal("My C# Sample", sample.GetProperty("label").GetString());
    }

    [Fact]
    public async Task ScalarCodeSampleAttribute_UsesLanguageAsLabel_WhenTitleIsEmpty()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarCodeSampleAttribute("python", "print('hi')")));

        using var doc = await GetOpenApiDocumentAsync(client);
        var sample = GetTestOperation(doc).GetProperty("x-codeSamples")[0];

        // When Title is empty the transformer should fall back to Language
        Assert.Equal("python", sample.GetProperty("label").GetString());
    }

    // -----------------------------------------------------------------------
    // x-scalar-ignore
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ScalarExcludeAttribute_Writes_XScalarIgnore_Extension()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarExcludeAttribute()));

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.True(operation.TryGetProperty("x-scalar-ignore", out var ignore),
            "Expected 'x-scalar-ignore' extension to be present.");

        Assert.Equal(JsonValueKind.True, ignore.ValueKind);
    }

    // -----------------------------------------------------------------------
    // x-scalar-stability
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData(ScalarStabilityLevel.Stable, "stable")]
    [InlineData(ScalarStabilityLevel.Experimental, "experimental")]
    [InlineData(ScalarStabilityLevel.Deprecated, "deprecated")]
    public async Task ScalarStabilityAttribute_Writes_XScalarStability_Extension(
        ScalarStabilityLevel level, string expectedValue)
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarStabilityAttribute(level)));

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.True(operation.TryGetProperty("x-scalar-stability", out var stability),
            "Expected 'x-scalar-stability' extension to be present.");

        Assert.Equal(expectedValue, stability.GetString());
    }

    // -----------------------------------------------------------------------
    // x-scalar-color  (from ScalarOperationAttribute.ThemeColor)
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ScalarOperationAttribute_ThemeColor_Writes_XScalarColor_Extension()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarOperationAttribute { ThemeColor = "#AABBCC" }));

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.True(operation.TryGetProperty("x-scalar-color", out var color),
            "Expected 'x-scalar-color' extension to be present.");

        Assert.Equal("#AABBCC", color.GetString());
    }

    // -----------------------------------------------------------------------
    // ScalarOperationAttribute — plain metadata (summary, operationId)
    // -----------------------------------------------------------------------

    [Fact]
    public async Task ScalarOperationAttribute_OverridesSummary()
    {
        using var client = BuildClient(ep =>
            ep.WithMetadata(new ScalarOperationAttribute { Summary = "My custom summary" }));

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.Equal("My custom summary", operation.GetProperty("summary").GetString());
    }

    // -----------------------------------------------------------------------
    // No attributes — no extension keys pollution
    // -----------------------------------------------------------------------

    [Fact]
    public async Task NoScalarAttributes_NoExtensionKeysAdded()
    {
        using var client = BuildClient(_ => { /* no metadata */ });

        using var doc = await GetOpenApiDocumentAsync(client);
        var operation = GetTestOperation(doc);

        Assert.False(operation.TryGetProperty("x-badges", out _));
        Assert.False(operation.TryGetProperty("x-codeSamples", out _));
        Assert.False(operation.TryGetProperty("x-scalar-ignore", out _));
        Assert.False(operation.TryGetProperty("x-scalar-stability", out _));
        Assert.False(operation.TryGetProperty("x-scalar-color", out _));
    }
}
