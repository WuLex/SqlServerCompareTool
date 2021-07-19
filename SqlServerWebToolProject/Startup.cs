using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlServerWebToolProject.MvcEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlServerWebToolLib.BLL;
using SqlServerWebToolLib.Interfaces;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using SqlServerWebToolLib.Helpers;

namespace SqlServerWebToolProject
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
            services.AddScoped<IActionResultExecutor<DataTableResult>, DataTableResultExecutor<DataTableResult>>();
            services.AddScoped<IActionResultExecutor<DataSetResult>, DataSetResultExecutor<DataSetResult>>();
            services.AddScoped<IConnectionManager, ConnectionManager>();
            services.AddMvc().AddJsonOptions(options =>
            {

                //��ʽ������ʱ���ʽ
                options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                //���ݸ�ʽ����ĸСд
                //options.JsonSerializerOptions.PropertyNamingPolicy =JsonNamingPolicy.CamelCase;
                //���ݸ�ʽԭ�����
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                //ȡ��Unicode����
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                //���Կ�ֵ
                options.JsonSerializerOptions.IgnoreNullValues = true;
                //����������
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                //�����л����������������Ƿ�ʹ�ò����ִ�Сд�ıȽ�
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;

            }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
