using System;
using System.Collections.Generic;
using System.Web;


namespace SqlServerWebToolModel
{
    public sealed class JsTreeNode
    {
        public string id { get; set; }
        public string text { get; set; }
        public string state;
        public List<JsTreeNode> children { get; set; }
        public JsTreeNodeCustAttr attributes { get; set; }
    }


    public sealed class JsTreeNodeCustAttr
    {
        public string NodeFlag { get; set; }

        public JsTreeNodeCustAttr(string flag)
        {
            this.NodeFlag = flag;
        }
    }

    public sealed class GetTreeNodesResult
    {
        public List<JsTreeNode> dbList { get; set; }
        public string ErrorMessage { get; set; }
    }
}