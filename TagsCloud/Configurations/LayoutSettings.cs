using System.Drawing;
using TagsCloud.Layout.Layouters.Geometry;

namespace TagsCloud.Configurations;

public class LayoutSettings
{
    public Point Center { get; init; }
    public CloudShape Shape { get; init; } = CloudShape.Circle;
}