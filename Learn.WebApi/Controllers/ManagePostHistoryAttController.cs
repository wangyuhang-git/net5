using Learn.Interface;
using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
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
        /// <param name="value"></param>
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
        /// <param name="value"></param>
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
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("GetStatisticsAsync")]
        public async Task<ManagePostAttStatistics> GetManagePostStatisticsAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostHistoryAtt.GetManagePostStatistics(search);
        }

        [HttpPost("GetListAggregateAsync")]
        public async Task<BaseResultModel<ManagePostHistoryAtt>> GetListAggregateAsync(int pageIndex, int pageSize, string type = "", string today = "")
        {
            return await _ManagePostHistoryAtt.GetListAggregateAsync(pageIndex: pageIndex, pageSize: pageSize, type: type, today: today);
        }

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
