using YamlDotNet.Serialization;

namespace Tsarev.ResumeBuilder
{
    public class ResumeFrontMatter
    {
        [YamlMember(Alias = "title")]
        public string? Title { get; set; }

        [YamlMember(Alias = "description")]
        public string? Description { get; set; }

        [YamlMember(Alias = "author")]
        public string? Author { get; set; }

        [YamlMember(Alias = "language")]
        public string? Language { get; set; } = "ru";
    }
}
