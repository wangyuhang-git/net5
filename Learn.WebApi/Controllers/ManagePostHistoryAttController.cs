using Learn.Interface;
using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Learn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class ManagePostHistoryAttController : ControllerBase
    {
        private readonly IBaseManagePostAtt<ManagePostHistoryAtt, ManagePostAtt> _ManagePostHistoryAtt;
        private readonly ILogger<ManagePostHistoryAttController> _Logger;
        public ManagePostHistoryAttController(ILogger<ManagePostHistoryAttController> Logger, IBaseManagePostAtt<ManagePostHistoryAtt, ManagePostAtt> ManagePostHistoryAtt)
        {
            this._Logger = Logger;
            this._ManagePostHistoryAtt = ManagePostHistoryAtt;
        }

        /// <summary>
        /// 批量新增管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="value">postman eg:{"HistoryAtts": [{"AttendanceId": "3a53d5c5-b5c3-4514-a90e-115f51e66f92" }]}</param>
        /// <returns></returns>
        [HttpPost("AddManyAsync")]
        public async Task AddManyAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttList<ManagePostHistoryAtt> managePostHistoryAttList = @object.ToObject<ManagePostAttList<ManagePostHistoryAtt>>();
            if (null != managePostHistoryAttList && managePostHistoryAttList.HistoryAtts.Count > 0)
            {
                await this._ManagePostHistoryAtt.AddManyAsync(managePostHistoryAttList.HistoryAtts);
            }
        }

        /// <summary>
        /// 分页获取考勤数据[异步]
        /// </summary>
        /// <param name="value">postman eg:{"PageIndex":1,"PageSize":15,"SortDic":{"AttendanceTime":"d","CreateTime":"d"},"SearchDic":{"s_1_SegmentAddressArea":"市本级","s_1_SupervisionDepartment":"房建监督二科"}}</param>
        /// <returns></returns>
        [HttpPost("GetPageListAsync")]
        public async Task<BaseResultModel<ManagePostHistoryAtt>> GetPageListAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostHistoryAtt.GetPageManagePostAtt(search);
        }

        /// <summary>
        /// 获取管理人员考勤统计数据[异步]
        /// 包含总考勤项目数、今日考勤项目数、今日考勤人数
        /// </summary>
        /// <param name="value">postman eg:{"PageIndex":1,"PageSize":15,"SortDic":{"AttendanceTime":"d","CreateTime":"d"},"SearchDic":{"s_1_SegmentAddressArea":"德清县"}}</param>
        /// <returns></returns>
        [HttpPost("GetStatisticsAsync")]
        public async Task<ManagePostAttStatistics> GetManagePostStatisticsAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostHistoryAtt.GetManagePostStatistics(search);
        }

        /// <summary>
        /// 取分组后的数据
        /// </summary>
        /// <param name="value">postman eg:{"PageIndex":1,"PageSize":15,"GroupField":"ConstructPermitNum","SortDic":{"AttendanceTime":"d"},"SearchDic":{"s_1_SegmentAddressArea":"南浔区","s_3_AttendanceTime":"2021-01-18 00:00:01,2021-01-19 23:59:59","s_0_ProjectName|PersonName":"浔适园单元CD-01-01-06地块"}}</param>
        /// <returns></returns>
        [HttpPost("GetListAggregateAsync")]
        public async Task<BaseResultModel<ManagePostHistoryAtt>> GetListAggregateAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostHistoryAtt.GetListAggregateAsync(search);
        }

        /// <summary>
        /// 根据施工许可证号码、身份证号码、日期分组统计考勤人数
        /// </summary>
        /// <param name="value">postman eg:{//"SortDic":{"AttendanceTime":"d"},"SearchDic":{"s_1_ConstructPermitNum":"330591202009290202","s_3_AttendanceTime":"2021-01-24 00:00:01,2021-01-26 23:59:59"}}</param>
        /// <returns></returns>
        [HttpPost("GetListStatisticsAsync")]
        public async Task<IEnumerable<ManagePostAttStatistics>> GetListStatisticsAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostHistoryAtt.GetListStatisticsAsync(search);
        }

        /// <summary>
        /// 分流符合考勤规则的数据（暂时未window service调用）
        /// </summary>
        /// <param name="addressArea">地区</param>
        /// <param name="defaultRule">是否启用默认考勤规则，默认启用</param>
        /// <param name="limit">一次取top的条数</param>
        /// <returns></returns>
        [HttpPost("ByPassAttAsync")]
        public ActionResult ByPassAttAsync(string addressArea = "", bool defaultRule = true, int limit = 100)
        {
            try
            {
                _ManagePostHistoryAtt.ByPassAttAsync(addressArea, defaultRule, limit);
                return Ok("操作成功");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
