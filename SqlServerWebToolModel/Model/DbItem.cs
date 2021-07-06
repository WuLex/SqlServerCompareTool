using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerWebToolModel
{
    public sealed class DbItem
    {
        public string Name;

        public DbItem(string name)
        {
            this.Name = name;
        }
    }


    public sealed class SVFInfo
    {
        public int total;
        public List<DbItem> rows;
    }
}