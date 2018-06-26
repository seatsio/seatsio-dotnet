using System;

namespace SeatsioDotNet.Events
{
    public class Labels
    {
        public LabelAndType Own { get; set; }
        public LabelAndType Parent { get; set; }
        public string Section { get; set; }
        public Entrance Entrance { get; set; }

        public Labels()
        {
        }

        public Labels(string ownLabel = null, string ownType = null, string parentLabel = null, string parentType = null, string section = null, string entrance = null)
        {
            Own = new LabelAndType(ownLabel, ownType);
            if (parentLabel != null)
            {
                Parent = new LabelAndType(parentLabel, parentType);
            }

            if (entrance != null)
            {
                Entrance = new Entrance(entrance);
            }

            Section = section;
        }

    }
}