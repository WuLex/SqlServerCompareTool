using System;
using System.Collections.Generic;
using System.Text;

namespace SqlServerWebToolModel
{
    public sealed class DbItem
    {
        public string Name { get; set; }

        public DbItem(string name)
        {
            this.Name = name;
        }
    }

    public sealed class SVFInfo
    {
        public int total { get; set; }
        public List<DbItem> rows { get; set; }
    }
}