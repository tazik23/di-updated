using System.Drawing;

namespace TagsCloud.Layout.Layouters;

public interface ILayouter
{
    IReadOnlyList<Rectangle> Rectangles { get; }
    Rectangle PutNextRectangle(Size rectangleSize);
}