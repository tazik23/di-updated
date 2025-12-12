using System.Drawing;

namespace TagsCloud.IO.Savers;

public interface IImageSaver
{
    void Save(Image image, string filePath);
}