using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class TableBookingConfig
    {
        public string Mode { get; set; }
        public Dictionary<string, string> Tables { get; set; }

        public TableBookingConfig()
        {
        }

        private TableBookingConfig(string mode, Dictionary<string, string> tables)
        {
            Mode = mode;
            Tables = tables;
        }

        public static TableBookingConfig Inherit()
        {
            return new TableBookingConfig("INHERIT", null);
        }

        public static TableBookingConfig AllByTable()
        {
            return new TableBookingConfig("ALL_BY_TABLE", null);
        }

        public static TableBookingConfig AllBySeat()
        {
            return new TableBookingConfig("ALL_BY_SEAT", null);
        }

        public static TableBookingConfig Custom(Dictionary<string, string> tables)
        {
            return new TableBookingConfig("CUSTOM", tables);
        }

        public object AsJsonObject()
        {
            if (Mode == "CUSTOM")
            {
                return new
                {
                    mode = Mode,
                    tables = Tables
                };
            }

            return new
            {
                mode = Mode
            };
        }
    }
}