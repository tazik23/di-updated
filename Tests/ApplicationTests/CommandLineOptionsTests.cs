using Clients;
using CommandLine;
using FluentAssertions;
using TagsCloud.Layout.Layouters.Geometry;

namespace Tests.ApplicationTests;

public class CommandLineOptionsTests
{
    [Test]
    public void ParseArguments_WithRequiredOptionsOnly_ShouldSucceedAndSetDefaultValues()
    {
        var args = new[]
        {
            "-i", "input.txt",
            "-o", "output.png"
        };

        var result = Parser.Default.ParseArguments<CommandLineOptions>(args);

        result.Should().NotBeNull();
        result.Tag.Should().Be(ParserResultType.Parsed);

        var options = ((Parsed<CommandLineOptions>)result).Value;
        options.InputFile.Should().Be("input.txt");
        options.OutputFile.Should().Be("output.png");
        options.FontFamily.Should().Be("Arial");
        options.MinFontSize.Should().Be(10);
        options.MaxFontSize.Should().Be(50);
        options.Shape.Should().Be(CloudShape.Circle);
        options.BackgroundColor.Should().Be("White");
        options.TextColor.Should().Be("Black");
        options.ImageWidth.Should().Be(800);
        options.ImageHeight.Should().Be(600);
        options.UseLemmatization.Should().BeFalse();
        options.MinLength.Should().Be(2);
    }

    [Test]
    public void ParseArguments_WithoutRequiredOptions_ShouldFail()
    {
        var args = Array.Empty<string>();

        var result = Parser.Default.ParseArguments<CommandLineOptions>(args);

        result.Tag.Should().Be(ParserResultType.NotParsed);
    }

    [Test]
    public void ParseArguments_WithAllOptions_ShouldSetAllProperties()
    {
        var args = new[]
        {
            "-i", "input.txt",
            "-o", "output.png",
            "-f", "Times New Roman",
            "--min-font-size", "12",
            "--max-font-size", "60",
            "--center-x", "500",
            "--center-y", "400",
            "-s", "Rectangle",
            "--background-color", "Blue",
            "--text-color", "White",
            "--image-width", "1024",
            "--image-height", "768",
            "--separators", " ,.;",
            "--stop-words-file", "stopwords.txt",
            "--use-lemmatization",
            "--min-length", "3"
        };

        var result = Parser.Default.ParseArguments<CommandLineOptions>(args);
        var options = ((Parsed<CommandLineOptions>)result).Value;

        options.InputFile.Should().Be("input.txt");
        options.OutputFile.Should().Be("output.png");
        options.FontFamily.Should().Be("Times New Roman");
        options.MinFontSize.Should().Be(12);
        options.MaxFontSize.Should().Be(60);
        options.CenterX.Should().Be(500);
        options.CenterY.Should().Be(400);
        options.Shape.Should().Be(CloudShape.Rectangle);
        options.BackgroundColor.Should().Be("Blue");
        options.TextColor.Should().Be("White");
        options.ImageWidth.Should().Be(1024);
        options.ImageHeight.Should().Be(768);
        options.Separators.Should().Be(" ,.;");
        options.StopWordsFile.Should().Be("stopwords.txt");
        options.UseLemmatization.Should().BeTrue();
        options.MinLength.Should().Be(3);
    }
}