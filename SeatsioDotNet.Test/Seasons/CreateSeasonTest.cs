using System;
using System.Collections.Generic;
using System.Linq;
using SeatsioDotNet.Events;
using Xunit;

namespace SeatsioDotNet.Test.Seasons
{
    public class CreateSeasonTest : SeatsioClientTest
    {
        [Fact]
        public void ChartKeyIsMandatory()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey);

            Assert.NotNull(season.Key);
            Assert.NotEqual(0, season.Id);
            Assert.True(season.IsTopLevelSeason);
            Assert.Null(season.TopLevelSeasonKey);
            Assert.Empty(season.PartialSeasonKeys);
            Assert.Empty(season.Events);
            Assert.Equal(chartKey, season.ChartKey);
            Assert.Equal("INHERIT", season.TableBookingConfig.Mode);
            Assert.True(season.SupportsBestAvailable);
            Assert.Null(season.ForSaleConfig);
            CustomAssert.CloseTo(DateTimeOffset.Now, season.CreatedOn.Value);
            Assert.Null(season.UpdatedOn);
        }

        [Fact]
        public void KeyCanBePassedIn()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey, key: "aSeason");

            Assert.Equal("aSeason", season.Key);
        }    
        
        [Fact]
        public void NumberOfEventsCanBePassedIn()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey, numberOfEvents: 2);

            Assert.Equal(2, season.Events.Count);
        }   
        
        [Fact]
        public void EventKeysCanBePassedIn()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey, eventKeys: new[] {"event1", "event2"});

            Assert.Equal(new[] {"event1", "event2"}, season.Events.Select(e => e.Key));
        }   
        
        [Fact]
        public void TableBookingConfigCanBePassedIn()
        {
            var chartKey = CreateTestChart();

            var season = Client.Seasons.Create(chartKey, tableBookingConfig: TableBookingConfig.AllBySeat());

            Assert.Equal(TableBookingConfig.AllBySeat().Mode, season.TableBookingConfig.Mode);
        }
        
        [Fact]
        public void ChannelsCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var channels = new List<Channel>
            {
                new("channelKey1", "channel 1", "#FFFF00", 1, new[] {"A-1", "A-2"}),
                new("channelKey2", "channel 2", "#00FFFF", 2, new String[] {})
            };
            
            var season = Client.Seasons.Create(chartKey, channels: channels);

            Assert.Equivalent(channels, season.Channels);
        }
        
        [Fact]
        public void ForSaleConfigCanBePassedIn()
        {
            var chartKey = CreateTestChart();
            var forSaleConfig = new ForSaleConfig().WithForSale(false).WithObjects(new []{"A-1"}).WithAreaPlaces(new(){{"GA1", 5}}).WithCategories(new []{"Cat1"});
            
            var season = Client.Seasons.Create(chartKey, forSaleConfig: forSaleConfig);

            Assert.Equivalent(forSaleConfig, season.ForSaleConfig);
        }
    }
}