using Autofac;
using AutoMapper;
using Learn.Business.ManagePositionAtt;
using Learn.Business.Student;
using Learn.Interface;
using Learn.Models.Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

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
            //跨域配置
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    string[] cors = Configuration.GetSection("AllowedCors").Value.Split(new char[] { ',', '|' });
                    //Console.WriteLine(Configuration.GetSection("AllowedCors").Value);
                    builder
                    .WithOrigins(cors)
                    //.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();//指定处理cookie
                });
            });

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

            services.AddOptions().Configure<List<ManagePostAttRule>>(Configuration.GetSection("AttendanceRules"));

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

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ManagePostAttBusiness<,>)).As(typeof(IBaseManagePostAtt<,>));//默认为瞬时模式InstancePerDependency

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

            //启用跨域
            app.UseCors("AllowSpecificOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
