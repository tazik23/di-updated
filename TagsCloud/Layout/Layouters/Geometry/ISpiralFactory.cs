using System.Drawing;

namespace TagsCloud.Layout.Layouters.Geometry;

public interface ISpiralFactory
{
    ISpiral CreateSpiral(Point center, CloudShape shape);
}