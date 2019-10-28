using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using NLogUsage.CommonUtils;

namespace NLogUsage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();
            try
            {
                using (IServiceScope scope = host.Services.CreateScope())
                {
                    IConfiguration configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                    //获取到appsettings.json中的连接字符串
                    string sqlString = configuration.GetSection("ConectionStrings:MySqlConnection").Value;
                    //确保NLog.config中连接字符串与appsettings.json中同步
                    NLogUtil.EnsureNlogConfig("NLog.config", sqlString);
                }
                //throw new Exception("测试异常");//for test

                //其他项目启动时需要做的事情
                //code
                NLogUtil.WriteDBLog(NLog.LogLevel.Trace, LogType.Web, "网站启动成功");
                host.Run();
            }
            catch (Exception ex)
            {
                //使用nlog写到本地日志文件（万一数据库没创建/连接成功）
                string errorMessage = "网站启动初始化数据异常";
                NLogUtil.WriteFileLog(NLog.LogLevel.Error, LogType.Web, errorMessage, new Exception(errorMessage, ex));
                NLogUtil.WriteDBLog(NLog.LogLevel.Error, LogType.Web, errorMessage, new Exception(errorMessage, ex));
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging(logging => {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                }).UseNLog();  // NLog: 依赖注入Nlog
    }
}
