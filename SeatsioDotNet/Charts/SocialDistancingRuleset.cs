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
        public int MaxOccupancyAbsolute { get; set; }
        public int MaxOccupancyPercentage { get; set; }
        public bool OneGroupPerTable { get; set; }
        public bool FixedGroupLayout { get; set; }
        public List<string> DisabledSeats { get; set; }
        public List<string> EnabledSeats { get; set; }

        public SocialDistancingRuleset()
        {
        }

        public SocialDistancingRuleset(int index = 0, string name = null, int numberOfDisabledSeatsToTheSides = 0,
            bool disableSeatsInFrontAndBehind = false, int numberOfDisabledAisleSeats = 0, int maxGroupSize = 0,
            int maxOccupancyAbsolute = 0, int maxOccupancyPercentage = 0, bool oneGroupPerTable = false, bool fixedGroupLayout = false,
            List<string> disabledSeats = null, List<string> enabledSeats = null)
        {
            Index = index;
            Name = name;
            NumberOfDisabledSeatsToTheSides = numberOfDisabledSeatsToTheSides;
            DisableSeatsInFrontAndBehind = disableSeatsInFrontAndBehind;
            NumberOfDisabledAisleSeats = numberOfDisabledAisleSeats;
            MaxGroupSize = maxGroupSize;
            MaxOccupancyAbsolute = maxOccupancyAbsolute;
            MaxOccupancyPercentage = maxOccupancyPercentage;
            OneGroupPerTable = oneGroupPerTable;
            FixedGroupLayout = fixedGroupLayout;
            DisabledSeats = disabledSeats ?? new List<string>();
            EnabledSeats = enabledSeats ?? new List<string>();
        }

        public static SocialDistancingRuleset Fixed(int index = 0, string name = null, List<string> disabledSeats = null)
        {
            return new SocialDistancingRuleset(index, name, 0, false, 0, 0, 0, 0, false, true, disabledSeats, null);
        }   
        
        public static SocialDistancingRuleset RuleBased(int index = 0, string name = null, int numberOfDisabledSeatsToTheSides = 0,
            bool disableSeatsInFrontAndBehind = false, int numberOfDisabledAisleSeats = 0, int maxGroupSize = 0,
            int maxOccupancyAbsolute = 0, int maxOccupancyPercentage = 0, bool oneGroupPerTable = false,
            List<string> disabledSeats = null, List<string> enabledSeats = null)
        {
            return new SocialDistancingRuleset(index, name, numberOfDisabledSeatsToTheSides, disableSeatsInFrontAndBehind,
                numberOfDisabledAisleSeats, maxGroupSize, maxOccupancyAbsolute, maxOccupancyPercentage, oneGroupPerTable,
                false, disabledSeats, enabledSeats);
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
                maxOccupancyAbsolute = MaxOccupancyAbsolute,
                maxOccupancyPercentage = MaxOccupancyPercentage,
                oneGroupPerTable = OneGroupPerTable,
                fixedGroupLayout = FixedGroupLayout,
                disabledSeats = DisabledSeats,
                enabledSeats = EnabledSeats
            };
        }
    }
}