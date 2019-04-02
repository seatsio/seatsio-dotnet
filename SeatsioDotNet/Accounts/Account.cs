namespace SeatsioDotNet.Accounts
{
    public class Account
    {
        public string SecretKey { get; set; }
        public string DesignerKey { get; set; }
        public string PublicKey { get; set; }
        public string Email { get; set; }
        public bool IsSubaccount { get; set; }
        public AccountSettings Settings { get; set; }
    }
}