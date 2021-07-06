using System;
using System.Collections.Generic;
using System.Web;


namespace SqlServerWebToolModel
{
    public sealed class ItemCode
    {
        public ItemCode()
        {
        }

        public ItemCode(string name, ItemType type, string sql)
        {
            this.Name = name;
            this.Type = type;
            this.SqlScript = sql;
        }

        public string Name;
        public string SqlScript;
        public ItemType Type;

        public string TypeText
        {
            get { return Type.ToString(); }
        }
    }


    public enum ItemType
    {
        Table,
        Procedure,
        View,
        Function
    }


    [Flags]
    public enum DbOjbectType
    {
        None = 0,
        Table = 1,
        Procedure = 2,
        View = 4,
        Function = 8,
        All = Table + Procedure + View + Function
    }
}