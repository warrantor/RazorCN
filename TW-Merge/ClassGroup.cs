namespace TW_Merge;

/// <summary>
/// Represents a group of Tailwind classes that can conflict with each other.
/// </summary>
public sealed class ClassGroup(string id)
{
    public string Id { get; } = id;
    public HashSet<string> ClassNames { get; } = [];
    public List<Func<string, bool>> Validators { get; } = [];
    public HashSet<string> ConflictingGroups { get; } = [];

    public ClassGroup WithClasses(params string[] classes)
    {
        foreach (var c in classes) ClassNames.Add(c);
        return this;
    }

    public ClassGroup WithValidator(Func<string, bool> validator)
    {
        Validators.Add(validator);
        return this;
    }

    public ClassGroup ConflictsWith(params string[] groupIds)
    {
        foreach (var id in groupIds) ConflictingGroups.Add(id);
        return this;
    }

    public bool Matches(string className)
    {
        if (ClassNames.Contains(className)) return true;
        return Validators.Any(v => v(className));
    }
}
