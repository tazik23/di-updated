using System.Drawing;
using TagsCloud.Configurations;
using TagsCloud.Layout.Layouters.Geometry;

namespace Clients.Applications.AppBuilders;

public class CloudSettingsBuilder
{
    private string _inputPath = null!;
    private string _outputPath = null!;
    private FontSettings _fontSettings = null!;
    private LayoutSettings _layoutSettings = null!;
    private VisualizationSettings _visualizationSettings = null!;
    private TextProcessingSettings _textProcessingSettings = null!;

    public CloudSettingsBuilder WithInputPath(string path)
    {
        _inputPath = path;
        return this;
    }

    public CloudSettingsBuilder WithOutputPath(string path)
    {
        _outputPath = path;
        return this;
    }

    public CloudSettingsBuilder WithFontSettings(FontFamily fontFamily, int minSize, int maxSize)
    {
        _fontSettings = new FontSettings(fontFamily, minSize, maxSize);
        return this;
    }

    public CloudSettingsBuilder WithLayoutSettings(Point center, CloudShape shape)
    {
        _layoutSettings = new LayoutSettings(center, shape);
        return this;
    }

    public CloudSettingsBuilder WithVisualizationSettings(
        Color backgroundColor,
        Color textColor,
        Size imageSize)
    {
        _visualizationSettings = new VisualizationSettings(backgroundColor, textColor, imageSize);
        return this;
    }

    public CloudSettingsBuilder WithTextProcessingSettings(
        string[] separators, bool useLemmatization, string? stopWordsFilePath, int minLength)
    {
        _textProcessingSettings = new TextProcessingSettings(separators, useLemmatization, stopWordsFilePath, minLength);
        return this;
    }

    public CloudSettings Build()
    {
        return new CloudSettings(
            _inputPath,
            _outputPath,
            _fontSettings,
            _layoutSettings,
            _visualizationSettings,
            _textProcessingSettings
        );
    }
}