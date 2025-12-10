using System.Drawing;

namespace TagsCloud.Layout.Layouters.Geometry;

public interface ISpiral
{
    Point Center { get; }
    Point GetNextPoint();
}