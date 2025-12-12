using System.Drawing;

namespace TagsCloud.Configurations;

public class FontSettings
{
    public FontFamily FontFamily { get; init; } = new("Arial");
    public int MinFontSize { get; init; } = 10;
    public int MaxFontSize { get; init; } = 100;
}