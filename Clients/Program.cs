using System.Drawing;
using Clients.Applications;
using CommandLine;

namespace Clients;

public class Program
{
    public static void Main(string[] args)
    { 
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(RunApplication);
    }

    private static void RunApplication(CommandLineOptions options)
    {
        try
        {
            var application = ConsoleTagCloudApplication.Create(builder =>
            {
                builder.ConfigureSettings(settingsBuilder =>
                {
                    settingsBuilder
                        .WithInputPath(options.InputFile)
                        .WithOutputPath(options.OutputFile)
                        .WithFontSettings(
                            new FontFamily(options.FontFamily),
                            options.MinFontSize,
                            options.MaxFontSize)
                        .WithLayoutSettings(
                            new Point(
                                options.CenterX ?? options.ImageWidth / 2, 
                                options.CenterY ?? options.ImageHeight / 2),
                            options.Shape)
                        .WithVisualizationSettings(
                            ParseColor(options.BackgroundColor),
                            ParseColor(options.TextColor),
                            new Size(options.ImageWidth, options.ImageHeight))
                        .WithTextProcessingSettings(
                            options.Separators?.Split(' '),
                            options.UseStemming,
                            options.StopWordsFile,
                            options.MinLength);
                });
            });
            
            application.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }

    private static Color ParseColor(string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentNullException(nameof(color));

        try
        {
            return Color.FromName(color);
        }
        catch
        {
            try
            {
                return ColorTranslator.FromHtml(color);
            }
            catch
            {
                throw new FormatException("Invalid color format.");
            }
        }
    }
}