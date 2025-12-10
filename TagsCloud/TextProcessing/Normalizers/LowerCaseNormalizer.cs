namespace TagsCloud.TextProcessing.Normalizers;

public class LowerCaseNormalizer : IWordNormalizer
{
    public string Normalize(string word)
    {
        return word.ToLowerInvariant();
    }
}