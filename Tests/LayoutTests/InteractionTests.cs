using System.Drawing;
using FakeItEasy;
using FluentAssertions;
using TagsCloud.Layout;
using TagsCloud.Layout.Layouters;
using TagsCloud.WordMetricsCalculation;

namespace Tests.LayoutTests;

public class InteractionTests
{
    private ILayouter _layouter;
    private CloudLayouter _cloudLayouter;

    [SetUp]
    public void SetUp()
    {
        _layouter = A.Fake<ILayouter>();
        A.CallTo(() => _layouter.PutNextRectangle(A<Size>._))
            .ReturnsLazily((Size size) => new Rectangle(Point.Empty, size));

        _cloudLayouter = new CloudLayouter(_layouter);
    }

    [Test]
    public void Arrange_ShouldProcessAllWordMetrics()
    {
        var wordMetrics = MetricsGenerator.Generate(10);

        var result = _cloudLayouter.Arrange(wordMetrics).ToList();

        A.CallTo(() => _layouter.PutNextRectangle(A<Size>._))
            .MustHaveHappened(wordMetrics.Count, Times.Exactly);

        result.Should().HaveCount(wordMetrics.Count);

        result.Select(t => t.Text).Should().BeEquivalentTo(wordMetrics.Select(m => m.Text));
    }
}