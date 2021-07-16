using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServerWebToolModel;

namespace SqlServerWebToolLib.Interfaces
{
    public interface IConnectionManager
    {
        List<ConnectionInfo> GetList();
        void AddConnection(ConnectionInfo info);
        void RemoveConnection(string ConnectionId);
        void UpdateConnection(ConnectionInfo info);
        ConnectionInfo GetConnectionInfoById(string connectionId, bool increasePriority);
    }
}