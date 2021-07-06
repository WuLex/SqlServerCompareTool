using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Data.Common;
using SqlServerWebToolModel;


namespace SqlServerSmallToolLib
{

	public sealed class SqlServerBLL : BaseBLL
	{
		protected override DbConnection CreateConnection(string connectionString)
		{
			return new System.Data.SqlClient.SqlConnection(connectionString);
		}

		protected override void EnsureMinVersion(DbConnection connection)
		{
			try {
				DbCommand command = connection.CreateCommand();
				command.CommandText = "select (@@microsoftversion / 0x01000000);";
				int version = Convert.ToInt32(command.ExecuteScalar());
				if( version < 9 )
					throw new NotImplementedException();
			}
			catch {
				throw new Exception("本程序只支持 SQL Server 2005 及以上版本。");
			}
		}

		private static readonly string[] s_NewLine = new string[] { "\r\n" };

		public override string[] SplitCodeToLineArray(string code)
		{
			return code.Split(s_NewLine, StringSplitOptions.None);
		}


		#region 字符串定义

		private static readonly string s_GetObjectNamesFormat =
			"select name from ( SELECT obj.name AS [Name],  " +
			"CAST( case when obj.is_ms_shipped = 1 then 1 " +
			"    when ( select major_id from sys.extended_properties  " +
			"        where major_id = obj.object_id and  minor_id = 0 and class = 1 and name = N'microsoft_database_tools_support')  " +
			"        is not null then 1  else 0 " +
			"end  AS bit) AS [IsSystemObject] " +
			"FROM sys.all_objects AS obj where obj.type in ({0}) )as tables where [IsSystemObject] = 0 ORDER BY [Name] ASC ";

		private static readonly string s_ProcedureType = " N'P', N'PC' ";
		private static readonly string s_FunctionType = " N'FN', N'IF', N'TF', N'FS', N'FT' ";
		private static readonly string s_TableType = " N'U' ";
		private static readonly string s_ViewType = " N'V' ";

		private static string GetObjectDbType(ItemType type)
		{
			switch( type ) {
				case ItemType.Procedure:
					return s_ProcedureType;
				case ItemType.Function:
					return s_FunctionType;
				case ItemType.View:
					return s_ViewType;
				default:
					return s_TableType;
			}
		}

		private static readonly string s_QueryDatabaseListScript =
			"SELECT dtb.name AS [Database_Name] FROM master.sys.databases AS dtb " +
			"WHERE (CAST(case when dtb.name in ('master','model','msdb','tempdb') then 1 else dtb.is_distributor end AS bit)=0 " +
			"	and CAST(isnull(dtb.source_database_id, 0) AS bit)=0) " +
			"ORDER BY [Database_Name] ASC";

		private static readonly string s_QueryTableDefinitionScription =
			"SELECT clmns.name AS [Name], " +
			"baset.name AS [DataType], " +
			"CAST(CASE WHEN baset.name IN (N'nchar', N'nvarchar') AND clmns.max_length <> -1 THEN clmns.max_length/2 ELSE clmns.max_length END AS int) AS [Length], " +
			"CAST(clmns.precision AS int) AS [NumericPrecision], " +
			"clmns.is_identity AS [Identity], " +
			"clmns.is_nullable AS [Nullable] " +
			"FROM sys.tables AS tbl " +
			"INNER JOIN sys.all_columns AS clmns ON clmns.object_id=tbl.object_id " +
			"LEFT OUTER JOIN sys.types AS baset ON baset.user_type_id = clmns.system_type_id and baset.user_type_id = baset.system_type_id " +
			"LEFT OUTER JOIN sys.schemas AS sclmns ON sclmns.schema_id = baset.schema_id " +
			"LEFT OUTER JOIN sys.identity_columns AS ic ON ic.object_id = clmns.object_id and ic.column_id = clmns.column_id " +
			"WHERE (tbl.name=N'{0}' ) ORDER BY clmns.column_id ASC";

		private static readonly string s_DeleteObjectFormat =
			"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[{0}]') AND type in ({1})) " +
			"   DROP {2} [{0}] ";

		private static readonly string s_CreateObjectFormat = "EXEC dbo.sp_executesql @statement = N'{0}' ";

		protected override string GetDeleteObjectScript(string name, ItemType type)
		{
			return string.Format(s_DeleteObjectFormat, name, GetObjectDbType(type), type.ToString());
		}

		#endregion


		

		protected override List<string> GetDatabaseNames(DbConnection connection)
		{
			return ExecuteQueryToStringList(connection, s_QueryDatabaseListScript);
		}

		protected override List<string> GetDbProcedureNames(DbConnection connection)
		{
			//string sql = "select name from sys.objects where type='P' order by name";
			string sql = string.Format(s_GetObjectNamesFormat, s_ProcedureType);
			return ExecuteQueryToStringList(connection, sql);
		}

		protected override List<string> GetDbFunctionNames(DbConnection connection)
		{
			//string sql = "select name from sys.objects where type='FN' order by name";
			string sql = string.Format(s_GetObjectNamesFormat, s_FunctionType);
			return ExecuteQueryToStringList(connection, sql);
		}

