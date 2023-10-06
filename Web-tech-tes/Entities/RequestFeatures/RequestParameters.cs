namespace Entities.RequestFeatures
{
    public abstract class RequestParameters
    {
        private readonly int _maxPageSize = 50;
        private int _pageSize = 10;

        protected RequestParameters()
        {
        }
        protected RequestParameters(int maxPageSize, int pageSize)
        {
            _maxPageSize = maxPageSize;
            _pageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }
        // Поиск
        public string? SearchTerm { get; set; }
        // Сортировка
        public string OrderBy { get; set; }
        // Шейпинг
        public string? Fields { get; set; }
    }
}