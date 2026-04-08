namespace Wallet.Communication.Requests.User
{
    public class RequestUpdateRegistrationUser
    {
        public string Email { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Income { get; set; } = string.Empty;
    }
}
