using FluentMigrator;

namespace ModularMonolithModule.Infrastructure.Migrations;

[Migration(3, "Create queue")]
public class Migration3 : Migration
{
    // Installs https://github.com/tembo-io/pgmq
    public override void Up()
    {
        Execute.Sql($"SELECT pgmq.create('{QueueName}');");
    }

    public override void Down()
    {
        Execute.Sql($"SELECT pgmq.drop_queue('{QueueName}');");
    }
}