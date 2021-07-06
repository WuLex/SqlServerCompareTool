using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using SqlServerWebToolModel;


namespace SqlServerSmallToolLib
{
	public static class CompareDBHelper
	{
		public sealed class ThreadParam
		{
			public BaseBLL Instance;
			public string DbName;
			public DbOjbectType DbOjbectType;
			public List<ItemCode> Result;
			public Exception Exception;

			public ThreadParam(BaseBLL instance, string dbName, DbOjbectType type)
			{
				this.Instance = instance;
				this.DbName = dbName;
				this.DbOjbectType = type;
				this.Result = new List<ItemCode>();
			}
		}

		private static void ThreadWorkAction(object obj)
		{
			ThreadParam param = (ThreadParam)obj;
			try {
				param.Result = param.Instance.GetDbAllObjectScript(param.Instance.ConnectionInfo, param.DbName, param.DbOjbectType);
			}
			catch( Exception ex ) {
				param.Exception = ex;
			}
		}

		public static DbOjbectType GetDbOjbectTypeByFlag(string flag)
		{
			if( string.IsNullOrEmpty(flag) )
				return DbOjbectType.None;

			DbOjbectType types = DbOjbectType.None;

			if( flag.IndexOf('T') >= 0 )
				types |= DbOjbectType.Table;
			if( flag.IndexOf('V') >= 0 )
				types |= DbOjbectType.View;
			if( flag.IndexOf('P') >= 0 )
				types |= DbOjbectType.Procedure;
			if( flag.IndexOf('F') >= 0 )
				types |= DbOjbectType.Function;

			return types;
		}

		public static List<CompareResultItem> CompareDB(string srcConnId, string destConnId, string srcDB, string destDB, string flag)
		{
			BaseBLL instance1 = BaseBLL.GetInstance(srcConnId);
			BaseBLL instance2 = BaseBLL.GetInstance(destConnId);
			if( instance1.GetType() != instance2.GetType() )
				throw new Exception("数据库的种类不一致，比较没有意义。");


			DbOjbectType types = GetDbOjbectTypeByFlag(flag);
			ThreadParam param1 = new ThreadParam(instance1, srcDB, types);
			ThreadParam param2 = new ThreadParam(instance2, destDB, types);
			Thread thread1 = new Thread(ThreadWorkAction);
			Thread thread2 = new Thread(ThreadWorkAction);
			thread1.Start(param1);
			thread2.Start(param2);
			thread1.Join();
			thread2.Join();

			if( param1.Exception != null )
				throw param1.Exception;
			if( param2.Exception != null )
				throw param2.Exception;

			List<ItemCode> list1 = param1.Result;
			List<ItemCode> list2 = param2.Result;


			List<CompareResultItem> result = new List<CompareResultItem>();
			ItemCode dest = null;

			// 按数据库对象类别分次比较。
			for( int typeIndex = 0; typeIndex < 4; typeIndex++ ) {
				ItemType currentType = (ItemType)typeIndex;

				foreach( ItemCode item1 in list1 ) {
					// 如果不是当前要比较的对象类别，则跳过。
					if( item1.Type != currentType )
						continue;

					dest = null;
					foreach( ItemCode item2 in list2 ) {
						if( item1.Type == item2.Type && string.Compare(item1.Name, item2.Name, true) == 0 ) {
							dest = item2;
							break;
						}
					}

					if( dest == null ) {
						CompareResultItem cri = new CompareResultItem();
						cri.ObjectType = item1.TypeText;
						cri.ObjectName = item1.Name;
						cri.LineNumber = -1;
						cri.SrcLine = string.Empty;
						cri.DestLine = string.Empty;
						cri.Reason = "源数据库中存在，而目标数据库中不存在。";
						result.Add(cri);
						continue;
					}
					else {
						if( item1.SqlScript == dest.SqlScript )
							continue;

						// 开始比较代码了。
						CompareResultItem cri = null;
						string[] lines1 = instance1.SplitCodeToLineArray(item1.SqlScript);
						string[] lines2 = instance1.SplitCodeToLineArray(dest.SqlScript);
	
						for( int i = 0; i < lines1.Length; i++ ) {
							if( i >= lines2.Length ) {
								// 目标对象的代码行数比较少
								cri = new CompareResultItem();
								cri.ObjectType = item1.TypeText;
								cri.ObjectName = item1.Name;
								cri.LineNumber = i + 1;
								GetNearLines(lines1, lines2, i, cri);
								cri.Reason = "目标对象中已没有对应行数的代码。";
								result.Add(cri);
								break;
							}

							string s1 = lines1[i].Trim();
							string s2 = lines2[i].Trim();
							if( string.Compare(s1, s2, true) != 0 ) {
								cri = new CompareResultItem();
								cri.ObjectType = item1.TypeText;
								cri.ObjectName = item1.Name;
								cri.LineNumber = i + 1;
								GetNearLines(lines1, lines2, i, cri);
								cri.Reason = "代码不一致。";
								result.Add(cri);
								break;
							}
						}

						if( cri != null )
							continue;	// 比较下一个对象

						if( lines2.Length > lines1.Length ) {
							// 目标对象的代码行数比较少
							cri = new CompareResultItem();
							cri.ObjectType = item1.TypeText;
							cri.ObjectName = item1.Name;
							cri.LineNumber = lines1.Length + 1;
							GetNearLines(lines1, lines2, lines1.Length, cri);
							cri.Reason = "源对象中已没有对应行数的代码。";
							result.Add(cri);
							break;
						}
					}
				}


				foreach( ItemCode item2 in list2 ) {
					// 如果不是当前要比较的对象类别，则跳过。
					if( item2.Type != currentType )
						continue;

					dest = null;
					foreach( ItemCode item1 in list1 ) {
						if( item1.Type == item2.Type && string.Compare(item1.Name, item2.Name, true) == 0 ) {
							dest = item2;
							break;
						}
					}

					if( dest == null ) {
						CompareResultItem cri = new CompareResultItem();
						cri.ObjectType = item2.TypeText;
						cri.ObjectName = item2.Name;
						cri.LineNumber = -2;
						cri.SrcLine = string.Empty;
						cri.DestLine = string.Empty;
						cri.Reason = "目标数据库中存在，而源数据库中不存在。";
						result.Add(cri);
						continue;
					}
				}
			}


			return result;
		}


		private static void GetNearLines(string[] lines1, string[] lines2, int index, CompareResultItem cri)
		{
			int firstLine;
			cri.SrcLine = GetOneNearLines(lines1, index, out firstLine);
			cri.SrcFirstLine = firstLine;

			cri.DestLine = GetOneNearLines(lines2, index, out firstLine);
			cri.DestFirstLine = firstLine;
		}

		private static string GetOneNearLines(string[] lines, int index, out int firstLine)
		{
			firstLine = -1;
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			int start = index - 5;
			for( int i = 0; i < 11; i++ )
				if( start + i >= 0 && start + i < lines.Length ) {
					if( firstLine < 0 )
						firstLine = start + i + 1;
					sb.AppendLine(lines[start + i]);
				}

			return sb.ToString();
		}
	}
}
