using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Learn.WinServices
{
    public static class Helper
    {
        /// <summary>
        /// 保存日志信息
        /// </summary>
        /// <param name="msg"></param>
        public static void SaveLog(string msg)
        {
            string path = "";
            //如果日志路径为空，则默认为当前应用程序的根路径
            if (null == System.Configuration.ConfigurationManager.AppSettings["LogPath"])
            {
                path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                path += "\\log";
            }
            else
            {
                path = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
            }
            //判断文件夹是否存在
            if (!Directory.Exists(path))//判断目录是否存在{}
            { Directory.CreateDirectory(path); }
            string filepath = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            FileStream fs = new FileStream(filepath, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine("\r" + msg + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sw.Close();
            fs.Close();
        }
    }
}
