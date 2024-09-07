namespace Plank.Core.Contracts
{
    public abstract class AbstractSearchDto : ISearchDto
    {
        private int _length = 10;
        private int _start = 0;

        public int PageNumber
        {
            get { return (_start / _length) + 1; }
        }

        public int PageSize
        {
            get { return _length; }
            set { _length = value > 0 ? value : _length; }
        }

        public int Start
        {
            get { return _start; }
            set { _start = value >= 0 ? value : _start; }
        }
    }
}