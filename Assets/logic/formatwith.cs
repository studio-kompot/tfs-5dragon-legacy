using System.Regex;
/* FIXME: MAKE IT BACKWARDS COMPATIBLE
namespace FormatS
{
    public static string FormatS(string format, IFormatProvider provider, object source)
    {
        if (format == null)
            throw new ArgumentNullException("format");
        Regex r = new Regex(@"(?<start>\{)+(?<property>[\w\.\[\]]+)(?<format>:[^}]+)?(?<end>\})+",
          RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        List<object> values = new List<object>();
        string rewrittenFormat = r.Replace(format, delegate (Match m)
        {
            Group startGroup = m.Groups["start"];
            Group propertyGroup = m.Groups["property"];
            Group formatGroup = m.Groups["format"];
            Group endGroup = m.Groups["end"];
            values.Add((propertyGroup.Value == "0")
        ? source
        : DataBinder.Eval(source, propertyGroup.Value));
            return new string('{', startGroup.Captures.Count) + (values.Count - 1) + formatGroup.Value
        + new string('}', endGroup.Captures.Count);
        });
        return string.Format(provider, rewrittenFormat, values.ToArray());
    }
}
*/