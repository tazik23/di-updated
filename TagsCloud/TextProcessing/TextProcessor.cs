using TagsCloud.TextProcessing.Filters;
using TagsCloud.TextProcessing.Normalizers;
using TagsCloud.TextProcessing.Tokenizers;

namespace TagsCloud.TextProcessing;

public class TextProcessor
{
    private readonly ITokenizer _tokenizer;
    private readonly IWordNormalizer[] _normalizers;
    private readonly IWordFilter[] _filters;

    public TextProcessor(ITokenizer tokenizer, IWordNormalizer[] normalizers, IWordFilter[] filters)
    {
        _tokenizer = tokenizer;
        _normalizers = normalizers;
        _filters = filters;
    }

    public IEnumerable<string> Process(string text)
    {
        var words = _tokenizer.Tokenize(text);

        foreach (var word in words)
        {
            var normalized = _normalizers
                .Aggregate(word, (current, normalizer) => normalizer.Normalize(current));

            if(!_filters.Any(f => f.ShouldExclude(normalized)))
                yield return normalized;
        }
    }
}