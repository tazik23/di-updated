namespace TagsCloud.Configurations;

public class CloudSettings
{
    public required string InputPath { get; init; }
    public required string OutputPath { get; init; }
    public FontSettings FontSettings { get; init; } = new();
    public LayoutSettings LayoutOptions { get; init; } = new();
    public VisualizationSettings VisualizationSettings { get; init; } = new();
}