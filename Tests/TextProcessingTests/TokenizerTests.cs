using FluentAssertions;
using TagsCloud.TextProcessing.Tokenizers;

namespace Tests.TextProcessingTests;

public class TokenizerTests
{
    [Test]
    public void Tokenizer_ShouldSplitBySeparators()
    {
        var separators = new[] { " ", ",", ".", "!", "?" };
        var tokenizer = new Tokenizer(separators);

        var text = "Hello, world! How are you?";
        var tokens = tokenizer.Tokenize(text).ToList();

        tokens.Should().HaveCount(5);
        tokens.Should().ContainInOrder("Hello", "world", "How", "are", "you");
    }

    [Test]
    public void Tokenizer_ShouldRemoveEmptyEntries()
    {
        var separators = new[] { ",", ";" };
        var tokenizer = new Tokenizer(separators);

        var text = "a,,b;;c";
        var tokens = tokenizer.Tokenize(text).ToList();

        tokens.Should().HaveCount(3);
        tokens.Should().Equal("a", "b", "c");
    }

    [Test]
    public void Tokenizer_ShouldHandleMultipleCharacterSeparators()
    {
        var separators = new[] { "->", "=>", " " };
        var tokenizer = new Tokenizer(separators);

        var text = "a->b=>c d";
        var tokens = tokenizer.Tokenize(text).ToList();

        tokens.Should().HaveCount(4);
        tokens.Should().Equal("a", "b", "c", "d");
    }

    [Test]
    public void Tokenizer_ShouldHandleEmptyText()
    {
        var tokenizer = new Tokenizer([" "]);

        var tokens = tokenizer.Tokenize("").ToList();

        tokens.Should().BeEmpty();
    }
}