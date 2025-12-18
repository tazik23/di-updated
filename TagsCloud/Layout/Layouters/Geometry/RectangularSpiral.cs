using System.Drawing;

namespace TagsCloud.Layout.Layouters.Geometry;

public class RectangularSpiral : ISpiral
{
    private readonly int _step;
    private int _currentStep;
    private int _leg;
    private int _currentX;
    private int _currentY;
    private int _segmentsInCurrentLeg = 1;

    public Point Center { get; }

    public RectangularSpiral(Point center, int step = 5)
    {
        Center = center;
        _step = Math.Max(1, step);
        _currentX = center.X;
        _currentY = center.Y;
    }

    public Point GetNextPoint()
    {
        if (_currentStep == 0)
        {
            _currentStep++;
            return new Point(_currentX, _currentY);
        }

        switch (_leg)
        {
            case 0:
                _currentX += _step;
                break;
            case 1:
                _currentY += _step;
                break;
            case 2:
                _currentX -= _step;
                break;
            case 3:
                _currentY -= _step;
                break;
        }
        _currentStep++;
        if (_currentStep >= _segmentsInCurrentLeg)
        {
            if (_leg % 2 == 1)
            {
                _segmentsInCurrentLeg++;
            }
            
            _leg = (_leg + 1) % 4;
            _currentStep = 0;
        }

        return new Point(_currentX, _currentY);
    }
}