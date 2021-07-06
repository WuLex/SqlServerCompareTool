using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Specialized;

namespace SqlServerSmallToolLib
{
	public static class SmoHelper
	{
		private static readonly string STR_SMO9 = "Microsoft.SqlServer.Smo, Version=9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";
		private static readonly string STR_ConnectionInfo9 = "Microsoft.SqlServer.ConnectionInfo, Version=9.0.242.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

		private static readonly string STR_SMO10 = "Microsoft.SqlServer.Smo, Version=10.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";
		private static readonly string STR_ConnectionInfo10 = "Microsoft.SqlServer.ConnectionInfo, Version=10.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

		private static readonly string STR_ServerConnection = "Microsoft.SqlServer.Management.Common.ServerConnection";
		private static readonly string STR_Server = "Microsoft.SqlServer.Management.Smo.Server";
		private static readonly string STR_Database = "Microsoft.SqlServer.Management.Smo.Database";
		private static readonly string STR_Table = "Microsoft.SqlServer.Management.Smo.Table";
		private static readonly string STR_ScriptingOptions = "Microsoft.SqlServer.Management.Smo.ScriptingOptions";
		private static readonly string STR_DatabaseCollection = "Microsoft.SqlServer.Management.Smo.DatabaseCollection";
		private static readonly string STR_TableCollection = "Microsoft.SqlServer.Management.Smo.TableCollection";


		private static readonly BindingFlags bf_GetProperty = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;
		private static readonly BindingFlags bf_SetProperty = BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public;
		private static readonly BindingFlags bf_Invoke = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public;

		public static string ScriptTable(DbConnection connection, string dbName, string tableName)
		{
			if( connection == null )
				throw new ArgumentNullException("connection");
			if( string.IsNullOrEmpty(tableName) )
				throw new ArgumentNullException("tableName");

			SqlConnection conn = (SqlConnection)connection;
			if( string.IsNullOrEmpty(dbName) )
				dbName = conn.Database;

			if( string.IsNullOrEmpty(dbName) )
				throw new ArgumentNullException("dbName");

			string[] array1 = new string[2] { STR_ConnectionInfo9, STR_ConnectionInfo10 };
			string[] array2 = new string[] { STR_SMO9, STR_SMO10 };

			Type serverType = null;
			Type databaseType = null;
			Type tableType = null;
			Type scriptingOptionsType = null;
			Type databaseCollType = null;
			Type tableCollType = null;
			Type serverConnectionType = null;

			for( int i = 0; i < 2; i++ ) {
				serverConnectionType = Type.GetType(string.Concat(STR_ServerConnection, ", ", array1[i]));
				if( serverConnectionType != null ) {
					serverType = Type.GetType(string.Concat(STR_Server, ", ", array2[i]));
					databaseType = Type.GetType(string.Concat(STR_Database, ", ", array2[i]));
					tableType = Type.GetType(string.Concat(STR_Table, ", ", array2[i]));
					scriptingOptionsType = Type.GetType(string.Concat(STR_ScriptingOptions, ", ", array2[i]));
					databaseCollType = Type.GetType(string.Concat(STR_DatabaseCollection, ", ", array2[i]));
					tableCollType = Type.GetType(string.Concat(STR_TableCollection, ", ", array2[i]));
					break;
				}
			}

			if( serverConnectionType == null || serverType == null || databaseType == null ||
				tableType == null || scriptingOptionsType == null || databaseCollType == null || tableCollType == null )
				throw new Exception("Not found [SQL Server Management Objects] components.");

			object serverConnection = Activator.CreateInstance(serverConnectionType, conn);
			object serverObject = Activator.CreateInstance(serverType, serverConnection);

			object databaseColl = serverType.InvokeMember("Databases", bf_GetProperty, null, serverObject, null);
			object database = databaseCollType.InvokeMember("Item", bf_GetProperty, null, databaseColl, new object[] { dbName });
			object tableColl = databaseType.InvokeMember("Tables", bf_GetProperty, null, database, null);
			object table = tableCollType.InvokeMember("Item", bf_GetProperty, null, tableColl, new object[] { tableName });
			object options = Activator.CreateInstance(scriptingOptionsType);
			scriptingOptionsType.InvokeMember("ClusteredIndexes", bf_SetProperty, null, options, new object[] { true });
			scriptingOptionsType.InvokeMember("Default", bf_SetProperty, null, options, new object[] { true });
			scriptingOptionsType.InvokeMember("DriAll", bf_SetProperty, null, options, new object[] { true });
			scriptingOptionsType.InvokeMember("Indexes", bf_SetProperty, null, options, new object[] { true });

			StringCollection coll = (StringCollection)tableType.InvokeMember("Script", bf_Invoke, null, table, new object[] { options });

			StringBuilder sb = new StringBuilder();
			foreach( string str in coll ) {
				sb.Append(str);
				sb.Append(Environment.NewLine);
			}

			return sb.ToString();
		}
	}
}