		protected override List<string> GetDbTableNames(DbConnection connection)
		{
			//string sql = "select name from sys.objects where type='U' where name != 'sysdiagrams' order by name";
			string sql = string.Format(s_GetObjectNamesFormat, s_TableType);
			return ExecuteQueryToStringList(connection, sql);
		}

		protected override List<string> GetDbViewNames(DbConnection connection)
		{
			//string sql = "select name from sys.objects where type='V' order by name";
			string sql = string.Format(s_GetObjectNamesFormat, s_ViewType);
			return ExecuteQueryToStringList(connection, sql);
		}

		

		private static readonly string s_CannotGetScript = "程序没能获取到对象的定义脚本。";
		private static readonly string s_NotFoundSMO = "没有找到SQL Server Management Objects相关组件。";


		private string TryExecuteQuery(DbConnection connection, string query)
		{
			try {
				return ExecuteQueryToString(connection, query) ?? s_CannotGetScript;
			}
			catch( Exception ex ) {
				return ex.Message;
			}
		}


		protected override ItemCode GetProcedureItem(DbConnection connection, string name)
		{
			string query = string.Format("SELECT definition FROM sys.sql_modules JOIN sys.objects ON sys.sql_modules.object_id = sys.objects.object_id AND type in ({1}) and name = '{0}'", name, s_ProcedureType);
			string script = TryExecuteQuery(connection, query);
			return new ItemCode(name, ItemType.Procedure, script);
		}

		protected override ItemCode GetFunctionItem(DbConnection connection, string name)
		{
			string query = string.Format("SELECT definition FROM sys.sql_modules JOIN sys.objects ON sys.sql_modules.object_id = sys.objects.object_id AND type in ({1}) and name = '{0}'", name, s_FunctionType);
			string script = TryExecuteQuery(connection, query);
			return new ItemCode(name, ItemType.Function, script);
		}

		protected override ItemCode GetViewItem(DbConnection connection, string name)
		{
			string query = string.Format("SELECT definition FROM sys.sql_modules JOIN sys.objects ON sys.sql_modules.object_id = sys.objects.object_id AND type in ({1}) and name = '{0}'", name, s_ViewType);
			string script = TryExecuteQuery(connection, query);
			return new ItemCode(name, ItemType.View, script);
		}

		protected override ItemCode GetTableItem(DbConnection connection, string name)
		{
			string script = null;
			try {
				script = SmoHelper.ScriptTable(connection, null, name);
				if( string.IsNullOrEmpty(script) )
					script = s_CannotGetScript;
			}
			catch( Exception ex ) {
				script = ex.Message;
			}
			return new ItemCode(name, ItemType.Table, script);
		}


		protected override DataTable GetTableFields(DbConnection connection, string tableName)
		{
			DbCommand command = connection.CreateCommand();
			command.CommandText = string.Format(s_QueryTableDefinitionScription, tableName);

			using( DbDataReader reader = command.ExecuteReader() ) {
				DataTable table = new DataTable(tableName);
				table.Load(reader);
				return table;
			}
		}


		public override string UpdateProcedures(ConnectionInfo info, string dbName, List<ItemCode> list)
		{
			string connectionString = GetDbConnectionString(info, dbName);

			using( DbConnection connection = CreateConnection(connectionString) ) {
				connection.Open();
				DbCommand command = connection.CreateCommand();
				
				foreach(ItemCode item in list) {
					command.CommandText = GetDeleteObjectScript(item.Name, item.Type);
					command.ExecuteNonQuery();

					command.CommandText = string.Format(s_CreateObjectFormat, item.SqlScript.Replace("'", "''"));
					command.ExecuteNonQuery();
				}
			}

			return string.Format("操作成功，共复制了 {0} 个对象。", list.Count);
		}






		protected override void DeleteSelectedItemsInternal(DbConnection connection, 
			string[] tblNames, string[] spNames, string[] viewNames, string[] funcNames)
		{
			DbCommand command = connection.CreateCommand();

			if( tblNames != null && tblNames.Length > 0 ) {
				foreach( string name in tblNames ) {
					command.CommandText = GetDeleteObjectScript(name, ItemType.Table);
					command.ExecuteNonQuery();
				}
			}

			if( spNames != null && spNames.Length > 0 ) {
				foreach( string name in spNames ) {
					command.CommandText = GetDeleteObjectScript(name, ItemType.Procedure);
					command.ExecuteNonQuery();
				}
			}

			if( funcNames != null && funcNames.Length > 0 ) {
				foreach( string name in funcNames ) {
					command.CommandText = GetDeleteObjectScript(name, ItemType.Function);
					command.ExecuteNonQuery();
				}
			}

			if( viewNames != null && viewNames.Length > 0 ) {
				foreach( string name in viewNames ) {
					command.CommandText = GetDeleteObjectScript(name, ItemType.View);
					command.ExecuteNonQuery();
				}
			}
		}

		
	}
}
