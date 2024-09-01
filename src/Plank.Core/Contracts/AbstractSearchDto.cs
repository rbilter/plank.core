namespace Plank.Core.Contracts
{
    public abstract class AbstractSearchDto : ISearchDto
    {
        private int _length = 10;
        private string _searchValue = string.Empty;
        private int _start = 0;

        public int Length
        {
            get { return _length; }
            set { _length = value > 0 ? value : _length; }
        }

        public int PageNumber
        {
            get { return (_start / _length) + 1; }
        }

        public string SearchValue
        {
            get { return _searchValue; }
            set { _searchValue = value?.Trim() ?? string.Empty; }
        }

        public int Start
        {
            get { return _start; }
            set { _start = value >= 0 ? value : _start; }
        }
    }
}