using Markdig.Extensions.Yaml;
using YamlDotNet.Serialization;

namespace Tsarev.ResumeBuilder
{
    public static class FrontMatterExtensions
    {
        private static readonly IDeserializer YamlDeserializer =
        new DeserializerBuilder()
        .IgnoreUnmatchedProperties()
        .Build();

        public static ResumeFrontMatter GetFrontMatter(this YamlFrontMatterBlock? block)
        {
            if (block == null)
                return new ResumeFrontMatter();

            var yaml =
           block
           // this is not a mistake
           // we have to call .Lines 2x
           .Lines // StringLineGroup[]
           .Lines // StringLine[]
           .OrderByDescending(x => x.Line)
           .Select(x => x.ToString().Replace("---", string.Empty))
           .Where(x => !string.IsNullOrWhiteSpace(x));

            return YamlDeserializer.Deserialize<ResumeFrontMatter>(string.Join("\n", yaml));
        }
    }
    

}
