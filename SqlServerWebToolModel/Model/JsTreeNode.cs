using System;
using System.Collections.Generic;
using System.Web;


namespace SqlServerWebToolModel
{
    public sealed class JsTreeNode
    {
        public string id;
        public string text;
        public string state;
        public List<JsTreeNode> children;
        public JsTreeNodeCustAttr attributes;
    }


    public sealed class JsTreeNodeCustAttr
    {
        public string NodeFlag;

        public JsTreeNodeCustAttr(string flag)
        {
            this.NodeFlag = flag;
        }
    }

    public sealed class GetTreeNodesResult
    {
        public List<JsTreeNode> dbList;
        public string ErrorMessage;
    }
}