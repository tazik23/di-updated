namespace TagsCloud.TextProcessing.Tokenizers;

public interface ITokenizer
{
    IEnumerable<string> Tokenize(string text);
}