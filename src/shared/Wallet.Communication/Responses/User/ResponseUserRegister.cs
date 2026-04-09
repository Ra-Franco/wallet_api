using Wallet.Communication.Responses.Token;

namespace Wallet.Communication.Responses.User
{
    public class ResponseUserRegister
    {
        public string Name { get; set; } = string.Empty;
        public ResponseTokenJson Tokens { get; set; } = default!;
    }
}
