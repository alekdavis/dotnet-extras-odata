using DotNetExtras.OData;
using ODataSampleModels;

#region Data examples
Dictionary<string, List<string>> _data = new()
{
    {
    //    "email eq 'a@b.com'",
    //    new List<string>()
    //    {
    //        "email",
    //        "email:1,1",
    //        "email:1,1|eq:1,1",
    //        "email[eq]:1,1",
    //        "eq:,1|email[eq]:1,1",
    //        "email[ne]:0,1|email[eq]:1,1",
    //        "email[ne]:1,1|email[eq]:0,1"
    //    }
    //},
    //{
        "email eq 'a@b.com' and startsWith(email, name/givenName)",
        new List<string>()
        {
            "email",
            "email:1,2",
            "email:1,2|eq:1,1|startsWith:1,1|and:1,1",
            "email[eq]:1,1|email[startswith]:1,1",
            "email[eq]:1,1|email[startswith]:1,1|email[and]:2,2",
            "email[eq]|email[startswith]",
            "email[eq,startswith,and]",
            "email[eq]|email[startswith]|email[and]",
            "email[eq]"
        }
    //},
    //{
    //    "socialLogins/any(s: s/name eq 'Facebook')",
    //    new List<string>()
    //    {
    //        "any:,1|eq:,2|socialLogins/name:,1",
    //    }
    //},
    //{
    //    "Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'",
    //    new List<string>()
    //    {
    //        @"{""Operators"":{""eq"":null,""and"":null,""ne"":null,""startsWith"":null}," +
    //            @"""Properties"":{""type"":null,""name/givenName"":null}}",

    //        @"{""Operators"":{""eq"":{""min"":1},""and"":null,""ne"":null,""startsWith"":null}," +
    //            @"""Properties"":{""type"":{""min"":1},""name/givenName"":{""min"":1}}}",

    //        @"{""Operators"":{""eq"":null,""and"":null,""ne"":null,""endsWith"":null}," +
    //            @"""Properties"":{""type"":null,""name/givenName"":null}}",

    //        "eq|and|ne|startsWith|type|name/givenName",

    //        "eq|and|ne|startsWith|type:,5|name/givenName",

    //        "eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,",

    //        "eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,",
    //    }
    }
};
#endregion

#region Main method
{
    foreach (string filter in _data.Keys)
    {
        foreach (string rules in _data[filter])
        {

            try
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("FILTER: " + filter);
                Console.WriteLine("RULES : " + rules);
                Console.WriteLine("------------------------------------------------------");
                ODataFilterValidator<User> validator = new(filter, rules);

                if (validator.Passed)
                {
                    Console.WriteLine("PASSED.");
                }
                else
                {
                    Console.WriteLine("FAILED: " + validator.Details);
                }
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
}
#endregion