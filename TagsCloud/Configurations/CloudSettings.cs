namespace TagsCloud.Configurations;

public record CloudSettings(
    string InputPath,
    string OutputPath,
    FontSettings FontSettings,
    LayoutSettings LayoutOptions,
    VisualizationSettings VisualizationSettings
);
