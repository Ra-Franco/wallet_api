using FluentMigrator;
using Wallet.Infrastructure.Migrations;

namespace Wallet.Infrasctructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Create a table to save user's refresh token")]
    public class Version0000005 : VersionBase
    {
        public override void Up()
        {
            CreateTable("refresh_token")
                .WithColumn("value").AsString().NotNullable()
                .WithColumn("user_id").AsInt64().NotNullable()
                    .ForeignKey("fk_refresh_token_user_id", "users", "id");
        }
    }
}
