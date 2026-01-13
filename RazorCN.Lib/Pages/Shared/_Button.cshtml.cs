using RazorCN.Lib.Utilities;

namespace RazorCN.Lib.Pages.Shared;

/// <summary>
/// Button style variants
/// </summary>
public enum ButtonVariant
{
    Default,
    Destructive,
    Outline,
    Secondary,
    Ghost,
    Link
}

/// <summary>
/// Button component model with shadcn-style variants
/// </summary>
public class ButtonModel
{
    /// <summary>
    /// Button text content
    /// </summary>
    public string Text { get; set; } = "";

    /// <summary>
    /// Button variant style
    /// </summary>
    public ButtonVariant Variant { get; set; } = ButtonVariant.Default;

    /// <summary>
    /// Button size: sm, md (default), lg, xl, icon, icon-sm, icon-lg
    /// </summary>
    public string Size { get; set; } = "";

    /// <summary>
    /// HTML button type: button, submit, reset
    /// </summary>
    public string Type { get; set; } = "button";

    /// <summary>
    /// Make button full width
    /// </summary>
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
        Block ? "w-full" : null
    );

    private string GetVariantClasses() => Variant switch
    {
        ButtonVariant.Destructive => "bg-destructive text-destructive-foreground shadow-sm hover:bg-destructive/90",
        ButtonVariant.Outline => "border border-input bg-background shadow-sm hover:bg-accent hover:text-accent-foreground",
        ButtonVariant.Secondary => "bg-secondary text-secondary-foreground shadow-sm hover:bg-secondary/80",
        ButtonVariant.Ghost => "hover:bg-accent hover:text-accent-foreground",
        ButtonVariant.Link => "text-primary underline-offset-4 hover:underline",
        _ => "bg-primary text-primary-foreground shadow-sm hover:bg-primary/90" // Default
    };

    private string GetSizeClasses() => Size switch
    {
        "sm" => "h-8 px-3 text-xs",
        "lg" => "h-10 px-6",
        "xl" => "h-11 px-8",
        "icon" => "h-9 w-9",
        "icon-sm" => "h-8 w-8",
        "icon-lg" => "h-10 w-10",
        _ => "h-9 px-4 py-2" // default (md)
    };
}
