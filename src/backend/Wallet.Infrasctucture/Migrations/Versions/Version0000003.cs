using FluentMigrator;
using Wallet.Infrastructure.Migrations;

namespace Wallet.Infrasctructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_WALLET, "Create a table to save wallet's information of a user")]
    public class Version0000003 : VersionBase
    {
        public override void Up()
        {
            CreateTable("wallet")
                .WithColumn("Balance").AsDecimal(15, 4).NotNullable()
                .WithColumn("Pending_Balance").AsDecimal(15, 4).NotNullable()
                .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(1)
                .WithColumn("Transaction_Password").AsString()
                .WithColumn("User_Id").AsInt64().NotNullable().ForeignKey("Wallet_User_Id_FK", "users", "Id");
        }
    }
}
