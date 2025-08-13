namespace ODataSampleModels;
/// <summary>
/// Illustrates a class to which an OData filter can be applied.
/// </summary>
public class User
{
    /// <summary>
    /// User ID.
    /// </summary>
    public int? Id
    {
        get; set;
    }

    /// <summary>
    /// User type.
    /// </summary>
    public UserType? Type
    {
        get; set;
    }

    /// <summary>
    /// User name.
    /// </summary>
    public PersonName? Name
    {
        get; set;
    }

    /// <summary>
    /// Display name.
    /// </summary>
    public string? DisplayName
    {
        get; set;
    }

    /// <summary>
    /// Email address.
    /// </summary>
    public string? Email
    {
        get; set;
    }

    /// <summary>
    /// User enabled flag.
    /// </summary>
    public bool? Enabled
    {
        get; set;
    }

    /// <summary>
    /// Create date.
    /// </summary>
    public DateTime? CreateDate
    {
        get; set;
    }

    /// <summary>
    /// Sponsor (used to illustrate indefinite nesting).
    /// </summary>
    public User? Sponsor
    {
        get; set;
    }

    /// <summary>
    /// Phone numbers.
    /// </summary>
    public string[]? PhoneNumbers
    {
        get; set;
    }

    /// <summary>
    /// Social logins.
    /// </summary>
    public List<SocialLogin>? SocialLogins
    {
        get; set;
    }
}

