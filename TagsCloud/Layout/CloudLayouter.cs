using TagsCloud.Layout.Layouters;
using TagsCloud.WordMetricsCalculation;

namespace TagsCloud.Layout;

public class CloudLayouter : ICloudLayouter
{
    private readonly ILayouter _layouter;

    public CloudLayouter(ILayouter layouter)
    {
        _layouter = layouter;
    }

    public IEnumerable<Tag> Arrange(IEnumerable<WordMetrics> wordMetrics)
    {
        foreach (var wordStatistic in wordMetrics)
        {
            var bounds = _layouter.PutNextRectangle(wordStatistic.Size);

            yield return new Tag(wordStatistic.Text, bounds.Location, wordStatistic.Font);
        }
    }
}