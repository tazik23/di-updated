using System.Drawing;
using TagsCloud.Configurations;
using TagsCloud.WordAnalysis;

namespace TagsCloud.WordMetricsCalculation;

public class WordMetricsCalculator : IWordMetricsCalculator
{
    private readonly ITextMeasurer _textMeasurer;
    private readonly FontSettings _fontSettings;

    public WordMetricsCalculator(ITextMeasurer textMeasurer, FontSettings fontSettings)
    {
        _textMeasurer = textMeasurer;
        _fontSettings = fontSettings;
    }

    public IEnumerable<WordMetrics> CalculateWordsMetrics(IEnumerable<WordStatistic> wordStatistics)
    {
        if (!wordStatistics.Any())
            yield break;

        var statistics = wordStatistics.OrderByDescending(statistic => statistic.Weight).ToList();

        var minWeight = statistics.Min(w => w.Weight);
        var maxWeight = statistics.Max(w => w.Weight);

        foreach (var statistic in statistics)
        {
            var fontSize = CalculateFontSize(
                statistic.Weight,
                minWeight,
                maxWeight,
                _fontSettings.MinFontSize,
                _fontSettings.MaxFontSize);

            var font = new Font(
                _fontSettings.FontFamily,
                (float)fontSize);

            var size = _textMeasurer.MeasureText(statistic.Word, font);

            yield return new WordMetrics(statistic.Word, font, size);
        }
    }

    private static double CalculateFontSize(
        double weight,
        double minWeight,
        double maxWeight,
        int minFontSize,
        int maxFontSize)
    {
        if (weight <= minWeight)
            return minFontSize;

        return maxFontSize * (weight - minWeight) / (maxWeight - minWeight);
    }
}