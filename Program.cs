using Markdig;
using McMaster.Extensions.CommandLineUtils;
using System.Diagnostics.CodeAnalysis;

namespace Tsarev.ResumeBuilder;

[HelpOption]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class Program
{
    public static int Main(string[] args)
        => CommandLineApplication.Execute<Program>(args);

    [Argument(0, Description = "Имя markdown файла")]
    public string MarkdownResumeName { get; } = "resume.md";

    [Argument(1, Description = "Имя готового резюме")]
    public string ResumeName { get; } = "resume.html";

    [Option]
    public string[] IncludeTags { get; } = [];

    public void OnExecute()
    {
        Console.WriteLine($"Will parse '{MarkdownResumeName}' file using tags {string.Join(", ", IncludeTags)}");

        var sourceResumeLines = File.ReadAllLines( MarkdownResumeName );

        var sectionsFilter = new SectionsFilter(IncludeTags);

        var resultingFile = string.Join("\n", sectionsFilter.GetFilteredLines(sourceResumeLines));

        var markdownPipeline = new MarkdownPipelineBuilder()
                    .UseMediaLinks()
                    .UseAutoLinks()
                    .UseYamlFrontMatter()
            .Build();

        var parsed = Markdown.Parse(resultingFile, markdownPipeline);

        var htmlOutput = new HtmlOutput(markdownPipeline, parsed);

        File.WriteAllText(ResumeName, htmlOutput.WriteHtml());
        Console.WriteLine($"Result written to {ResumeName}");
    }

}