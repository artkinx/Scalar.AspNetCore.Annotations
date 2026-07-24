# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0-preview.17] - 2026-07-20
### Added
- Added `ScalarEnumAttribute` to enable enum decoration and transformation
- Added `DisplayName` to `ScalarOperationAttribute` to enable the definition for Scalar's `x-displayName` tag
- Updated `Scalar.Annotation.Test.Api` to carry the example for enum decorations

## [1.0.0-preview.16] - 2026-07-15

### Added
- Added `ScalarOrderAttribute` attribute to allow ordering of properties in the Scalar UI.
- Added a new test project `Scalar.Annotation.Test.Api` to demonstrate and test the new attribute.
- Updated the `ScalarExcludeAttribute` to also exclude enums from the Scalar UI.

### Changed
- Improved the `ScalarStabilityAttribute` to also exclude enums from the Scalar UI.
- Improved the `ScalarBadgeAttribute` to also exclude enums from the Scalar UI.
- Improved the `ScalarCodeSampleAttribute` to also exclude enums from the Scalar UI.

## [1.0.0-preview.15] - 2026-07-08
- Improved the implementation modularity
- Improved the Schema generation for the `ScalarResponseAttribute`
- Made sure to fallback to Swagger for .NET8

## [1.0.0-preview.14] - 2026-07-03

### Changed
- Improved the detection of `ScalarStabilityAttribute` in the endpoint metadata.
- Ensured the the fallback to Swashbuckle for .NET8 is operational
---

## [1.0.0-preview.13] - 2026-07-03

### Added
- Added an integration test suite under the `Scalar.AspNetCore.Annotations.UnitTests` project to verify `x-scalar-*` extension metadata generation.
- Configured support for `.NET 10` interceptor compilation via `InterceptorsNamespaces` settings.

### Fixed
- Fixed a runtime `NullReferenceException` in `.NET 10.0` operation and schema transformers by dynamically initializing the `Extensions` collection when null.
- Resolved dependency resolution errors in unit tests by including the `Microsoft.AspNetCore.App` framework reference and referencing `Microsoft.AspNetCore.Mvc.Testing` and `Microsoft.AspNetCore.TestHost`.

### Changed
- Updated internal package references for `.NET 8.0` and `.NET 10.0` targets.

---

## [1.0.0-preview.12] - 2026-07-02

### Fixed
- Resolved `OpenApiTag` casting and reference type mismatches when generating OpenAPI documents under `.NET 10.0`.

---

## [1.0.0-preview.11] - 2026-07-02

### Changed
- Renamed the package and root namespace to `Artkinx.ScalarAspNetCore.Annotations` for consistency and publication.

---

## [1.0.0-preview.10] - 2026-07-01

### Fixed
- Fixed compilation and framework differences across TargetFrameworks `net8.0`, `net9.0`, and `net10.0` by adding appropriate preprocessor checks.

---

## [1.0.0-preview.1.0] - 2026-07-01

### Added
- Initial release containing basic Scalar UI API annotation attributes:
  - `ScalarOperationAttribute` (for operation summaries, descriptions, tags, and theme colors)
  - `ScalarResponseAttribute` (for overriding HTTP responses)
  - `ScalarBadgeAttribute` (for rendering custom badges in the Scalar UI)
  - `ScalarCodeSampleAttribute` (for custom language code snippets)
  - `ScalarExcludeAttribute` (for ignoring endpoints)
  - `ScalarStabilityAttribute` (for stable, experimental, and deprecated endpoint badges)
  - Native OpenAPI transformers: `ScalarOperationTransformer` and `ScalarSchemaTransformer`
  - Swashbuckle filters: `ScalarSwashbuckleOperationFilter` and `ScalarSwashbuckleSchemaFilter`
