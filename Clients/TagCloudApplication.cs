using TagsCloud.Configurations;
using TagsCloud.IO.Readers;
using TagsCloud.IO.Savers;
using TagsCloud.Layout;
using TagsCloud.TextProcessing;
using TagsCloud.Visualization;
using TagsCloud.WordAnalysis;
using TagsCloud.WordMetricsCalculation;

namespace Clients;

public class TagCloudApplication
{
    private readonly CloudSettings _settings;
    private readonly ITextReader _reader;
    private readonly ITextProcessor _processor;
    private readonly IWordWeightAnalyzer _analyzer;
    private readonly IWordMetricsCalculator _calculator;
    private readonly ICloudLayouter _layouter;
    private readonly ICloudVisualizer _visualizer;
    private readonly IImageSaver _saver;

    public TagCloudApplication(
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

    public void Run()
    {
        try
        {
            Console.WriteLine($"Reading text from: {_settings.InputPath}");
            var text = _reader.Read(_settings.InputPath);

            Console.WriteLine("Processing text...");
            var words = _processor.Process(text).ToList();
            Console.WriteLine($"Found {words.Count} unique words after filtering");

            Console.WriteLine("Analyzing word frequencies...");
            var statistics = _analyzer.Analyze(words).ToList();
            Console.WriteLine($"Analyzed {statistics.Count} words");

            Console.WriteLine("Calculating word metrics...");
            var metrics = _calculator.CalculateWordsMetrics(statistics).ToList();

            Console.WriteLine("Arranging words in cloud...");
            var tags = _layouter.Arrange(metrics).ToList();

            Console.WriteLine("Generating image...");
            var image = _visualizer.Visualize(tags);

            Console.WriteLine($"Saving image to: {_settings.OutputPath}");
            _saver.Save(image, _settings.OutputPath);

            Console.WriteLine("Done!");
            Console.WriteLine($"Cloud saved to: {Path.GetFullPath(_settings.OutputPath)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
}