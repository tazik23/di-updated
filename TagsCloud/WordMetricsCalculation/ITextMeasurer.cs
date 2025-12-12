using System.Drawing;

namespace TagsCloud.WordMetricsCalculation;

public interface ITextMeasurer
{
    Size MeasureText(string text, Font font);
}