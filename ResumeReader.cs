namespace Tsarev.ResumeBuilder;

internal class ResumeReader(bool ReadFromStdin, string MarkdownResumeName)
{
    public async Task<string[]> ReadSourceResume()
    {
        string text;

        if (ReadFromStdin)
        {
            var reader = new StreamReader(Console.OpenStandardInput());
            text = await reader.ReadToEndAsync();

        }
        else
        {
            text = await File.ReadAllTextAsync(MarkdownResumeName);
        }
        return text.Split("\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
