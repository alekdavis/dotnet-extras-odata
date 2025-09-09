using DotNetExtras.Extended;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DotNetExtras.OData;
/// <summary>
/// Defines collections of rules that can be applied to the operators and properties used in an OData filter expression.
/// </summary>
public class ODataFilterRules
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes instance with empty rules. 
    /// </summary>
    public ODataFilterRules()
    {
        Operators  = new(StringComparer.InvariantCultureIgnoreCase);
        Properties = new(StringComparer.InvariantCultureIgnoreCase);
        PropertyOperators = new(StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Initializes instance with specific rules defined for operators and/or properties.
    /// </summary>
    /// <param name="operators">
    /// Rules applied to the operators used in filter expression.
    /// </param>
    /// <param name="properties">
    /// Rules applied to the properties used in the filter expression.
    /// </param>
    /// <param name="propertyOperators">
    /// Rules applied to the operators and properties used in the filter expression,
    /// </param>
    public ODataFilterRules
    (
        Dictionary<string, ODataFilterNodeCount?> operators,
        Dictionary<string, ODataFilterNodeCount?> properties,
        Dictionary<string, Dictionary<string, ODataFilterNodeCount?>?> propertyOperators
    )
    {
        Operators  = operators;
        Properties = properties;
        PropertyOperators = propertyOperators ?? new(StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Rules applied to operators used in the filter expression,
    /// i.e. which operator can be used and how many times.
    /// </summary>
    public Dictionary<string, ODataFilterNodeCount?> Operators { get; set; }

    /// <summary>
    /// Rules applied to properties used in the filter expression,
    /// i.e. which property can be used and how many times.
    /// </summary>
    public Dictionary<string, ODataFilterNodeCount?> Properties { get; set; }

    /// <summary>
    /// Rules applied to operators and properties used in the filter expression,
    /// i.e. which operator can be used for which property and how many times.
    /// </summary>
    public Dictionary<string, Dictionary<string, ODataFilterNodeCount?>?> PropertyOperators { get; set; }

    /// <summary>
    /// Returns an instance deserialized from a JSON or text string.
    /// </summary>
    /// <param name="input">
    /// JSON or text representation of the rules.
    /// </param>
    /// <returns>
    /// Rules instance.
    /// </returns>
    /// <remarks>
    /// <para>
    /// When specifying the rules using a JSON string, it must reflect the structure of this class.
    /// </para>
    /// <para>
    /// A text version of the rules must be in the format <em>rule1[|rule2[|rule3[|...]]]</em> where 
    /// the rules are separated by the pipe (|) character.
    /// Each rule must follow the format <em>{[o|p]:}name{:{min}{,}{max}}</em>, 
    /// which only requires the name of the operator or property allowed in the OData filter expression
    /// to be included in the string value. 
    /// To explicitly mark the name as an operator or a property, 
    /// prefix the name with <em>o:</em> or <em>p:</em> respectively (generally, the prefix is not required). 
    /// To specify the minimum and/or maximum number of occurrences of the name or property 
    /// in the filter expression, append them after the name followed by the colon (:) character.
    /// If you only need to specify one non-default number, you can omit the other but keep the
    /// comma (,) character separating the minimum number from the maximum number in place
    /// (if you only specify one number without the comma, it will be interpreted as the 
    /// required number of occurrences, i.e. min=max).
    /// The maximum number of 0 means there is no maximum.
    /// If no minimum or maximum number is specified, the operator or property is considered optional
    /// (i.e. it is allowed but not required).
    /// Example: <em>eq|and:2,|o:ne:1,2|startsWith:1|type:,5|p:name/givenName:2</em>
    /// </para>
    /// </remarks>
    public static ODataFilterRules Deserialize
    (
        string input
    )
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new ODataFilterRules();
        }

        input = input.Trim();

        if (input.IsJson())
        {
            try
            {
                return JsonSerializer.Deserialize<ODataFilterRules>(input, _jsonSerializerOptions) 
                    ?? new ODataFilterRules();
            }
            catch (Exception ex) 
            {
                throw new Exception($"Cannot deserialize OData filter rules from the JSON string '{input}'.", ex);
            }
        }

        ODataFilterRules rules = new();

        string[] textRuleList = input.Split('|', StringSplitOptions.RemoveEmptyEntries);

        try
        {
            foreach (string textRule in textRuleList)
            {
                ParseTextRule(textRule, out bool isOperator, out string name, out int min, out int max);

                if (isOperator)
                {
                    rules.Operators.Add(name, new(min, max));
                }
                else
                {
                    ParsePropertyName(name, out name, out List<string>? operators);
                    
                    if (rules.Properties.TryGetValue(name, out ODataFilterNodeCount? existingCount))
                    {
                        int totalMin = Math.Min(min, existingCount!.Min);
                        int totalMax = max + existingCount!.Max;

                        rules.Properties[name] = new(totalMin, totalMax);
                    }
                    else
                    {
                        rules.Properties.Add(name, new(min, max));
                    }

                    if (operators != null && operators.Count > 0)
                    {
                        rules.PropertyOperators ??= new(StringComparer.InvariantCultureIgnoreCase);

                        if (!rules.PropertyOperators.TryGetValue(name, out Dictionary<string, ODataFilterNodeCount?>? propertyOperators))
                        {
                            propertyOperators = new(StringComparer.InvariantCultureIgnoreCase);
                            rules.PropertyOperators.Add(name, propertyOperators);
                        }

                        foreach (string operatorName in operators)
                        {
                            propertyOperators![operatorName] = new(min, max);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Cannot deserialize OData filter rules from the text string '{input}'.", ex);
        }

        return rules;
    }

    /// <summary>
    /// Indicates whether the name belongs to an operator or a property
    /// and populates the name, min, and max values.
    /// </summary>
    /// <param name="input">
    /// A single rule in a text form.
    /// </param>
    /// <param name="isOperator">
    /// Returns the flag indicating whether the rule applies to an operator or a property.
    /// </param>
    /// <param name="name">
    /// Returned name.
    /// </param>
    /// <param name="min">
    /// Detected minimum number of the occurrences.
    /// </param>
    /// <param name="max">
    /// Detected maximum number of occurrences.
    /// </param>
    /// <returns>
    /// Indicates if the rule applies and the extracted name belongs to an operator.
    /// </returns>
    private static void ParseTextRule
    (
        string input,
        out bool isOperator,
        out string name,
        out int min,
        out int max
    ) 
    {
        isOperator     = false;
        bool isUndetermined = true;

        min = 0;
        max = 0;

        // Split string into parts.
        // Examples:
        //
        // eq
        // o:eq
        // eq:1
        // o:eq:1
        // eq:1,
        // o:eq:1,
        // eq:,2
        // o:eq:,2
        // eq:1,2
        // o:eq:1,2

        string[] inputs = input.Split(':', StringSplitOptions.TrimEntries);
        string? prefix, count;

        if (inputs.Length >= 3)
        {
            // If we got all three parts, it's easy.
            prefix = inputs[0];
            name   = inputs[1];
            count  = inputs[2];
        }
        else if (inputs.Length == 2)
        {
            // If we got two parts, it may be prefix:name or name:counts,
            // so, we'll check if the second part only contains numbers
            // and a possible comma character.
            if (Regex.IsMatch(inputs[1], "^[0-9]*[,]{0,1}[0-9]*$"))
            {
                prefix = null;
                name   = inputs[0];
                count  = inputs[1];
            }
            else
            {
                prefix = inputs[0];
                name   = inputs[1];
                count  = null;
            }
        }
        // If we only got one part, it's also easy.
        else
        {
            prefix = null;
            name   = inputs[0];
            count  = null;
        }

        // If we got count, see if both numbers are present or only one.
        if (count != null)
        {
            string[] counts = count.Split(',', StringSplitOptions.TrimEntries);

            // If we got comma, the min/max numbers may be different.
            if (counts.Length >= 2) 
            {
                if (!string.IsNullOrEmpty(counts[0]))
                {
                    min = int.Parse(counts[0]);
                }

                if (!string.IsNullOrEmpty(counts[1]))
                {
                    max = int.Parse(counts[1]);
                }
            }
            // If we have no comma, then min=max
            else if (counts.Length == 1)
            {
                if (!string.IsNullOrEmpty(counts[0]))
                {
                    min = int.Parse(counts[0]);
                    max = min;
                }
            }
        }

        // If we got the prefix, we can skip auto detection.
        if (!string.IsNullOrEmpty(prefix))
        {
            if (prefix.StartsWith("o", StringComparison.InvariantCultureIgnoreCase))
            {
                isOperator      = true;
                isUndetermined  = false;
            }
            else if (prefix.StartsWith("p", StringComparison.InvariantCultureIgnoreCase))
            {
                isOperator      = false;
                isUndetermined  = false;
            }
        }

        // The prefix was not specified, so we need to determine if the entity is an operator or a property.
        if (isUndetermined)
        {
            // If the name contains a slash (/), it must be a property.
            if (name.Contains('/'))
            {
                isUndetermined  = false;
                isOperator      = false;
            }
        }

        if (isUndetermined)
        {
            // We know about these operators.
            isOperator = name.ToLower() switch
            {
                "not" or 
                "and" or 
                "or" or 
                "eq" or 
                "ne" or 
                "gt" or 
                "ge" or 
                "lt" or 
                "le" or 
                "in" or
                "has" or
                "startswith" or 
                "endswith" or 
                "contains" or 
                "substringof" or
                "any" or 
                "all" => true,
                _ => false,
            };
        }
    }

    private static void ParsePropertyName
    (
        string input,
        out string name,
        out List<string>? operators
    )
    {
        int startIndex = input.IndexOf('[');
        int endIndex = input.IndexOf(']');

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            name = input[..startIndex].Trim();

            string substring = input.Substring(startIndex + 1, endIndex - startIndex - 1);

            operators = substring.Length == 0 
                ? null 
                : [.. substring.Split(',', StringSplitOptions.TrimEntries)];
        }
        else
        {
            name = input.Trim();
            operators = null;
        }
    }
}

