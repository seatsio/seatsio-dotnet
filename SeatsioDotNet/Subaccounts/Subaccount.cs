using SeatsioDotNet.Workspaces;

namespace SeatsioDotNet.Subaccounts
{
    public class Subaccount
    {
        public long Id { get; set; }
        public string SecretKey { get; set; }
        public string DesignerKey { get; set; }
        public string PublicKey { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public Workspace workspace { get; set; }
    }
}