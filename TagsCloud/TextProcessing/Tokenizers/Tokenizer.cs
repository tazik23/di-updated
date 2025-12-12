namespace TagsCloud.TextProcessing.Tokenizers;

public class Tokenizer : ITokenizer
{
    private readonly string[] _separators;

    public Tokenizer(string[] separators)
    {
        _separators = separators.Length > 0
            ? separators
            : ["\r\n", "\n", "\r"];
    }

    public IEnumerable<string> Tokenize(string text)
    {
        return text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
    }
}