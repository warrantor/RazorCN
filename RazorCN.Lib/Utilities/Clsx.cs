using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace RazorCN.Lib.Utilities;

public static class Clsx
{
    public static string Join(params object?[] args)
    {
        var sb = new StringBuilder();

        foreach (var arg in args)
        {
            if (!IsTruthy(arg)) continue;

            var part = ToVal(arg);
            if (string.IsNullOrWhiteSpace(part)) continue;

            if (sb.Length > 0) sb.Append(' ');
            sb.Append(part);
        }

        return sb.ToString();
    }

    private static string ToVal(object? mix) => mix switch
    {
        null => "",
        string s => s,
        // numeric types -> invariant string
        sbyte or byte or short or ushort or int or uint or long or ulong or decimal
            => Convert.ToString(mix, CultureInfo.InvariantCulture) ?? string.Empty,
        float f when f != 0f && !float.IsNaN(f)
            => f.ToString(CultureInfo.InvariantCulture),
        double d when d != 0d && !double.IsNaN(d)
            => d.ToString(CultureInfo.InvariantCulture),

        // IEnumerable (arrays/lists), but not string (already handled)
        IEnumerable e => JoinEnumerable(e),

        // dictionaries / objects (POCO/anonymous) handled below
        _ => JoinObjectKeysWhereTruthy(mix)
    };

    private static string JoinEnumerable(IEnumerable e)
    {
        var sb = new StringBuilder();

        foreach (var item in e)
        {
            if (!IsTruthy(item)) continue;

            var part = ToVal(item);
            if (string.IsNullOrWhiteSpace(part)) continue;

            if (sb.Length > 0) sb.Append(' ');
            sb.Append(part);
        }

        return sb.ToString();
    }

    private static string JoinObjectKeysWhereTruthy(object obj)
    {
        // Prefer IDictionary<string, ?> if possible; fall back to reflection.
        if (TryAsDictionary(obj, out var dict))
        {
            var sb = new StringBuilder();
            foreach (var kv in dict)
            {
                if (!IsTruthy(kv.Value)) continue;
                if (sb.Length > 0) sb.Append(' ');
                sb.Append(kv.Key);
            }
            return sb.ToString();
        }

        // Reflection fallback (anonymous objects, POCOs)
        var t = obj.GetType();
        var sb2 = new StringBuilder();

        foreach (var p in t.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!p.CanRead) continue;
            object? value;
            try { value = p.GetValue(obj); } catch { continue; }

            if (!IsTruthy(value)) continue;
            if (sb2.Length > 0) sb2.Append(' ');
            sb2.Append(p.Name);
        }

        foreach (var f in t.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            object? value;
            try { value = f.GetValue(obj); } catch { continue; }

            if (!IsTruthy(value)) continue;
            if (sb2.Length > 0) sb2.Append(' ');
            sb2.Append(f.Name);
        }

        return sb2.ToString();
    }

    private static bool TryAsDictionary(object obj, out IReadOnlyDictionary<string, object?> dict) => obj switch
    {
        IDictionary<string, object?> d1 => (dict = new Dictionary<string, object?>(d1)) is not null,
        IDictionary<string, bool> d3 => (dict = d3.ToDictionary(k => k.Key, v => (object?)v.Value)) is not null,
        _ => (dict = default!) is not null && false
    };

    private static bool IsTruthy(object? v) => v switch
    {
        null => false,
        bool b => b,
        string s => s.Length > 0,

        // numbers: 0 is falsy; NaN is falsy
        sbyte n => n != 0,
        byte n => n != 0,
        short n => n != 0,
        ushort n => n != 0,
        int n => n != 0,
        uint n => n != 0,
        long n => n != 0,
        ulong n => n != 0,
        decimal n => n != 0m,
        float n => n != 0f && !float.IsNaN(n),
        double n => n != 0d && !double.IsNaN(n),

        // In JS, any non-null object is truthy
        _ => true
    };
}