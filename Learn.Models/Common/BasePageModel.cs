using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Common
{
    public class BasePageModel //: BaseModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数据数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 排序字典值
        /// </summary>
        public object SortDic { get; set; }
        /// <summary>
        /// 查询条件字典值
        /// </summary>
        public object SearchDic { get; set; }
        /// <summary>
        /// 去重的字段
        /// </summary>
        public string DistinctField { get; set; }

    }
}
