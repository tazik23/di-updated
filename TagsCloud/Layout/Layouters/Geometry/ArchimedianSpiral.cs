using System.Drawing;

namespace TagsCloud.Layout.Layouters.Geometry;

public class ArchimedeanSpiral : ISpiral
{
    private double _currentAngle;
    private readonly double _angleStep;
    private readonly int _spiralStep;

    public Point Center { get; }

    public ArchimedeanSpiral(Point center, double angleStep = 0.1, int spiralStep = 1)
    {
        Center = center;
        _angleStep = angleStep;
        _spiralStep = spiralStep;
    }

    public Point GetNextPoint()
    {
        var radius = _spiralStep / (2 * Math.PI) * _currentAngle;
        var x = (int)(Center.X + radius * Math.Cos(_currentAngle));
        var y = (int)(Center.Y + radius * Math.Sin(_currentAngle));

        _currentAngle += _angleStep;

        return new Point(x, y);
    }
}