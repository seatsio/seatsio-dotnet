using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class ListParams
    {
        protected Dictionary<string, string> _params = new Dictionary<string, string>();

        public Dictionary<string, string> AsDictionary()
        {
            return _params;
        }
    }
}