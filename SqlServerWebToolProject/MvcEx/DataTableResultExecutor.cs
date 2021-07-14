using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SqlServerWebToolProject.MvcEx
{
    public class DataTableResultExecutor<T> : IActionResultExecutor<T> where T : DataTableResult
    {
        public async Task ExecuteAsync(ActionContext context, T result)
        {
            context.HttpContext.Response.ContentType = "Content-Type: text/html; charset=utf-8";
            //await context.HttpContext.Response.WriteAsync(valueString);

            string html = DataTableHelper.TableToHtml(result._table);
            await context.HttpContext.Response.WriteAsync(html);
        }
    }
}
