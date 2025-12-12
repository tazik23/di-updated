namespace TagsCloud.TextProcessing.Filters;

public class BoringWordsFilter : IWordFilter
{
    private readonly HashSet<string> _boringWords;

    public BoringWordsFilter(HashSet<string> boringWords)
    {
        _boringWords = boringWords;
    }

    public bool ShouldExclude(string word)
    {
        return _boringWords.Contains(word);
    }
}