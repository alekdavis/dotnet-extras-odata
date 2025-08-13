using System.Text;

namespace DotNetExtras.OData;
/// <summary>
/// Validates OData filter expression for the given data type and the set of rules.
/// </summary>
/// <typeparam name="T">
/// Data type of the primary object used as a model for the OData filter expression.
/// </typeparam>
/// <remarks>
/// For the format of the rules, see <see cref="ODataFilterRules.Deserialize(string)"/> documentation.
/// </remarks>
/// <example>
/// /// <code>
/// <![CDATA[
/// string filter = "Type eq 'Employee' and startsWith(name/givenName, 'john') and name/givenName ne 'Johnson'";
/// string rules  = "eq|and:2|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2,";
/// 
/// ODataFilterValidator<User> validator = new(filter, rules);
/// 
/// if (validator.Failed)
/// {
///     Console.WriteLine(validator.Details);
/// }
/// ]]>
/// </code>
/// </example>
public class ODataFilterValidator<T>
{
    /// <summary>
    /// Errors encountered during validation.
    /// </summary>
    public List<string>? Errors { get; set; }

    /// <summary>
    /// Indicates whether validation passed.
    /// </summary>
    public bool Passed => Errors == null;

    /// <summary>
    /// Indicates whether validation failed.
    /// </summary>
    public bool Failed => !Passed;

    /// <summary>
    /// Errors encountered during validation and serialized as a single string.
    /// </summary>
    public string? Details
    {
        get
        {
            if (Errors == null || Errors.Count == 0)
            {
                return null;
            }

            if (Errors.Count == 1)
            {
                return Errors[0];
            }

            StringBuilder details = new(Errors[0]);

            for (int i = 1; i < Errors.Count; i++)
            {
                details.Append(" " + Errors[i]);
            }
            
            return details.ToString();
        }
    }

    /// <summary>
    /// Initializes instance for the rules serialized as a JSON or text string.
    /// </summary>
    /// <param name="filter">
    /// OData filter expression.
    /// </param>
    /// <param name="rules">
    /// Serialized rules (<see cref="ODataFilterRules.Deserialize(string)"/>).
    /// </param>
    public ODataFilterValidator
    (
        string filter,
        string rules
    )
    {
        ODataFilterRules? oDataFilterRules = string.IsNullOrWhiteSpace(rules)
            ? null
            : ODataFilterRules.Deserialize(rules);

        Initialize(filter, oDataFilterRules);
    }

    /// <summary>
    /// Initializes instance for the specified rules.
    /// </summary>
    /// <param name="filter">
    /// OData filter expression.
    /// </param>
    /// <param name="rules">
    /// Strongly typed rules.
    /// </param>
    public ODataFilterValidator
    (
        string filter,
        ODataFilterRules? rules = null
    )
    {
        Initialize(filter, rules);
    }

    /// <summary>
    /// Initializes instance.
    /// </summary>
    /// <param name="filter">
    /// OData filter expression.
    /// </param>
    /// <param name="rules">
    /// Strongly typed rules.
    /// </param>
    /// <exception cref="Exception"></exception>
    private void Initialize
    (
        string filter,
        ODataFilterRules? rules
    )
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(filter);

        ODataFilterTree<T> tree;

        tree = new(filter);
        
        if (rules == null)
        {
            return;
        }

        Errors = [];

        if (rules.Operators != null &&  rules.Operators.Count > 0) 
        {
            foreach (string op in tree.Operators)
            {
                // For some reason, ContainsKey() does not handle case even though the dictionary is case-insensitive.
                if (!rules.Operators.Keys.Contains(op, StringComparer.InvariantCultureIgnoreCase))
                {
                    string msg = $"Operator '{op}' is not allowed.";

                    if (!Errors.Contains(msg))
                    {
                        Errors.Add(msg);
                    }
                }
            }

            foreach (string op in rules.Operators.Keys)
            {
                ODataFilterNodeCount? rule = rules.Operators[op];

                if (rule == null)
                {
                    continue;
                }

                int count = tree.Operators.Count(v => string.Equals(v, op, StringComparison.InvariantCultureIgnoreCase));

                if (rule.Min > 0 && count == 0)
                {
                    Errors.Add($"Missing required operator '{op}'.");
                }
                else if (rule.Min > 0 && rule.Max > 0 && rule.Min == rule.Max && count != rule.Min)
                {
                    string expectedTimes = rule.Max == 1 ? "time" : "times";
                    string actualTimes   = count == 1 ? "time" : "times";

                    Errors.Add($"Operator '{op}' must be used exactly {rule.Max} {expectedTimes}, but it is used {count} {actualTimes}");
                }
                else if (rule.Min > 0 && count < rule.Min)
                {
                    string expectedTimes = rule.Min == 1 ? "time" : "times";
                    string actualTimes   = count == 1 ? "time" : "times";
                    
                    Errors.Add($"Operator '{op}' must be used at least {rule.Min} {expectedTimes}, but it is used {count} {actualTimes}.");
                }
                else if (rule.Max > 0 && count > rule.Max)
                {
                    string expectedTimes = rule.Max == 1 ? "time" : "times";
                    string actualTimes   = count == 1 ? "time" : "times";

                    Errors.Add($"Operator '{op}' can be used at most {rule.Max} {expectedTimes}, but it is used {count} {actualTimes}.");
                }
            }
        }

