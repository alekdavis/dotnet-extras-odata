using DotNetExtras.OData;
using ODataSampleModels;

namespace ODataFilterParserDemo;

/// <summary>
/// Illustrates the use of OData filter parser.
/// </summary>
internal class Program
{
    #region Filter examples
    // Here are some basic filter we may want to test.

    // By default, property names are case-insensitive, but we'll make them case-insensitive.
    private static readonly string[] _filters =
    [
        "not(null)",
        "not(true)",
        "not(false)",
        "not(enabled)",
        "not(sponsor/enabled)",
        "not(sponsor/sponsor/enabled)",
        "email eq null",
        "email ne null",
        "email eq 'john@mail.com'",
        "email ne 'john@mail.com'",
        "email ne 'john''s@mail.com'",
        "email eq displayName",
        "email ne displayName",
        "contains(email, '@mail')",
        "not contains(email, '@mail')",
        "contains(email, displayName)",
        "not contains(email, displayName)",
        "startsWith(email, 'john')",
        "not startsWith(email, 'john')",
        "endsWith(email, '@mail.com')",
        "not endsWith(email, '@mail.com')",
        "email in ('john@mail.com', 'mary@mail.com')",
        "not (email in ('john@mail.com', 'mary@mail.com'))",
        "id eq 0",
        "id gt 1.5",
        "id lt 2000",
        "id ge 1",
        "id le 2000",
        "name eq null",
        "name ne null",
        "name/givenName eq null",
        "sponsor/name/givenName eq null",
        "name/surname ne sponsor/name/surname",
        "name/givenName in ('John', 'Mary')",
        "name/givenName ne name/nickName",
        "startsWith(displayName, 'J')",
        "type eq 'Employee'",
        "type has 'Employee'",
        "createDate gt 2021-01-02T12:00:00Z",
        "type eq 'Guest' and name/Surname eq 'Johnson'",
        "type eq 'Contractor' and not(endsWith(email, '@mail.com'))",
        "enabled eq false and type in ('Employee', 'Contractor') ",
        "((enabled eq true) and (type eq 'Employee')) or ((email ne null) and ((type eq 'Guest') or (endsWith(email, '@mail.com'))))",
        "phoneNumbers/any(p: p eq '123-456-7890')",
        "phoneNumbers/any(p: p eq '123-456-7890' or p eq '321-456-7890')",
        "socialLogins/any(s: s/name eq 'Facebook')",
        "socialLogins/any(s: s/name eq 'Facebook' or endsWith(s/url, 'google.com'))",
        "sponsor/phoneNumbers/any(p: p eq '123-456-7890')",
        "sponsor/socialLogins/any(s: s/name eq 'Facebook')",
        "phoneNumbers/all(p: p eq '123-456-7890')",
        "phoneNumbers/all(p: p eq '123-456-7890' or p eq '321-456-7890')",
        "socialLogins/all(s: s/name eq 'Facebook')",
        "socialLogins/all(s: s/name eq 'Facebook' or endsWith(s/url, 'google.com'))",
        "sponsor/phoneNumbers/all(p: p eq '123-456-7890')",
        "sponsor/socialLogins/all(s: s/name eq 'Facebook')",
    ];
    #endregion

    #region Main method
    private static void Main()
    {
        bool printedSchema =  false;
        foreach (string filter in _filters)
        {
            try
            {
                ODataFilterTree<User> tree = new(filter);

                if (!printedSchema) 
                {
                    tree.PrintSchema();
                    Console.WriteLine();
                    printedSchema = true;
                }

                tree.PrintTree(true, false, true);
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    Console.WriteLine(ex.Message + " ");

                    if (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            Console.WriteLine();
        }
    }
    #endregion
}
