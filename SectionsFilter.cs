namespace Tsarev.ResumeBuilder
{
    internal class SectionsFilter(string[] IncludeTags)
    {
        const string TAG_START = "<!--";
        const string TAG_END = "-->";
        const string TAG_CLOSE = "<!--end-->";
        public IEnumerable<string> GetFilteredLines(string[] lines)
        {
            var withinTagSection = false;
            string[] currentTags = [];

            foreach (var line in lines)
            {
                if (line.StartsWith(TAG_CLOSE))
                {
                    //TODO: Warn if not in section
                    withinTagSection = false;
                    currentTags = [];
                }
                else if (line.Trim().StartsWith(TAG_START))
                {
                    withinTagSection = true;
                    var startIdx = line.IndexOf(TAG_START) + TAG_START.Length;
                    var endIdx = line.IndexOf(TAG_END);
                    if (endIdx == -1)
                    {
                        //TODO Warn
                        continue;
                    }
                    currentTags = [.. line[startIdx..endIdx].Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)];
                }
                else
                {
                    var shouldRender = !withinTagSection || !currentTags.Except(IncludeTags).Any();
                    if (shouldRender)
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}
