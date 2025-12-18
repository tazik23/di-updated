namespace TagsCloud.TextProcessing.Filters;

public class StopWordsFilter : IWordFilter
{
    private readonly HashSet<string> _stopWords;

    public StopWordsFilter(HashSet<string> stopWords)
    {
        _stopWords = stopWords;
    }

    public bool ShouldExclude(string word)
    {
        return _stopWords.Contains(word);
    }
}