using CommandLine;

namespace Clients;

public class CommandLineOptions
{
    [Option('i', "input", Required = true, HelpText = "Input text file path")]
    public string InputFile { get; set; } = string.Empty;

    [Option('o', "output", Required = true, HelpText = "Output image file path (.png)")]
    public string OutputFile { get; set; } = string.Empty;

    [Option("font-family", HelpText = "Font family name (default: Arial)")]
    public string? FontFamily { get; set; }

    [Option("min-font-size", HelpText = "Minimum font size (default: 10)")]
    public int? MinFontSize { get; set; }

    [Option("max-font-size", HelpText = "Maximum font size (default: 100)")]
    public int? MaxFontSize { get; set; }

    [Option("center-x", HelpText = "X coordinate of cloud center (default: 400)")]
    public int? CenterX { get; set; }

    [Option("center-y", HelpText = "Y coordinate of cloud center (default: 300)")]
    public int? CenterY { get; set; }

    [Option("spiral-step", HelpText = "Spiral step size (default: 0.1)")]
    public double? SpiralStep { get; set; }

    [Option("spiral-angle-step", HelpText = "Spiral angle step (default: 0.1)")]
    public double? SpiralAngleStep { get; set; }

    [Option("background-color", HelpText = "Background color name or hex (default: White)")]
    public string? BackgroundColor { get; set; }

    [Option("text-color", HelpText = "Text color name or hex (default: Black)")]
    public string? TextColor { get; set; }

    [Option("image-width", HelpText = "Image width in pixels (default: 800)")]
    public int? ImageWidth { get; set; }

    [Option("image-height", HelpText = "Image height in pixels (default: 600)")]
    public int? ImageHeight { get; set; }

    [Option('h', "help", HelpText = "Display help")]
    public bool Help { get; set; }
}