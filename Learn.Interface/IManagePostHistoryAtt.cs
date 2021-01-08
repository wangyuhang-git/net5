using Learn.Models.Business;
using Learn.Models.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    /// <summary>
    /// 管理岗位人员历史考勤数据
    /// </summary>
    public interface IManagePostHistoryAtt
    {
        /// <summary>
        /// 新增多条数据[异步]
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task AddManyAsync(IEnumerable<ManagePostHistoryAtt> list);
        /// <summary>
        /// 分页查询列表[异步]
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="sortDic">排序字典值</param>
        /// <param name="searchDic">查询条件字典值</param>
        /// <returns></returns>
        Task<IEnumerable<ManagePostHistoryAtt>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic);
        /// <summary>
        /// 分页查询列表[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<IEnumerable<ManagePostHistoryAtt>> GetPageManagePostAtt(ManagePostAttPageSearch search);
    }
}
