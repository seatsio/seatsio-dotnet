using SeatsioDotNet.Events;

namespace SeatsioDotNet.Test
{
    public class TestCompany
    {
        public User admin { get; set; }
        public Workspace Workspace { get; set; }
    }

    public class User
    {
        public string SecretKey { get; set; }
    }
}