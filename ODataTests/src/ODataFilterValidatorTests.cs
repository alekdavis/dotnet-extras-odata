using DotNetExtras.OData;
using ODataSampleModels;

namespace ODataTests;
public class ODataFilterValidatorTests
{
    [Fact]
    public void ODataFilter_RuleValidator()
    {
        string filter;
        string rules;
        ODataFilterValidator<User> validator;

        filter = "Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'";
        
        rules = @"{""Operators"":{""eq"":null,""and"":null,""ne"":null,""startsWith"":null}," +
                @"""Properties"":{""type"":null,""name/givenName"":null}}";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = @"{""Operators"":{""eq"":{""min"":1},""and"":null,""ne"":null,""startsWith"":null}," +
                @"""Properties"":{""type"":{""min"":1},""name/givenName"":{""min"":1}}}";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "eq|and|ne|startsWith|type|name/givenName";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "eq|and|ne|startsWith|type:,5|name/givenName";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "eq|and:2,0|o:ne:1,2|startsWith:1,0|type:,5|p:name/givenName:2,0";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "lt|eq|and|ne|startsWith|type:,5|name/givenName";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = @"{""Operators"":{""eq"":null,""and"":null,""ne"":null,""endsWith"":null}," +
                @"""Properties"":{""type"":null,""name/givenName"":null}}";
        validator = new(filter, rules);
        Assert.True(validator.Failed);
        Assert.Equal(1, validator?.Errors?.Count ?? 0);
        Assert.Contains("startsWith", validator?.Errors?[0], StringComparison.OrdinalIgnoreCase);

        rules = "type[eq,and]|name/givenName[ne,startsWith,and]";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "type[eq,and]:1,2|name/givenName[ne,startsWith,and]:1,3";
        validator = new(filter, rules);
        Assert.True(validator.Passed);

        rules = "type[eq]|name/givenName[ne,startsWith,and]";
        validator = new(filter, rules);
        Assert.True(validator.Failed);
        Assert.Equal(1, validator?.Errors?.Count ?? 0);
        Assert.Contains("and", validator?.Errors?[0], StringComparison.OrdinalIgnoreCase);
        Assert.Contains("type", validator?.Errors?[0], StringComparison.OrdinalIgnoreCase);

        rules = "type[eq]|type[and]|name/givenName[ne]|name/givenName[startsWith]|name/givenName[and]";
        validator = new(filter, rules);
        Assert.True(validator.Passed);
    }
}
