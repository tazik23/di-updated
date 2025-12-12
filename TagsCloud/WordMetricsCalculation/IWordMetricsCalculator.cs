using TagsCloud.WordAnalysis;

namespace TagsCloud.WordMetricsCalculation;

public interface IWordMetricsCalculator
{
    IEnumerable<WordMetrics> CalculateWordsMetrics(IEnumerable<WordStatistic> wordStatistics);
}