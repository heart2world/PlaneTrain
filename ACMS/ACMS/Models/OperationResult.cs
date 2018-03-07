using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACMS.Models
{
    public class OperationResult<T>
    {
        public bool Result { get; set; }
        public string ResultMsg { get; set; }
        public T ResultData { get; set; }
    }

    public class OperationResult
    {
        public bool Result { get; set; }
        public string ResultMsg { get; set; }
    }

    public class PageResult<T>
    {
        public List<T> ResultData { get; set; }
        public int Total { get; set; }
        public int TotalPagesCount { get; set; }
    }
}