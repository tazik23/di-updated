namespace TagsCloud.Configurations;

public record TextProcessingSettings(
    string[] Separators,
    bool UseLemmatization,
    string? StopWordsFilePath,
    int MinLength);