using Microsoft.OData;
using Microsoft.OData.UriParser;

namespace DotNetExtras.OData;

/// <summary>
/// Implements a node of the binary tree holding OData operator or operand use in a filter query.
/// </summary>
public class ODataFilterNode
{
    private int             _level = -1;
    private object?         _operator = null;
    private string?         _operatorName = null;
    private bool?           _isOperator = null;
    private bool?           _isOperand =  null;
    private List<string>?   _operands = null;
    private string?         _property = null;

    /// <summary>
    /// Name of the parent node used by 'any' or 'all' operators.
    /// </summary>
    /// <remarks>
    /// We need this property to be able to resolve property aliases,
    /// so in the expression 'sponsor/socialLogins/any(s: s/name eq 'Facebook')',
    /// instead of 's/name', we'd get the property 'sponsor/socialLogins/name'.
    /// </remarks>
    protected string? ParentName { get; private set; }

    /// <summary>
    /// Parent node.
    /// </summary>
    public ODataFilterNode? Parent {  get; private set; } 

    /// <summary>
    /// Left child node.
    /// </summary>
    public ODataFilterNode? Left { get; private set; }

    /// <summary>
    /// Right child node.
    /// </summary>
    public ODataFilterNode? Right { get; private set; }

    /// <summary>
    /// Level of the node from the root.
    /// </summary>
    public int Level
    {
        get
        {
            if (_level >= 0)
            {
                return _level;
            }

            _level = 0;

            ODataFilterNode? parent = Parent;

            while (parent != null)
            {
                _level++;
                parent = parent.Parent;
            }

            return _level;
        }
    }

    /// <summary>
    /// Native node object.
    /// </summary>
    public object Node { get; private set; }

    /// <summary>
    /// Defines the type of operator or operand.
    /// </summary>
    public QueryNodeKind? Kind => Node is SingleValueNode singleValueNode
                ? singleValueNode.Kind
                : (QueryNodeKind?)null;

    /// <summary>
    /// Indicates whether the node is an operator (as opposed to an operand).
    /// </summary>
    public bool IsOperator
    {
        get
        {
            if (_isOperator.HasValue)
            {
                return _isOperator.Value;
            }

            _isOperator = Node is
                UnaryOperatorNode or
                BinaryOperatorNode or
                SingleValueFunctionCallNode or
            InNode or
                AnyNode or
                AllNode;

            return _isOperator.Value;
        }
    }

    /// <summary>
    /// Native operator node object (for operator node).
    /// </summary>
    public object? Operator 
    { 
        get
        {
            if (_operator != null)
            {
                return _operator;
            }

            if (IsOperator)
            {
                if (Node is UnaryOperatorNode unaryOperatorNode)
                {
                    _operator = unaryOperatorNode.OperatorKind;
                }
                else if (Node is BinaryOperatorNode binaryOperatorNode)
                {
                    _operator = binaryOperatorNode.OperatorKind;
                }
                else if (Node is SingleValueFunctionCallNode singleValueFunctionCallNode)
                {
                    _operator = singleValueFunctionCallNode.Name;
                }
                else if (Node is InNode inNode)
                {
                    _operator = inNode.Kind;
                }
                else if (Node is AnyNode anyNode)
                {
                    _operator = anyNode.Kind;
                }
                else if (Node is AllNode allNode)
                {
                    _operator = allNode.Kind;
                }
            }           
            
            return _operator;
        }
    }

    /// <summary>
    /// Name of the operator as used in the filter query.
    /// </summary>
    /// <remarks>
    /// The name of the operator will be always be in lower case.
    /// </remarks>
    public string? OperatorName 
    { 
        get
        {
            if (_operatorName != null)
            {
                return _operatorName;
            }

            if (IsOperator)
            {
                string? operatorKind = Operator?.ToString();

                _operatorName = operatorKind switch
                {
                    "Not" => "not",
                    "Equal" => "eq",
                    "NotEqual" => "ne",
                    "GreaterThan" => "gt",
                    "GreaterThanOrEqual" => "ge",
                    "LessThan" => "lt",
                    "LessThanOrEqual" => "le",
                    _ => operatorKind?.ToLower(),
                };
            }
            
            return _operatorName;
        }
    }

