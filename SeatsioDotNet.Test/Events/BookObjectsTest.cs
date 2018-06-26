﻿using System.Collections.Generic;
using FluentAssertions;
using SeatsioDotNet.Events;
using SeatsioDotNet.HoldTokens;
using Xunit;

namespace SeatsioDotNet.Test.Events
{
    public class BookObjectsTest : SeatsioClientTest
    {
        [Fact]
        public void Test()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            var result = Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"});

            Assert.Equal(ObjectStatus.Booked, Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").Status);
            Assert.Equal(ObjectStatus.Booked, Client.Events.RetrieveObjectStatus(evnt.Key, "A-2").Status);
            Assert.Equal(ObjectStatus.Free, Client.Events.RetrieveObjectStatus(evnt.Key, "A-3").Status);

            result.Labels.Should().BeEquivalentTo(new Dictionary<string, Labels>
            {
                {"A-1", new Labels("1", "seat", "A", "row")},
                {"A-2", new Labels("2", "seat", "A", "row")}
            });
        }
        
        
        [Fact]
        public void Sections()
        {
            var chartKey = CreateTestChartWithSections();
            var evnt = Client.Events.Create(chartKey);

            var result = Client.Events.Book(evnt.Key, new[] {"Section A-A-1", "Section A-A-2"});

            Assert.Equal(ObjectStatus.Booked, Client.Events.RetrieveObjectStatus(evnt.Key, "Section A-A-1").Status);
            Assert.Equal(ObjectStatus.Booked, Client.Events.RetrieveObjectStatus(evnt.Key, "Section A-A-2").Status);
            Assert.Equal(ObjectStatus.Free, Client.Events.RetrieveObjectStatus(evnt.Key, "Section A-A-3").Status);

            result.Labels.Should().BeEquivalentTo(new Dictionary<string, Labels>
            {
                {"Section A-A-1", new Labels("1", "seat", "A", "row", "Section A", "Entrance 1")},
                {"Section A-A-2", new Labels("2", "seat", "A", "row", "Section A", "Entrance 1")}
            });
        }

        [Fact]
        public void HoldToken()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);
            HoldToken holdToken = Client.HoldTokens.Create();
            Client.Events.Hold(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, holdToken.Token);

            var status1 = Client.Events.RetrieveObjectStatus(evnt.Key, "A-1");
            Assert.Equal(ObjectStatus.Booked, status1.Status);
            Assert.Null(status1.HoldToken);

            var status2 = Client.Events.RetrieveObjectStatus(evnt.Key, "A-2");
            Assert.Equal(ObjectStatus.Booked, status2.Status);
            Assert.Null(status2.HoldToken);
        }

        [Fact]
        public void OrderId()
        {
            var chartKey = CreateTestChart();
            var evnt = Client.Events.Create(chartKey);

            Client.Events.Book(evnt.Key, new[] {"A-1", "A-2"}, null, "order1");

            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "A-1").OrderId);
            Assert.Equal("order1", Client.Events.RetrieveObjectStatus(evnt.Key, "A-2").OrderId);
        }
    }
}