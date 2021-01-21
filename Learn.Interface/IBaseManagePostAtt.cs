using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Interface
{
    /// <summary>
    /// 管理岗位考勤数据接口【泛型接口】
    /// </summary>
    /// <typeparam name="T">历史考勤表（全部考勤数据）</typeparam>
    /// <typeparam name="S">考勤表（符合规则的考勤数据）</typeparam>
    public interface IBaseManagePostAtt<T, S>
        where T : ManagePostAtt
        where S : ManagePostAtt
    {
        /// <summary>
        /// 新增多条数据[异步]
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task AddManyAsync(IEnumerable<T> list);
        /// <summary>
        /// 分页查询列表[异步]
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">条数</param>
        /// <param name="sortDic">排序字典值</param>
        /// <param name="searchDic">查询条件字典值</param>
        /// <returns></returns>
        Task<BaseResultModel<T>> GetPageManagePostAtt(int pageIndex, int pageSize, Dictionary<string, string> sortDic, Dictionary<string, string> searchDic);
        /// <summary>
        /// 分页查询列表[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<BaseResultModel<T>> GetPageManagePostAtt(ManagePostAttPageSearch search);

        /// <summary>
        /// 获取管理人员考勤统计数据[异步]
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ManagePostAttStatistics> GetManagePostStatistics(ManagePostAttPageSearch search);

        ///// <summary>
        ///// 根据管道获取相关数据
        ///// </summary>
        ///// <param name="pageIndex"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="type"></param>
        ///// <param name="today"></param>
        ///// <returns></returns>
        /// <summary>
        /// 根据管道获取相关数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<BaseResultModel<T>> GetListAggregateAsync(ManagePostAttPageSearch search);

        /// <summary>
        /// 分流符合考勤规则的数据
        /// </summary>
        /// <param name="addressArea">项目所属地</param>
        /// <param name="defaultRule">是否启用默认的考勤规则</param>
        /// <param name="limit">取Top多少条数据</param>
        void ByPassAttAsync(string addressArea = "", bool defaultRule = true, int limit = 100);

    }
}
