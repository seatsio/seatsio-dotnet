namespace SeatsioDotNet.Test
{
    public class TestUser
    {
        public string SecretKey { get; set; }
        public string DesignerKey { get; set; }
        public TestUserWorkspace MainWorkspace { get; set; }
    }

    public class TestUserWorkspace
    {
        public TestTechnicalUser PrimaryUser { get; set; }
    }

    public class TestTechnicalUser
    {
        public long Id { get; set; }
    }
}