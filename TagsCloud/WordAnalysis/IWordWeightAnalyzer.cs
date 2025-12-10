namespace TagsCloud.WordAnalysis;

public interface IWordWeightAnalyzer
{
    IEnumerable<WordStatistic> Analyze(IEnumerable<string> words);
}