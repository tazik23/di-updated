using CommandLine;
using TagsCloud.Layout.Layouters.Geometry;

namespace Clients;

public class CommandLineOptions
{
    [Option('i', "input", Required = true, HelpText = "Input text file path")]
    public string InputFile { get; set; } = string.Empty;

    [Option('o', "output", Required = true, HelpText = "Output image file path (.png)")]
    public string OutputFile { get; set; } = string.Empty;

    [Option('f', "font", Default = "Arial", HelpText = "Font family name")]
    public string FontFamily { get; set; } = null!;

    [Option("min-font-size", Default = 10, HelpText = "Minimum font size")]
    public int MinFontSize { get; set; }

    [Option("max-font-size", Default = 50, HelpText = "Maximum font size")]
    public int MaxFontSize { get; set; }

    [Option("center-x", HelpText = "X coordinate of cloud center")]
    public int? CenterX { get; set; }

    [Option("center-y", HelpText = "Y coordinate of cloud center")]
    public int? CenterY { get; set; }

    [Option('s', "shape",
        Default = CloudShape.Circle,
        HelpText = "Cloud shape. Available values: circle, rectangle")]
    public CloudShape Shape { get; set; }

    [Option("background-color", Default = "White", HelpText = "Background color name")]
    public string BackgroundColor { get; set; } = null!;

    [Option("text-color", Default = "Black", HelpText = "Text color name")]
    public string TextColor { get; set; } = null!;

    [Option("image-width", Default = 800, HelpText = "Image width in pixels")]
    public int ImageWidth { get; set; }

    [Option("image-height", Default = 600, HelpText = "Image height in pixels")]
    public int ImageHeight { get; set; }

    [Option("separators", Default = "  , . \t \r\n \r \n",
        HelpText = "Word separators. Default: \" , . \\r\\n \\t \\r \\n \"")]
    public string Separators { get; set; }

    [Option("stop-words-file",
        HelpText = "Path to file with stop words (one word per line)")]
    public string? StopWordsFile { get; set; }

    [Option("use-lemmatization", Default = false,
        HelpText = "Use lemmatization to reduce words to their base form")]
    public bool UseStemming { get; set; }

    [Option("min-length", Default = 2, HelpText = "")]
    public int MinLength { get; set; }
}