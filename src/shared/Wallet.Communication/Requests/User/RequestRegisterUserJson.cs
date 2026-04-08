namespace Wallet.Communication.Requests.User
{
    public class RequestRegisterUserJson
    {
        public string Name { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phonenumber {  get; set; } = string.Empty;
        public string Occupation{  get; set; } = string.Empty;
        public string Income{  get; set; } = string.Empty;
    }
}
