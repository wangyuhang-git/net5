using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Business
{
    /// <summary>
    /// 管理人员考勤信息统计
    /// </summary>
    public class ManagePostAttStatistics : ManagePostAtt
    {
        /// <summary>
        /// 总考勤项目数
        /// </summary>
        public long AttProjectTotal { get; set; }

        /// <summary>
        /// 考勤人数
        /// </summary>
        public long AttPersonCount { get; set; }

        /// <summary>
        /// 今日考勤项目数
        /// </summary>
        public long AttProjectTodayCount { get; set; }

        /// <summary>
        /// 今日考勤人数
        /// </summary>
        public long AttPersonTodayCount { get; set; }
    }
}
