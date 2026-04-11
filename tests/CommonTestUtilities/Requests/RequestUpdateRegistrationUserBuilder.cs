using Bogus;
using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User;
using Wallet.Communication.Utils;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateRegistrationUserBuilder
    {
        public static RequestUpdateRegistrationUser Build()
        {
            return new Faker<RequestUpdateRegistrationUser>()
                .RuleFor(u => u.Email, (f) => f.Internet.Email())
                .RuleFor(u => u.Income, (f) => f.Finance.Amount(0, 1000).DecimalToStringCurrency())
                .RuleFor(u => u.Occupation, (f) => f.Name.JobTitle())
                .RuleFor(u => u.Phonenumber, (f) => f.Phone.PhoneNumber("(##) 9####-####"));
        }

        public static JsonPatchDocument<RequestUpdateRegistrationUser> JsonPatchBuild()
        {
            var faker = new Faker("pt_BR");
            var patchDoc = new JsonPatchDocument<RequestUpdateRegistrationUser>();

            patchDoc.Replace(u => u.Email, faker.Internet.Email());
            patchDoc.Replace(u => u.Income, faker.Finance.Amount(0, 1000).DecimalToStringCurrency());
            patchDoc.Replace(u => u.Occupation, faker.Name.JobTitle());
            patchDoc.Replace(u => u.Phonenumber, faker.Phone.PhoneNumber("(##) 9####-####"));

            return patchDoc;
        }
    }
}
