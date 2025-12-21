namespace TagsCloud.Configurations;

public record CloudSettings(
    string InputPath,
    string OutputPath,
    FontSettings FontSettings,
    LayoutSettings LayoutSettings,
    VisualizationSettings VisualizationSettings,
    TextProcessingSettings TextProcessingSettings
);