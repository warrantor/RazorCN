using TW_Merge;

namespace RazorCN.Lib.Utilities;

/// <summary>
/// Utility for merging Tailwind CSS class names, combining clsx-style 
/// conditional logic with tailwind-merge conflict resolution.
/// </summary>
public static class Cn
{
    /// <summary>
    /// Merges multiple Tailwind CSS class name values into a single string,
    /// resolving conflicts by keeping the last occurrence of conflicting classes.
    /// Supports strings, arrays, dictionaries, and objects with truthy/falsy logic.
    /// </summary>
    /// <param name="inputs">Class name values to merge (strings, arrays, objects, etc.)</param>
    /// <returns>A merged string of class names without conflicts</returns>
    /// <example>
    /// Cn.Merge("px-2 py-1", "px-4") // => "py-1 px-4"
    /// Cn.Merge("base", new { active = isActive, disabled = isDisabled })
    /// Cn.Merge("text-sm", condition &amp;&amp; "font-bold", new[] { "flex", "gap-2" })
    /// </example>
    public static string Merge(params object?[] inputs)
    {
        var combined = Clsx.Join(inputs);
        return TailwindMerge.Default.Merge(combined);
    }
}
