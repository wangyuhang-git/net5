using Learn.Models.Common;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Entity
{
    /// <summary>
    /// 考勤数据（完整的历史考勤数据）
    /// </summary>
    [Serializable]
    public class ManagePostHistoryAtt : ManagePostAtt
    {
        /// <summary>
        /// 是否满足考勤规则
        /// </summary>
        public bool IsRule { get; set; }

    }
}
