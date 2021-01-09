using Learn.Models.Common;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Models.Entity
{
    /// <summary>
    /// 考勤数据（满足规则的数据）
    /// </summary>
    [Serializable]
    public class ManagePostAtt : BaseModel
    {
        /// <summary>
        /// 考勤表的GUID
        /// </summary>
        public string AttendanceId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCard { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string PostType { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public string SegmentAddressArea { get; set; }
        /// <summary>
        /// 考勤时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime AttendanceTime { get; set; }
        /// <summary>
        /// 考勤图片
        /// </summary>
        public string ImageBuffer { get; set; }
    }
}
