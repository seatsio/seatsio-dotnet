using RestSharp;
using Xunit;

namespace seatsio_dotnet
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var client = new RestClient("http://www.google.be");
            IRestResponse response = client.Execute(new RestRequest("", Method.GET));
            Assert.Contains("Google", response.Content);
            
        }
    }
}