using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using SqlServerWebToolLib.Exceptions;
using SqlServerWebToolModel;

namespace SqlServerWebToolLib.BLL
{
    public abstract class BaseBLL
    {
        public static BaseBLL GetInstance(string connectionId)
        {
            BaseBLL instance = new SqlServerBLL();

            if (string.IsNullOrEmpty(connectionId) == false)
                instance.ConnectionInfo = new ConnectionManager().GetConnectionInfoById(connectionId, false);

            return instance;
        }

        public ConnectionInfo ConnectionInfo { get; private set; }

        public virtual void Init()
        {
        }

        protected abstract DbConnection CreateConnection(string connectionString);

        protected virtual void EnsureMinVersion(DbConnection connection)
        {
        }

        public abstract string[] SplitCodeToLineArray(string code);

        protected abstract List<string> GetDatabaseNames(DbConnection connection);

        protected abstract List<string> GetDbProcedureNames(DbConnection connection);

        protected abstract List<string> GetDbFunctionNames(DbConnection connection);

        protected abstract List<string> GetDbTableNames(DbConnection connection);

        protected abstract List<string> GetDbViewNames(DbConnection connection);

        protected abstract ItemCode GetProcedureItem(DbConnection connection, string name);

        protected abstract ItemCode GetFunctionItem(DbConnection connection, string name);

        protected abstract ItemCode GetViewItem(DbConnection connection, string name);

        protected abstract ItemCode GetTableItem(DbConnection connection, string name);

        protected abstract DataTable GetTableFields(DbConnection connection, string tableName);

        public abstract string UpdateProcedures(ConnectionInfo info, string dbName, List<ItemCode> list);

        protected abstract string GetDeleteObjectScript(string name, ItemType type);

        protected List<string> ExecuteQueryToStringList(DbConnection connection, string query)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;

            using (DbDataReader reader = command.ExecuteReader())
            {
                List<string> list = new List<string>();

                while (reader.Read())
                    list.Add(reader.GetString(0));

                return list;
            }
        }

        protected string ExecuteQueryToString(DbConnection connection, string query)
        {
            DbCommand command = connection.CreateCommand();
            command.CommandText = query;

            object result = command.ExecuteScalar();

            if (result == null || DBNull.Value.Equals(result))
                return null;
            else
                return result.ToString();
        }

