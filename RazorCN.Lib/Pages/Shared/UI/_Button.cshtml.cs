using System.ComponentModel;
using System.Globalization;
using RazorCN.Lib.Utilities;

namespace RazorCN.Lib.Pages.Shared.UI;

/// <summary>
/// Button style variants.
/// </summary>
[TypeConverter(typeof(ButtonVariantConverter))]
public record ButtonVariant(string Value)
{
    public static readonly ButtonVariant Default = new("default");
    public static readonly ButtonVariant Destructive = new("destructive");
    public static readonly ButtonVariant Outline = new("outline");
    public static readonly ButtonVariant Secondary = new("secondary");
    public static readonly ButtonVariant Ghost = new("ghost");
    public static readonly ButtonVariant Link = new("link");

    public static ButtonVariant FromString(string? value) => value?.ToLowerInvariant() switch
    {
        "destructive" => Destructive,
        "outline" => Outline,
        "secondary" => Secondary,
        "ghost" => Ghost,
        "link" => Link,
        _ => Default 
    };

    public override string ToString() => Value;
}

public class ButtonVariantConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        value is string s ? ButtonVariant.FromString(s) : base.ConvertFrom(context, culture, value);
}

/// <summary>
/// Button size options.
/// </summary>
[TypeConverter(typeof(ButtonSizeConverter))]
public record ButtonSize(string Value)
{
    public static readonly ButtonSize Default = new("default");
    public static readonly ButtonSize Sm = new("sm");
    public static readonly ButtonSize Md = Default;
    public static readonly ButtonSize Lg = new("lg");
    public static readonly ButtonSize Xl = new("xl");
    public static readonly ButtonSize Icon = new("icon");
    public static readonly ButtonSize IconSm = new("icon-sm");
    public static readonly ButtonSize IconLg = new("icon-lg");

    public static ButtonSize FromString(string? value) => value?.ToLowerInvariant() switch
    {
        "sm" => Sm,
        "md" => Default,
        "lg" => Lg,
        "xl" => Xl,
        "icon" => Icon,
        "icon-sm" => IconSm,
        "icon-lg" => IconLg,
        _ => Default
    };

    public override string ToString() => Value;
}

public class ButtonSizeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        value is string s ? ButtonSize.FromString(s) : base.ConvertFrom(context, culture, value);
}

/// <summary>
/// HTML button type attribute options.
/// </summary>
[TypeConverter(typeof(ButtonTypeConverter))]
public record ButtonType(string Value)
{
    public static readonly ButtonType Default = new("button");
    public static readonly ButtonType Button = Default;
    public static readonly ButtonType Submit = new("submit");
    public static readonly ButtonType Reset = new("reset");

    public static ButtonType FromString(string? value) => value?.ToLowerInvariant() switch
    {
        "submit" => Submit,
        "reset" => Reset,
        _ => Default
    };

    public override string ToString() => Value;
}

public class ButtonTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        value is string s ? ButtonType.FromString(s) : base.ConvertFrom(context, culture, value);
}

/// <summary>
/// Button component model with shadcn-style variants
/// </summary>
public class ButtonModel
{
    /// <summary>
    /// Additional CSS classes
    /// </summary>
    public string Class { get; set; } = string.Empty;
    
    /// <summary>
    /// Button text content
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Button variant style.
    /// </summary>
    /// <remarks>Values: default, destructive, outline, secondary, ghost, link</remarks>
    public ButtonVariant Variant { get; set; } = ButtonVariant.Default;

    /// <summary>
    /// Button size.
    /// </summary>
    /// <remarks>Values: sm, md, lg, xl, icon, icon-sm, icon-lg</remarks>
    public ButtonSize Size { get; set; } = ButtonSize.Default;

    /// <summary>
    /// HTML button type attribute.
    /// </summary>
    /// <remarks>Values: button, submit, reset</remarks>
    public ButtonType Type { get; set; } = ButtonType.Button;

    /// <summary>
    /// Make button full width
    /// </summary>
    /// <remarks>Makes the button full width</remarks>
    public bool Block { get; set; } = false;

    /// <summary>
    /// Disable the button
    /// </summary>
    public bool Disabled { get; set; } = false;

    // Base classes shared by all button variants
    private const string BaseClasses =
        "inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium " +
        "transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring " +
        "focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50";

    /// <summary>
    /// Gets the computed CSS classes for the button based on variant and size.
    /// </summary>
    public string Classes => Cn.Merge(
        BaseClasses,
        GetVariantClasses(),
        GetSizeClasses(),
        Block ? "w-full" : null, 
        Class
    );

    private string GetVariantClasses() => Variant.Value switch
    {
        "destructive" => "bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/90",
        "outline" => "border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground",
        "secondary" => "bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80",
        "ghost" => "hover:bg-accent hover:text-accent-foreground",
        "link" => "text-primary underline-offset-4 hover:underline",
        _ => "bg-primary text-primary-foreground shadow-sm hover:bg-primary/90" // default
    };

    private string GetSizeClasses() => Size.Value switch
    {
        "sm" => "h-8 px-3 text-xs",
        "lg" => "h-10 px-6",
        "xl" => "h-11 px-8",
        "icon" => "h-9 w-9",
        "icon-sm" => "h-8 w-8",
        "icon-lg" => "h-10 w-10",
        _ => "h-9 px-4 py-2" // default
    };
}