        if (rules.Properties != null &&  rules.Properties.Count > 0) 
        {
            foreach (string property in tree.Properties)
            {
                // For some reason, ContainsKey() does not handle case even though the dictionary is case-insensitive.
                if (!rules.Properties.Keys.Contains(property, StringComparer.InvariantCultureIgnoreCase))
                {
                    string msg = $"Property '{property}' is not allowed.";

                    if (!Errors.Contains(msg))
                    {
                        Errors.Add(msg);
                    }
                }
            }

            foreach (string property in rules.Properties.Keys)
            {
                ODataFilterNodeCount? rule = rules.Properties[property];

                if (rule == null)
                {
                    continue;
                }

                int count = tree.Properties.Count(v => string.Equals(v, property, StringComparison.InvariantCultureIgnoreCase));

                if (rule.Min > 0 && count == 0)
                {
                    Errors.Add($"Missing required property '{property}'.");
                }
                else if (rule.Min > 0 && rule.Max > 0 && rule.Min == rule.Max && count != rule.Min)
                {
                    string expectedTimes = rule.Max == 1 ? "time" : "times";
                    string actualTimes   = count == 1 ? "time" : "times";

                    Errors.Add($"Property '{property}' must be used exactly {rule.Max} {expectedTimes}, but it is used {count} {actualTimes}.");
                }
                else if (rule.Min > 0 && count < rule.Min)
                {
                    string expectedTimes = rule.Min == 1 ? "time" : "times";
                    string actualTimes   = count == 1 ? "time" : "times";
                    
                    Errors.Add($"Property '{property}' must be used at least {rule.Min} {expectedTimes}, but it is used {count} {actualTimes}.");
                }
                else if (rule.Max > 0 && count > rule.Max)
                {
                    string expectedTimes = rule.Max == 1 ? "time" : "times";
                    string actualTimes   = count == 1 ? "time" : "times";

                    Errors.Add($"Property '{property}' can be used at most {rule.Max} {expectedTimes}, but it is used {count} {actualTimes}.");
                }
            }
        }

        if (rules.PropertyOperators != null &&  rules.PropertyOperators.Count > 0) 
        {
            foreach (string property in tree.PropertyOperators.Keys)
            {
                // For some reason, ContainsKey() does not handle case even though the dictionary is case-insensitive.
                if (!rules.PropertyOperators.Keys.Contains(property, StringComparer.InvariantCultureIgnoreCase))
                {
                    string msg = $"Property '{property}' is not allowed.";

                    if (!Errors.Contains(msg))
                    {
                        Errors.Add(msg);
                    }
                }
                else
                {
                    foreach (string? op in tree.PropertyOperators[property])
                    {
                        if (op == null)
                        {
                            continue;
                        }

                        if (!rules.PropertyOperators[property]!.Keys.Contains(op, StringComparer.InvariantCultureIgnoreCase))
                        {
                            string msg = $"Operation '{op}' applied to property '{property}' is not allowed.";
                            if (!Errors.Contains(msg))
                            {
                                Errors.Add(msg);
                            }
                        }
                    }
                }
            }

            foreach (string property in rules.PropertyOperators.Keys)
            {
                foreach (string? op in rules.PropertyOperators![property]!.Keys)
                {
                    ODataFilterNodeCount? rule = rules.PropertyOperators[property]![op];

                    if (rule == null)
                    {
                        continue;
                    }

                    int count = tree.PropertyOperators[property].Count(v => string.Equals(v, op, StringComparison.InvariantCultureIgnoreCase));

                    if (rule.Min > 0 && count == 0)
                    {
                        Errors.Add($"Missing required operation '{op}' applied to property '{property}'.");
                    }
                    else if (rule.Min > 0 && rule.Max > 0 && rule.Min == rule.Max && count != rule.Min)
                    {
                        string expectedTimes = rule.Max == 1 ? "time" : "times";
                        string actualTimes   = count == 1 ? "time" : "times";

                        Errors.Add($"Operation '{op}' applied to property '{property}' must be used exactly {rule.Max} {expectedTimes}, but it is used {count} {actualTimes}");
                    }
                    else if (rule.Min > 0 && count < rule.Min)
                    {
                        string expectedTimes = rule.Min == 1 ? "time" : "times";
                        string actualTimes   = count == 1 ? "time" : "times";
                    
                        Errors.Add($"Operation '{op}' applied to property '{property}' must be used at least {rule.Min} {expectedTimes}, but it is used {count} {actualTimes}.");
                    }
                    else if (rule.Max > 0 && count > rule.Max)
                    {
                        string expectedTimes = rule.Max == 1 ? "time" : "times";
                        string actualTimes   = count == 1 ? "time" : "times";

                        Errors.Add($"Operation '{op}' applied to property '{property}' can be used at most {rule.Max} {expectedTimes}, but it is used {count} {actualTimes}.");
                    }
                }
            }
        }

        if (Errors.Count == 0)
        {
            Errors = null;
        }
    }
}

