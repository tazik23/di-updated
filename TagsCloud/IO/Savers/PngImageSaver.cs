using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloud.IO.Savers;

public class PngImageSaver : IImageSaver
{
    public void Save(Image image, string filePath)
    {
        image.Save(filePath, ImageFormat.Png);
    }
}