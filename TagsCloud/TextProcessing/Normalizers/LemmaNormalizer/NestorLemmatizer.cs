using Nestor;

namespace TagsCloud.TextProcessing.Normalizers.LemmaNormalizer;

public class NestorLemmatizer : ILemmatizer
{
    private readonly NestorMorph _nMorph;

    public NestorLemmatizer(NestorMorph nMorph)
    {
        _nMorph = nMorph;
    }

    public string GetLemma(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return word;
            
        var wordInfos = _nMorph.WordInfo(word);
        
        return wordInfos.Length > 0 ? wordInfos[0].Lemma.Word : word.ToLower();
    }
}