using Xunit;

namespace SeatsioDotNet.Test
{
    public class ErrorHandlingTest : SeatsioClientTest
    {
        [Fact]
        public void Test4xx()
        {
            var e = Assert.Throws<SeatsioException>(() =>
                Client.Events.RetrieveObjectStatus("unexistingEvent", "unexistingObject"));

            Assert.StartsWith("GET https://api-staging.seatsio.net/events/unexistingEvent/objects/unexistingObject resulted in a 404 Not Found response. Reason: Event not found: unexistingEvent. Request ID:", e.Message);
            Assert.Equal(new[] {"Event not found: unexistingEvent"}, e.Messages);
            Assert.NotNull(e.RequestId);
        }

        [Fact]
        public void WeirdError()
        {
            var e = Assert.Throws<SeatsioException>(() =>
                new SeatsioClient("", "unknownProtocol://").Events.RetrieveObjectStatus("unexistingEvent", "unexistingObject"));

            Assert.Equal("GET  resulted in a 0  response.", e.Message);
            Assert.Null(e.Messages);
            Assert.Null(e.RequestId);
        }
    }
}