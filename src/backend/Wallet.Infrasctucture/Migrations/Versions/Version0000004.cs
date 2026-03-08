using FluentMigrator;
using Wallet.Infrastructure.Migrations;

namespace Wallet.Infrasctructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_TRANSACTION, "Create a table to save wallet transaction's ")]
    public class Version0000004 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Transactions")
                .WithColumn("transactional_number").AsString(50).NotNullable()
                .WithColumn("wallet_id").AsInt64().NotNullable()
                    .ForeignKey("wallet", "id")
                .WithColumn("type").AsInt32().NotNullable()
                .WithColumn("status").AsInt32().NotNullable().WithDefaultValue(1)
                .WithColumn("amount").AsDecimal(19, 4).NotNullable()
                .WithColumn("related_wallet_id").AsInt64().NotNullable()
                    .ForeignKey("wallet", "id")
                .WithColumn("description").AsString(500).Nullable()
                .WithColumn("transaction_date").AsDateTime().NotNullable();
        }
    }
}