    /// <summary>
    /// Indicates whether the node is an operand (as opposed to an operator).
    /// </summary>
    public bool IsOperand
    {
        get
        {
            if (_isOperand.HasValue)
            {
                return _isOperand.Value;
            }

            _isOperand = !IsOperator && Node is SingleValueNode or IEnumerable<QueryNode> or CollectionConstantNode;

            return _isOperand.Value;
        }
    }

    /// <summary>
    /// List of operands in the operand node.
    /// </summary>
    public List<string>? Operands 
    {
        get
        {
            if (_operands != null)
            {
                return _operands;
            }

            if (!IsOperand)
            {
                return null;
            }

            _operands = Node is IEnumerable<QueryNode> queryNodes
                ? queryNodes.Select(GetOperand).ToList()
                : Node is CollectionConstantNode collectionConstantNode
                    ? collectionConstantNode.Collection.Select(GetOperand).ToList()
                    : ([GetOperand(Node)]);

            return _operands;
        }
    }

    /// <summary>
    /// The left-most operand in the <see cref="Operands">Operands</see> list (when applicable).
    /// </summary>
    /// <remarks>
    /// Most property nodes have only one operand, but operands used in functions,
    /// such as `startsWith` will have the first operand identifying the property
    /// and the rest holding the values.
    /// Some functions, such as `contains`, may have two operands mapped to properties,
    /// such as in `contains(name, displayName)`, but in this case, the first operand
    /// is still uses as a primary and therefore, only the first operand is returned.
    /// </remarks>
    public string? Property 
    {
        get
        {
            if (_property != null)
            {
                return _property;
            }

            if (!IsOperand || Operands == null || Operands.Count == 0)
            {
                return null;
            }

            if (Node is SingleValuePropertyAccessNode or
                SingleValueOpenPropertyAccessNode or
                SingleComplexNode or
                NonResourceRangeVariableReferenceNode or
                IEnumerable<QueryNode>)
            {
                _property = Operands.First();
            }

            return _property;
        }
    }

