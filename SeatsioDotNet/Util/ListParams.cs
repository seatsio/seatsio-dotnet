using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class ListParams
    {
        protected Dictionary<string, string> _params = new Dictionary<string, string>();

        public ListParams SetPageSize(int pageSize)
        {
            _params.Add("pageSize", pageSize.ToString());
            return this;
        }

        public Dictionary<string, string> AsDictionary()
        {
            return _params;
        }
    }
}