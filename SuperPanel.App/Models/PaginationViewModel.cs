using System.Collections.Generic;

namespace SuperPanel.App.Models
{
    public class PaginationViewModel<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; } = 1;
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
