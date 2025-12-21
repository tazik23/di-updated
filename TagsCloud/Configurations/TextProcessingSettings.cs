namespace TagsCloud.Configurations;

public record TextProcessingSettings(
    string[] Separators,
    bool UseStemming,
    string? StopWordsFilePath,
    int MinLength);