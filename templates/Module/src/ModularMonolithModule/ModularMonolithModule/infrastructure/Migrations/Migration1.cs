using FluentMigrator;

namespace ModularMonolithModule.infrastructure.Migrations;

[Migration(1)]
public class Migration1 : Migration
{
    public override void Up()
    {
        Create.Table("widgets")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsString().NotNullable().Unique()
            .WithColumn("price").AsDecimal(18, 2).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("widgets");
    }
}