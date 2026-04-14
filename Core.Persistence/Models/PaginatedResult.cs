using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Persistence.Models
{
    public class PaginatedResult<T> where T : Entity
    {
        public IEnumerable<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < PageCount;
    }

    public class PageRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
