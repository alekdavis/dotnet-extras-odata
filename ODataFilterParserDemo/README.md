# DotNetExtras.ODataFilterParserDemo project
This project implements a console application with the code samples illustrating how to use the `DotNetExtras.OData` library APIs to parse OData filter expressions.

## Output

The application will output parsed OData filter tree along with the usage summary for each tried expression as shown below:

```plain
ODATA SCHEMA ELEMENTS:
- ODataSampleModels.User: TypeDefinition
- ODataSampleModels.PersonName: TypeDefinition
- ODataSampleModels.SocialLogin: TypeDefinition
- ODataSampleModels.UserType: TypeDefinition
- Default.Container: EntityContainer

------------------------------------------------------------------------
EXPRESSION: not(null)
------------------------------------------------------------------------
OPERATOR: not
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  not

------------------------------------------------------------------------
EXPRESSION: not(true)
------------------------------------------------------------------------
OPERATOR: not
  OPERAND: True
------------------------------------------------------------------------
OPERATORS:
  not

------------------------------------------------------------------------
EXPRESSION: not(false)
------------------------------------------------------------------------
OPERATOR: not
  OPERAND: False
------------------------------------------------------------------------
OPERATORS:
  not

------------------------------------------------------------------------
EXPRESSION: not(enabled)
------------------------------------------------------------------------
OPERATOR: not
  OPERAND: Enabled
------------------------------------------------------------------------
OPERATORS:
  not
PROPERTIES:
  Enabled
USAGE:
  Enabled: not

------------------------------------------------------------------------
EXPRESSION: not(sponsor/enabled)
------------------------------------------------------------------------
OPERATOR: not
  OPERAND: Sponsor/Enabled
------------------------------------------------------------------------
OPERATORS:
  not
PROPERTIES:
  Sponsor/Enabled
USAGE:
  Sponsor/Enabled: not

------------------------------------------------------------------------
EXPRESSION: not(sponsor/sponsor/enabled)
------------------------------------------------------------------------
OPERATOR: not
  OPERAND: Sponsor/Sponsor/Enabled
------------------------------------------------------------------------
OPERATORS:
  not
PROPERTIES:
  Sponsor/Sponsor/Enabled
USAGE:
  Sponsor/Sponsor/Enabled: not

------------------------------------------------------------------------
EXPRESSION: email eq null
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Email
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Email
USAGE:
  Email: eq

------------------------------------------------------------------------
EXPRESSION: email ne null
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Email
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  Email
USAGE:
  Email: ne

------------------------------------------------------------------------
EXPRESSION: email eq 'john@mail.com'
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Email
  OPERAND: "john@mail.com"
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Email
USAGE:
  Email: eq

------------------------------------------------------------------------
EXPRESSION: email ne 'john@mail.com'
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Email
  OPERAND: "john@mail.com"
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  Email
USAGE:
  Email: ne

------------------------------------------------------------------------
EXPRESSION: email ne 'john''s@mail.com'
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Email
  OPERAND: "john's@mail.com"
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  Email
USAGE:
  Email: ne

------------------------------------------------------------------------
EXPRESSION: email eq displayName
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Email
  OPERAND: DisplayName
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  DisplayName, Email
USAGE:
  Email: eq
  DisplayName: eq

------------------------------------------------------------------------
EXPRESSION: email ne displayName
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Email
  OPERAND: DisplayName
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  DisplayName, Email
USAGE:
  Email: ne
  DisplayName: ne

------------------------------------------------------------------------
EXPRESSION: contains(email, '@mail')
------------------------------------------------------------------------
OPERATOR: contains
  OPERAND: Email
  OPERAND: "@mail"
------------------------------------------------------------------------
OPERATORS:
  contains
PROPERTIES:
  Email
USAGE:
  Email: contains

------------------------------------------------------------------------
EXPRESSION: not contains(email, '@mail')
------------------------------------------------------------------------
OPERATOR: not
  OPERATOR: contains
    OPERAND: Email
    OPERAND: "@mail"
------------------------------------------------------------------------
OPERATORS:
  contains, not
PROPERTIES:
  Email
USAGE:
  Email: contains, not

------------------------------------------------------------------------
EXPRESSION: contains(email, displayName)
------------------------------------------------------------------------
OPERATOR: contains
  OPERAND: Email
  OPERAND: DisplayName
------------------------------------------------------------------------
OPERATORS:
  contains
PROPERTIES:
  Email
USAGE:
  Email: contains

------------------------------------------------------------------------
EXPRESSION: not contains(email, displayName)
------------------------------------------------------------------------
OPERATOR: not
  OPERATOR: contains
    OPERAND: Email
    OPERAND: DisplayName
------------------------------------------------------------------------
OPERATORS:
  contains, not
PROPERTIES:
  Email
USAGE:
  Email: contains, not

------------------------------------------------------------------------
EXPRESSION: startsWith(email, 'john')
------------------------------------------------------------------------
OPERATOR: startswith
  OPERAND: Email
  OPERAND: "john"
------------------------------------------------------------------------
OPERATORS:
  startswith
PROPERTIES:
  Email
USAGE:
  Email: startswith

------------------------------------------------------------------------
EXPRESSION: not startsWith(email, 'john')
------------------------------------------------------------------------
OPERATOR: not
  OPERATOR: startswith
    OPERAND: Email
    OPERAND: "john"
------------------------------------------------------------------------
OPERATORS:
  not, startswith
PROPERTIES:
  Email
USAGE:
  Email: startswith, not

------------------------------------------------------------------------
EXPRESSION: endsWith(email, '@mail.com')
------------------------------------------------------------------------
OPERATOR: endswith
  OPERAND: Email
  OPERAND: "@mail.com"
------------------------------------------------------------------------
OPERATORS:
  endswith
PROPERTIES:
  Email
USAGE:
  Email: endswith

------------------------------------------------------------------------
EXPRESSION: not endsWith(email, '@mail.com')
------------------------------------------------------------------------
OPERATOR: not
  OPERATOR: endswith
    OPERAND: Email
    OPERAND: "@mail.com"
------------------------------------------------------------------------
OPERATORS:
  endswith, not
PROPERTIES:
  Email
USAGE:
  Email: endswith, not

------------------------------------------------------------------------
EXPRESSION: email in ('john@mail.com', 'mary@mail.com')
------------------------------------------------------------------------
OPERATOR: in
  OPERAND: Email
  OPERAND: "john@mail.com"
  OPERAND: "mary@mail.com"
------------------------------------------------------------------------
OPERATORS:
  in
PROPERTIES:
  Email
USAGE:
  Email: in

------------------------------------------------------------------------
EXPRESSION: not (email in ('john@mail.com', 'mary@mail.com'))
------------------------------------------------------------------------
OPERATOR: not
  OPERATOR: in
    OPERAND: Email
    OPERAND: "john@mail.com"
    OPERAND: "mary@mail.com"
------------------------------------------------------------------------
OPERATORS:
  in, not
PROPERTIES:
  Email
USAGE:
  Email: in, not

------------------------------------------------------------------------
EXPRESSION: id eq 0
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Id
  OPERAND: 0
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Id
USAGE:
  Id: eq

------------------------------------------------------------------------
EXPRESSION: id gt 1.5
------------------------------------------------------------------------
OPERATOR: gt
  OPERAND: Id
  OPERAND: 1.5
------------------------------------------------------------------------
OPERATORS:
  gt
PROPERTIES:
  Id
USAGE:
  Id: gt

------------------------------------------------------------------------
EXPRESSION: id lt 2000
------------------------------------------------------------------------
OPERATOR: lt
  OPERAND: Id
  OPERAND: 2000
------------------------------------------------------------------------
OPERATORS:
  lt
PROPERTIES:
  Id
USAGE:
  Id: lt

------------------------------------------------------------------------
EXPRESSION: id ge 1
------------------------------------------------------------------------
OPERATOR: ge
  OPERAND: Id
  OPERAND: 1
------------------------------------------------------------------------
OPERATORS:
  ge
PROPERTIES:
  Id
USAGE:
  Id: ge

------------------------------------------------------------------------
EXPRESSION: id le 2000
------------------------------------------------------------------------
OPERATOR: le
  OPERAND: Id
  OPERAND: 2000
------------------------------------------------------------------------
OPERATORS:
  le
PROPERTIES:
  Id
USAGE:
  Id: le

------------------------------------------------------------------------
EXPRESSION: name eq null
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Name
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Name
USAGE:
  Name: eq

------------------------------------------------------------------------
EXPRESSION: name ne null
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Name
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  Name
USAGE:
  Name: ne

------------------------------------------------------------------------
EXPRESSION: name/givenName eq null
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Name/GivenName
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Name/GivenName
USAGE:
  Name/GivenName: eq

------------------------------------------------------------------------
EXPRESSION: sponsor/name/givenName eq null
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Sponsor/Name/GivenName
  OPERAND: null
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Sponsor/Name/GivenName
USAGE:
  Sponsor/Name/GivenName: eq

------------------------------------------------------------------------
EXPRESSION: name/surname ne sponsor/name/surname
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Name/Surname
  OPERAND: Sponsor/Name/Surname
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  Name/Surname, Sponsor/Name/Surname
USAGE:
  Name/Surname: ne
  Sponsor/Name/Surname: ne

------------------------------------------------------------------------
EXPRESSION: name/givenName in ('John', 'Mary')
------------------------------------------------------------------------
OPERATOR: in
  OPERAND: Name/GivenName
  OPERAND: "John"
  OPERAND: "Mary"
------------------------------------------------------------------------
OPERATORS:
  in
PROPERTIES:
  Name/GivenName
USAGE:
  Name/GivenName: in

------------------------------------------------------------------------
EXPRESSION: name/givenName ne name/nickName
------------------------------------------------------------------------
OPERATOR: ne
  OPERAND: Name/GivenName
  OPERAND: Name/NickName
------------------------------------------------------------------------
OPERATORS:
  ne
PROPERTIES:
  Name/GivenName, Name/NickName
USAGE:
  Name/GivenName: ne
  Name/NickName: ne

------------------------------------------------------------------------
EXPRESSION: startsWith(displayName, 'J')
------------------------------------------------------------------------
OPERATOR: startswith
  OPERAND: DisplayName
  OPERAND: "J"
------------------------------------------------------------------------
OPERATORS:
  startswith
PROPERTIES:
  DisplayName
USAGE:
  DisplayName: startswith

------------------------------------------------------------------------
EXPRESSION: type eq 'Employee'
------------------------------------------------------------------------
OPERATOR: eq
  OPERAND: Type
  OPERAND: 'Employee'
------------------------------------------------------------------------
OPERATORS:
  eq
PROPERTIES:
  Type
USAGE:
  Type: eq

------------------------------------------------------------------------
EXPRESSION: type has 'Employee'
------------------------------------------------------------------------
OPERATOR: has
  OPERAND: Type
  OPERAND: 'Employee'
------------------------------------------------------------------------
OPERATORS:
  has
PROPERTIES:
  Type
USAGE:
  Type: has

------------------------------------------------------------------------
EXPRESSION: createDate gt 2021-01-02T12:00:00Z
------------------------------------------------------------------------
OPERATOR: gt
  OPERAND: CreateDate
  OPERAND: 1/2/2021 12:00:00 PM +00:00
------------------------------------------------------------------------
OPERATORS:
  gt
PROPERTIES:
  CreateDate
USAGE:
  CreateDate: gt

------------------------------------------------------------------------
EXPRESSION: type eq 'Guest' and name/Surname eq 'Johnson'
------------------------------------------------------------------------
OPERATOR: and
  OPERATOR: eq
    OPERAND: Type
    OPERAND: 'Guest'
  OPERATOR: eq
    OPERAND: Name/Surname
    OPERAND: "Johnson"
------------------------------------------------------------------------
OPERATORS:
  and, eq
PROPERTIES:
  Name/Surname, Type
USAGE:
  Type: eq, and
  Name/Surname: eq, and

------------------------------------------------------------------------
EXPRESSION: type eq 'Contractor' and not(endsWith(email, '@mail.com'))
------------------------------------------------------------------------
OPERATOR: and
  OPERATOR: eq
    OPERAND: Type
    OPERAND: 'Contractor'
  OPERATOR: not
    OPERATOR: endswith
      OPERAND: Email
      OPERAND: "@mail.com"
------------------------------------------------------------------------
OPERATORS:
  and, endswith, eq, not
PROPERTIES:
  Email, Type
USAGE:
  Type: eq, and
  Email: endswith, not, and

------------------------------------------------------------------------
EXPRESSION: enabled eq false and type in ('Employee', 'Contractor')
------------------------------------------------------------------------
OPERATOR: and
  OPERATOR: eq
    OPERAND: Enabled
    OPERAND: False
  OPERATOR: in
    OPERAND: Type
    OPERAND: 'Employee'
    OPERAND: 'Contractor'
------------------------------------------------------------------------
OPERATORS:
  and, eq, in
PROPERTIES:
  Enabled, Type
USAGE:
  Enabled: eq, and
  Type: in, and

------------------------------------------------------------------------
EXPRESSION: ((enabled eq true) and (type eq 'Employee')) or ((email ne null) and ((type eq 'Guest') or (endsWith(email, '@mail.com'))))
------------------------------------------------------------------------
OPERATOR: or
  OPERATOR: and
    OPERATOR: eq
      OPERAND: Enabled
      OPERAND: True
    OPERATOR: eq
      OPERAND: Type
      OPERAND: 'Employee'
  OPERATOR: and
    OPERATOR: ne
      OPERAND: Email
      OPERAND: null
    OPERATOR: or
      OPERATOR: eq
        OPERAND: Type
        OPERAND: 'Guest'
      OPERATOR: endswith
        OPERAND: Email
        OPERAND: "@mail.com"
------------------------------------------------------------------------
OPERATORS:
  and, endswith, eq, ne, or
PROPERTIES:
  Email, Enabled, Type
USAGE:
  Enabled: eq, and, or
  Type: eq, and, or, eq, or, and, or
  Email: ne, and, or, endswith, or, and, or

------------------------------------------------------------------------
EXPRESSION: phoneNumbers/any(p: p eq '123-456-7890')
------------------------------------------------------------------------
OPERATOR: any
  OPERATOR: eq
    OPERAND: PhoneNumbers
    OPERAND: "123-456-7890"
------------------------------------------------------------------------
OPERATORS:
  any, eq
PROPERTIES:
  PhoneNumbers
USAGE:
  PhoneNumbers: eq, any

------------------------------------------------------------------------
EXPRESSION: phoneNumbers/any(p: p eq '123-456-7890' or p eq '321-456-7890')
------------------------------------------------------------------------
OPERATOR: any
  OPERATOR: or
    OPERATOR: eq
      OPERAND: PhoneNumbers
      OPERAND: "123-456-7890"
    OPERATOR: eq
      OPERAND: PhoneNumbers
      OPERAND: "321-456-7890"
------------------------------------------------------------------------
OPERATORS:
  any, eq, or
PROPERTIES:
  PhoneNumbers
USAGE:
  PhoneNumbers: eq, or, any, eq, or, any

------------------------------------------------------------------------
EXPRESSION: socialLogins/any(s: s/name eq 'Facebook')
------------------------------------------------------------------------
OPERATOR: any
  OPERATOR: eq
    OPERAND: SocialLogins/Name
    OPERAND: "Facebook"
------------------------------------------------------------------------
OPERATORS:
  any, eq
PROPERTIES:
  SocialLogins/Name
USAGE:
  SocialLogins/Name: eq, any

------------------------------------------------------------------------
EXPRESSION: socialLogins/any(s: s/name eq 'Facebook' or endsWith(s/url, 'google.com'))
------------------------------------------------------------------------
OPERATOR: any
  OPERATOR: or
    OPERATOR: eq
      OPERAND: SocialLogins/Name
      OPERAND: "Facebook"
    OPERATOR: endswith
      OPERAND: SocialLogins/Url
      OPERAND: "google.com"
------------------------------------------------------------------------
OPERATORS:
  any, endswith, eq, or
PROPERTIES:
  SocialLogins/Name, SocialLogins/Url
USAGE:
  SocialLogins/Name: eq, or, any
  SocialLogins/Url: endswith, or, any

------------------------------------------------------------------------
EXPRESSION: sponsor/phoneNumbers/any(p: p eq '123-456-7890')
------------------------------------------------------------------------
OPERATOR: any
  OPERATOR: eq
    OPERAND: Sponsor/PhoneNumbers
    OPERAND: "123-456-7890"
------------------------------------------------------------------------
OPERATORS:
  any, eq
PROPERTIES:
  Sponsor/PhoneNumbers
USAGE:
  Sponsor/PhoneNumbers: eq, any

------------------------------------------------------------------------
EXPRESSION: sponsor/socialLogins/any(s: s/name eq 'Facebook')
------------------------------------------------------------------------
OPERATOR: any
  OPERATOR: eq
    OPERAND: Sponsor/SocialLogins/Name
    OPERAND: "Facebook"
------------------------------------------------------------------------
OPERATORS:
  any, eq
PROPERTIES:
  Sponsor/SocialLogins/Name
USAGE:
  Sponsor/SocialLogins/Name: eq, any

------------------------------------------------------------------------
EXPRESSION: phoneNumbers/all(p: p eq '123-456-7890')
------------------------------------------------------------------------
OPERATOR: all
  OPERATOR: eq
    OPERAND: PhoneNumbers
    OPERAND: "123-456-7890"
------------------------------------------------------------------------
OPERATORS:
  all, eq
PROPERTIES:
  PhoneNumbers
USAGE:
  PhoneNumbers: eq, all

------------------------------------------------------------------------
EXPRESSION: phoneNumbers/all(p: p eq '123-456-7890' or p eq '321-456-7890')
------------------------------------------------------------------------
OPERATOR: all
  OPERATOR: or
    OPERATOR: eq
      OPERAND: PhoneNumbers
      OPERAND: "123-456-7890"
    OPERATOR: eq
      OPERAND: PhoneNumbers
      OPERAND: "321-456-7890"
------------------------------------------------------------------------
OPERATORS:
  all, eq, or
PROPERTIES:
  PhoneNumbers
USAGE:
  PhoneNumbers: eq, or, all, eq, or, all

------------------------------------------------------------------------
EXPRESSION: socialLogins/all(s: s/name eq 'Facebook')
------------------------------------------------------------------------
OPERATOR: all
  OPERATOR: eq
    OPERAND: SocialLogins/Name
    OPERAND: "Facebook"
------------------------------------------------------------------------
OPERATORS:
  all, eq
PROPERTIES:
  SocialLogins/Name
USAGE:
  SocialLogins/Name: eq, all

------------------------------------------------------------------------
EXPRESSION: socialLogins/all(s: s/name eq 'Facebook' or endsWith(s/url, 'google.com'))
------------------------------------------------------------------------
OPERATOR: all
  OPERATOR: or
    OPERATOR: eq
      OPERAND: SocialLogins/Name
      OPERAND: "Facebook"
    OPERATOR: endswith
      OPERAND: SocialLogins/Url
      OPERAND: "google.com"
------------------------------------------------------------------------
OPERATORS:
  all, endswith, eq, or
PROPERTIES:
  SocialLogins/Name, SocialLogins/Url
USAGE:
  SocialLogins/Name: eq, or, all
  SocialLogins/Url: endswith, or, all

------------------------------------------------------------------------
EXPRESSION: sponsor/phoneNumbers/all(p: p eq '123-456-7890')
------------------------------------------------------------------------
OPERATOR: all
  OPERATOR: eq
    OPERAND: Sponsor/PhoneNumbers
    OPERAND: "123-456-7890"
------------------------------------------------------------------------
OPERATORS:
  all, eq
PROPERTIES:
  Sponsor/PhoneNumbers
USAGE:
  Sponsor/PhoneNumbers: eq, all

------------------------------------------------------------------------
EXPRESSION: sponsor/socialLogins/all(s: s/name eq 'Facebook')
------------------------------------------------------------------------
OPERATOR: all
  OPERATOR: eq
    OPERAND: Sponsor/SocialLogins/Name
    OPERAND: "Facebook"
------------------------------------------------------------------------
OPERATORS:
  all, eq
PROPERTIES:
  Sponsor/SocialLogins/Name
USAGE:
  Sponsor/SocialLogins/Name: eq, all
```