using FluentMigrator;

namespace ModularMonolithModule.Infrastructure.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table(WidgetsTable)
            .WithColumn(IdColumn).AsGuid().PrimaryKey()
            .WithColumn(NameColumn).AsString().NotNullable().Unique()
            .WithColumn(PriceColumn).AsDecimal(18, 2).NotNullable();
    }

    public override void Down()
    {
        Delete.Table(WidgetsTable);
    }
}