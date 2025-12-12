using Autofac;
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
            var commandLineArgs = new CommandLineArgs(
                options.InputFile,
                options.OutputFile,
                options.FontFamily,
                options.MinFontSize,
                options.MaxFontSize,
                options.CenterX,
                options.CenterY,
                options.SpiralStep,
                options.SpiralAngleStep,
                options.BackgroundColor,
                options.TextColor,
                options.ImageWidth,
                options.ImageHeight
            );

            var container = DiContainer.BuildContainer(commandLineArgs);
            using var scope = container.BeginLifetimeScope();
            var application = scope.Resolve<TagCloudApplication>();

            application.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}