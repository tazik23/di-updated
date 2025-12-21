using System.Drawing;
using Clients.Applications;
using FakeItEasy;
using FluentAssertions;
using TagsCloud.Configurations;
using TagsCloud.IO.Readers;
using TagsCloud.IO.Savers;
using TagsCloud.Layout;
using TagsCloud.Layout.Layouters.Geometry;
using TagsCloud.TextProcessing;
using TagsCloud.Visualization;
using TagsCloud.WordAnalysis;
using TagsCloud.WordMetricsCalculation;

namespace Tests.ApplicationTests;

public class ConsoleApplicationTests
{
    private ITextReader _reader;
    private ITextProcessor _processor;
    private IWordWeightAnalyzer _analyzer;
    private IWordMetricsCalculator _calculator;
    private ICloudLayouter _layouter;
    private ICloudVisualizer _visualizer;
    private IImageSaver _saver;
    private CloudSettings _settings;

    [SetUp]
    public void SetUp()
    {
        _reader = A.Fake<ITextReader>();
        _processor = A.Fake<ITextProcessor>();
        _analyzer = A.Fake<IWordWeightAnalyzer>();
        _calculator = A.Fake<IWordMetricsCalculator>();
        _layouter = A.Fake<ICloudLayouter>();
        _visualizer = A.Fake<ICloudVisualizer>();
        _saver = A.Fake<IImageSaver>();

        _settings = new CloudSettings(
            "input.txt",
            "output.png",
            new FontSettings(new FontFamily("Arial"), 10, 50),
            new LayoutSettings(new Point(400, 300), CloudShape.Circle),
            new VisualizationSettings(Color.White, Color.Black, new Size(800, 600)),
            new TextProcessingSettings(new[] { " " }, false, null, 2)
        );
    }

    [Test]
    public void Run_WithValidInput_ShouldCompleteSuccessfully()
    {
        var fakeWords = new[] { "word1", "word2" };
        var fakeStatistics = new[] { new WordStatistic("word1", 1) };
        var fakeMetrics = new[] { new WordMetrics("word1", new Font("Arial", 10), new Size(10, 10)) };
        var fakeLayout = new[] { new Tag("word1", Point.Empty, new Font("Arial", 10)) };
        var fakeImage = new Bitmap(10, 10);

        A.CallTo(() => _reader.Read(A<string>._)).Returns("text");
        A.CallTo(() => _processor.Process(A<string>._)).Returns(fakeWords);
        A.CallTo(() => _analyzer.Analyze(fakeWords)).Returns(fakeStatistics);
        A.CallTo(() => _calculator.CalculateWordsMetrics(fakeStatistics)).Returns(fakeMetrics);
        A.CallTo(() => _layouter.Arrange(fakeMetrics)).Returns(fakeLayout);
        A.CallTo(() => _visualizer.Visualize(fakeLayout)).Returns(fakeImage);

        var app = new ConsoleTagCloudApplication(
            _settings, _reader, _processor, _analyzer, _calculator,
            _layouter, _visualizer, _saver);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        app.Run();

        consoleOutput.ToString().Should().Contain("Done!");
    }

    [Test]
    public void Run_WithEmptyText_ShouldShowWarning()
    {
        A.CallTo(() => _reader.Read(A<string>._)).Returns("");

        var app = new ConsoleTagCloudApplication(
            _settings, _reader, _processor, _analyzer, _calculator,
            _layouter, _visualizer, _saver);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        app.Run();

        consoleOutput.ToString().Should().Contain("ERROR");
        A.CallTo(() => _processor.Process(A<string>._)).MustNotHaveHappened();
    }

    [Test]
    public void Run_WhenProcessorReturnsNoWords_ShouldShowWarning()
    {
        A.CallTo(() => _reader.Read(A<string>._)).Returns("some text");
        A.CallTo(() => _processor.Process(A<string>._)).Returns([]);

        var app = new ConsoleTagCloudApplication(
            _settings, _reader, _processor, _analyzer, _calculator,
            _layouter, _visualizer, _saver);

        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        app.Run();

        consoleOutput.ToString().Should().Contain("WARNING");
        A.CallTo(() => _analyzer.Analyze(A<IEnumerable<string>>._)).MustNotHaveHappened();
    }
}