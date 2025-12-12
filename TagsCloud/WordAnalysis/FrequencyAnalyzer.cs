namespace TagsCloud.WordAnalysis;

public class FrequencyAnalyzer : IWordWeightAnalyzer
{
    public IEnumerable<WordStatistic> Analyze(IEnumerable<string> words)
    {
        var frequencyDict = new Dictionary<string, int>();

        foreach (var word in words)
            frequencyDict[word] = frequencyDict.TryGetValue(word, out var count)
                ? count + 1
                : 1;

        if (frequencyDict.Count == 0)
            yield break;

        var maxFrequency = frequencyDict.Values.Max();

        foreach (var (word, frequency) in frequencyDict)
        {
            var weight = (double)frequency / maxFrequency;
            yield return new WordStatistic(word, weight);
        }
    }
}