using Clients.Applications.AppBuilders;
using TagsCloud.Configurations;
using TagsCloud.IO.Readers;
using TagsCloud.IO.Savers;
using TagsCloud.Layout;
using TagsCloud.TextProcessing;
using TagsCloud.Visualization;
using TagsCloud.WordAnalysis;
using TagsCloud.WordMetricsCalculation;

namespace Clients.Applications;

public class ConsoleTagCloudApplication : ITagCloudApplication
{
    private readonly CloudSettings _settings;
    private readonly ITextReader _reader;
    private readonly ITextProcessor _processor;
    private readonly IWordWeightAnalyzer _analyzer;
    private readonly IWordMetricsCalculator _calculator;
    private readonly ICloudLayouter _layouter;
    private readonly ICloudVisualizer _visualizer;
    private readonly IImageSaver _saver;

    public ConsoleTagCloudApplication(
        CloudSettings settings,
        ITextReader reader,
        ITextProcessor processor,
        IWordWeightAnalyzer analyzer,
        IWordMetricsCalculator calculator,
        ICloudLayouter layouter,
        ICloudVisualizer visualizer,
        IImageSaver saver)
    {
        _settings = settings;
        _reader = reader;
        _processor = processor;
        _analyzer = analyzer;
        _calculator = calculator;
        _layouter = layouter;
        _visualizer = visualizer;
        _saver = saver;
    }

    public static ITagCloudApplicationBuilder CreateBuilder()
    {
        return new TagCloudConsoleApplicationBuilder();
    }

    public static ITagCloudApplication Create(Action<ITagCloudApplicationBuilder> configure)
    {
        var builder = CreateBuilder();
        configure(builder);

        return builder.Build();
    }

    public void Run()
    {
        try
        {
            Console.WriteLine($"\nReading text from: {_settings.InputPath}");
            var text = _reader.Read(_settings.InputPath);
            Console.WriteLine("Reading completed.");

            if (string.IsNullOrWhiteSpace(text))
            {
                ShowError("\nInput file is empty. Nothing to process.");
                return;
            }

            Console.WriteLine("\nProcessing text...");
            var words = _processor.Process(text).ToList();

            if (words.Count == 0)
            {
                ShowWarning("\nAll words were filtered out.");
                return;
            }
            Console.WriteLine($"Found {words.Count} words after filtering");

            Console.WriteLine("\nAnalyzing word frequencies...");
            var statistics = _analyzer.Analyze(words).ToList();
            Console.WriteLine($"Analyzed {statistics.Count} unique words");

            Console.WriteLine("\nCalculating word metrics...");
            var metrics = _calculator.CalculateWordsMetrics(statistics).ToList();
            Console.WriteLine("Metrics calculation completed.");

            Console.WriteLine("\nArranging words in cloud...");
            var tags = _layouter.Arrange(metrics).ToList();
            Console.WriteLine("Arrangement completed.");

            Console.WriteLine("\nGenerating image...");
            var image = _visualizer.Visualize(tags);
            Console.WriteLine("Image generation completed.");

            Console.WriteLine($"\nSaving image to: {_settings.OutputPath}");
            _saver.Save(image, _settings.OutputPath);

            Console.WriteLine("\nDone!");
            Console.WriteLine($"\nCloud saved to: {Path.GetFullPath(_settings.OutputPath)}");
        }
        catch (Exception ex)
        {
            ShowError($"Error: {ex.Message}");
            throw;
        }
    }

    private static void ShowError(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"ERROR: {message}");
        Console.ForegroundColor = originalColor;
    }

    private static void ShowWarning(string message)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"WARNING: {message}");
        Console.ForegroundColor = originalColor;
    }
}