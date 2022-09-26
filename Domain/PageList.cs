namespace Domain
{
    public class PageList<T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int TatalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TatalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public static PageList<T> CreateAsync(
            IEnumerable<T> source, int pageNumber, int pageSize
        )
        {
            var count = source.Count();
            var items = source.Skip((pageNumber-1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
        
    }
}
