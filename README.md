# DotNetExtras.OData

`DotNetExtras.OData` is a .NET Core library handles [OData filter expressions](https://learn.microsoft.com/en-us/dynamics365/business-central/dev-itpro/webservices/use-filter-expressions-in-odata-uris).

Use the `DotNetExtras.OData` library to:

- Parse OData the filter expressions tree.
- Validate a given filter against the custom rules.

By default, the library can check if a filter expression uses valid OData syntax (based on the Microsoft's implementation) for the given data type. In particular, it validates that the filter expression uses:

- Supported operators (e.g., `eq`, `ne`, `and`, `or`, etc.).
- Supported properties (e.g., `name/givenName`, `type`, etc.).

To extend the default validation capabilities, use custom rules to specify:

- Allowed operators in the filter expression and the minimum and maximum number each operator can be used.
- Allowed properties in the filter expression and the minimum and maximum number of times each property can be used.
- Allowed operators that can be used with each property and the minimum and maximum number of times each operator can be used with each property.

The `DotNetExtras.OData` library's implementation of the OData filter validation rules is not based on any official specification (since there isn't any), but it is simple enough and easy to implement the basic checks beyond the defaults. The documentation site provides a detailed explanation of the rules syntax. You can find several examples in the unit test and demo projects.

## Disclaimer

The `DotNetExtras.OData` library's OData specification coverage is limited to the cases covered in the demos and unit tests. While they can handle the most common scenarios, there may be features or edge cases that are not fully implemented. If you run into such issue or edge case, please [open an issue](https://github.com/alekdavis/dotnet-extras-odata/issues).

## Usage

The following example demonstrates how to parse and validate an OData filter expression:

```cs
using DotNetExtras.OData;
...
string rules;

// OData filter validator object.
ODataFilterValidator<User> validator;

// We will use this filter expression to illustrate the validation rules.
// The filter is based on the User class provided in the sample project.
string filter = "type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'";

// This validation rule defines them as a JSON string that will be converted to the rules object.
// The rule allows the 'eq', 'and', 'ne', and 'startsWith' operators and 
// the 'type' and 'name/givenName' properties.
rules = @"{""Operators"":{""eq"":null,""and"":null,""ne"":null,""startsWith"":null}," +
        @"""Properties"":{""type"":null,""name/givenName"":null}}";

// The constructor will perform the validation of the rules.
validator = new(filter, rules);

// Check if the validation passed
if (validator.Passed)
...

// The same rule as above, only using the abbreviated notation.
rules = "eq|and|ne|startsWith|type|name/givenName";
validator = new(filter, rules);

// The following rule will make the filter expression to fail validation.
// The rule only allows the 'eq' operation for the 'type' property
// and the 'ne', 'startsWith', and 'and' operations for the 'name/givenName' property.
// The expression fails because the 'and' operator applies to the 'type' property,
// but it is not explicitly allowed (logical 'and' and 'or' operators must
// apply to both left and right operands)
rules = "type[eq]|name/givenName[ne,startsWith,and]";
validator = new(filter, rules);

// Check if the results and print validation errors.
if (!validator.Failed)
{
    foreach (string error in validator.Errors)
    {
        Console.WriteLine($"Error: {error}");
    }
}

```
You can find more examples in the unity tests and demos. The detailed explanation of the rules syntax is available in the documentation.

## Documentation

For the complete documentation, usage details, and code samples, see:

- [Documentation](https://alekdavis.github.io/dotnet-extras-odata)
- [Unit tests](https://github.com/alekdavis/dotnet-extras-mail/tree/main/ODataTests)
- [OData filter parser demo](https://github.com/alekdavis/dotnet-extras-mail/tree/main/ODataFilterParserDemo)
- [OData filter validator demo](https://github.com/alekdavis/dotnet-extras-mail/tree/main/ODataFilterValidatorDemo)

## Package

Install the latest version of the `DotNetExtras.OData` NuGet package from:

- [https://www.nuget.org/packages/DotNetExtras.OData](https://www.nuget.org/packages/DotNetExtras.OData)

## See also

Check out other `DotNetExtras` libraries at:

- [https://github.com/alekdavis/dotnet-extras](https://github.com/alekdavis/dotnet-extras)