        public string TestConnection(ConnectionInfo info)
        {
            try
            {
                string connString = GetDbConnectionString(info);
                using (DbConnection connection = CreateConnection(connString))
                {
                    connection.Open();
                    EnsureMinVersion(connection);
                    return "ok";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        protected string GetDbConnectionString(ConnectionInfo info)
        {
            if (info.SSPI)
                return string.Format("server={0};integrated security=SSPI", info.ServerIP);
            else
                return string.Format("server={0};user id={1};password={2}", info.ServerIP, info.UserName,
                    info.Password);
        }

        protected string GetDbConnectionString(ConnectionInfo info, string dbName)
        {
            if (string.IsNullOrEmpty(dbName))
                throw new MyMessageException("dbName is empty.");

            if (info.SSPI)
                return string.Format("server={0};database={1};integrated security=SSPI", info.ServerIP, dbName);
            else
                return string.Format("server={0};user id={1};password={2};database={3}", info.ServerIP, info.UserName,
                    info.Password, dbName);
        }


        public GetTreeNodesResult GetTreeNodes(ConnectionInfo info)
        {
            GetTreeNodesResult result = new GetTreeNodesResult();
            List<string> databases = null;
            string connString = GetDbConnectionString(info);

            using (DbConnection connection = CreateConnection(connString))
            {
                connection.Open();
                EnsureMinVersion(connection);
                databases = GetDatabaseNames(connection);
            }

            if (databases.Count == 0)
                return result;

            StringBuilder sb = new StringBuilder();
            List<JsTreeNode> dbList = new List<JsTreeNode>();

            foreach (string dbname in databases)
            {
                try
                {
                    // 数据库节点
                    JsTreeNode root = new JsTreeNode();
                    root.text = dbname;
                    root.state = "closed";
                    root.attributes = new JsTreeNodeCustAttr("root");

                    root.id = dbname + ";" + info.ConnectionId;

                    // 2012-03-04, 加载数据库节点时，不加载数据库的下层节点。
                    //root.children = GetTreeNodesByDbName(info, dbname);

                    dbList.Add(root);
                }
                catch (Exception ex)
                {
                    sb.AppendLine(string.Concat(System.Web.HttpUtility.HtmlEncode(ex.Message), "<br />"));
                }
            }

            result.dbList = dbList;
            result.ErrorMessage = sb.ToString();
            return result;
        }

        public List<JsTreeNode> GetTreeNodesByDbName(ConnectionInfo info, string dbname)
        {
            List<JsTreeNode> list = new List<JsTreeNode>();

            string connectionString = GetDbConnectionString(info, dbname);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                //=============================================================================
                // 获取所有表名
                JsTreeNode tables = new JsTreeNode();
                tables.state = "closed";
                tables.attributes = new JsTreeNodeCustAttr("Tables");
                tables.children = new List<JsTreeNode>();

                List<string> tblNameList = GetDbTableNames(connection);
                tables.text = string.Format("Tables({0})", tblNameList.Count);

                foreach (string tblName in tblNameList)
                {
                    JsTreeNode node = new JsTreeNode();
                    node.text = tblName;
                    node.attributes = new JsTreeNodeCustAttr("tbl");
                    tables.children.Add(node);
                }

                list.Add(tables);

                //=============================================================================
                // 获取所有视图名
                JsTreeNode views = new JsTreeNode();
                views.state = "closed";
                views.attributes = new JsTreeNodeCustAttr("Views");
                views.children = new List<JsTreeNode>();

                List<string> viewNameList = GetDbViewNames(connection);
                views.text = string.Format("Views({0})", viewNameList.Count);

                foreach (string viewName in viewNameList)
                {
                    JsTreeNode node = new JsTreeNode();
                    node.text = viewName;
                    node.attributes = new JsTreeNodeCustAttr("view");
                    views.children.Add(node);
                }

                list.Add(views);


                //=============================================================================
                // 获取所有存储过程名
                JsTreeNode procedures = new JsTreeNode();
                procedures.state = "closed";
                procedures.attributes = new JsTreeNodeCustAttr("Procedures");
                procedures.children = new List<JsTreeNode>();

                List<string> spNameList = GetDbProcedureNames(connection);
                procedures.text = string.Format("Procedures({0})", spNameList.Count);

                foreach (string spname in spNameList)
                {
                    JsTreeNode node = new JsTreeNode();
                    node.text = spname;
                    node.attributes = new JsTreeNodeCustAttr("sp");
                    procedures.children.Add(node);
                }

                list.Add(procedures);

                //=============================================================================
                // 获取所有自定义函数名
                JsTreeNode functions = new JsTreeNode();
                functions.state = "closed";
                functions.attributes = new JsTreeNodeCustAttr("Functions");
                functions.children = new List<JsTreeNode>();

                List<string> funcNameList = GetDbFunctionNames(connection);
                functions.text = string.Format("Functions({0})", funcNameList.Count);

                foreach (string funcname in funcNameList)
                {
                    JsTreeNode node = new JsTreeNode();
                    node.text = funcname;
                    node.attributes = new JsTreeNodeCustAttr("func");
                    functions.children.Add(node);
                }

                list.Add(functions);
            }

            return list;
        }

        public SVFInfo[] GetDbSpViewFuncList(ConnectionInfo info, string dbName)
        {
            List<string> spNameList = null;
            List<string> viewNameList = null;
            List<string> funcNameList = null;

            string connectionString = GetDbConnectionString(info, dbName);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                spNameList = GetDbProcedureNames(connection);
                viewNameList = GetDbViewNames(connection);
                funcNameList = GetDbFunctionNames(connection);
            }

            SVFInfo[] result = new SVFInfo[3];

            //------------------------------------------------------------------
            List<DbItem> list1 = new List<DbItem>(spNameList.Count);
            foreach (string name in spNameList)
                list1.Add(new DbItem(name));

            result[0] = new SVFInfo();
            result[0].total = list1.Count;
            result[0].rows = list1;


            //------------------------------------------------------------------
            List<DbItem> list2 = new List<DbItem>(viewNameList.Count);
            foreach (string name in viewNameList)
                list2.Add(new DbItem(name));

            result[1] = new SVFInfo();
            result[1].total = list2.Count;
            result[1].rows = list2;

            //------------------------------------------------------------------
            List<DbItem> list3 = new List<DbItem>(funcNameList.Count);
            foreach (string name in funcNameList)
                list3.Add(new DbItem(name));

            result[2] = new SVFInfo();
            result[2].total = list3.Count;
            result[2].rows = list3;

            return result;
        }

        public string GetStoreProcedureCode(ConnectionInfo info, string dbName, string spName)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName");

            string connectionString = GetDbConnectionString(info, dbName);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                ItemCode item = GetProcedureItem(connection, spName);
                return (item == null ? "[StoreProcedure not found.]" : item.SqlScript);
            }
        }

        public string GetFunctionCode(ConnectionInfo info, string dbName, string funcName)
        {
            if (string.IsNullOrEmpty(funcName))
                throw new ArgumentNullException("funcName");

            string connectionString = GetDbConnectionString(info, dbName);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                ItemCode item = GetFunctionItem(connection, funcName);
                return (item == null ? "[Function not found.]" : item.SqlScript);
            }
        }

        public string GetViewCode(ConnectionInfo info, string dbName, string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
                throw new ArgumentNullException("viewName");

            string connectionString = GetDbConnectionString(info, dbName);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                ItemCode item = GetViewItem(connection, viewName);
                return (item == null ? "[View not found.]" : item.SqlScript);
            }
        }

        public DataTable GetTableFields(ConnectionInfo info, string dbName, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            string connectionString = GetDbConnectionString(info, dbName);

            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                return GetTableFields(connection, tableName);
            }
        }

        public DataSet GetTables(ConnectionInfo info, string dbName, string tableNames)
        {
            if (string.IsNullOrEmpty(tableNames))
                throw new ArgumentNullException("tableNames");

            string[] tblNameArray = tableNames.Split(new char[] {';', ','}, StringSplitOptions.RemoveEmptyEntries);
            DataSet ds = new DataSet();

            string connectionString = GetDbConnectionString(info, dbName);
            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                foreach (string tableName in tblNameArray)
                {
                    DataTable table = GetTableFields(connection, tableName);
                    ds.Tables.Add(table);
                }
            }

            return ds;
        }

        public List<ItemCode> GetSelectedItemCode(ConnectionInfo info,
            string dbName, string tblNames, string spNames, string viewNames, string funcNames)
        {
            string[] tableNameArray = null;
            if (string.IsNullOrEmpty(tblNames) == false)
                tableNameArray = tblNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string[] spNameArray = null;
            if (string.IsNullOrEmpty(spNames) == false)
                spNameArray = spNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string[] funcNameArray = null;
            if (string.IsNullOrEmpty(funcNames) == false)
                funcNameArray = funcNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string[] viewNameArray = null;
            if (string.IsNullOrEmpty(viewNames) == false)
                viewNameArray = viewNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            List<ItemCode> list = new List<ItemCode>();
            string connectionString = GetDbConnectionString(info, dbName);

            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                if (tableNameArray != null && tableNameArray.Length > 0)
                {
                    foreach (string name in tableNameArray)
                        try
                        {
                            list.Add(GetTableItem(connection, name));
                        }
                        catch
                        {
                        }
                }

                if (spNameArray != null && spNameArray.Length > 0)
                {
                    foreach (string name in spNameArray)
                        try
                        {
                            list.Add(GetProcedureItem(connection, name));
                        }
                        catch
                        {
                        }
                }

                if (viewNameArray != null && viewNameArray.Length > 0)
                {
                    foreach (string name in viewNameArray)
                        try
                        {
                            list.Add(GetViewItem(connection, name));
                        }
                        catch
                        {
                        }
                }

                if (funcNameArray != null && funcNameArray.Length > 0)
                {
                    foreach (string name in funcNameArray)
                        try
                        {
                            list.Add(GetFunctionItem(connection, name));
                        }
                        catch
                        {
                        }
                }
            }

            return list;
        }

        public List<DbItem> GetDbList(ConnectionInfo info)
        {
            List<string> databases = null;
            string connectionString = GetDbConnectionString(info);

            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                EnsureMinVersion(connection);
                databases = GetDatabaseNames(connection);
            }

            if (databases.Count == 0)
                return new List<DbItem>();


            List<DbItem> list = new List<DbItem>(databases.Count);
            foreach (string name in databases)
                list.Add(new DbItem(name));

            return list;
        }


        public List<ItemCode> GetDbAllObjectScript(ConnectionInfo info, string dbName, DbOjbectType type)
        {
            List<ItemCode> list = new List<ItemCode>();
            string connectionString = GetDbConnectionString(info, dbName);

            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                if ((type & DbOjbectType.Table) == DbOjbectType.Table)
                {
                    List<string> nameList = GetDbTableNames(connection);
                    foreach (string name in nameList)
                        list.Add(GetTableItem(connection, name));
                }

                if ((type & DbOjbectType.Procedure) == DbOjbectType.Procedure)
                {
                    List<string> nameList = GetDbProcedureNames(connection);
                    foreach (string name in nameList)
                        list.Add(GetProcedureItem(connection, name));
                }

                if ((type & DbOjbectType.Function) == DbOjbectType.Function)
                {
                    List<string> nameList = GetDbFunctionNames(connection);
                    foreach (string name in nameList)
                        list.Add(GetFunctionItem(connection, name));
                }

                if ((type & DbOjbectType.View) == DbOjbectType.View)
                {
                    List<string> nameList = GetDbViewNames(connection);
                    foreach (string name in nameList)
                        list.Add(GetViewItem(connection, name));
                }
            }

            return list;
        }

        public List<ItemCode> GetDbAllObjectScript(ConnectionInfo info,
            string dbName, string spNames, string viewNames, string funcNames)
        {
            List<ItemCode> list = new List<ItemCode>();
            string connectionString = GetDbConnectionString(info, dbName);

            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();

                if (string.IsNullOrEmpty(spNames) == false)
                {
                    foreach (string name in spNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                        list.Add(GetProcedureItem(connection, name));
                }

                if (string.IsNullOrEmpty(funcNames) == false)
                {
                    foreach (string name in funcNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                        list.Add(GetFunctionItem(connection, name));
                }

                if (string.IsNullOrEmpty(viewNames) == false)
                {
                    foreach (string name in viewNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                        list.Add(GetViewItem(connection, name));
                }
            }

            return list;
        }


        public void DeleteSelectedItems(ConnectionInfo info,
            string dbName, string tblNames, string spNames, string viewNames, string funcNames)
        {
            string[] tableNameArray = null;
            if (string.IsNullOrEmpty(tblNames) == false)
                tableNameArray = tblNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string[] spNameArray = null;
            if (string.IsNullOrEmpty(spNames) == false)
                spNameArray = spNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string[] funcNameArray = null;
            if (string.IsNullOrEmpty(funcNames) == false)
                funcNameArray = funcNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string[] viewNameArray = null;
            if (string.IsNullOrEmpty(viewNames) == false)
                viewNameArray = viewNames.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            string connectionString = GetDbConnectionString(info, dbName);

            using (DbConnection connection = CreateConnection(connectionString))
            {
                connection.Open();
                DeleteSelectedItemsInternal(connection, tableNameArray, spNameArray, viewNameArray, funcNameArray);
            }
        }


        protected abstract void DeleteSelectedItemsInternal(DbConnection connection,
            string[] tblNames, string[] spNames, string[] viewNames, string[] funcNames);
    }
}