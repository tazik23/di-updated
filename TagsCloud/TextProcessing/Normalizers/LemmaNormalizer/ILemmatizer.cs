namespace TagsCloud.TextProcessing.Normalizers.LemmaNormalizer;

public interface ILemmatizer
{
    string GetLemma(string word);
}