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
        private readonly IManagePostHistoryAtt _ManagePostHistoryAtt;
        private readonly ILogger<ManagePostHistoryAttController> _Logger;
        public ManagePostHistoryAttController(ILogger<ManagePostHistoryAttController> Logger, IManagePostHistoryAtt ManagePostHistoryAtt)
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
            ManagePostHistoryAttList managePostHistoryAttList = @object.ToObject<ManagePostHistoryAttList>();
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

        [HttpPost("GetStatisticsAsync")]
        public async Task<ManagePostAttStatistics> GetManagePostStatisticsAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostHistoryAtt.GetManagePostStatistics(search);
        }

    }
}
