using AutoMapper;
using Learn.Common;
using Learn.Interface;
using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using Microsoft.Extensions.Options;
using Mongodb.Service;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Business.ManagePositionAtt
{
    /// <summary>
    /// 管理岗位人员历史考勤数据操作【泛型接口实现类】
    /// </summary>
    /// <typeparam name="T">历史考勤表（全部考勤数据）</typeparam>
    /// <typeparam name="S">考勤表（符合规则的考勤数据）</typeparam>
    public class ManagePostAttBusiness<T, S> : IBaseManagePostAtt<T, S>
        where T : ManagePostAtt
        where S : ManagePostAtt
    {
        private readonly IMapper _mapper;
        private readonly IOptions<List<ManagePostAttRule>> _options;
        public ManagePostAttBusiness(IMapper mapper, IOptions<List<ManagePostAttRule>> options)
        {
            this._mapper = mapper;
            this._options = options;
        }
        MongodbService<T> service = new MongodbService<T>(typeof(T).Name);
        MongodbService<S> serviceS = new MongodbService<S>(typeof(S).Name);
        MongodbBsonService mongodbBsonService = new MongodbBsonService(typeof(T).Name);
        WhereHelper<T> whereHelper = new WhereHelper<T>();
        WhereHelper<S> whereHelperS = new WhereHelper<S>();
        /// <summary>
        /// 批量增加管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task AddManyAsync(IEnumerable<T> list)
        {
            await service.AddManyAsync(list);
        }

        /// <summary>
        /// 分页获取管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortDic"></param>
        /// <param name="searchDic"></param>
        /// <returns></returns>
        public async Task<BaseResultModel<T>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic)
        {
            Expression<Func<T, bool>> expression = whereHelper.GetExpression(searchDic);
            return await service.GetPageListAsync(pageIndex, pageSize, sortDic, expression);
        }
        /// <summary>
        /// 分页获取管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<BaseResultModel<T>> GetPageManagePostAtt(ManagePostAttPageSearch search)
        {
            Dictionary<string, string> sortDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SortDic.ToString());
            Dictionary<string, string> searchDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SearchDic.ToString());
            return await this.GetPageManagePostAtt(search.PageIndex, search.PageSize, sortDic, searchDic);
        }

        /// <summary>
        /// 获取管理岗位统计数据，包含总考勤项目数、今日考勤项目数、今日考勤人数[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<ManagePostAttStatistics> GetManagePostStatistics(ManagePostAttPageSearch search)
        {
            ManagePostAttStatistics managePostAttStatistics = new ManagePostAttStatistics();
            Dictionary<string, string> searchDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SearchDic.ToString());

            Dictionary<string, string> searchDicTotal = null;

            searchDicTotal = new Dictionary<string, string>();

            if (!searchDic.ContainsKey("s_3_AttendanceTime"))
            {
                searchDic.Add("s_3_AttendanceTime", $"{DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:01"},{DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"}");
            }

            if (searchDic.ContainsKey("s_1_SegmentAddressArea"))
            {
                searchDicTotal.Add("s_1_SegmentAddressArea", searchDic["s_1_SegmentAddressArea"]);
            }

            Expression<Func<T, bool>> expression = whereHelper.GetExpression(searchDic);
            managePostAttStatistics.AttProjectTodayCount = await service.GetDistinctCountAsync(expression, "ConstructPermitNum");
            managePostAttStatistics.AttPersonTodayCount = await service.GetDistinctCountAsync(expression, "IdCard");

            expression = whereHelper.GetExpression(searchDicTotal);
            //whereHelper.GetExpression(searchDicTotal)
            managePostAttStatistics.AttProjectTotal = await service.GetDistinctCountAsync(expression, "ConstructPermitNum");

            return managePostAttStatistics;
        }

        /// <summary>
        /// 根据管道获取数据[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<BaseResultModel<T>> GetListAggregateAsync(ManagePostAttPageSearch search)
        {
            Dictionary<string, string> searchDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SearchDic.ToString());
            Dictionary<string, string> sortDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SortDic.ToString());

            PipeLineBsonHelper pipeLineBsonHelper = new PipeLineBsonHelper();
            List<string> pipeLinelist = new List<string>();

            #region 过滤（条件查询）
            pipeLineBsonHelper.AddMatchList(searchDic, pipeLinelist);
            #endregion

            #region 分组(用 $last排序，后续改为属性映射独立方法 )

            if (string.IsNullOrEmpty(search.GroupField))
            {
                throw new ArgumentNullException($"分组参数{nameof(search.GroupField)}不能为空！");
            }

            string group = "{'" + search.GroupField + "':'$" + search.GroupField + "'}";
            string pipelineGroup = @"{$group:
                    {
                        _id:" + group + @",
                        'id':{ '$first':'$_id'},
                        'ConstructPermitNum':{ '$first' :'$ConstructPermitNum'},
                        'ProjectName':{ '$last' :'$ProjectName'},
                        'ProjectGuid':{ '$last' :'$ProjectGuid'},
                        'Company':{ '$last':'$Company'},
                        'OrganizationCode':{ '$last':'$OrganizationCode'},
                        'CorpType':{ '$last':'$CorpType'},
                        'SegmentAddressArea':{ '$last':'$SegmentAddressArea'}, 
                        'AttendanceTime':{ '$last':'$AttendanceTime'},
                        'AttendanceId':{ '$last':'$AttendanceId'},
                        'PersonGUID':{ '$last':'$PersonGUID'},
                        'PersonName':{ '$last':'$PersonName'},
                        'IdCard':{ '$last':'$IdCard'},
                        'PostType':{ '$last':'$PostType'},
                        'ImageBuffer':{ '$last':'$ImageBuffer'},
                        'SupervisionDepartment':{ '$last':'$SupervisionDepartment'},
                        'SupervisionDepartmentGUID':{ '$last':'$SupervisionDepartmentGUID'},
                        'CreateTime':{ '$last':'$CreateTime'}
                }
            }";
            pipeLinelist.Add(pipelineGroup);
            #endregion

            #region 总数（根据条件查询总数）
            string pipelineCount = "{$count:'total'}";
            pipeLinelist.Add(pipelineCount);

            IList<IPipelineStageDefinition> stageList = new List<IPipelineStageDefinition>();
            foreach (string item in pipeLinelist)
            {
                PipelineStageDefinition<BsonDocument, BsonDocument> stageGroup = new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(item);
                stageList.Add(stageGroup);
            }
            BaseResultModel<T> baseResultModel = new BaseResultModel<T>();
            BsonDocument bsonDocument = await mongodbBsonService.GetAggregateAsync(stageList);
            if (null != bsonDocument)
            {
                baseResultModel = BsonSerializer.Deserialize<BaseResultModel<T>>(bsonDocument);
            }
            else
            {
                baseResultModel.total = 0;
                baseResultModel.rows = new List<T>();
                baseResultModel.success = false;
                return baseResultModel;
            }
            #endregion

            //移除总数
            pipeLinelist.Clear();
            stageList.RemoveAt(stageList.Count - 1);

            #region 排序
            pipeLineBsonHelper.AddSortList(sortDic, pipeLinelist);
            #endregion

            #region 分页
            string pipelineSkip = "{$skip:" + ((search.PageIndex - 1) * search.PageSize) + "}";
            string pipelineLimit = "{$limit:" + search.PageSize + "}";
            pipeLinelist.Add(pipelineSkip);
            pipeLinelist.Add(pipelineLimit);
            #endregion

            #region 映射
            string pipelineProject = "{$project:{'_id':0,'id':0}}";
            pipeLinelist.Add(pipelineProject);
            #endregion

            foreach (string item in pipeLinelist)
            {
                PipelineStageDefinition<BsonDocument, BsonDocument> stageGroup = new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(item);
                stageList.Add(stageGroup);
            }
            //baseResultModel.rows = await mongodbBsonService.GetListAggregateAsync(stageList);
            List<T> list = new List<T>();
            foreach (var item in await mongodbBsonService.GetListAggregateAsync(stageList))
            {
                var d = BsonSerializer.Deserialize<BsonDocument>(item);
                list.Add(BsonSerializer.Deserialize<T>(item));
            }
            baseResultModel.rows = list;
            baseResultModel.success = true;
            return baseResultModel;
        }

        /// <summary>
        /// 根据考勤规则进行分流数据，符合条件的放在考勤表中，并将所有的数据更新状态
        /// </summary>
        /// <param name="addressArea"></param>
        /// <param name="defaultRule"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public void ByPassAttAsync(string addressArea = "", bool defaultRule = true, int limit = 100)
        {
            var sortDic = new Dictionary<string, string>();
            sortDic.Add("AttendanceTime", "a");
            var searchDic = new Dictionary<string, string>();
            searchDic.Add("s_1_Status", "0");//状态是0的（未交换）
            if (!string.IsNullOrEmpty(addressArea))
            {
                searchDic.Add("s_1_SegmentAddressArea", addressArea);
            }
            Expression<Func<T, bool>> expression = whereHelper.GetExpression(searchDic);
            List<T> alllist = service.GetList(limit, sortDic, expression);

            ManagePostAttRuleBusiness managePostAttRuleBusiness = new ManagePostAttRuleBusiness(this._options);

            //默认使用缺省考勤规则
            ManagePostAttRule rule = managePostAttRuleBusiness.GetPostAttRule();
            string amStart, amEnd, pmStart, pmEnd;
            long IntervalHours;
            DateTime attendanceTime;
            ////需要新增到考勤表的集合
            //List<S> inRuleList = new List<S>();
            alllist.ForEach((item) =>
            {
                item.StatusTime = DateTime.Now;
                if (!defaultRule)
                    rule = managePostAttRuleBusiness.GetPostAttRule(item.CorpType, item.PostType);
                amStart = rule.AmStart; amEnd = rule.AmEnd; pmStart = rule.PmStart; pmEnd = rule.PmEnd;
                IntervalHours = rule.IntervalHours;

                attendanceTime = item.AttendanceTime;
                //1、判断是否符合考勤时间段规则
                if (
                (attendanceTime >= Convert.ToDateTime($"{attendanceTime.ToString("yyyy-MM-dd")} {amStart}")
                && attendanceTime <= Convert.ToDateTime($"{attendanceTime.ToString("yyyy-MM-dd")} {amEnd}"))
                || (attendanceTime >= Convert.ToDateTime($"{attendanceTime.ToString("yyyy-MM-dd")} {pmStart}")
                && attendanceTime >= Convert.ToDateTime($"{attendanceTime.ToString("yyyy-MM-dd")} {pmStart}"))
                )
                {
                    //2、判断间隔小时是否满足
                    sortDic = new Dictionary<string, string>();
                    sortDic.Add("AttendanceTime", "d");
                    searchDic = new Dictionary<string, string>();
                    searchDic.Add("s_1_PersonGUID", item.PersonGUID);

                    if (attendanceTime.AddHours(-IntervalHours) < Convert.ToDateTime($"{attendanceTime.ToString("yyyy-MM-dd")} {amStart}"))
                    {
                        searchDic.Add("s_3_AttendanceTime", $"{attendanceTime.ToString("yyyy-MM-dd")} {amStart},{attendanceTime}");
                    }
                    else
                    {
                        searchDic.Add("s_3_AttendanceTime", $"{attendanceTime.AddHours(-IntervalHours)},{attendanceTime}");
                    }
                    var oldList = serviceS.GetList(1, sortDic, whereHelperS.GetExpression(searchDic));
                    if (null != oldList && oldList.Count > 0)//两个小时内有考勤数据，更新历史表状态为-1
                    {
                        item.Status = -1;
                        service.Update(item.Id, item);
                    }
                    else//新增到考勤表，并更新历史表状态为1
                    {
                        item.Status = 1;
                        service.Update(item.Id, item);
                        //inRuleList.Add(item);
                        S s = _mapper.Map<S>(item);
                        serviceS.Add(s);
                    }
                }
                else//更新历史表状态为-2
                {
                    item.Status = -2;
                    service.Update(item.Id, item);
                }
            });
        }
    }
}
