using System;
using System.Collections.Generic;


namespace SqlServerWebToolModel
{
    [Serializable]
    public sealed class ConnectionInfo
    {
        public string ConnectionId { get; set; }
        public string ServerIP { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool SSPI { get; set; }
        public int Priority { get; set; }
    }

    public sealed class ConnectionInfoDataGridJsonResult
    {
        public int total { get; set; }
        public List<ConnectionInfo> rows { get; set; }
    }
}