using FluentAssertions;
using TagsCloud.TextProcessing.Normalizers;

namespace Tests.TextProcessingTests;

public class NormalizerTests
{
    [TestCase("HELLO", "hello")]
    [TestCase("Hello", "hello")]
    [TestCase("hElLo", "hello")]
    [TestCase("hello", "hello")]
    [TestCase("123", "123")]
    public void LowerCaseNormalizer_ShouldConvertToLowercase(string input, string expected)
    {
        var normalizer = new LowerCaseNormalizer();

        normalizer.Normalize(input).Should().Be(expected);
    }
}