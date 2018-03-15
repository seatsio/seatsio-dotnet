namespace SeatsioDotNet.Util
{
    public class ListParams
    {
        private int? _pageSize;

        public ListParams SetPageSize(int? pageSize)
        {
            _pageSize = pageSize;
            return this;
        }

        public int? GetPageSize()
        {
            return _pageSize;
        }
    }
}