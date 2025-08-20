# DotNetExtras.ODataFilterValidatorDemo project

This project implements a console application with the code samples illustrating how to use the `DotNetExtras.OData` library APIs to validate OData filter expressions.

## Output

The application will output the validation results to the console. If the validation fails, it will print the error messages describing the issues with the filter expression. Here is an example of the output:

```plain
------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : email
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : email:1,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : email:1,1|eq:1,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : email[eq]:1,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : eq:,1|email[eq]:1,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : email[ne]:0,1|email[eq]:1,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com'
RULES : email[ne]:1,1|email[eq]:0,1
------------------------------------------------------
FAILED: Missing required operation 'ne' applied to property 'email'.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email:1,2
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email:1,2|eq:1,1|startsWith:1,1|and:1,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email[eq]:1,1|email[startswith]:1,1
------------------------------------------------------
FAILED: Operation 'and' applied to property 'Email' is not allowed.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email[eq]:1,1|email[startswith]:1,1|email[and]:2,2
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email[eq]|email[startswith]
------------------------------------------------------
FAILED: Operation 'and' applied to property 'Email' is not allowed.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email[eq,startswith,and]
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email[eq]|email[startswith]|email[and]
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: email eq 'a@b.com' and startsWith(email, name/givenName)
RULES : email[eq]
------------------------------------------------------
FAILED: Operation 'and' applied to property 'Email' is not allowed. Operation 'startswith' applied to property 'Email' is not allowed.

------------------------------------------------------
FILTER: socialLogins/any(s: s/name eq 'Facebook')
RULES : any:,1|eq:,2|socialLogins/name:,1
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : {"Operators":{"eq":null,"and":null,"ne":null,"startsWith":null},"Properties":{"type":null,"name/givenName":null}}
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : {"Operators":{"eq":{"min":1},"and":null,"ne":null,"startsWith":null},"Properties":{"type":{"min":1},"name/givenName":{"min":1}}}
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : {"Operators":{"eq":null,"and":null,"ne":null,"endsWith":null},"Properties":{"type":null,"name/givenName":null}}
------------------------------------------------------
FAILED: Operator 'startswith' is not allowed.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : eq|and|ne|startsWith|type|name/givenName
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : eq|and|ne|startsWith|type:,5|name/givenName
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,
------------------------------------------------------
PASSED.

------------------------------------------------------
FILTER: Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'
RULES : eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,
------------------------------------------------------
PASSED.

```