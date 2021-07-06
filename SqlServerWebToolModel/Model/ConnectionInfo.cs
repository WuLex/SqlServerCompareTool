using System;
using System.Collections.Generic;


namespace SqlServerWebToolModel
{
    [Serializable]
    public sealed class ConnectionInfo
    {
        public string ConnectionId;
        public string ServerIP;
        public string UserName;
        public string Password;
        public bool SSPI;
        public int Priority;
    }

    public sealed class ConnectionInfoDataGridJsonResult
    {
        public int total;
        public List<ConnectionInfo> rows;
    }
}