using FluentMigrator;
using static Common.Infrastructure.Integration.DbConstants;

namespace Common.Infrastructure.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(IntegrationEventsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(TypeColumn).AsString(255).NotNullable()
            .WithColumn(JsonColumn).AsString().NotNullable();
    }

    public override void Down()
    {
        Delete.Table(IntegrationEventsTable);
    }
}