using System.Drawing;
using FluentAssertions;
using TagsCloud.Layout;
using TagsCloud.Layout.Layouters;
using TagsCloud.Layout.Layouters.Geometry;
using TagsCloud.Layout.Layouters.Geometry.Extensions;
using TagsCloud.WordMetricsCalculation;

namespace Tests.LayoutTests;

public class ArrangementTests
{
    [TestCaseSource(nameof(CenterTestCases))]
    public void Arrange_ShouldPlaceFirstWordInCenter(Point center, CloudShape shape)
    {
        var cloudLayouter = CreateLayouter(center, shape);

        var wordMetrics = MetricsGenerator.Generate(10);

        var expectedLocation = center - wordMetrics[0].Size / 2;
        var tag = cloudLayouter.Arrange(wordMetrics).First();

        tag.Location.Should().Be(expectedLocation);
    }

    [TestCaseSource(nameof(IntersectionTestCases))]
    public void Arrange_ShouldPlaceFirstWordsWithoutIntersections(CloudShape shape)
    {
        var cloudLayouter = CreateLayouter(Point.Empty, shape);

        var wordMetrics = MetricsGenerator.Generate(10);

        var rectangles = cloudLayouter
            .Arrange(wordMetrics)
            .Select(t => new Rectangle(t.Location, GetBounds(t))).ToList();

        foreach (var r1 in rectangles)
        foreach (var r2 in rectangles.Where(r2 => r2 != r1))
            r1.IntersectsWith(r2).Should().BeFalse();
    }

    [TestCaseSource(nameof(DensityTestCases))]
    public void Arrange_ShouldTightlyDistributeWords(CloudShape shape,
        Func<List<Rectangle>, double> getCircumscribedArea)
    {
        var cloudLayouter = CreateLayouter(Point.Empty, shape);

        var wordMetrics = MetricsGenerator.Generate(100);

        var rectangles = cloudLayouter
            .Arrange(wordMetrics)
            .Select(t => new Rectangle(t.Location, GetBounds(t))).ToList();

        var rectanglesArea = rectangles.Select(r => r.GetArea()).Sum();
        var circumscribedFigureArea = getCircumscribedArea(rectangles);

        var actualDensityCoefficient = rectanglesArea / circumscribedFigureArea;

        actualDensityCoefficient.Should().BeGreaterThanOrEqualTo(0.7);
    }


    private static IEnumerable<TestCaseData> CenterTestCases
    {
        get
        {
            yield return new TestCaseData(Point.Empty, CloudShape.Rectangle);
            yield return new TestCaseData(Point.Empty, CloudShape.Circle);

            yield return new TestCaseData(new Point(12, 47), CloudShape.Rectangle);
            yield return new TestCaseData(new Point(12, 47), CloudShape.Circle);
        }
    }

    private static IEnumerable<TestCaseData> IntersectionTestCases
    {
        get
        {
            yield return new TestCaseData(CloudShape.Rectangle);
            yield return new TestCaseData(CloudShape.Circle);
        }
    }

    private static IEnumerable<TestCaseData> DensityTestCases
    {
        get
        {
            yield return new TestCaseData(CloudShape.Rectangle, GetCircumscribedRectangleArea);
            yield return new TestCaseData(CloudShape.Circle, GetCircumscribedCircleArea);
        }
    }

    private static double GetCircumscribedCircleArea(List<Rectangle> rectangles)
    {
        var radius = 0d;
        var center = Point.Empty;

        foreach (var vertex in rectangles.SelectMany(rectangle => rectangle.GetVertices()))
        {
            var distance = center.DistanceTo(vertex);
            radius = Math.Max(radius, distance);
        }

        return radius * radius * Math.PI;
    }

    private static double GetCircumscribedRectangleArea(List<Rectangle> rectangles)
    {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        foreach (var vertex in rectangles.SelectMany(rectangle => rectangle.GetVertices()))
        {
            minX = Math.Min(minX, vertex.X);
            minY = Math.Min(minY, vertex.Y);
            maxX = Math.Max(maxX, vertex.X);
            maxY = Math.Max(maxY, vertex.Y);
        }

        var width = maxX - minX;
        var height = maxY - minY;

        return width * height;
    }

    private static Size GetBounds(Tag tag)
    {
        return new GraphicsTextMeasurer().MeasureText(tag.Text, tag.Font);
    }

    private static CloudLayouter CreateLayouter(Point center, CloudShape shape)
    {
        var spiral = new SpiralFactory().CreateSpiral(center, shape);
        var layouter = new SpiralLayouter(spiral);

        return new CloudLayouter(layouter);
    }
}