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
            var settings = ctx.Resolve<CloudSettings>();
            return new Tokenizer(settings.Separators);
        }).As<ITokenizer>();

        builder.RegisterType<LowerCaseNormalizer>().As<IWordNormalizer>();

        builder.Register(_ => new StopWordsFilter(GetDefaultBoringWords())).As<IWordFilter>();
        
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
    
    private static HashSet<string> GetDefaultBoringWords()
    {
        return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "и", "в", "не", "на", "я", "что", "он", "с", "а", "как", "то",
            "но", "его", "она", "они", "мы", "вы", "это", "из", "у", "за",
            "по", "от", "к", "до", "для", "о", "же", "бы", "быть", "сказать",
            "только", "весь", "ещё", "уже", "вот", "когда", "даже", "мне",
            "там", "себя", "ни", "чем", "при", "да", "нет", "если", "так",
            "их", "был", "ему", "того", "или", "чтобы", "ли", "тоже", "него",
            "под", "без", "раз", "нам", "со", "будет", "ж", "тот", "зачем",
            "сейчас", "потом", "очень", "хорошо", "здесь", "тогда", "можно",
            "который", "другой", "мой", "свой", "наш", "ваш", "свои", "сам",
            "им", "ей", "тебя", "меня", "него", "нее", "них", "нас", "вас",
            "ими", "вами", "нами", "мной", "тобой", "собой", "кем", "чем",
            "кого", "чего", "кому", "чему", "ком", "чём", "какой", "какая",
            "какое", "какие", "чей", "чья", "чьё", "чьи", "сколько", "который",
            "которая", "которое", "которые"
        };
    }
}