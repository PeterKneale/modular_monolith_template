using System.Reflection;
using Common.Infrastructure.Configuration;
using Common.Infrastructure.Migrations;
using Microsoft.Extensions.Configuration;

namespace Common;

public class CommonModuleStart(IConfiguration configuration) : IModuleStartup
{
    public void Startup()
    {
        DbMigrations.Apply("common", configuration.GetDbConnectionString("common"), Assembly.GetExecutingAssembly());
    }

    public void Destroy()
    {
        DbMigrations.Apply("common", configuration.GetDbConnectionString("common"), Assembly.GetExecutingAssembly(), reset: true);
    }
}