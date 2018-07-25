using Xunit;

namespace SeatsioDotNet.Test.Subaccounts
{
    public class UpdateSubaccountTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var email = RandomEmail();
            var subaccount = Client.Subaccounts.Create("joske");
            
            Client.Subaccounts.Update(subaccount.Id, "jefke", email);

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.Equal("jefke", retrievedSubaccount.Name);
            Assert.Equal(email, retrievedSubaccount.Email);
        }   
        
        [Fact]
        public void EmailIsOptional()
        {
            var email = RandomEmail();
            var subaccount = Client.Subaccounts.CreateWithEmail(email, "joske");

            Client.Subaccounts.Update(subaccount.Id, "jefke");

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.Equal("jefke", retrievedSubaccount.Name);
            Assert.Equal(email, retrievedSubaccount.Email);
        }  
        
        [Fact]
        public void NameIsOptional()
        {
            var email = RandomEmail();
            var subaccount = Client.Subaccounts.Create("joske");
            
            Client.Subaccounts.Update(subaccount.Id, email: email);

            var retrievedSubaccount = Client.Subaccounts.Retrieve(subaccount.Id);
            Assert.Equal("joske", retrievedSubaccount.Name);
            Assert.Equal(email, retrievedSubaccount.Email);
        }
    }
}