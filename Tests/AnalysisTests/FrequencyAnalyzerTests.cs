using FluentAssertions;
using TagsCloud.WordAnalysis;

namespace Tests.AnalysisTests;

[TestFixture]
public class FrequencyAnalyzerTests
{
    private FrequencyAnalyzer _analyzer;

    [SetUp]
    public void SetUp()
    {
        _analyzer = new FrequencyAnalyzer();
    }

    [Test]
    public void Analyze_ShouldCalculateCorrectFrequencies()
    {
        var words = new[] { "a", "b", "a", "c", "b", "a" };

        var result = _analyzer.Analyze(words).ToList();

        result.First(s => s.Word == "a").Weight.Should().Be(1.0);
        result.First(s => s.Word == "b").Weight.Should().Be(2.0 / 3);
        result.First(s => s.Word == "c").Weight.Should().Be(1.0 / 3);
    }

    [Test]
    public void FrequencyAnalyzer_ShouldReturnUniqueWordsOnly()
    {
        var analyzer = new FrequencyAnalyzer();
        var words = new[] { "word", "word", "word", "word", "hello", "hello" };

        var result = analyzer.Analyze(words).ToList();

        result.Select(s => s.Word).Should().BeEquivalentTo(["word", "hello"]);
    }

    [Test]
    public void Analyze_ShouldReturnWeight1_ForSingleUniqueWord()
    {
        var result = _analyzer.Analyze(["test"]).ToList();

        result.Should().ContainSingle();
        result[0].Weight.Should().Be(1.0);
    }
}