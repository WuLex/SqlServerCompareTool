using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using SqlServerWebToolLib.BLL;
using SqlServerWebToolLib.Exceptions;
using SqlServerWebToolLib.Helpers;
using SqlServerWebToolLib.Interfaces;
using SqlServerWebToolModel;

namespace SqlServerWebToolProject.Controllers
{
    public class AjaxServiceController : Controller
    {
        public readonly IConnectionManager _connectionManager;

        public AjaxServiceController(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public IActionResult GetTreeNodes(string connectionId)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            var result = instance.GetTreeNodes(instance.ConnectionInfo);
            return Json(result);
        }

        public IActionResult GetDataBaseChildren(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            string[] array = id.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length != 2)
                throw new ArgumentException("id is error.");

            return GetTreeNodesByDbName(array[1], array[0]);
        }

        public IActionResult GetTreeNodesByDbName(string connectionId, string dbName)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            var result = instance.GetTreeNodesByDbName(instance.ConnectionInfo, dbName);
            return Json(result);
        }

        public IActionResult GetDbSpViewFuncList(string connectionId, string dbName)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            var result = instance.GetDbSpViewFuncList(instance.ConnectionInfo, dbName);
            return Json(result);
        }

        public string GetStoreProcedureCode(string connectionId, string dbName, string spName)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            return instance.GetStoreProcedureCode(instance.ConnectionInfo, dbName, spName);
        }

        public string GetFunctionCode(string connectionId, string dbName, string funcName)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            return instance.GetFunctionCode(instance.ConnectionInfo, dbName, funcName);
        }

        public string GetViewCode(string connectionId, string dbName, string viewName)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            return instance.GetViewCode(instance.ConnectionInfo, dbName, viewName);
        }

        public IActionResult GetSelectedItemCode(string connectionId,
            string dbName, string tblNames, string spNames, string viewNames, string funcNames)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            var result =
                instance.GetSelectedItemCode(instance.ConnectionInfo, dbName, tblNames, spNames, viewNames, funcNames);
            return Json(result);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult GetDbList(string connectionId)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            var result = instance.GetDbList(instance.ConnectionInfo);
            return Json(result);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult GetAllConnectionInfo()
        {
            List<ConnectionInfo> list = _connectionManager.GetList();

            ConnectionInfoDataGridJsonResult result = new ConnectionInfoDataGridJsonResult();
            result.total = list.Count;
            result.rows = list;

            return Json(result);
            //var Res = new JsonResult(result);
            //return Res;
            //return new JsonResult(result);
            //return Json(new {name = "wu", age = 12});
        }


        [HttpPost]
        //[Consumes("application/x-www-form-urlencoded")]
        //public string SubmitConnectionInfo([FromForm]ConnectionInfo ddd )
        public string SubmitConnectionInfo(string ServerIP, bool SSPI, string UserName, string Password,
            string ConnectionId)
        {
            //IFormCollection 

            ConnectionInfo info = new ConnectionInfo()
            {
                ServerIP = ServerIP,
                SSPI = SSPI,
                UserName = UserName,
                Password = Password,
                ConnectionId = ConnectionId,
            };


            if (string.IsNullOrEmpty(info.ServerIP))
                throw new MyMessageException("ServerIP is empty.");

            if (info.SSPI == false && string.IsNullOrEmpty(info.UserName))
                throw new MyMessageException("UserName is empty.");

            bool isAdd = string.IsNullOrEmpty(info.ConnectionId);

            if (isAdd)
            {
                info.ConnectionId = Guid.NewGuid().ToString();
                _connectionManager.AddConnection(info);
                return info.ConnectionId;
            }
            else
            {
                _connectionManager.UpdateConnection(info);
                return "update OK";
            }
        }

        public void DeleteConnection(string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
                throw new MyMessageException("connectionId is empty.");

            _connectionManager.RemoveConnection(connectionId);
        }

        public string TestConnection(ConnectionInfo info)
        {
            BaseBLL instance = BaseBLL.GetInstance(null);
            return instance.TestConnection(info);
        }

        public IActionResult GetConnectionInfoByURL(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            int p = url.IndexOf('?');
            if (p > 0)
                url = url.Substring(p + 1);

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(url);

            string connectionId = queryString["id"];

            ConnectionInfo info = _connectionManager.GetConnectionInfoById(connectionId, true);
            return Json(info);
        }

        //[Action]
        //public IActionResult CompareDB(string srcConnId, string destConnId, string srcDB, string destDB, string flag)
        //{
        //    var result = CompareDBHelper.CompareDB(srcConnId, destConnId, srcDB, destDB, flag);
        //    return Json(result);
        //}

        public string CopyProcedures(string srcConnId, string destConnId, string srcDB, string destDB,
            string spNames, string viewNames, string funcNames)
        {
            BaseBLL instance1 = BaseBLL.GetInstance(srcConnId);
            BaseBLL instance2 = BaseBLL.GetInstance(destConnId);
            if (instance1.GetType() != instance2.GetType())
                throw new Exception("数据库的种类不一致，不能执行复制操作。");

            if (srcConnId == destConnId && srcDB == destDB)
                throw new Exception("无效的操作。");

            List<ItemCode> procedures =
                instance1.GetDbAllObjectScript(instance1.ConnectionInfo, srcDB, spNames, viewNames, funcNames);
            return instance2.UpdateProcedures(instance2.ConnectionInfo, destDB, procedures);
        }

        public void DeleteSelectedItems(string connectionId,
            string dbName, string tblNames, string spNames, string viewNames, string funcNames)
        {
            BaseBLL instance = BaseBLL.GetInstance(connectionId);
            instance.DeleteSelectedItems(instance.ConnectionInfo, dbName, tblNames, spNames, viewNames, funcNames);
        }

        public IActionResult SearchDB(string connectionId, string dbName, string searchWord,
            int wholeMatch, int caseSensitive, string searchScope, string limitCount)
        {
            if (string.IsNullOrEmpty(searchWord))
                throw new ArgumentNullException("searchWord");

            BaseBLL instance = BaseBLL.GetInstance(connectionId);

            DbOjbectType types = CompareDBHelper.GetDbOjbectTypeByFlag(searchScope);
            List<ItemCode> list = instance.GetDbAllObjectScript(instance.ConnectionInfo, dbName, types);

            List<SearchResultItem> result = new List<SearchResultItem>(list.Count);

            int limitResultCount = 0;
            int.TryParse(limitCount, out limitResultCount);

            StringSearcher searcher =
                StringSearcher.GetStringSearcher(searchWord, (wholeMatch == 1), (caseSensitive == 1));

            foreach (ItemCode code in list)
            {
                if (limitResultCount != 0 && result.Count >= limitResultCount)
                    break;

                if (code.SqlScript.IndexOf(searchWord, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string[] lines = instance.SplitCodeToLineArray(code.SqlScript);
                    for (int i = 0; i < lines.Length; i++)
                        if (searcher.IsMatch(lines[i]))
                        {
                            SearchResultItem item = new SearchResultItem();
                            item.LineNumber = i + 1;
                            item.ObjectName = code.Name;
                            item.ObjectType = code.Type.ToString();
                            item.SqlScript = code.SqlScript;
                            result.Add(item);
                            break;
                        }
                }
            }

            return Json(result);
        }

        public IActionResult CompareDB(CompareDbOption option)
        {
            var result = CompareDBHelper.CompareDB(option.SrcConnId, option.DestConnId,
                option.SrcDb, option.DestDb, option.Flag);
            return Json(result);
        }
    }

    public class CompareDbOption
    {
        public string SrcConnId;
        public string DestConnId;
        public string SrcDb;
        public string DestDb;
        public string Flag;
    }
}