using System.Drawing;
using Autofac;
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

namespace Clients.Applications.AppBuilders;

public static class BuilderExtensions
{
    public static void ConfigureSettings(this ContainerBuilder builder, CommandLineArgs args)
    {
        builder.RegisterInstance(args)
               .AsSelf()
               .SingleInstance();
        
        builder.Register(_ => new FontSettings(
            new FontFamily(args.FontFamily),
            args.MinFontSize,
            args.MaxFontSize
        )).AsSelf().SingleInstance();

        builder.Register(_ => new LayoutSettings(
            new Point(args.CenterX ?? 400, args.CenterY ?? 300),
            args.Shape
        )).AsSelf().SingleInstance();

        builder.Register(_ => new VisualizationSettings(
            ParseColor(args.BackgroundColor),
            ParseColor(args.TextColor),
            new Size(args.ImageWidth, args.ImageHeight)
        )).AsSelf().SingleInstance();
        
        builder.Register(ctx =>
        {
            var fontSettings = ctx.Resolve<FontSettings>();
            var layoutSettings = ctx.Resolve<LayoutSettings>();
            var visualizationSettings = ctx.Resolve<VisualizationSettings>();
            
            return new CloudSettings(
                args.InputFile,
                args.OutputFile,
                fontSettings,
                layoutSettings,
                visualizationSettings
            );
        }).AsSelf().SingleInstance();
    }

    public static void AddServices(this ContainerBuilder builder)
    {
        builder.RegisterType<TextReader>().As<ITextReader>();
        builder.RegisterType<PngImageSaver>().As<IImageSaver>();

        builder.RegisterType<Tokenizer>().As<ITokenizer>()
               .WithParameter("separators", new[] { " ", "\t", "\r\n", "\n", "\r" });

        builder.RegisterType<LowerCaseNormalizer>().As<IWordNormalizer>();

        builder.RegisterType<BoringWordsFilter>().As<IWordFilter>()
               .WithParameter("boringWords", GetDefaultBoringWords());

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

    private static Color ParseColor(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
            throw new ArgumentException($"Unable to parse color {color}");

        try
        {
            return Color.FromName(color);
        }
        catch
        {
            try
            {
                return ColorTranslator.FromHtml(color);
            }
            catch
            {
                throw new ArgumentException($"Unable to parse color {color}");
            }
        }
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