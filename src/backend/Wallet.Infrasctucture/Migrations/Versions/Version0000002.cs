using FluentMigrator;

namespace Wallet.Infrasctructure.Migrations.Versions
{
    [Migration(DatabaseVersions.USER_ADD_USERIDENTIFIER, "Adding column UserIdentifier for table users")]
    public class Version0000002 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("users")
                .AddColumn("UserIdentifier")
                .AsGuid()
                .NotNullable();
        }
    }
}
