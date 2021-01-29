using Learn.Common;
using Learn.Models.Business;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Business.ManagePositionAtt
{
    /// <summary>
    /// 考勤规则操作类
    /// </summary>
    public class ManagePostAttRuleBusiness
    {
        public List<ManagePostAttRule> ManagePostAttRules { set; get; }

        public ManagePostAttRuleBusiness(IOptions<List<ManagePostAttRule>> options)
        {
            ManagePostAttRules = options.Value;
        }
        /// <summary>
        /// 获取考勤规则列表
        /// </summary>
        /// <returns></returns>
        public List<ManagePostAttRule> GetPostAttRuleList()
        {
            //List<ManagePostAttRule> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ManagePostAttRule>>(ConfigHelper.GetSectionValue("AttendanceRules:Rules"));
            List<ManagePostAttRule> list = ManagePostAttRules;
            return list;
        }
        /// <summary>
        /// 根据岗位得到考勤规则，默认使用缺省参数
        /// </summary>
        /// <param name="corpType">企业类型</param>
        /// <param name="postType">岗位类型</param>
        /// <returns></returns>
        public  ManagePostAttRule GetPostAttRule(string corpType = "Default", string postType = "Default")
        {
            List<ManagePostAttRule> list = this.GetPostAttRuleList();
            return list.FirstOrDefault(c => c.CorpType == corpType && c.PostType == postType);
        }

    }
}
