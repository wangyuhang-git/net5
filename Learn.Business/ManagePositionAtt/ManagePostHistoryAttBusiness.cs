using Learn.Common;
using Learn.Interface;
using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using Mongodb.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Business.ManagePositionAtt
{
    /// <summary>
    /// 管理岗位人员历史考勤数据操作【作废，改为了泛型接口实现类】
    /// </summary>
    public class ManagePostHistoryAttBusiness : IManagePostHistoryAtt
    {
        MongodbService<ManagePostHistoryAtt> service = new MongodbService<ManagePostHistoryAtt>("ManagePostHistoryAtt");
        WhereHelper<ManagePostHistoryAtt> whereHelper = new WhereHelper<ManagePostHistoryAtt>();
        /// <summary>
        /// 批量增加管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task AddManyAsync(IEnumerable<ManagePostHistoryAtt> list)
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
        public async Task<BaseResultModel<ManagePostHistoryAtt>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic)
        {
            Expression<Func<ManagePostHistoryAtt, bool>> expression = whereHelper.GetExpression(searchDic);
            return await service.GetPageListAsync(pageIndex, pageSize, sortDic, expression);
        }
        /// <summary>
        /// 分页获取管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<BaseResultModel<ManagePostHistoryAtt>> GetPageManagePostAtt(ManagePostAttPageSearch search)
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

            Expression<Func<ManagePostHistoryAtt, bool>> expression = whereHelper.GetExpression(searchDic);
            managePostAttStatistics.AttProjectTodayCount = await service.GetDistinctCountAsync(expression, "ConstructPermitNum");
            managePostAttStatistics.AttPersonTodayCount = await service.GetDistinctCountAsync(expression, "IdCard");

            expression = whereHelper.GetExpression(searchDicTotal);
            //whereHelper.GetExpression(searchDicTotal)
            managePostAttStatistics.AttProjectTotal = await service.GetDistinctCountAsync(expression, "ConstructPermitNum");

            return managePostAttStatistics;
        }
    }

}
