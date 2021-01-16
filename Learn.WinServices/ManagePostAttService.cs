using Learn.Models.Business;
using Learn.Models.Common;
using Learn.Models.Entity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Learn.WinServices
{
    public partial class ManagePostAttService : ServiceBase
    {
        public ManagePostAttService()
        {
            InitializeComponent();
        }

        System.Threading.Timer timer;
        string url = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"];//Api地址
        string addressArea = System.Configuration.ConfigurationManager.AppSettings["AddressArea"];//区域
        bool defaultRule = System.Configuration.ConfigurationManager.AppSettings["DefaultRule"] == "true";//是否默认规则
        int limit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Limit"]);//默认条数
        protected override void OnStart(string[] args)
        {
            //读取配置文件中的间隔时间
            string timeSpan = ConfigurationManager.AppSettings["ExecuteTimeSpan"];
            //默认一分钟执行一次
            int _time = string.IsNullOrEmpty(timeSpan) ? 60 : Convert.ToInt32(timeSpan);
            long interval = 1000 * _time;
            timer = new System.Threading.Timer(new TimerCallback(timer_Elapsed), null, 0, interval);
        }

        private void timer_Elapsed(object state)
        {
            ToDo();
        }

        protected override void OnStop()
        {

        }

        void ToDo()
        {
            //1、判断考勤记录是否考勤规定时间内的第一条记录，是的话就写入
            //2、不是第一条的话判断是否为规定时间内的并且是否满足间隔时间
            //3、将满足以上条件的考勤数据放在一个集合中，等待批量添加
            //   将不满足以上条件的考勤数据放一个集合中，等待批量更新历史状态
            //4、批量添加和更新（符合规则的List及不符合规则的List）历史文档中的数据交换状态和时间（建议3、4组成事务）

            string reString = this.Post(url, addressArea, defaultRule, limit);
        }

        //ByPassAttAsync(string addressArea = "", bool defaultRule = true, int limit = 100)

        string Post(string url, string addressArea = "", bool defaultRule = true, int limit = 50)
        {
            HttpClient httpClient = new HttpClient();
            List<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();

            param.Add(new KeyValuePair<string, string>("addressArea", addressArea));
            param.Add(new KeyValuePair<string, string>("defaultRule", defaultRule.ToString()));
            param.Add(new KeyValuePair<string, string>("limit", limit.ToString()));
            string paramStr = Newtonsoft.Json.JsonConvert.SerializeObject(param);
            Task<HttpResponseMessage> responseMessage = httpClient.PostAsync(url, new FormUrlEncodedContent(param));

            responseMessage.Wait();
            Task<string> reString = responseMessage.Result.Content.ReadAsStringAsync();
            reString.Wait();
            return reString.Result;
        }

    }
}
