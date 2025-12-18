using System.Drawing;

namespace TagsCloud.Layout.Layouters.Geometry;

public class SpiralFactory : ISpiralFactory
{
    public ISpiral CreateSpiral(Point center, CloudShape shape)
    {
        return shape switch
        {
            CloudShape.Rectangle => new RectangularSpiral(center),
            CloudShape.Circle or _ => new ArchimedeanSpiral(center)
        };
    }
}