using System.Drawing;

namespace TagsCloud.Configurations;

public class VisualizationSettings
{
    public Color BackgroundColor { get; init; } = Color.White;
    public Color TextColor { get; init; } = Color.Black;
    public Size ImageSize { get; init; } = new(800, 600);
}