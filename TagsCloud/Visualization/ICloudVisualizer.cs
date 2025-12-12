using System.Drawing;
using TagsCloud.Layout;

namespace TagsCloud.Visualization;

public interface ICloudVisualizer
{
    Image Visualize(IEnumerable<Tag> tags);
}