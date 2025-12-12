using System.Drawing;
using TagsCloud.Layout.Layouters.Geometry;
using TagsCloud.Layout.Layouters.Geometry.Extensions;

namespace TagsCloud.Layout.Layouters;

public class SpiralLayouter : ILayouter
{
    private readonly ISpiral _spiral;
    private readonly List<Rectangle> _rectangles = new();
    private readonly QuadTree _quadTree;

    public IReadOnlyList<Rectangle> Rectangles => _rectangles.AsReadOnly();

    public SpiralLayouter(ISpiral spiral)
    {
        _spiral = spiral;
        _quadTree = new QuadTree(new Rectangle(
            spiral.Center.X,
            spiral.Center.Y,
            1000,
            1000));
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = GetValidPosition(rectangleSize);
        rectangle = TryMoveToCenter(rectangle);

        _rectangles.Add(rectangle);
        _quadTree.Insert(rectangle);

        return rectangle;
    }

    private Rectangle GetValidPosition(Size rectangleSize)
    {
        Rectangle rectangle;
        do
        {
            var candidatePoint = _spiral.GetNextPoint();
            rectangle = CreateRectangleWithCenterAt(candidatePoint, rectangleSize);
        } while (HasIntersections(rectangle));

        return rectangle;
    }

    private Rectangle TryMoveToCenter(Rectangle rectangle, int maxIterationsToTry = 10000)
    {
        if (_rectangles.Count == 0)
            return rectangle;

        var current = rectangle;
        var iterations = 0;

        var directionX = GetDirectionToCenter(rectangle.GetCenter(), Axis.X);
        var directionY = GetDirectionToCenter(rectangle.GetCenter(), Axis.Y);

        while (iterations < maxIterationsToTry)
        {
            var movedX = TryMoveAlongAxis(current, directionX, Axis.X, out var xCandidate);
            if (movedX) current = xCandidate;

            var movedY = TryMoveAlongAxis(current, directionY, Axis.Y, out var yCandidate);
            if (movedY) current = yCandidate;

            if (!(movedX || movedY))
                break;

            iterations++;
        }

        return current;
    }

    private bool TryMoveAlongAxis(Rectangle rectangle, Point direction, Axis axis, out Rectangle candidate)
    {
        candidate = rectangle;

        if (direction.IsZero())
            return false;

        var stepSize = 1;
        var moved = rectangle.MoveInDirection(direction, stepSize);

        if (GetDirectionToCenter(rectangle.GetCenter(), axis) != direction)
            return false;

        if (HasIntersections(moved))
            return false;

        candidate = moved;
        return true;
    }

    private Point GetDirectionToCenter(Point point, Axis axis)
    {
        return axis switch
        {
            Axis.X => new Point(-Math.Sign(point.X - _spiral.Center.X), 0),
            Axis.Y => new Point(0, -Math.Sign(point.Y - _spiral.Center.Y)),
            _ => new Point(0, 0)
        };
    }

    private bool HasIntersections(Rectangle rectangle)
    {
        return _quadTree.HasIntersection(rectangle);
    }

    private static Rectangle CreateRectangleWithCenterAt(Point center, Size size)
    {
        var location = new Point(center.X - size.Width / 2, center.Y - size.Height / 2);
        return new Rectangle(location, size);
    }
}