using System.Drawing;
using Autofac;
using TagsCloud.Configurations;

namespace Clients.Di;

public static class SettingsConfiguration
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
                visualizationSettings,
                args.Separators.Split()
            );
        }).AsSelf().SingleInstance();
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
}