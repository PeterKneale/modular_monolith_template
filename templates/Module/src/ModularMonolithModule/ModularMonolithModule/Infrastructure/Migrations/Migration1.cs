using FluentMigrator;

namespace ModularMonolithModule.Infrastructure.Migrations;

[Migration(1, "Install pgmq extension")]
public class Migration1 : Migration
{
    // Installs https://github.com/tembo-io/pgmq
    private const string ExtensionName="pgmq";
    
    public override void Up()
    {
        Execute.Sql($"CREATE EXTENSION IF NOT EXISTS {ExtensionName};");
    }

    public override void Down()
    {
        Execute.Sql($"DROP EXTENSION IF EXISTS {ExtensionName};");
    }
}