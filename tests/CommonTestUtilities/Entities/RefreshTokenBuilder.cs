using Bogus;
using Wallet.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class RefreshTokenBuilder
    {
        public static RefreshToken Build(User user)
        {
            return new Faker<RefreshToken>()
                .RuleFor(r => r.UserId, user.Id)
                .RuleFor(r => r.Value, Convert.ToBase64String(Guid.NewGuid().ToByteArray()))
                .RuleFor(r => r.User, user)
                .RuleFor(r => r.Created_On, DateTime.UtcNow);
        }
    }
}
