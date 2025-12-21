using Autofac;
using Clients.Di;

namespace Clients.Applications.AppBuilders;

public class TagCloudConsoleApplicationBuilder : ITagCloudApplicationBuilder
{
    public ContainerBuilder Container { get; } = new();

    public ITagCloudApplication Build()
    {
        Container.AddServices();
        var container = Container.Build();

        return container.Resolve<ConsoleTagCloudApplication>();
    }

    public ITagCloudApplicationBuilder ConfigureSettings(Action<CloudSettingsBuilder> configureSettings)
    {
        var settingsBuilder = new CloudSettingsBuilder();
        configureSettings(settingsBuilder);

        var cloudSettings = settingsBuilder.Build();

        Container.RegisterInstance(cloudSettings).AsSelf().SingleInstance();
        Container.RegisterInstance(cloudSettings.FontSettings).AsSelf().SingleInstance();
        Container.RegisterInstance(cloudSettings.LayoutSettings).AsSelf().SingleInstance();
        Container.RegisterInstance(cloudSettings.VisualizationSettings).AsSelf().SingleInstance();

        return this;
    }
}