using RestSharp;
using RestSharp.Authenticators;

namespace SeatsioDotNet
{
    public class SeatsioClient
    {
        private readonly string _secretKey;
        private readonly string _baseUrl;

        public SeatsioClient(string secretKey, string baseUrl)
        {
            _secretKey = secretKey;
            _baseUrl = baseUrl;
        }  
        
        public SeatsioClient(string secretKey)
        {
            _secretKey = secretKey;
            _baseUrl = "https://api.seats.io";
        }

        public Subaccounts.Subaccounts Subaccounts()
        {
            return new Subaccounts.Subaccounts(CreateRestClient());
        }

        public Events.Events Events()
        {
            return new Events.Events(CreateRestClient());
        }
        
        private RestClient CreateRestClient()
        {
            var client = new RestClient(_baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(_secretKey, null);
            return client;
        }
    }
}