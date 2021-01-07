using Learn.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Business
{
    public class StudentPageSearch : BaseSearchModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public object SortDic { get; set; }

        public StudentSearch StudentSearch { get; set; }
    }
}
