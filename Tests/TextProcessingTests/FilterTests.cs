using FluentAssertions;
using TagsCloud.TextProcessing.Filters;

namespace Tests.TextProcessingTests;

public class FilterTests
{
    [TestCase("a", true)]
    [TestCase("ab", true)]
    [TestCase("abc", false)]
    [TestCase("abcd", false)]
    [TestCase("", true)]
    public void MinLengthFilter_ShouldExcludeShortWords(string word, bool shouldExclude)
    {
        var filter = new MinLengthFilter(3);

        filter.ShouldExclude(word).Should().Be(shouldExclude);
    }

    [Test]
    public void MinLengthFilter_ShouldUseCustomMinLength()
    {
        var filter = new MinLengthFilter(5);

        filter.ShouldExclude("test").Should().BeTrue();
        filter.ShouldExclude("tests").Should().BeFalse();
    }

    [TestCase("the", true)]
    [TestCase("and", true)]
    [TestCase("hello", false)]
    public void StopWordsFilter_ShouldExcludeStopWords(string word, bool shouldExclude)
    {
        var stopWords = new HashSet<string> { "the", "and", "a", "an", "in", "on" };
        var filter = new StopWordsFilter(stopWords);

        filter.ShouldExclude(word).Should().Be(shouldExclude);
    }

    [Test]
    public void StopWordsFilter_ShouldHandleEmptyStopWords()
    {
        var filter = new StopWordsFilter([]);

        filter.ShouldExclude("any").Should().BeFalse();
    }
}