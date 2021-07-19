using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SqlServerWebToolProject.MvcEx
{
    public class DataTableResult : IActionResult
    {
        public DataTable _table;

        public DataTableResult(DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            _table = table;
        }

        //void IActionResult.Ouput(HttpContext context)
        //{
        //	context.Response.ContentType = "text/html";
        //	string html = DataTableHelper.TableToHtml(_table);
        //	context.Response.Write(html);
        //}

        public async Task  ExecuteResultAsync(ActionContext context)
        {

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<DataTableResult>>();
            await executor.ExecuteAsync(context, this);
            //context.HttpContext.Response.ContentType = "text/html";
            //context.Response.ContentType = "text/html";
            //string html = DataTableHelper.TableToHtml(_table);
            //context.Response.Write(html);
        }
    }

    public class DataSetResult : IActionResult
    {
        public DataSet _ds;

        public DataSetResult(DataSet ds)
        {
            if( ds == null )
                throw new ArgumentNullException("ds");

            _ds = ds;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<DataSetResult>>();
            return executor.ExecuteAsync(context, this);
        }

        //void IActionResult.Ouput(HttpContext context)
        //{
        //    List<DataSetJsonItem> list = new List<DataSetJsonItem>();

        //    for( int i = 0; i < _ds.Tables.Count; i++ ) {
        //        DataTable table = _ds.Tables[i];
        //        string html = DataTableHelper.TableToHtml(table);
        //        list.Add(new DataSetJsonItem { TableName = table.TableName, Html = html });
        //    }

        //    JsonResult json = Json(list);
        //    (json as IActionResult).Ouput(context);
        //}
        
    }
    

     



    
    public class DataSetJsonItem
    {
        public string TableName;
        public string Html;
    }

    public static class DataTableHelper
    {
        public static string TableToHtml(DataTable table)
        {
            if( table == null )
                throw new ArgumentNullException("table");

            StringBuilder html = new StringBuilder();
            html.AppendLine("<table cellpadding=\"2\" cellspacing=\"1\" class=\"myGridVew\"><thead><tr>");

            for( int i = 0; i < table.Columns.Count; i++ )
                html.AppendFormat("<th>{0}</th>", HttpUtility.HtmlEncode(table.Columns[i].ColumnName));

            html.AppendLine("</tr></thead><tbody>");

            object cell = null;
            for( int j = 0; j < table.Rows.Count; j++ ) {
                html.AppendLine("<tr>");

                for( int i = 0; i < table.Columns.Count; i++ ) {
                    cell = table.Rows[j][i];
                    if( cell == null || DBNull.Value.Equals(cell) )
                        html.Append("<td></td>");
                    else
                        html.AppendFormat("<td>{0}</td>", HttpUtility.HtmlEncode(cell.ToString()));
                }
                html.AppendLine("</tr>");
            }

            html.AppendLine("</tbody></table>");
            return html.ToString();
        }
    }

}
