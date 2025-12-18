using Clients.Applications;
using Clients.Applications.AppBuilders;
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
        var commandLineArgs = new CommandLineArgs(
            options.InputFile,
            options.OutputFile,
            options.FontFamily,
            options.MinFontSize,
            options.MaxFontSize,
            options.CenterX,
            options.CenterY,
            options.Shape,
            options.BackgroundColor,
            options.TextColor,
            options.ImageWidth,
            options.ImageHeight
        );
        try
        {
            var builder = ConsoleTagCloudApplication.CreateBuilder();
        
            builder.Container.ConfigureSettings(commandLineArgs);
            builder.Container.AddServices();
            
            var application = builder.Build();
            
            application.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}