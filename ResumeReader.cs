namespace Tsarev.ResumeBuilder;

internal class ResumeReader(string? markdownResumeName)
{
    public async Task<string[]> ReadSourceResume()
    {
        string text;

        if (markdownResumeName is null)
        {
            using var reader = new StreamReader(Console.OpenStandardInput());
            text = await reader.ReadToEndAsync();

        }
        else
        {
            text = await File.ReadAllTextAsync(markdownResumeName);
        }
        return text.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
