namespace TagsCloud.TextProcessing.Normalizers.LemmaNormalizer;

public class LemmaNormalizer : IWordNormalizer
{
    private readonly ILemmatizer _lemmatizer;

    public LemmaNormalizer(ILemmatizer lemmatizer)
    {
        _lemmatizer = lemmatizer;
    }
    
    public string Normalize(string word)
    {
        return _lemmatizer.GetLemma(word);
    }
}