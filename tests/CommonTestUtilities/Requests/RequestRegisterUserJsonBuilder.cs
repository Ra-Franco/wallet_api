using Bogus;
using Bogus.Extensions.Brazil;
using Wallet.Communication.Requests.User;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterUserJsonBuilder
    {
        public static RequestRegisterUserJson Build(int password = 10)
        {
            var validateGender = new[] { Gender.F, Gender.M };

            return new Faker<RequestRegisterUserJson>()
                .RuleFor(u => u.Name, (f) => f.Person.FirstName)
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(u => u.Password, (f) => f.Internet.Password(password))
                .RuleFor(u => u.CPF, (f) => f.Person.Cpf())
                .RuleFor(u => u.BirthDate, (f) => f.Person.DateOfBirth)
                .RuleFor(u => u.Gender, (f) => f.PickRandom(validateGender).ToString())
                .RuleFor(u => u.Phonenumber, (f) => f.Person.Phone)
                .RuleFor(u => u.Occupation, (f) => f.Name.JobTitle())
                .RuleFor(u => u.Income, (f) => f.Finance.Amount(0, 1000).ToString());
        }
    }
}
