using System.Text.RegularExpressions;

namespace TW_Merge;

/// <summary>
/// Utility to merge Tailwind CSS classes without style conflicts.
/// C# port of tailwind-merge (https://github.com/dcastil/tailwind-merge).
/// </summary>
public sealed partial class TailwindMerge
{
    private readonly TwMergeConfig _config;
    private readonly Dictionary<string, string?> _classGroupCache = new();

    /// <summary>
    /// Default instance with standard Tailwind configuration.
    /// </summary>
    public static TailwindMerge Default { get; } = new(TwMergeConfig.Default());

    public TailwindMerge(TwMergeConfig config)
    {
        _config = config;
    }

    /// <summary>
    /// Merges Tailwind CSS classes, resolving conflicts by keeping the last occurrence.
    /// </summary>
    /// <param name="classes">Space-separated class strings to merge</param>
    /// <returns>Merged class string without conflicts</returns>
    public string Merge(params string?[] classes)
    {
        var input = string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var classNames = SplitClasses(input);
        return MergeClassList(classNames);
    }

    private string MergeClassList(List<string> classNames)
    {
        // Process from right to left, keeping track of seen groups
        var seenGroups = new HashSet<string>();
        var result = new List<string>();

        for (var i = classNames.Count - 1; i >= 0; i--)
        {
            var className = classNames[i];
            var parsed = ParseClassName(className);

            if (parsed.GroupId == null)
            {
                // Unknown class, always include
                result.Insert(0, className);
                continue;
            }

            var groupKey = GetGroupKey(parsed);

            if (seenGroups.Contains(groupKey))
            {
                // Skip this class, a later one with the same group exists
                continue;
            }

            seenGroups.Add(groupKey);

            // Also mark conflicting groups as seen
            if (_config.ClassGroups.TryGetValue(parsed.GroupId, out var group))
            {
                foreach (var conflictId in group.ConflictingGroups)
                {
                    var conflictKey = parsed.HasImportant
                        ? $"{parsed.Modifiers}!{conflictId}"
                        : $"{parsed.Modifiers}{conflictId}";
                    seenGroups.Add(conflictKey);
                }
            }

            result.Insert(0, className);
        }

        return string.Join(" ", result);
    }

    private ParsedClass ParseClassName(string className)
    {
        var hasImportant = className.StartsWith('!');
        var workingName = hasImportant ? className[1..] : className;

        // Split by : to get modifiers
        var parts = workingName.Split(':');
        var modifiers = parts.Length > 1
            ? string.Join(":", parts[..^1]) + ":"
            : "";
        var baseClass = parts[^1];

        // Handle negative prefix
        var isNegative = baseClass.StartsWith('-');
        if (isNegative) baseClass = baseClass[1..];

        var groupId = GetClassGroupId(baseClass);

        return new ParsedClass
        {
            Original = className,
            BaseClass = baseClass,
            Modifiers = modifiers,
            HasImportant = hasImportant,
            IsNegative = isNegative,
            GroupId = groupId
        };
    }

    private string? GetClassGroupId(string baseClass)
    {
        if (_classGroupCache.TryGetValue(baseClass, out var cached))
        {
            return cached;
        }

        foreach (var (id, group) in _config.ClassGroups)
        {
            if (group.Matches(baseClass))
            {
                _classGroupCache[baseClass] = id;
                return id;
            }
        }

        _classGroupCache[baseClass] = null;
        return null;
    }

    private static string GetGroupKey(ParsedClass parsed)
    {
        return parsed.HasImportant
            ? $"{parsed.Modifiers}!{parsed.GroupId}"
            : $"{parsed.Modifiers}{parsed.GroupId}";
    }

    private static List<string> SplitClasses(string input)
    {
        return WhitespaceRegex()
            .Split(input.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    private sealed class ParsedClass
    {
        public required string Original { get; init; }
        public required string BaseClass { get; init; }
        public required string Modifiers { get; init; }
        public required bool HasImportant { get; init; }
        public required bool IsNegative { get; init; }
        public required string? GroupId { get; init; }
    }
}

/// <summary>
/// Static convenience methods for TailwindMerge.
/// </summary>
public static class TwMergeExtensions
{
    /// <summary>
    /// Merges Tailwind CSS classes using the default configuration.
    /// </summary>
    public static string TwMerge(params string?[] classes) =>
        TailwindMerge.Default.Merge(classes);
}
