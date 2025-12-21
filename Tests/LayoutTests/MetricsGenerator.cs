using System.Drawing;
using TagsCloud.WordMetricsCalculation;

namespace Tests.LayoutTests;

public static class MetricsGenerator
{
    private static Font[] fonts =
    [
        new(FontFamily.GenericSansSerif, 12),
        new(FontFamily.GenericSerif, 14),
        new(FontFamily.GenericMonospace, 10)
    ];

    public static List<WordMetrics> Generate(int count)
    {
        var textMeasurer = new GraphicsTextMeasurer();
        var wordMetrics = new List<WordMetrics>();
        for (var i = 0; i < count; i++)
        {
            var text = $"test{i}";
            var font = fonts[i % fonts.Length];
            var size = textMeasurer.MeasureText(text, font);
            wordMetrics.Add(new WordMetrics(text, font, size));
        }

        return wordMetrics;
    }
}