namespace Clients;

public record CommandLineArgs(
    string InputFile,
    string OutputFile,
    string? FontFamily = null,
    int? MinFontSize = null,
    int? MaxFontSize = null,
    int? CenterX = null,
    int? CenterY = null,
    double? SpiralStep = null,
    double? SpiralAngleStep = null,
    string? BackgroundColor = null,
    string? TextColor = null,
    int? ImageWidth = null,
    int? ImageHeight = null);