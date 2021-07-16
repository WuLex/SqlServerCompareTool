#pragma checksum "D:\mynetcoreproject\SqlServerWebTool\SqlServerWebToolProject\Views\Manager\CopyProcedure.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4639901f34bc4f0e7646b2e1134c8b37c56a07be"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Manager_CopyProcedure), @"mvc.1.0.view", @"/Views/Manager/CopyProcedure.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\mynetcoreproject\SqlServerWebTool\SqlServerWebToolProject\Views\_ViewImports.cshtml"
using SqlServerWebToolProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\mynetcoreproject\SqlServerWebTool\SqlServerWebToolProject\Views\_ViewImports.cshtml"
using SqlServerWebToolProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4639901f34bc4f0e7646b2e1134c8b37c56a07be", @"/Views/Manager/CopyProcedure.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0ed1d9939d19c8c63fef2ca9af7cbd6c20467ca4", @"/Views/_ViewImports.cshtml")]
    public class Views_Manager_CopyProcedure : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("easyui-layout"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\mynetcoreproject\SqlServerWebTool\SqlServerWebToolProject\Views\Manager\CopyProcedure.cshtml"
  
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4639901f34bc4f0e7646b2e1134c8b37c56a07be3897", async() => {
                WriteLiteral(@"
    <title>复制存储过程工具 - SqlServerSmallTool - http://www.cnblogs.com/fish-li</title>
    <link type=""text/css"" rel=""Stylesheet"" href=""/js/jquery/jquery-easyui-1.2/themes/default/easyui.css""/>
    <link type=""text/css"" rel=""Stylesheet"" href=""/js/jquery/jquery-easyui-1.2/themes/icon.css""/>
    <link type=""text/css"" rel=""Stylesheet"" href=""/css/StyleSheet.css""/>
    <script type=""text/javascript"" src=""/js/jquery/jquery-1.4.4.min.js""></script>
");
                WriteLiteral(@"    <script type=""text/javascript"" src=""/js/jquery/jquery-easyui-1.2/jquery.easyui.min.js""></script>
    <script type=""text/javascript"" src=""/js/jquery/jquery-easyui-1.2/easyui-lang-zh_CN.js""></script>

    <script type=""text/javascript"" src=""/js/my/fish.js""></script>
    <script type=""text/javascript"" src=""/js/my/CopyProcedure.js""></script>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "4639901f34bc4f0e7646b2e1134c8b37c56a07be5730", async() => {
                WriteLiteral(@"


<div region=""north"" split=""true"" style=""height: 100px; overflow: hidden;"">
    <div class=""easyui-layout"" fit=""true"" border=""false"">

        <div region=""west"" style=""width: 850px; overflow: hidden; padding: 3px 10px; background-color: #D2E9FF;"" split=""true"" title=""选择数据库"">
            <table cellpadding=""3"" cellspacing=""0"" style=""border: 0px;"" id=""tblChoiceConn"">
                <tr>
                    <td>源</td>
                    <td>连接帐号</td>
                    <td style=""width: 320px"">
                        <input type=""text"" id=""txtSrcConn"" class=""myTextbox"" style=""width: 300px""/>
                    </td>
                    <td>数据库</td>
                    <td style=""width: 220px"">
                        <select id=""cboSrcDB"" style=""width: 200px""></select>
                    </td>
                    <td>
                        <a href=""javascript:void(0)"" id=""btnRefresh"" class=""easyui-linkbutton"" iconCls=""icon-reload"">刷新列表</a>
                    </td>
                ");
                WriteLiteral(@"</tr>
                <tr>
                    <td>目标</td>
                    <td>连接帐号</td>
                    <td>
                        <input type=""text"" id=""txtDestConn"" class=""myTextbox"" style=""width: 300px""/>
                    </td>
                    <td>数据库</td>
                    <td>
                        <select id=""cboDestDB"" style=""width: 200px""></select>
                    </td>
                    <td>
                        <a href=""javascript:void(0)"" id=""btnRun"" class=""easyui-linkbutton"" iconCls=""icon-Run"">开始复制</a>
                    </td>
                </tr>
            </table>
            <input type=""hidden"" id=""hfSrcConnId""");
                BeginWriteAttribute("value", " value=\"", 2684, "\"", 2692, 0);
                EndWriteAttribute();
                WriteLiteral("/>\r\n            <input type=\"hidden\" id=\"hfDestConnId\"");
                BeginWriteAttribute("value", " value=\"", 2747, "\"", 2755, 0);
                EndWriteAttribute();
                WriteLiteral(@"/>
        </div>

        <div region=""center"" title=""运行消息"" id=""divExecuteResult"" style=""overflow: scroll; padding: 5px; background-color: #D2E9FF;"" split=""true"">
        </div>

    </div>
</div>

<div region=""center"">

    <div class=""easyui-layout"" fit=""true"" border=""false"">
        <div region=""west"" split=""true"" style=""width: 430px;"" title=""源数据库存储过程（只读）"">
            <table id=""tblSpList""></table>
        </div>

        <div region=""center"" split=""true"" title=""源数据库视图（只读）"">
            <table id=""tblViewList""></table>
        </div>

        <div region=""east"" split=""true"" style=""width: 330px;"" title=""源数据库自定义函数（只读）"">
            <table id=""tblFuncList""></table>
        </div>

    </div>

</div>

<div class=""waitMessageStyle"">
    <img src=""Images/progress_loading.gif""/><span>请稍后......</span>
</div>

");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
