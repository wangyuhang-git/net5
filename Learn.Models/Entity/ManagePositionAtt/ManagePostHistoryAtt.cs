using Learn.Models.Common;
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
        /// 数据交换状态，默认为0，交换成功1，失败为2
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 数据交换时间
        /// </summary>
        public DateTime StatusTime { get; set; }
        /// <summary>
        /// 是否满足考勤规则
        /// </summary>
        public bool IsRule { get; set; }

    }
}
