using System.Drawing;

namespace TagsCloud.Configurations;

public class LayoutSettings
{
    public Point Center { get; init; } = new(400, 300);
    public double SpiralStep { get; init; } = 0.1;
    public double SpiralAngleStep { get; init; } = 0.1;
}