namespace TagsCloud.IO.Readers;

public class TextReader : ITextReader
{
    public string Read(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}