using FluentMigrator;

namespace Wallet.Infrasctructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USERS_SECURITY_SETTINGS, "Create a table to save user's security settings ")]
    public class Version0000006 : VersionBase
    {
        public override void Up()
        {
            CreateTable("users_security_settings")
                .WithColumn("transaction_limit_period").AsInt32().NotNullable()
                .WithColumn("transaction_limit").AsDecimal(15, 4).NotNullable()
                .WithColumn("user_id").AsInt64().NotNullable()
                    .ForeignKey("fk_user_security_setting_user_id", "users", "id");
        }
    }
}
