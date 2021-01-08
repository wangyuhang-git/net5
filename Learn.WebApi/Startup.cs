using Autofac;
using AutoMapper;
using Learn.Business.ManagePositionAtt;
using Learn.Business.Student;
using Learn.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Learn.WebApi", Version = "v1" });
            });

            //services.AddSession();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //注册瞬时服务
            //services.AddTransient<IStudent, StudentBusiness>();
            services.AddTransient<IStudent, StudentBusinessEx>();
            services.Replace(ServiceDescriptor.Singleton<IStudent, StudentBusiness>());

            //注册管理岗位人员考勤服务[工程模式]
            services.AddTransient<IManagePostHistoryAtt>(serviceProvider =>
            {
                //serviceProvider.GetService(typeof(ManagePostHistoryAttBusiness));
                return new ManagePostHistoryAttBusiness();
            });

            //services.AddTransient<IStudent>(serviceProvider =>
            //{
            //    Console.WriteLine(serviceProvider.GetService(typeof(StudentBusiness)));
            //    return new StudentBusiness();
            //});

        }

        public void ConfigureContainer(ContainerBuilder builer)
        {

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Learn.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
