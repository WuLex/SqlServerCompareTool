using System;
using System.Collections.Generic;
using System.Web;

namespace SqlServerWebToolModel
{
    public sealed class CompareResultItem
    {
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public int LineNumber { get; set; }
        public string SrcLine { get; set; }
        public string DestLine { get; set; }
        public string Reason { get; set; }
        public int SrcFirstLine { get; set; }
        public int DestFirstLine { get; set; }
    }

    public sealed class SearchResultItem
    {
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public int LineNumber { get; set; }
        public string SqlScript { get; set; }
    }
}