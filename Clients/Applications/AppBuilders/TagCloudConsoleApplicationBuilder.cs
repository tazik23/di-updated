using Autofac;

namespace Clients.Applications.AppBuilders;

public class TagCloudConsoleApplicationBuilder : ITagCloudApplicationBuilder
{
    public ContainerBuilder Container { get; } = new();

    public ITagCloudApplication Build()
    {
        var container = Container.Build();
        using var scope = container.BeginLifetimeScope();
        var application = scope.Resolve<ConsoleTagCloudApplication>();
        
        return application;
    }
}