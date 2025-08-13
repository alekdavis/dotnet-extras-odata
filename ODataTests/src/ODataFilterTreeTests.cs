using DotNetExtras.OData;
using ODataSampleModels;

namespace ODataTests;
public class ODataFilterTreeTests
{
    [Fact]
    public void ODataFilter_ExpressionParser()
    {
        string expression;
        ODataFilterTree<User> filter;

        expression = "not(null)";
        filter = new(expression);
        Assert.All(["not"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not(true)";
        filter = new(expression);
        Assert.All(["not"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not(false)";
        filter = new(expression);
        Assert.All(["not"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not(enabled)";
        filter = new(expression);
        Assert.All(["enabled"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not(sponsor/enabled)";
        filter = new(expression);
        Assert.All(["sponsor/enabled"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not(sponsor/sponsor/enabled)";
        filter = new(expression);
        Assert.All(["sponsor/sponsor/enabled"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email eq null";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email ne null";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email eq 'john@mail.com'";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email ne 'john@mail.com'";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email ne 'john''s@mail.com'";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email eq displayName";
        filter = new(expression);
        Assert.All(["email", "displayName"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email ne displayName";
        filter = new(expression);
        Assert.All(["email", "displayName"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "contains(email, '@mail')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["contains"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not contains(email, '@mail')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not", "contains"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "startsWith(email, 'john')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["startsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not startsWith(email, 'john')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not", "startsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "endsWith(email, '.com')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["endsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not endsWith(email, '.com')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not", "endsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "email in ('john@mail.com', 'mary@mail.com')";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["in"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "not (email in ('john@mail.com', 'mary@mail.com'))";
        filter = new(expression);
        Assert.All(["email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["not", "in"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "id eq 0";
        filter = new(expression);
        Assert.All(["id"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "id gt 1.5";
        filter = new(expression);
        Assert.All(["id"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["gt"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "id lt 2000";
        filter = new(expression);
        Assert.All(["id"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["lt"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "id ge 1.5";
        filter = new(expression);
        Assert.All(["id"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ge"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "id le 2000";
        filter = new(expression);
        Assert.All(["id"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["le"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "name eq null";
        filter = new(expression);
        Assert.All(["name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "name ne null";
        filter = new(expression);
        Assert.All(["name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "name/givenName eq null";
        filter = new(expression);
        Assert.All(["name/givenName"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "sponsor/name/givenName eq null";
        filter = new(expression);
        Assert.All(["sponsor/name/givenName"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "name/surname ne sponsor/name/surname";
        filter = new(expression);
        Assert.All(["name/surname", "sponsor/name/surname"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "name/givenName in ('John', 'Mary')";
        filter = new(expression);
        Assert.All(["name/givenName"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["in"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "name/givenName ne name/nickName";
        filter = new(expression);
        Assert.All(["name/givenName", "name/nickName"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["ne"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "type eq 'Employee'";
        filter = new(expression);
        Assert.All(["type"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "type has 'Employee'";
        filter = new(expression);
        Assert.All(["type"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["has"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "type has 'Employee'";
        filter = new(expression);
        Assert.All(["type"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["has"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "createDate gt 2021-01-02T12:00:00Z";
        filter = new(expression);
        Assert.All(["createDate"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["gt"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "type eq 'Guest' and name/Surname eq 'Johnson'";
        filter = new(expression);
        Assert.All(["type", "name/Surname"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq", "and"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "enabled eq false and type in ('Employee', 'Contractor')";
        filter = new(expression);
        Assert.All(["enabled", "type"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq", "and", "in"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "((enabled eq true) and (type eq 'Employee')) or ((email ne null) and ((type eq 'Guest') or (endsWith(email, '@mail.com'))))";
        filter = new(expression);
        Assert.All(["enabled", "type", "email"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["eq", "and", "or", "ne", "endsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "phoneNumbers/any(p: p eq '123-456-7890')";
        filter = new(expression);
        Assert.All(["phoneNumbers"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["any", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "phoneNumbers/any(p: p eq '123-456-7890' or p eq '321-456-7890')";
        filter = new(expression);
        Assert.All(["phoneNumbers"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["any", "eq", "or"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "socialLogins/any(s: s/name eq 'Facebook')";
        filter = new(expression);
        Assert.All(["socialLogins/name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["any", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "socialLogins/any(s: s/name eq 'Facebook' or endsWith(s/url, 'google.com'))";
        filter = new(expression);
        Assert.All(["socialLogins/name", "socialLogins/url"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["any", "eq", "or", "endsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "sponsor/phoneNumbers/any(p: p eq '123-456-7890')";
        filter = new(expression);
        Assert.All(["sponsor/phoneNumbers"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["any", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "sponsor/socialLogins/any(s: s/name eq 'Facebook')";
        filter = new(expression);
        Assert.All(["sponsor/socialLogins/name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["any", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "phoneNumbers/all(p: p eq '123-456-7890')";
        filter = new(expression);
        Assert.All(["phoneNumbers"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["all", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "phoneNumbers/all(p: p eq '123-456-7890' or p eq '321-456-7890')";
        filter = new(expression);
        Assert.All(["phoneNumbers"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["all", "eq", "or"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "socialLogins/all(s: s/name eq 'Facebook')";
        filter = new(expression);
        Assert.All(["socialLogins/name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["all", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "socialLogins/all(s: s/name eq 'Facebook' or endsWith(s/url, 'google.com'))";
        filter = new(expression);
        Assert.All(["socialLogins/name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["all", "eq", "or", "endsWith"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "sponsor/phoneNumbers/all(p: p eq '123-456-7890')";
        filter = new(expression);
        Assert.All(["sponsor/phoneNumbers"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["all", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);

        expression = "sponsor/socialLogins/all(s: s/name eq 'Facebook')";
        filter = new(expression);
        Assert.All(["sponsor/socialLogins/name"], op => Assert.Contains(op, filter.Properties, StringComparer.OrdinalIgnoreCase));
        Assert.All(["all", "eq"], op => Assert.Contains(op, filter.Operators, StringComparer.OrdinalIgnoreCase));
        Assert.Equal(expression, filter.Expression);
    }
}
