using System.Collections.Generic;

namespace SeatsioDotNet.Charts
{
    public class SocialDistancingRuleset
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int NumberOfDisabledSeatsToTheSides { get; set; }
        public bool DisableSeatsInFrontAndBehind { get; set; }
        public int NumberOfDisabledAisleSeats { get; set; }
        public int MaxGroupSize { get; set; }
        public List<string> DisabledSeats { get; set; }
        public List<string> EnabledSeats { get; set; }

        public SocialDistancingRuleset()
        {
        }

        public SocialDistancingRuleset(int index, string name, int numberOfDisabledSeatsToTheSides = 0,
            bool disableSeatsInFrontAndBehind = false, int numberOfDisabledAisleSeats = 0, int maxGroupSize = 0,
            List<string> disabledSeats = null, List<string> enabledSeats = null)
        {
            Index = index;
            Name = name;
            NumberOfDisabledSeatsToTheSides = numberOfDisabledSeatsToTheSides;
            DisableSeatsInFrontAndBehind = disableSeatsInFrontAndBehind;
            NumberOfDisabledAisleSeats = numberOfDisabledAisleSeats;
            MaxGroupSize = maxGroupSize;
            DisabledSeats = disabledSeats ?? new List<string>();
            EnabledSeats = enabledSeats ?? new List<string>();
        }

        public object AsJsonObject()
        {
            return new
            {
                index = Index,
                name = Name,
                numberOfDisabledSeatsToTheSides = NumberOfDisabledSeatsToTheSides,
                disableSeatsInFrontAndBehind = DisableSeatsInFrontAndBehind,
                numberOfDisabledAisleSeats = NumberOfDisabledAisleSeats,
                maxGroupSize = MaxGroupSize,
                disabledSeats = DisabledSeats,
                enabledSeats = EnabledSeats
            };
        }
    }
}