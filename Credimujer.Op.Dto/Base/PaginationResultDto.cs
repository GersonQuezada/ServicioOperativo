using System;
using System.Collections.Generic;

namespace Credimujer.Op.Dto.Base
{

    [Serializable()]
    public class PaginationResultDTO<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }

        public PaginationResultDTO()
        {
            Results = new List<T>();
        }
    }

    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
    
}
