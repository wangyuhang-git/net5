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
        /// <param name="value">postman eg:{"HistoryAtts": [{"AttendanceId": "3a53d5c5-b5c3-4514-a90e-115f51e66f92" }]}</param>
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
        /// <param name="value">postman eg:{"PageIndex":1,"PageSize":15,"SortDic":{"AttendanceTime":"d","CreateTime":"d"},"SearchDic":{"s_1_SegmentAddressArea":"市本级","s_1_SupervisionDepartment":"房建监督二科"}}</param>
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
        /// <param name="value">postman eg:{"PageIndex":1,"PageSize":15,"SortDic":{"AttendanceTime":"d","CreateTime":"d"},"SearchDic":{"s_1_SegmentAddressArea":"德清县"}}</param>
        /// <returns></returns>
        [HttpPost("GetStatisticsAsync")]
        public async Task<ManagePostAttStatistics> GetManagePostStatisticsAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostAtt.GetManagePostStatistics(search);
        }

        /// <summary>
        /// 取分组后的数据
        /// </summary>
        /// <param name="value">postman eg:{"PageIndex":1,"PageSize":15,"GroupField":"ConstructPermitNum","SortDic":{"AttendanceTime":"d"},"SearchDic":{"s_1_SegmentAddressArea":"南浔区","s_3_AttendanceTime":"2021-01-18 00:00:01,2021-01-19 23:59:59","s_0_ProjectName|PersonName":"浔适园单元CD-01-01-06地块"}}</param>
        /// <returns></returns>
        [HttpPost("GetListAggregateAsync")]
        public async Task<BaseResultModel<ManagePostAtt>> GetListAggregateAsync([FromBody] dynamic value)
        {
            JObject @object = JObject.Parse(value.ToString());
            ManagePostAttPageSearch search = @object.ToObject<ManagePostAttPageSearch>();
            return await _ManagePostAtt.GetListAggregateAsync(search);
        }
    }
}
