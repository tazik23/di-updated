namespace TagsCloud.TextProcessing.Filters;

public class MinLengthFilter : IWordFilter
{
    private readonly int _minLength;

    public MinLengthFilter(int minLength = 2)
    {
        _minLength = minLength;
    }

    public bool ShouldExclude(string word)
    {
        return word.Length <= _minLength;
    }
}