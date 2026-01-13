using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace RazorCN.Generators;

/// <summary>
/// Source generator that creates TagHelper classes for each component model
/// in the RazorCN.Lib/Pages/Shared/UI folder.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class ComponentTagHelperGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Find all class declarations that end with "Model" and are in the UI namespace
        var componentModels = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsComponentModelCandidate(node),
                transform: static (ctx, _) => GetComponentModelInfo(ctx))
            .Where(static model => model is not null)
            .Select(static (model, _) => model!);

        // Combine with compilation to access semantic information
        var compilationAndModels = context.CompilationProvider.Combine(componentModels.Collect());

        // Generate the source
        context.RegisterSourceOutput(compilationAndModels, static (spc, source) => Execute(source.Left, source.Right, spc));
    }

    private static bool IsComponentModelCandidate(SyntaxNode node)
    {
        // Look for class declarations ending with "Model"
        return node is ClassDeclarationSyntax classDecl &&
               classDecl.Identifier.Text.EndsWith("Model") &&
               classDecl.Modifiers.Any(SyntaxKind.PublicKeyword);
    }

    private static ComponentModelInfo? GetComponentModelInfo(GeneratorSyntaxContext context)
    {
        var classDecl = (ClassDeclarationSyntax)context.Node;
        var semanticModel = context.SemanticModel;
        var symbol = semanticModel.GetDeclaredSymbol(classDecl);

        if (symbol is null)
            return null;

        var namespaceName = symbol.ContainingNamespace.ToDisplayString();

        // Only process classes in the UI namespace
        if (!namespaceName.Contains(".Pages.Shared.UI"))
            return null;

        var className = symbol.Name;
        var componentName = className.EndsWith("Model")
            ? className.Replace("Model", string.Empty) // Remove "Model" suffix
            : className;

        // Get all public settable properties
        var properties = new List<PropertyInfo>();
        foreach (var member in symbol.GetMembers().OfType<IPropertySymbol>())
        {
            if (member.DeclaredAccessibility == Accessibility.Public &&
                member.SetMethod is not null &&
                member.SetMethod.DeclaredAccessibility == Accessibility.Public)
            {
                var xmlDocs = member.GetDocumentationCommentXml();
                var summary = ExtractSummary(xmlDocs);
                var remarks = ExtractRemarks(xmlDocs);

                properties.Add(new PropertyInfo(
                    member.Name,
                    member.Type.ToDisplayString(),
                    member.Type.TypeKind == TypeKind.Enum,
                    GetEnumFullName(member.Type),
                    summary,
                    remarks,
                    HasDefaultValue(member),
                    HasStaticDefaultField(member.Type)));
            }
        }

        return new ComponentModelInfo(
            namespaceName,
            className,
            componentName,
            [..properties]);
    }

    private static string? GetEnumFullName(ITypeSymbol type) => type.TypeKind == TypeKind.Enum ? type.ToDisplayString() : null;

    private static bool HasDefaultValue(IPropertySymbol property)
    {
        // Check if property has an initializer (default value)
        var syntaxRef = property.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef?.GetSyntax() is PropertyDeclarationSyntax propSyntax)
        {
            return propSyntax.Initializer is not null;
        }
        return false;
    }

    private static bool HasStaticDefaultField(ITypeSymbol type)
    {
        // Check if the type has a static readonly field named "Default"
        return type.GetMembers("Default")
            .OfType<IFieldSymbol>()
            .Any(f => f.IsStatic && f.IsReadOnly);
    }

    private static string ExtractSummary(string? xmlDocs)
    {
        if (string.IsNullOrEmpty(xmlDocs))
            return "";

        const string startTag = "<summary>";
        const string endTag = "</summary>";
        var startIndex = xmlDocs!.IndexOf(startTag, StringComparison.Ordinal);
        var endIndex = xmlDocs.IndexOf(endTag, StringComparison.Ordinal);

        if (startIndex >= 0 && endIndex > startIndex)
        {
            var summary = xmlDocs.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
            return summary.Trim();
        }

        return string.Empty;
    }

    private static string ExtractRemarks(string? xmlDocs)
    {
        if (string.IsNullOrEmpty(xmlDocs))
            return "";

        const string startTag = "<remarks>";
        const string endTag = "</remarks>";
        var startIndex = xmlDocs!.IndexOf(startTag, StringComparison.Ordinal);
        var endIndex = xmlDocs.IndexOf(endTag, StringComparison.Ordinal);

        if (startIndex >= 0 && endIndex > startIndex)
        {
            var remarks = xmlDocs.Substring(startIndex + startTag.Length, endIndex - startIndex - startTag.Length);
            return remarks.Trim();
        }

        return string.Empty;
    }

    private static void Execute(Compilation compilation, ImmutableArray<ComponentModelInfo> models, SourceProductionContext context)
    {
        if (models.IsDefaultOrEmpty)
            return;

        foreach (var model in models.Distinct())
        {
            var source = GenerateTagHelper(model);
            context.AddSource($"{model.ComponentName}TagHelper.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private static string GenerateTagHelper(ComponentModelInfo model)
    {
        var sb = new StringBuilder();

        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
        sb.AppendLine("using Microsoft.AspNetCore.Mvc.Rendering;");
        sb.AppendLine("using Microsoft.AspNetCore.Mvc.ViewFeatures;");
        sb.AppendLine("using Microsoft.AspNetCore.Razor.TagHelpers;");
        sb.AppendLine($"using {model.Namespace};");
        sb.AppendLine();

        // Generate the TagHelper namespace - use a dedicated namespace for TagHelpers
        var tagHelperNamespace = model.Namespace.Replace(".Pages.Shared.UI", ".TagHelpers");
        sb.AppendLine($"namespace {tagHelperNamespace};");
        sb.AppendLine();

        // Add XML documentation
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// TagHelper for the {model.ComponentName} component.");
        sb.AppendLine($"/// Renders the _{model.ComponentName} partial view with the specified model.");
        sb.AppendLine("/// </summary>");

        // Generate the TagHelper class with 'rcn-' prefix to avoid conflicts with native HTML elements
        var tagName = "rcn-" + ToKebabCase(model.ComponentName);
        sb.AppendLine($"[HtmlTargetElement(\"{tagName}\", TagStructure = TagStructure.NormalOrSelfClosing)]");
        sb.AppendLine($"public class {model.ComponentName}TagHelper : TagHelper");
        sb.AppendLine("{");

        // ViewContext property for rendering partial views
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// The current ViewContext.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    [ViewContext]");
        sb.AppendLine("    [HtmlAttributeNotBound]");
        sb.AppendLine("    public ViewContext ViewContext { get; set; } = null!;");
        sb.AppendLine();

        // IHtmlHelper for rendering partials
        sb.AppendLine("    private readonly IHtmlHelper _htmlHelper;");
        sb.AppendLine();

        // Constructor
        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Initializes a new instance of the <see cref=\"{model.ComponentName}TagHelper\"/> class.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    /// <param name=\"htmlHelper\">The HTML helper for rendering partial views.</param>");
        sb.AppendLine($"    public {model.ComponentName}TagHelper(IHtmlHelper htmlHelper)");
        sb.AppendLine("    {");
        sb.AppendLine("        _htmlHelper = htmlHelper;");
        sb.AppendLine("    }");
        sb.AppendLine();

        // Generate properties for each model property
        foreach (var prop in model.Properties)
        {
            if (!string.IsNullOrEmpty(prop.Summary))
            {
                sb.AppendLine("    /// <summary>");
                sb.AppendLine($"    /// {prop.Summary}");
                sb.AppendLine("    /// </summary>");
            }
            
            if (!string.IsNullOrEmpty(prop.Remarks))
            {
                sb.AppendLine($"    /// <remarks>{prop.Remarks}</remarks>");
            }

            var attributeName = ToKebabCase(prop.Name);
            sb.AppendLine($"    [HtmlAttributeName(\"{attributeName}\")]");
            
            // For record types with FromString, use string? to accept string values from attributes
            var propertyType = prop.HasStaticDefaultField ? "string?" : prop.TypeName;
            var defaultValue = prop.HasStaticDefaultField ? " = null;" : GetDefaultValueString(prop);
            sb.AppendLine($"    public {propertyType} {prop.Name} {{ get; set; }}{defaultValue}");
            sb.AppendLine();
        }

        // ProcessAsync method
        sb.AppendLine("    /// <inheritdoc />");
        sb.AppendLine("    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)");
        sb.AppendLine("    {");
        sb.AppendLine("        // Contextualize the HtmlHelper");
        sb.AppendLine("        ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);");
        sb.AppendLine();
        sb.AppendLine("        // Create the model");
        sb.AppendLine($"        var model = new {model.ClassName}");
        sb.AppendLine("        {");

        foreach (var prop in model.Properties)
        {
            // For record types with FromString, convert string to record type
            if (prop.HasStaticDefaultField)
            {
                sb.AppendLine($"            {prop.Name} = {prop.TypeName}.FromString(this.{prop.Name}),");
            }
            else
            {
                sb.AppendLine($"            {prop.Name} = this.{prop.Name},");
            }
        }

        sb.AppendLine("        };");
        sb.AppendLine();

        // Handle child content (slot)
        sb.AppendLine("        // Get child content if any");
        sb.AppendLine("        var childContent = await output.GetChildContentAsync();");
        sb.AppendLine("        var childContentString = childContent.GetContent();");
        sb.AppendLine();

        // Special handling for Text property if it exists and child content is provided
        var hasTextProp = model.Properties.Any(p => p.Name == "Text");
        if (hasTextProp)
        {
            sb.AppendLine("        // If child content provided and no explicit Text, use child content");
            sb.AppendLine("        if (!string.IsNullOrEmpty(childContentString) && string.IsNullOrEmpty(model.Text))");
            sb.AppendLine("        {");
            sb.AppendLine("            model.Text = childContentString;");
            sb.AppendLine("        }");
            sb.AppendLine();
        }

        sb.AppendLine("        // Suppress the wrapper element");
        sb.AppendLine("        output.TagName = null;");
        sb.AppendLine();
        sb.AppendLine("        // Render the partial view from the Razor Class Library");
        sb.AppendLine($"        var content = await _htmlHelper.PartialAsync(\"/Pages/Shared/UI/_{model.ComponentName}.cshtml\", model);");
        sb.AppendLine("        output.Content.SetHtmlContent(content);");
        sb.AppendLine("    }");

        sb.AppendLine("}");

        return sb.ToString();
    }

    private static string GetDefaultValueString(PropertyInfo prop)
    {
        // For reference types and nullable, don't add default
        if (prop.TypeName.EndsWith("?") || prop.TypeName == "string")
            return " = default!;";

        if (prop.IsEnum && prop.EnumFullName is not null)
            return $" = default({prop.EnumFullName});";

        // For record types with a static Default field (e.g., ButtonVariant.Default)
        if (prop.HasStaticDefaultField)
            return $" = {prop.TypeName}.Default;";

        if (prop.TypeName == "bool")
            return " = false;";

        if (prop.TypeName == "int" || prop.TypeName == "long" || prop.TypeName == "double" || prop.TypeName == "float" || prop.TypeName == "decimal")
            return " = default;";

        return " = default!;";
    }

    private static string ToKebabCase(string pascalCase)
    {
        if (string.IsNullOrEmpty(pascalCase))
            return pascalCase;

        var sb = new StringBuilder();
        for (int i = 0; i < pascalCase.Length; i++)
        {
            var c = pascalCase[i];
            if (char.IsUpper(c))
            {
                if (i > 0)
                    sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}

internal sealed record ComponentModelInfo(
    string Namespace,
    string ClassName,
    string ComponentName,
    ImmutableArray<PropertyInfo> Properties);

internal sealed record PropertyInfo(
    string Name,
    string TypeName,
    bool IsEnum,
    string? EnumFullName,
    string Summary,
    string Remarks,
    bool HasDefaultValue,
    bool HasStaticDefaultField);
