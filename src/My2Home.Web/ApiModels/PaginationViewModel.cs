using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My2Home.Web.ApiModels
{
    public class PaginationViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages => TotalRecords.HasValue ? (int)Math.Ceiling(TotalRecords.Value / (double)PageSize) : (int?)null;
    }

    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> items, int pageNumber, int pageSize, int? totalRecords)
        {
            Results = new List<T>(items);
            PagingInfo = new PaginationViewModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
            };
        }
        public List<T> Results { get; private set; }
        public PaginationViewModel PagingInfo { get; private set; }
    }

}
