using FakeItEasy;
using FluentAssertions;
using TagsCloud.TextProcessing;
using TagsCloud.TextProcessing.Filters;
using TagsCloud.TextProcessing.Normalizers;
using TagsCloud.TextProcessing.Tokenizers;

namespace Tests.TextProcessingTests;

[TestFixture]
public class TextProcessorTests
{
    private Tokenizer _tokenizer;

    [SetUp]
    public void SetUp()
    {
        _tokenizer = new Tokenizer([" "]);
    }

    [Test]
    public void Process_ShouldApplyNormalizersBeforeFilters()
    {
        var normalizer = A.Fake<IWordNormalizer>();
        A.CallTo(() => normalizer.Normalize("hello")).Returns("hello");
        A.CallTo(() => normalizer.Normalize("secret")).Returns("STOP_WORD");
        A.CallTo(() => normalizer.Normalize("world")).Returns("world");

        var filter = A.Fake<IWordFilter>();
        A.CallTo(() => filter.ShouldExclude("hello")).Returns(false);
        A.CallTo(() => filter.ShouldExclude("STOP_WORD")).Returns(true);
        A.CallTo(() => filter.ShouldExclude("world")).Returns(false);

        var processor = new TextProcessor(
            _tokenizer,
            [normalizer],
            [filter]
        );

        var result = processor.Process("hello secret world").ToList();

        A.CallTo(() => filter.ShouldExclude("STOP_WORD")).MustHaveHappened();
        result.Should().Equal("hello", "world");
    }

    [Test]
    public void Process_ShouldApplyAllNormalizersInOrder()
    {
        var normalizer1 = A.Fake<IWordNormalizer>();
        var normalizer2 = A.Fake<IWordNormalizer>();

        A.CallTo(() => normalizer1.Normalize(A<string>._))
            .ReturnsLazily((string word) => $"{word}_1");
        A.CallTo(() => normalizer2.Normalize(A<string>._))
            .ReturnsLazily((string word) => $"{word}_2");

        var processor = new TextProcessor(
            _tokenizer,
            [normalizer1, normalizer2],
            []
        );

        var result = processor.Process("word").Single();

        result.Should().Be("word_1_2");
    }

    [Test]
    public void Process_ShouldExcludeIfAnyFilterReturnsTrue()
    {
        var filter1 = A.Fake<IWordFilter>();
        var filter2 = A.Fake<IWordFilter>();
        var filter3 = A.Fake<IWordFilter>();

        A.CallTo(() => filter1.ShouldExclude("word1")).Returns(false);
        A.CallTo(() => filter2.ShouldExclude("word1")).Returns(true);
        A.CallTo(() => filter3.ShouldExclude("word1")).Returns(false);

        A.CallTo(() => filter1.ShouldExclude("word2")).Returns(false);
        A.CallTo(() => filter2.ShouldExclude("word2")).Returns(false);
        A.CallTo(() => filter3.ShouldExclude("word2")).Returns(false);

        var processor = new TextProcessor(
            _tokenizer,
            [],
            [filter1, filter2, filter3]
        );

        var result = processor.Process("word1 word2").ToList();

        result.Should().HaveCount(1);
        result[0].Should().Be("word2");
    }

    [Test]
    public void Process_ShouldProcessEachWordIndependently()
    {
        var normalizer = A.Fake<IWordNormalizer>();
        A.CallTo(() => normalizer.Normalize("good")).Returns("GOOD");
        A.CallTo(() => normalizer.Normalize("bad")).Returns("BAD");

        var filter = A.Fake<IWordFilter>();
        A.CallTo(() => filter.ShouldExclude("GOOD")).Returns(false);
        A.CallTo(() => filter.ShouldExclude("BAD")).Returns(true);

        var processor = new TextProcessor(
            _tokenizer,
            [normalizer],
            [filter]
        );

        var result = processor.Process("good bad good").ToList();

        A.CallTo(() => normalizer.Normalize("good")).MustHaveHappened(2, Times.Exactly);
        A.CallTo(() => normalizer.Normalize("bad")).MustHaveHappened(1, Times.Exactly);

        A.CallTo(() => filter.ShouldExclude("GOOD")).MustHaveHappened(2, Times.Exactly);
        A.CallTo(() => filter.ShouldExclude("BAD")).MustHaveHappened(1, Times.Exactly);

        result.Should().Equal("GOOD", "GOOD");
    }
}