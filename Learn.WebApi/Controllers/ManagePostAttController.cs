using AutoMapper;
using Learn.Interface;
using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagePostAttController : ControllerBase
    {
        private readonly IBaseManagePostAtt<ManagePostAtt, ManagePostAtt> _ManagePostAtt;
        private readonly ILogger<ManagePostAttController> _Logger;
        private readonly IMapper _Mapper;
        public ManagePostAttController(IMapper Mapper, ILogger<ManagePostAttController> Logger, IBaseManagePostAtt<ManagePostAtt, ManagePostAtt> ManagePostAtt)
        {
            this._Logger = Logger;
            this._ManagePostAtt = ManagePostAtt;
            this._Mapper = Mapper;
        }

        /// <summary>
        /// 批量新增规则内的管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("AddManyAsync")]
        public async Task AddManyAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttList<ManagePostAtt> ManagePostAttList = @object.ToObject<ManagePostAttList<ManagePostAtt>>();
            if (null != ManagePostAttList && ManagePostAttList.HistoryAtts.Count > 0)
            {
                await this._ManagePostAtt.AddManyAsync(ManagePostAttList.HistoryAtts);
            }
        }

        /// <summary>
        /// 分页获取考勤数据[异步]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("GetPageListAsync")]
        public async Task<BaseResultModel<ManagePostAtt>> GetPageListAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostAtt.GetPageManagePostAtt(search);
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
            return await _ManagePostAtt.GetManagePostStatistics(search);
        }
    }
}
