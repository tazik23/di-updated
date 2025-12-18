using TagsCloud.Layout.Layouters.Geometry;

namespace Clients;

public record CommandLineArgs(
    string InputFile,
    string OutputFile,
    string FontFamily,
    int MinFontSize,
    int MaxFontSize,
    int? CenterX,
    int? CenterY,
    CloudShape Shape,
    string BackgroundColor,
    string TextColor,
    int ImageWidth,
    int ImageHeight);