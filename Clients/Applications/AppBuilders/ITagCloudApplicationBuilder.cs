using Autofac;

namespace Clients.Applications.AppBuilders;

public interface ITagCloudApplicationBuilder
{
    ContainerBuilder Container { get; }
    ITagCloudApplication Build();
    ITagCloudApplicationBuilder ConfigureSettings(Action<CloudSettingsBuilder> configureSettings);
}