using Learn.Common;
using Learn.Interface;
using Learn.Models.Business;
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
    /// 管理岗位人员历史考勤数据操作
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
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManagePostHistoryAtt>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic)
        {
            Expression<Func<ManagePostHistoryAtt, bool>> expression = whereHelper.GetExpression(searchDic);
            return await service.GetPageListAsync(pageIndex, pageSize, sortDic, expression);
        }
        /// <summary>
        /// 分页获取管理岗位人员考勤数据[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManagePostHistoryAtt>> GetPageManagePostAtt(ManagePostAttPageSearch search)
        {
            Dictionary<string, string> sortDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SortDic.ToString());
            Dictionary<string, string> searchDic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(search.SearchDic.ToString());
            return await this.GetPageManagePostAtt(search.PageIndex, search.PageSize, sortDic, searchDic);
        }
    }

}
