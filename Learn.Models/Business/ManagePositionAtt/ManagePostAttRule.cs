using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Business
{
    /// <summary>
    /// 管理岗位相关考勤规则
    /// </summary>
    [Serializable]
    public class ManagePostAttRule
    {
        /// <summary>
        /// 企业类型
        /// </summary>
        public string CorpType { get; set; }
        /// <summary>
        /// 岗位类型
        /// </summary>
        public string PostType { get; set; }
        /// <summary>
        /// 上午考勤开始时间
        /// </summary>
        public string AmStart { get; set; }
        /// <summary>
        /// 上午考勤结束时间
        /// </summary>
        public string AmEnd { get; set; }
        /// <summary>
        /// 下午考勤开始时间
        /// </summary>
        public string PmStart { get; set; }
        /// <summary>
        /// 下午考勤结束时间
        /// </summary>
        public string PmEnd { get; set; }
        /// <summary>
        /// 间隔小时
        /// </summary>
        public long IntervalHours { get; set; }
        /// <summary>
        /// 要求每月达到的次数
        /// </summary>
        public long Minimum { get; set; }
    }
}
