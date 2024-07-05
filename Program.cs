using Markdig;
using McMaster.Extensions.CommandLineUtils;

namespace Tsarev.ResumeBuilder;

public class Program
{
    public static Task<int> Main(string[] args)
    {
        var app = new CommandLineApplication
        {
        };
        var markdownResumeName = app.Option<string>("--input", "Имя markdown файла", CommandOptionType.SingleValue);
        var resumeName = app.Option<string>("--output", "Имя готового резюме", CommandOptionType.SingleValue);
        var includeTags = app.Option<string>("--include-tag <tags>", "Теги, которые нужно включить", CommandOptionType.MultipleValue);
        app.HelpOption();
        app.OnExecuteAsync(async token =>
        {
            await OnExecuteAsync(markdownResumeName.Value(), resumeName.Value(), [.. includeTags.Values]);
        });
        if (args.Length > 0 && args[0] == "dotnet")
        {
            args = args.Skip(2).ToArray();
        }
        return app.ExecuteAsync(args);
    }

    public static async Task OnExecuteAsync(string? markdownResumeName, string? htmlResumeName, string[] IncludeTags)
    {
        Console.WriteLine($"Will parse '{markdownResumeName}' file using tags {string.Join(", ", IncludeTags)}");
        var resumeReader = new ResumeReader(markdownResumeName);
        string[] sourceResumeLines = await resumeReader.ReadSourceResume();

        var sectionsFilter = new SectionsFilter(IncludeTags);

        var resultingFile = string.Join("\n", sectionsFilter.GetFilteredLines(sourceResumeLines));

        var markdownPipeline = new MarkdownPipelineBuilder()
                    .UseMediaLinks()
                    .UseAutoLinks()
                    .UseYamlFrontMatter()
            .Build();

        var parsed = Markdown.Parse(resultingFile, markdownPipeline);

        var htmlOutput = new HtmlOutput(markdownPipeline, parsed);

        if (htmlResumeName is null)
        {
            using var writer = new StreamWriter(Console.OpenStandardOutput());
            await writer.WriteAsync(htmlOutput.WriteHtml());
        }
        else
        {
            File.WriteAllText(htmlResumeName, htmlOutput.WriteHtml());
            Console.WriteLine($"Result written to {htmlResumeName}");
        }
    }
}