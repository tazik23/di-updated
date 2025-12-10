namespace TagsCloud.TextProcessing.Normalizers;

public interface IWordNormalizer
{
    string Normalize(string word);
}