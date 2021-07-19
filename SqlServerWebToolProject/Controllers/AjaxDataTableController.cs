using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using SqlServerWebToolLib.BLL;
using SqlServerWebToolProject.MvcEx;

namespace SqlServerWebToolProject.Controllers
{
	public class AjaxDataTableController : Controller
	{
		
		public IActionResult TableDescribe(string connectionId, string dbName, string tableName)
		{
			if( string.IsNullOrEmpty(connectionId) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(tableName) )
				throw new ArgumentException("connString or tableName is null.");


			BaseBLL instance = BaseBLL.GetInstance(connectionId);
			DataTable table = instance.GetTableFields(instance.ConnectionInfo, dbName, tableName);
			return new DataTableResult(table);
		}

		public IActionResult MultiTableDescribe(string connectionId, string dbName, string tableNames)
		{
			if( string.IsNullOrEmpty(connectionId) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(tableNames) )
				throw new ArgumentException("connString or tableName is null.");


			BaseBLL instance = BaseBLL.GetInstance(connectionId);
			DataSet ds = instance.GetTables(instance.ConnectionInfo, dbName, tableNames);
			return new DataSetResult(ds);
		}
	}
}
