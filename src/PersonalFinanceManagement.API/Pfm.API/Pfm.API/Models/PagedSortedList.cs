namespace PersonalFinanceManagement.API.Models
{
    public class PagedSortedList<T>
    {
        public int PageSize { get; set; }

        public int Page { get; set; }

        public int TotalCount { get; set; }

        public string SortBy { get; set; }
        
        public SortOrder SortOrder { get; set; }

        public List<T> Items { get; set; }
    }
}
