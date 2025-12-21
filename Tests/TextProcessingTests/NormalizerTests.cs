using FluentAssertions;
using Nestor;
using TagsCloud.TextProcessing.Normalizers;
using TagsCloud.TextProcessing.Normalizers.LemmaNormalizer;

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
    
    [TestCase("бежали", "бежать")]
    [TestCase("красивых", "красивый")]
    [TestCase("собакой", "собака")]
    [TestCase("пишу", "писать")]
    public void LemmaNormalizerShouldLemmatize(string input, string expected)
    {
        var normalizer = new LemmaNormalizer(new NestorLemmatizer(new NestorMorph()));
        
        normalizer.Normalize(input).Should().Be(expected);
    }
}