using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using RwConfigDemo;
using SqlServerWebToolModel;
using Microsoft.AspNetCore.Hosting;
using SqlServerWebToolLib.Interfaces;

namespace SqlServerSmallToolLib
{
    public class ConnectionManager : IConnectionManager
    {
        private List<ConnectionInfo> s_list = null;
        private readonly Encoding DefaultEncoding = System.Text.Encoding.Unicode;
        private readonly string s_savePath; //= Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\Connection.xml");
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <inheritdoc />
        public ConnectionManager(IHostingEnvironment env)
        {
            try
            {
                _hostingEnvironment = env;

                string appDataPath = Path.Combine(_hostingEnvironment.WebRootPath, "App_Data");
                s_savePath = Path.Combine(_hostingEnvironment.ContentRootPath, @"App_Data\Connection.xml");
                if (Directory.Exists(appDataPath) == false)
                    Directory.CreateDirectory(appDataPath);
            }
            catch
            {
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<ConnectionInfo> GetList()
        {
            EnsureListLoaded();

            // 调用这个方法应该会比“修改”的次数会少很多，所以决定在这里排序。
            return (from c in s_list orderby c.Priority descending select c).ToList();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddConnection(ConnectionInfo info)
        {
            EnsureListLoaded();

            s_list.Add(info);
            SaveListToFile();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveConnection(string ConnectionId)
        {
            EnsureListLoaded();

            int index = -1;
            for (int i = 0; i < s_list.Count; i++)
                if (s_list[i].ConnectionId == ConnectionId)
                {
                    index = i;
                    break;
                }

            if (index >= 0)
            {
                s_list.RemoveAt(index);
                SaveListToFile();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateConnection(ConnectionInfo info)
        {
            EnsureListLoaded();

            ConnectionInfo exist = s_list.FirstOrDefault(x => x.ConnectionId == info.ConnectionId);

            if (exist != null)
            {
                exist.ServerIP = info.ServerIP;
                exist.UserName = info.UserName;
                exist.Password = info.Password;
                exist.SSPI = info.SSPI;
                // 注意：其它没列出的成员，表示不需要在此更新。
                SaveListToFile();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ConnectionInfo GetConnectionInfoById(string connectionId, bool increasePriority)
        {
            if (string.IsNullOrEmpty(connectionId))
                throw new ArgumentNullException("connectionId");

            EnsureListLoaded();

            ConnectionInfo exist = s_list.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (exist == null)
                throw new MyMessageException("connectionId is invalid.");

            if (increasePriority)
            {
                exist.Priority++;
                SaveListToFile();
            }

            return exist;
        }


        private void EnsureListLoaded()
        {
            if (s_list == null)
            {
                try
                {
                    s_list = XmlHelper.XmlDeserializeFromFile<List<ConnectionInfo>>(s_savePath, DefaultEncoding);
                }
                catch
                {
                    s_list = new List<ConnectionInfo>();
                }
            }
        }

        private void SaveListToFile()
        {
            if (s_list == null || s_list.Count == 0)
            {
                try
                {
                    File.Delete(s_savePath);
                }
                catch
                {
                }
            }
            else
            {
                XmlHelper.XmlSerializeToFile(s_list, s_savePath, DefaultEncoding);
            }
        }
    }
}