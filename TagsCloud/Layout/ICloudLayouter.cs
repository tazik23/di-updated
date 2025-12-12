using TagsCloud.WordMetricsCalculation;

namespace TagsCloud.Layout;

public interface ICloudLayouter
{
    IEnumerable<Tag> Arrange(IEnumerable<WordMetrics> wordMetrics);
}