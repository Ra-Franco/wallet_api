using FluentMigrator;
using Wallet.Infrastructure.Migrations;

namespace Wallet.Infrasctructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USER, "Create table to save the user's information")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("users")
                .WithColumn("Birth_Date").AsDateTime().NotNullable()
                .WithColumn("Name").AsString(60).NotNullable()
                .WithColumn("CPF").AsString(30).NotNullable()
                .WithColumn("Gender").AsCustom("ENUM('F','M')").NotNullable()
                .WithColumn("Email").AsString(100).NotNullable()
                .WithColumn("Phonenumber").AsString(16).NotNullable()
                .WithColumn("Income").AsDecimal(15, 4).NotNullable()
                .WithColumn("Occupation").AsString(100).Nullable()
                .WithColumn("Password").AsString().NotNullable();
        }
    }
}
