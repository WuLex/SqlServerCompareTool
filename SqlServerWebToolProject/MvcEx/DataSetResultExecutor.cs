using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SqlServerWebToolProject.MvcEx
{
    public class DataSetResultExecutor<T> : IActionResultExecutor<T> where T : DataSetResult
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="result">值</param>
        /// <returns></returns>
        public async Task ExecuteAsync(ActionContext context, T result)
        {
            context.HttpContext.Response.ContentType = "Content-Type: text/html; charset=utf-8";
            //await context.HttpContext.Response.WriteAsync(valueString);

            string html = DataTableHelper.TableToHtml(result._ds.Tables[0]);
            await context.HttpContext.Response.WriteAsync(html);
        }
    }
}