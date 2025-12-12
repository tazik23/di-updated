using System.Drawing;

namespace TagsCloud.WordMetricsCalculation;

public class GraphicsTextMeasurer : ITextMeasurer
{
    public Size MeasureText(string text, Font font)
    {
        using var bitmap = new Bitmap(1, 1);
        using var graphics = Graphics.FromImage(bitmap);

        return graphics.MeasureString(text, font).ToSize();
    }
}