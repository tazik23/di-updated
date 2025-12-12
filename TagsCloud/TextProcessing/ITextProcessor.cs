namespace TagsCloud.TextProcessing;

public interface ITextProcessor
{
    IEnumerable<string> Process(string text);
}