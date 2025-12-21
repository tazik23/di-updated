using Autofac;
using Clients.Applications;
using TagsCloud.Configurations;
using TagsCloud.IO.Readers;
using TagsCloud.IO.Savers;
using TagsCloud.Layout;
using TagsCloud.Layout.Layouters;
using TagsCloud.Layout.Layouters.Geometry;
using TagsCloud.TextProcessing;
using TagsCloud.TextProcessing.Filters;
using TagsCloud.TextProcessing.Normalizers;
using TagsCloud.TextProcessing.Tokenizers;
using TagsCloud.Visualization;
using TagsCloud.WordAnalysis;
using TagsCloud.WordMetricsCalculation;
using TextReader = TagsCloud.IO.Readers.TextReader;

namespace Clients.Di;

public static class ServiceConfiguration
{

    public static void AddServices(this ContainerBuilder builder)
    {
        builder.RegisterType<TextReader>().As<ITextReader>();
        builder.RegisterType<PngImageSaver>().As<IImageSaver>();

        builder.Register(ctx =>
        {
            var options = ctx.Resolve<TextProcessingSettings>();
            return new Tokenizer(options.Separators);
        }).As<ITokenizer>();

        builder.RegisterNormalizers();
        builder.RegisterFilters();

        builder.RegisterType<TextProcessor>().As<ITextProcessor>();

        builder.RegisterType<FrequencyAnalyzer>().As<IWordWeightAnalyzer>();

        builder.RegisterType<GraphicsTextMeasurer>().As<ITextMeasurer>();

        builder.RegisterType<WordMetricsCalculator>().As<IWordMetricsCalculator>();

        builder.RegisterType<SpiralFactory>().As<ISpiralFactory>();
        
        builder.Register(ctx =>
        {
            var factory = ctx.Resolve<ISpiralFactory>();
            var layoutSettings = ctx.Resolve<LayoutSettings>();
            return factory.CreateSpiral(layoutSettings.Center, layoutSettings.Shape);
        }).As<ISpiral>();

        builder.RegisterType<SpiralLayouter>().As<ILayouter>();

        builder.RegisterType<CloudLayouter>().As<ICloudLayouter>();

        builder.RegisterType<CloudVisualizer>().As<ICloudVisualizer>();

        builder.RegisterType<ConsoleTagCloudApplication>().AsSelf();
    }

    private static void RegisterNormalizers(this ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var options = ctx.Resolve<TextProcessingSettings>();
            var normalizers = new List<IWordNormalizer> {
                new LowerCaseNormalizer() };
            
            return normalizers.ToArray();
        }).As<IWordNormalizer[]>();
    }

    private static void RegisterFilters(this ContainerBuilder builder)
    {
        builder.Register(ctx =>
        {
            var options = ctx.Resolve<TextProcessingSettings>();
            var filters = new List<IWordFilter>();
            
            if (!string.IsNullOrEmpty(options.StopWordsFilePath) && File.Exists(options.StopWordsFilePath))
            {
                var stopWords = LoadStopWordsFromFile(options.StopWordsFilePath);
                filters.Add(new StopWordsFilter(stopWords));
            }
            
            filters.Add(new MinLengthFilter(options.MinLength)); 
        
            return filters.ToArray();
        }).As<IWordFilter[]>();
    }
    
    private static HashSet<string> LoadStopWordsFromFile(string filePath)
    {
        var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    
        foreach (var line in File.ReadLines(filePath))
        {
            var word = line.Trim();
            if (string.IsNullOrWhiteSpace(word))
                continue;
            
            stopWords.Add(word);
        }
    
        return stopWords;
    }
}