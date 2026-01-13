using System.Text.RegularExpressions;

namespace TW_Merge;

/// <summary>
/// Validators for matching Tailwind class patterns.
/// </summary>
public static partial class ClassValidators
{
    /// <summary>Matches arbitrary values like [100px], [#fff], [calc(100%-20px)]</summary>
    public static bool IsArbitraryValue(string value) => ArbitraryValueRegex().IsMatch(value);

    /// <summary>Matches numeric values like 0, 1, 2, 0.5, 1.5, etc.</summary>
    public static bool IsNumber(string value) => NumberRegex().IsMatch(value);

    /// <summary>Matches integer values like 0, 1, 2, 10, etc.</summary>
    public static bool IsInteger(string value) => IntegerRegex().IsMatch(value);

    /// <summary>Matches length values like 0, px, 1, 2, full, screen, etc.</summary>
    public static bool IsLength(string value) =>
        IsNumber(value) || LengthKeywords.Contains(value) || IsArbitraryLength(value);

    /// <summary>Matches arbitrary length values like [100px], [10rem], etc.</summary>
    public static bool IsArbitraryLength(string value) =>
        ArbitraryLengthRegex().IsMatch(value);

    /// <summary>Matches spacing scale values</summary>
    public static bool IsSpacing(string value) =>
        IsNumber(value) || SpacingKeywords.Contains(value) || IsArbitraryValue(value);

    /// <summary>Matches percentage values like 1/2, 2/3, etc.</summary>
    public static bool IsFraction(string value) => FractionRegex().IsMatch(value);

    /// <summary>Matches t-shirt sizes like xs, sm, md, lg, xl, 2xl, etc.</summary>
    public static bool IsTshirtSize(string value) => TshirtSizeRegex().IsMatch(value);

    /// <summary>Matches color values</summary>
    public static bool IsColor(string value) =>
        ColorKeywords.Contains(value) || IsArbitraryColor(value);

    /// <summary>Matches arbitrary color values</summary>
    public static bool IsArbitraryColor(string value) =>
        ArbitraryColorRegex().IsMatch(value);

    /// <summary>Matches any value (catch-all)</summary>
    public static bool IsAny(string _) => true;

    // Keyword sets
    private static readonly HashSet<string> LengthKeywords =
        ["px", "full", "screen", "svw", "lvw", "dvw", "svh", "lvh", "dvh", "min", "max", "fit"];

    private static readonly HashSet<string> SpacingKeywords =
        ["px", "auto"];

    private static readonly HashSet<string> ColorKeywords =
    [
        "inherit", "current", "transparent", "black", "white",
        "slate", "gray", "zinc", "neutral", "stone",
        "red", "orange", "amber", "yellow", "lime",
        "green", "emerald", "teal", "cyan", "sky",
        "blue", "indigo", "violet", "purple", "fuchsia",
        "pink", "rose"
    ];

    // Regex patterns
    [GeneratedRegex(@"^\[.+\]$")]
    private static partial Regex ArbitraryValueRegex();

    [GeneratedRegex(@"^(\d+(\.\d+)?|(\.\d+))$")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex IntegerRegex();

    [GeneratedRegex(@"^\[(\d+(\.\d+)?(px|em|rem|%|vh|vw|svh|svw|lvh|lvw|dvh|dvw|ch|ex|cm|mm|in|pt|pc)?|calc\(.+\)|min\(.+\)|max\(.+\)|clamp\(.+\))\]$")]
    private static partial Regex ArbitraryLengthRegex();

    [GeneratedRegex(@"^\d+/\d+$")]
    private static partial Regex FractionRegex();

    [GeneratedRegex(@"^(xs|sm|md|lg|xl|2xl|3xl|4xl|5xl|6xl|7xl|8xl|9xl)$")]
    private static partial Regex TshirtSizeRegex();

    [GeneratedRegex(@"^\[(#[0-9a-fA-F]{3,8}|rgba?\(.+\)|hsla?\(.+\)|hwb\(.+\)|lab\(.+\)|lch\(.+\)|oklch\(.+\)|oklab\(.+\)|color\(.+\))\]$")]
    private static partial Regex ArbitraryColorRegex();
}
