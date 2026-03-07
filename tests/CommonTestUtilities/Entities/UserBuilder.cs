using Bogus;
using Bogus.Extensions.Brazil;
using CommonTestUtilities.Cryptography;
using Wallet.Domain.Entities;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static (User user, string password) Build()
        {
            var passwordEncrypter = PasswordEncrypterBuilder.Build();

            var password = new Faker().Internet.Password();

            var user = new Faker<User>()
                .RuleFor(user => user.Id, () => 1)
                .RuleFor(user => user.Name, (f) => f.Person.FirstName)
                .RuleFor(user => user.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(user => user.Password, (f) => passwordEncrypter.Encrypt(password))
                .RuleFor(user => user.CPF, (f) => f.Person.Cpf())
                .RuleFor(user => user.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(user => user.Gender, f => f.PickRandom("M", "F"))
                .RuleFor(user => user.Phonenumber, f => f.Phone.PhoneNumber("(##) #####-####"))
                .RuleFor(user => user.Occupation, f => f.Name.JobTitle())
                .RuleFor(user => user.Income, f => f.Finance.Amount(1500, 10000))
                // Ensure test users are Active to avoid intermittent unauthorized results
                .RuleFor(user => user.Status, f => UserStatus.Active);

            return (user, password);

        }
    }
}
