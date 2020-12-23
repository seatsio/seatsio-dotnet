using System.Collections.Generic;

namespace SeatsioDotNet.Charts
{
    public class SocialDistancingRuleset
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int NumberOfDisabledSeatsToTheSides { get; set; }
        public bool DisableSeatsInFrontAndBehind { get; set; }
        public bool DisableDiagonalSeatsInFrontAndBehind { get; set; }
        public int NumberOfDisabledAisleSeats { get; set; }
        public int MaxGroupSize { get; set; }
        public int MaxOccupancyAbsolute { get; set; }
        public int MaxOccupancyPercentage { get; set; }
        public bool OneGroupPerTable { get; set; }
        public bool FixedGroupLayout { get; set; }
        public List<string> DisabledSeats { get; set; } = new List<string>();
        public List<string> EnabledSeats { get; set; } = new List<string>();

        public static FixedSocialDistancingRulesetBuilder Fixed(string name)
        {
            return new FixedSocialDistancingRulesetBuilder(name);
        }

        public static RuleBasedSocialDistancingRulesetBuilder RuleBased(string name)
        {
            return new RuleBasedSocialDistancingRulesetBuilder(name);
        }

        public object AsJsonObject()
        {
            return new
            {
                index = Index,
                name = Name,
                numberOfDisabledSeatsToTheSides = NumberOfDisabledSeatsToTheSides,
                disableSeatsInFrontAndBehind = DisableSeatsInFrontAndBehind,
                disableDiagonalSeatsInFrontAndBehind = DisableDiagonalSeatsInFrontAndBehind,
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

    public class FixedSocialDistancingRulesetBuilder
    {
        private readonly string Name;
        private int Index;
        private List<string> DisabledSeats;

        public FixedSocialDistancingRulesetBuilder(string name)
        {
            Name = name;
        }

        public SocialDistancingRuleset Build()
        {
            var ruleset = new SocialDistancingRuleset
            {
                Name = Name,
                Index = Index, 
                FixedGroupLayout = true,
                DisabledSeats = DisabledSeats
            };
            return ruleset;
        }

        public FixedSocialDistancingRulesetBuilder WithIndex(int index)
        {
            Index = index;
            return this;
        }

        public FixedSocialDistancingRulesetBuilder WithDisabledSeats(List<string> disabledSeats)
        {
            DisabledSeats = disabledSeats;
            return this;
        }
    }

    public class RuleBasedSocialDistancingRulesetBuilder
    {
        private readonly string Name;
        private int Index;
        private int NumberOfDisabledSeatsToTheSides;
        private bool DisableSeatsInFrontAndBehind;
        private bool DisableDiagonalSeatsInFrontAndBehind;
        private int NumberOfDisabledAisleSeats;
        private int MaxGroupSize;
        private int MaxOccupancyAbsolute;
        private int MaxOccupancyPercentage;
        private bool OneGroupPerTable;
        private bool FixedGroupLayout;
        private List<string> DisabledSeats = new List<string>();
        private List<string> EnabledSeats = new List<string>();

        public RuleBasedSocialDistancingRulesetBuilder(string name)
        {
            Name = name;
        }

        public SocialDistancingRuleset Build()
        {
            var ruleset = new SocialDistancingRuleset
            {
                Name = Name,
                Index = Index,
                NumberOfDisabledSeatsToTheSides = NumberOfDisabledSeatsToTheSides,
                DisableSeatsInFrontAndBehind = DisableSeatsInFrontAndBehind,
                DisableDiagonalSeatsInFrontAndBehind = DisableDiagonalSeatsInFrontAndBehind,
                NumberOfDisabledAisleSeats = NumberOfDisabledAisleSeats,
                MaxGroupSize = MaxGroupSize,
                MaxOccupancyAbsolute = MaxOccupancyAbsolute,
                MaxOccupancyPercentage = MaxOccupancyPercentage,
                OneGroupPerTable = OneGroupPerTable,
                FixedGroupLayout = false,
                DisabledSeats = DisabledSeats,
                EnabledSeats = EnabledSeats
            };
            return ruleset;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithIndex(int index)
        {
            Index = index;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithNumberOfDisabledSeatsToTheSides(int numberOfDisabledSeatsToTheSides)
        {
            NumberOfDisabledSeatsToTheSides = numberOfDisabledSeatsToTheSides;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithDisableSeatsInFrontAndBehind(bool disableSeatsInFrontAndBehind)
        {
            DisableSeatsInFrontAndBehind = disableSeatsInFrontAndBehind;
            return this;
        }   
        
        public RuleBasedSocialDistancingRulesetBuilder WithDisableDiagonalSeatsInFrontAndBehind(bool disableDiagonalSeatsInFrontAndBehind)
        {
            DisableDiagonalSeatsInFrontAndBehind = disableDiagonalSeatsInFrontAndBehind;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithNumberOfDisabledAisleSeats(int numberOfDisabledAisleSeats)
        {
            NumberOfDisabledAisleSeats = numberOfDisabledAisleSeats;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithMaxGroupSize(int maxGroupSize)
        {
            MaxGroupSize = maxGroupSize;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithMaxOccupancyAbsolute(int maxOccupancyAbsolute)
        {
            MaxOccupancyAbsolute = maxOccupancyAbsolute;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithMaxOccupancyPercentage(int maxOccupancyPercentage)
        {
            MaxOccupancyPercentage = maxOccupancyPercentage;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithOneGroupPerTable(bool oneGroupPerTable)
        {
            OneGroupPerTable = oneGroupPerTable;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithDisabledSeats(List<string> disabledSeats)
        {
            DisabledSeats = disabledSeats;
            return this;
        }

        public RuleBasedSocialDistancingRulesetBuilder WithEnabledSeats(List<string> enabledSeats)
        {
            EnabledSeats = enabledSeats;
            return this;
        }
    }
}