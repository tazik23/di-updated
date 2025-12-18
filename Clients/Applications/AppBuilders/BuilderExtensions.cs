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
        builder.RegisterInstance(args).As<CommandLineArgs>().SingleInstance();

        builder.Register(ctx =>
        {
            var cmdArgs = ctx.Resolve<CommandLineArgs>();
            return new CloudSettings
            {
                InputPath = cmdArgs.InputFile,
                OutputPath = cmdArgs.OutputFile,
                FontSettings = new FontSettings
                {
                    FontFamily = new FontFamily(cmdArgs.FontFamily),
                    MinFontSize = cmdArgs.MinFontSize,
                    MaxFontSize = cmdArgs.MaxFontSize
                },
                LayoutOptions = new LayoutSettings
                {
                    Center = new Point(
                        cmdArgs.CenterX ?? cmdArgs.ImageWidth / 2,
                        cmdArgs.CenterY ?? cmdArgs.ImageHeight / 2),
                    Shape = cmdArgs.Shape
                },
                VisualizationSettings = new VisualizationSettings
                {
                    BackgroundColor = ParseColor(cmdArgs.BackgroundColor) ?? Color.White,
                    TextColor = ParseColor(cmdArgs.TextColor) ?? Color.Black,
                    ImageSize = new Size(cmdArgs.ImageWidth, cmdArgs.ImageHeight)
                }
            };
        }).As<CloudSettings>().SingleInstance();

        builder.Register(ctx => ctx.Resolve<CloudSettings>().FontSettings).As<FontSettings>();
        builder.Register(ctx => ctx.Resolve<CloudSettings>().LayoutOptions).As<LayoutSettings>();
        builder.Register(ctx => ctx.Resolve<CloudSettings>().VisualizationSettings).As<VisualizationSettings>();
    }

    public static void AddServices(this ContainerBuilder builder)
    {
        builder.RegisterType<TextReader>().As<ITextReader>();
        builder.RegisterType<PngImageSaver>().As<IImageSaver>();

        builder.RegisterType<Tokenizer>()
            .As<ITokenizer>()
            .WithParameter("separators", new[] { " ", "\t", "\r\n", "\n", "\r" });

        builder.RegisterType<LowerCaseNormalizer>()
            .As<IWordNormalizer>()
            .SingleInstance();

        builder.RegisterType<BoringWordsFilter>()
            .As<IWordFilter>()
            .WithParameter("boringWords", GetDefaultBoringWords())
            .SingleInstance();

        builder.RegisterType<TextProcessor>().As<ITextProcessor>();

        builder.RegisterType<FrequencyAnalyzer>().As<IWordWeightAnalyzer>();

        builder.RegisterType<GraphicsTextMeasurer>().As<ITextMeasurer>();
        builder.RegisterType<WordMetricsCalculator>().As<IWordMetricsCalculator>();

        builder.RegisterType<SpiralFactory>().As<ISpiralFactory>().SingleInstance();
        
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

    private static Color? ParseColor(string? colorStr)
    {
        if (string.IsNullOrWhiteSpace(colorStr))
            return null;

        try
        {
            return Color.FromName(colorStr);
        }
        catch
        {
            try
            {
                return ColorTranslator.FromHtml(colorStr);
            }
            catch
            {
                return null;
            }
        }
    }
}