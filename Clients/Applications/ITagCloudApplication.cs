using Clients.Applications.AppBuilders;

namespace Clients.Applications;

public interface ITagCloudApplication
{
    static abstract ITagCloudApplicationBuilder CreateBuilder();

    static abstract ITagCloudApplication Create(Action<ITagCloudApplicationBuilder> configure);

    void Run();
}