namespace ODataSampleModels;

/// <summary>
/// Demonstrates complex property.
/// </summary>
public class PersonName
{
    /// <summary>
    /// Given name.
    /// </summary>
    public string? GivenName
    {
        get; set;
    }

    /// <summary>
    /// Nickname.
    /// </summary>
    public string? NickName
    {
        get; set;
    }

    /// <summary>
    /// Surname.
    /// </summary>
    public string? Surname
    {
        get; set;
    }

    /// <summary>
    /// Middle initial.
    /// </summary>
    public char? MiddleInitial
    {
        get; set;
    }
}
