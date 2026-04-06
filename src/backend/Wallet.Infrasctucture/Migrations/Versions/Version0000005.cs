namespace Wallet.Infrasctructure.Migrations.Versions
{
    public class Version0000005 : VersionBase
    {
        public override void Up()
        {
            CreateTable("refresh_tokens")
                .WithColumn("value").AsString().NotNullable()
                .WithColumn("user_id").AsInt64().NotNullable()
                    .ForeignKey("fk_refresh_token_user_id", "users", "id");
        }
    }
}
