namespace DotNetExtras.OData;
/// <summary>
/// Defines the minimum and maximum numbers.
/// </summary>
/// <remarks>
/// Initializes the instance with the minimum and maximum counts.
/// </remarks>
/// <param name="min">
/// Minimum number of occurrences (0 = optional).
/// </param>
/// <param name="max">
/// Maximum number of occurrences (0 = unlimited). 
/// </param>
public class ODataFilterNodeCount
(
    int min,
    int max
)
{
    /// <summary>
    /// Initializes the instance with the default counts.
    /// </summary>
    public ODataFilterNodeCount(): this(0, 0)
    {
    }

    /// <summary>
    /// Initializes the instance with the required count (min=max).
    /// </summary>
    /// <param name="count">
    /// Required count.
    /// </param>
    public ODataFilterNodeCount
    (
        int count
    )
    : this(count, count)
    {
    }

    /// <summary>
    /// The minimum number of expected occurrences.
    /// </summary>
    /// <remarks>
    /// Zero means optional.
    /// </remarks>
    public int Min { get; set; } = min;

    /// <summary>
    /// The maximum number of expected occurrences.
    /// </summary>
    /// <remarks>
    /// Zero means there is no maximum.
    /// </remarks>
    public int Max { get; set; } = max;
}

