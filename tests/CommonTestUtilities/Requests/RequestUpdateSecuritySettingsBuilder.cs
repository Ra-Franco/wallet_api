using Bogus;
using Microsoft.AspNetCore.JsonPatch;
using Wallet.Communication.Requests.User;
using Wallet.Communication.Requests.User.Security;
using Wallet.Communication.Utils;
using Wallet.Domain.Enum;

namespace CommonTestUtilities.Requests
{
    public class RequestUpdateSecuritySettingsBuilder
    {
        public static RequestUpdateSecuritySettings Build()
        {
            var faker = new Faker();
            return new Faker<RequestUpdateSecuritySettings>()
                .RuleFor(req => req.TransactionLimit, faker.Finance.Amount().DecimalToStringCurrency())
                .RuleFor(req => req.TransactionLimitPeriod, (f) => f.PickRandom<TransactionLimitPeriod>());
        }
        
        public static JsonPatchDocument<RequestUpdateSecuritySettings> JsonPatchBuild()
        {
            var faker = new Faker("pt_BR");
            var patchDoc = new JsonPatchDocument<RequestUpdateSecuritySettings>();

            patchDoc.Replace(req => req.TransactionLimit, faker.Finance.Amount().DecimalToStringCurrency());
            patchDoc.Replace(req => req.TransactionLimitPeriod, faker.PickRandom<TransactionLimitPeriod>());

            return patchDoc;
        }
    }
}
