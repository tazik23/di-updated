namespace TagsCloud.TextProcessing.Filters;

public interface IWordFilter
{
    bool ShouldExclude(string word);
}