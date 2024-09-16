using FluentMigrator;

namespace ModularMonolithModule.Infrastructure.Migrations;

[Migration(2)]
public class Migration2 : Migration
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