    /// <summary>
    /// Gets the operand name from a tree node.
    /// </summary>
    /// <param name="node">
    /// OData filter tree node.
    /// </param>
    /// <returns>
    /// Operand name.
    /// </returns>
    private string GetOperand
    (
        object node
    )
    {
        if (node is ConvertNode convertNode)
        {
            node = convertNode.Source;
        }

        if (node is ConstantNode constantNode)
        {
            return constantNode.Value == null
                ? "null"
                : (constantNode.Value is string)
                    ? $"\"{constantNode.Value}\""
                    : (constantNode.Value is ODataEnumValue)
                        ? $"'{constantNode.Value}'"
                        : constantNode.Value.ToString() ?? "";
        }
        else if (node is SingleValuePropertyAccessNode singleValuePropertyAccessNode)
        {
            return GetPropertyName(singleValuePropertyAccessNode, ParentName);
        }
        else if (node is SingleValueOpenPropertyAccessNode singleValueOpenPropertyAccessNode)
        {
            return GetPropertyName(singleValueOpenPropertyAccessNode, ParentName);
        }
        else if (node is SingleComplexNode singleComplexNode)
        {
            return GetPropertyName(singleComplexNode, ParentName);
        }
        else if (node is NonResourceRangeVariableReferenceNode nonResourceRangeVariableReferenceNode)
        {
            return GetPropertyName(nonResourceRangeVariableReferenceNode, ParentName);
        }

        return node.ToString() ?? "";
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="node">
    /// Native OData filter node.
    /// </param>
    /// <param name="parent">
    /// Parent node.
    /// </param>
    /// <param name="parentName">
    /// Name of the parent from the 'any' or 'all' operation.
    /// </param>
    public ODataFilterNode
    (
        object node,
        ODataFilterNode? parent,
        string? parentName
    )
    {
        Parent      = parent;
        ParentName  = parentName;

        Node = GetNode(node);

        if (Node is UnaryOperatorNode unaryOperatorNode)
        {
            Left = new(GetNode(unaryOperatorNode.Operand), this, ParentName);
        }
        else if (Node is BinaryOperatorNode binaryOperatorNode)
        {
            Left = new(GetNode(binaryOperatorNode.Left), this, ParentName);
            Right = new(GetNode(binaryOperatorNode.Right), this, ParentName);
        }
        else if (Node is SingleValueFunctionCallNode singleValueFunctionCallNode)
        {
            Left = new(singleValueFunctionCallNode.Parameters, this, ParentName);
        }
        else if (Node is InNode inNode)
        {
            Left = new(GetNode(inNode.Left), this, ParentName);
            Right = new(GetNode(inNode.Right), this, ParentName);
        }
        else if (Node is LambdaNode lambdaNode)
        {
            Left = new ODataFilterNode(GetNode(lambdaNode.Body), this, GetParentName(lambdaNode));
        }
    }

    /// <summary>
    /// Get name of the parent property from the 'any' or 'all' operation.
    /// </summary>
    /// <param name="node">
    /// Node corresponding to the 'any' or 'all' operation.
    /// </param>
    /// <returns>
    /// Name of the parent.
    /// </returns>
    private static string? GetParentName
    (
        LambdaNode node
    )
    {
        string? parentName = null;

        if (node.Source is CollectionPropertyAccessNode collectionPropertyAccessNode)
        {
            parentName = GetPropertyName(collectionPropertyAccessNode, null);
        }
        else if (node.Source is CollectionComplexNode collectionComplexNode)
        {
            parentName = GetPropertyName(collectionComplexNode, null);
        }
        else if (node.Source is CollectionNavigationNode collectionNavigationNode)
        {
            parentName = GetPropertyName(collectionNavigationNode, null);
        }

        return parentName;
    }

    /// <summary>
    /// Implements special handling for the convert node.
    /// </summary>
    /// <param name="node">
    /// Any node.
    /// </param>
    /// <returns>
    /// If the node is a convert node, returns the source; otherwise, returns the node.
    /// </returns>
    private static object GetNode
    (
        object node
    )
    {
        return node is ConvertNode convertNode 
            ? convertNode.Source 
            : node;
    }

    /// <summary>
    /// Gets the name of the property from a specific node.
    /// </summary>
    /// <param name="node">
    /// Tree node.
    /// </param>
    /// <param name="parentName">
    /// Name of the parent from the 'any' or 'all' operation.
    /// </param>
    /// <returns>
    /// Name of the property.
    /// </returns>
    private static string GetPropertyName
    (
        NonResourceRangeVariableReferenceNode node,
        string? parentName
    )
    {
        return string.IsNullOrEmpty(parentName) 
            ? node.Name 
            : parentName;
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        CollectionComplexNode node,
        string? parentName
    )
    {
        string path = "";
        string parent;

        if (node.Source == null)
        {
            return GetPropertyName(node.Property.Name, parentName);
        }

        // The source property point to the parent object referencing this property.
        SingleResourceNode source = node.Source;

        while (source != null)
        {
            // There may be a couple of types used as sources.
            // In our example, UserFilter.Name is a SingleComplexNode, 
            // while UserFilter.Sponsor.Name is a SingleNavigationNode.
            // There may be other case, but I'm not sure how to test.
            if (source is SingleComplexNode singleComplexNode &&
                !string.IsNullOrEmpty(singleComplexNode.Property?.Name))
            {
                parent = singleComplexNode.Property.Name ?? "";
                source = singleComplexNode.Source;
            }
            else if (source is SingleNavigationNode singleNavigationNode &&
                !string.IsNullOrEmpty(singleNavigationNode.NavigationProperty?.Name))
            {
                parent = singleNavigationNode.NavigationProperty?.Name ?? "";
                source = singleNavigationNode.Source;
            }
            else
            {
                break;
            }

            path = string.IsNullOrEmpty(path)
                ? parent
                : $"{parent}/{path}";
        }

        return string.IsNullOrEmpty(path)
            ? GetPropertyName(node.Property.Name, parentName)
            : GetPropertyName(GetPropertyName(node.Property.Name, path), parentName);
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        CollectionNavigationNode node,
        string? parentName
    )
    {
        string path = "";
        string parent;

        if (node.Source == null)
        {
            return GetPropertyName(node.NavigationProperty.Name, parentName);
        }

        // The source property point to the parent object referencing this property.
        SingleResourceNode source = node.Source;

        while (source != null)
        {
            // There may be a couple of types used as sources.
            // In our example, UserFilter.Name is a SingleComplexNode, 
            // while UserFilter.Sponsor.Name is a SingleNavigationNode.
            // There may be other case, but I'm not sure how to test.
            if (source is SingleComplexNode singleComplexNode &&
                !string.IsNullOrEmpty(singleComplexNode.Property?.Name))
            {
                parent = singleComplexNode.Property.Name ?? "";
                source = singleComplexNode.Source;
            }
            else if (source is SingleNavigationNode singleNavigationNode &&
                !string.IsNullOrEmpty(singleNavigationNode.NavigationProperty?.Name))
            {
                parent = singleNavigationNode.NavigationProperty?.Name ?? "";
                source = singleNavigationNode.Source;
            }
            else
            {
                break;
            }

            path = string.IsNullOrEmpty(path)
                ? parent
                : $"{parent}/{path}";
        }

        return string.IsNullOrEmpty(path)
            ? GetPropertyName(node.NavigationProperty.Name, parentName)
            : GetPropertyName(GetPropertyName(node.NavigationProperty.Name, path), parentName);
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        CollectionPropertyAccessNode node,
        string? parentName
    )
    {
        string path = "";
        string parent;

        if (node.Source == null)
        {
            return GetPropertyName(node.Property.Name, parentName);
        }

        // The source property point to the parent object referencing this property.
        SingleValueNode source = node.Source;

        while (source != null)
        {
            // There may be a couple of types used as sources.
            // In our example, UserFilter.Name is a SingleComplexNode, 
            // while UserFilter.Sponsor.Name is a SingleNavigationNode.
            // There may be other case, but I'm not sure how to test.
            if (source is SingleComplexNode singleComplexNode &&
                !string.IsNullOrEmpty(singleComplexNode.Property?.Name))
            {
                parent = singleComplexNode.Property.Name ?? "";
                source = singleComplexNode.Source;
            }
            else if (source is SingleNavigationNode singleNavigationNode &&
                !string.IsNullOrEmpty(singleNavigationNode.NavigationProperty?.Name))
            {
                parent = singleNavigationNode.NavigationProperty?.Name ?? "";
                source = singleNavigationNode.Source;
            }
            else
            {
                break;
            }

            path = string.IsNullOrEmpty(path)
                ? parent
                : $"{parent}/{path}";
        }

        return string.IsNullOrEmpty(path)
            ? GetPropertyName(node.Property.Name, parentName)
            : GetPropertyName(GetPropertyName(node.Property.Name, path), parentName);
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        string name,
        string? parent
    )
    {
        return string.IsNullOrEmpty(parent)
            ? name
            : $"{parent}/{name}";
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        SingleComplexNode node,
        string? parent
    )
    {
        return string.IsNullOrEmpty(parent)
            ? node.Property.Name
            : $"{parent}/{node.Property.Name}";
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        SingleValuePropertyAccessNode node,
        string? parentName
    )
    {
        string path = "";
        string parent;

        if (node.Source == null)
        {
            return GetPropertyName(node.Property.Name, parentName);
        }

        // The source property point to the parent object referencing this property.
        SingleValueNode source = node.Source;

        while (source != null)
        {
            // There may be a couple of types used as sources.
            // In our example, UserFilter.Name is a SingleComplexNode, 
            // while UserFilter.Sponsor.Name is a SingleNavigationNode.
            // There may be other case, but I'm not sure how to test.
            if (source is SingleComplexNode singleComplexNode &&
                !string.IsNullOrEmpty(singleComplexNode.Property?.Name))
            {
                parent = singleComplexNode.Property.Name ?? "";
                source = singleComplexNode.Source;
            }
            else if (source is SingleNavigationNode singleNavigationNode &&
                !string.IsNullOrEmpty(singleNavigationNode.NavigationProperty?.Name))
            {
                parent = singleNavigationNode.NavigationProperty?.Name ?? "";
                source = singleNavigationNode.Source;
            }
            else
            {
                break;
            }

            path = string.IsNullOrEmpty(path)
                ? parent
                : $"{parent}/{path}";
        }

        return string.IsNullOrEmpty(path)
            ? GetPropertyName(node.Property.Name, parentName)
            : GetPropertyName(GetPropertyName(node.Property.Name, path), parentName);
    }

    /// <inheritdoc cref="GetPropertyName(NonResourceRangeVariableReferenceNode, string?)"/>
    private static string GetPropertyName
    (
        SingleValueOpenPropertyAccessNode node,
        string? parentName
    )
    {
        string path = "";
        string parent;

        if (node.Source == null)
        {
            return GetPropertyName(node.Name, parentName);
        }

        // The source property point to the parent object referencing this property.
        SingleValueNode source = node.Source;

        while (source != null)
        {
            // There may be a couple of types used as sources.
            // In our example, UserFilter.Name is a SingleComplexNode, 
            // while UserFilter.Sponsor.Name is a SingleNavigationNode.
            // There may be other case, but I'm not sure how to test.
            if (source is SingleComplexNode singleComplexNode &&
                !string.IsNullOrEmpty(singleComplexNode.Property?.Name))
            {
                parent = singleComplexNode.Property.Name ?? "";
                source = singleComplexNode.Source;
            }
            else if (source is SingleNavigationNode singleNavigationNode &&
                !string.IsNullOrEmpty(singleNavigationNode.NavigationProperty?.Name))
            {
                parent = singleNavigationNode.NavigationProperty?.Name ?? "";
                source = singleNavigationNode.Source;
            }
            else
            {
                break;
            }

            path = string.IsNullOrEmpty(path)
                ? parent
                : $"{parent}/{path}";
        }

        return string.IsNullOrEmpty(path)
            ? GetPropertyName(node.Name, parentName)
            : GetPropertyName(GetPropertyName(node.Name, path), parentName);
    }

    /// <summary>
    /// Prints a line of text with proper indentation for the given node level.
    /// </summary>
    /// <param name="message">
    /// Message or message format.
    /// </param>
    /// <param name="args">
    /// Optional message arguments.
    /// </param>
    private void WriteLine
    (
        string message, 
        params object[] args
    )
    {
        string indent = new(' ', Level * 2);
        Console.WriteLine(indent + message, args);
    }

    /// <summary>
    /// Prints node information to console.
    /// </summary>
    /// <param name="withLabels">
    /// Indicates whether to print labels.
    /// </param>
    /// <param name="withNodeKind">
    /// Indicates whether to print node type/kind.
    /// </param>
    public void Print
    (
        bool withLabels = false,
        bool withNodeKind = false
    )
    {
        if (withNodeKind && Kind != null)
        {
            WriteLine(FormatNodeType(Kind, withLabels));
        }

        if (Operator != null)
        {
            WriteLine(FormatOperator(OperatorName ?? Operator, withLabels));
        }

        if (Operands != null && Operands.Count > 0)
        {
            foreach (object operand in Operands)
            {
                WriteLine(FormatOperand(operand, withLabels));
            }
        }

        Left?.Print(withLabels, withNodeKind);

        Right?.Print(withLabels, withNodeKind);
    }

    /// <summary>
    /// Formats the message with the node type info.
    /// </summary>
    /// <param name="node">
    /// Node object.
    /// </param>
    /// <param name="withLabels">
    /// Indicates whether to print labels.
    /// </param>
    /// <returns>
    /// Formatted string.
    /// </returns>
    private static string FormatNodeType
    (
        object node,
        bool withLabels = false
    )
    {
        return withLabels
            ? "NODE: " + node.ToString() 
            : node.ToString() ?? "";
    }

    /// <summary>
    /// Formats the message with the operator name.
    /// </summary>
    /// <param name="name">
    /// Name of the operator.
    /// </param>
    /// <param name="withLabels">
    /// Indicates whether to print labels.
    /// </param>
    /// <returns>
    /// Formatted string.
    /// </returns>
    private static string FormatOperator
    (
        object name,
        bool withLabels = false
    )
    {
        return withLabels? "OPERATOR: " + name.ToString() : name.ToString() ?? "";
    }

    /// <summary>
    /// Formats the message with the operand name (property name or value).
    /// </summary>
    /// <param name="operand">
    /// Name of the operator.
    /// </param>
    /// <param name="withLabels">
    /// Indicates whether to print labels.
    /// </param>
    /// <returns>
    /// Formatted string.
    /// </returns>
    private static string FormatOperand
    (
        object? operand,
        bool withLabels = false
    )
    {
        return withLabels ? "OPERAND: " + operand?.ToString() : operand?.ToString() ?? "";
    }
}
