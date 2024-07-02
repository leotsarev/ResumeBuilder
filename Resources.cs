using System.Reflection;
using System.Text;

namespace Tsarev.ResumeBuilder;

public static class Resources
{
    public static string NormalizeCss => _normalizeCss.Value;
    
    private static readonly Lazy<string> _normalizeCss = new(() => ReadFile("normalize.css"));

    public static string DefaultCss => _defaultCss.Value;

    private static readonly Lazy<string> _defaultCss = new(() => ReadFile("default.css"));

    private static string ReadFile(string filename)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream($"{typeof(Resources).Namespace}.{filename}")!;
        var streamReader = new StreamReader(stream, Encoding.UTF8);
        return streamReader.ReadToEnd();
    }
}
