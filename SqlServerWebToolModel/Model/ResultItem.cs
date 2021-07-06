using System;
using System.Collections.Generic;
using System.Web;

namespace SqlServerWebToolModel
{
    public sealed class CompareResultItem
    {
        public string ObjectType;
        public string ObjectName;
        public int LineNumber;
        public string SrcLine;
        public string DestLine;
        public string Reason;
        public int SrcFirstLine;
        public int DestFirstLine;
    }

    public sealed class SearchResultItem
    {
        public string ObjectType;
        public string ObjectName;
        public int LineNumber;
        public string SqlScript;
    }
}