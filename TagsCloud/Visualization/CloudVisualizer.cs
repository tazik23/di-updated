using System.Drawing;
using TagsCloud.Configurations;
using TagsCloud.Layout;

namespace TagsCloud.Visualization;

public class CloudVisualizer : ICloudVisualizer
{
    private readonly VisualizationSettings _settings;

    public CloudVisualizer(VisualizationSettings settings)
    {
        _settings = settings;
    }

    public Image Visualize(IEnumerable<Tag> tags)
    {
        var bitmap = new Bitmap(_settings.ImageSize.Width, _settings.ImageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(_settings.BackgroundColor);

        using var brush = new SolidBrush(_settings.TextColor);

        var tagList = tags.ToList();
        foreach (var tag in tagList) graphics.DrawString(tag.Text, tag.Font, brush, tag.Location);

        return bitmap;
    }
}