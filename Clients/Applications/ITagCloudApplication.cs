using Clients.Applications.AppBuilders;

namespace Clients.Applications;

public interface ITagCloudApplication
{
    static abstract ITagCloudApplicationBuilder CreateBuilder();
    void Run();
}