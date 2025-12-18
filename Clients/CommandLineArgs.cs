using TagsCloud.Layout.Layouters.Geometry;

namespace Clients;

public record CommandLineArgs(
    string InputFile,
    string OutputFile,
    string? FontFamily = null,
    int? MinFontSize = null,
    int? MaxFontSize = null,
    int? CenterX = null,
    int? CenterY = null,
    CloudShape Shape = CloudShape.Circle,
    string? BackgroundColor = null,
    string? TextColor = null,
    int? ImageWidth = null,
    int? ImageHeight = null);