#pragma checksum "D:\mynetcoreproject\SqlServerWebTool\SqlServerWebToolProject\Views\Manager\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Manager_Index), @"mvc.1.0.view", @"/Views/Manager/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf", @"/Views/Manager/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0ed1d9939d19c8c63fef2ca9af7cbd6c20467ca4", @"/Views/_ViewImports.cshtml")]
    public class Views_Manager_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/jquery/jquery.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "false", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("value", "true", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("formConnection"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("/AjaxService/SubmitConnectionInfo"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("easyui-layout"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "D:\mynetcoreproject\SqlServerWebTool\SqlServerWebToolProject\Views\Manager\Index.cshtml"
  
    Layout = null;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf6740", async() => {
                WriteLiteral(@"
    <title>SqlServerSmallTool - http://www.cnblogs.com/fish-li</title>
    <link type=""text/css"" rel=""Stylesheet"" href=""/js/jquery/jquery-easyui-1.9/themes/default/easyui.css""/>
    <link type=""text/css"" rel=""Stylesheet"" href=""/js/jquery/jquery-easyui-1.9/themes/icon.css""/>
    <link type=""text/css"" rel=""Stylesheet"" href=""/css/StyleSheet.css""/>
");
                WriteLiteral("    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf7405", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    <script type=""text/javascript"" src=""/js/jquery/jquery.form.js""></script>
    <script type=""text/javascript"" src=""/js/jquery/jquery.cookie.js""></script>
    <script type=""text/javascript"" src=""/js/jquery/jquery-easyui-1.9/jquery.easyui.min.js""></script>
    <script type=""text/javascript"" src=""/js/jquery/jquery-easyui-1.9/easyui-lang-zh_CN.js""></script>

    <script type=""text/javascript"" src=""/js/my/fish.js""></script>
    <script type=""text/javascript"" src=""/js/my/Home.js""></script>
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
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf9820", async() => {
                WriteLiteral(@"
<div region=""west"" title=""工具列表"" split=""true"" style=""width: 450px; overflow: hidden"">
    <div id=""divToolList"" class=""easyui-accordion"" fit=""true"">
        <div title=""比较数据库"" iconcls=""icon-Copy2"" style=""overflow: auto; padding: 10px;"" selected=""true"">
            <h3 class=""blueText"">
                “比较数据库”工具能做什么？
            </h3>
            <p>
                此工具可以比较二个数据库的表结构定义、存储过程代码是否一致。
            </p>
            <p>
                说明：如果要比较SQL Server的数据库，请确保机器中已安装过SQL Server Management Objects (2005/2008)相关组件 （通常在安装SQL
                Server 或 SQL Server Management Studio或其它SQL Server工具时会自动安装），否则将不会比较数据表结构的差异。
            </p>
            <p>
                <a href=""CompareDB.htm"" target=""_blank"" class=""easyui-linkbutton"" iconcls=""icon-Run"">
                    运行此工具
                </a>
            </p>
        </div>
        <div title=""复制存储过程"" iconcls=""icon-reload"" style=""overflow: auto; padding: 10px;"">
            <h3 class=""blueText"">
                “复制存储过程”工具能做什么？
 ");
                WriteLiteral(@"           </h3>
            <p>
                此工具可以将一个数据库的所有存储过程、视图、用户函数复制到另一个数据库。
            </p>
            <p>
                如果目标数据库中存在同名的存储过程，将会被覆盖，没有的存储过程将会被创建。
            </p>
            <p>
                <a href=""/Manager/CopyProcedure"" target=""_blank"" class=""easyui-linkbutton"" iconcls=""icon-Run"">
                    运行此工具
                </a>
            </p>
        </div>
    </div>
</div>
<div region=""center"" style=""overflow: hidden;"" title=""数据库连接列表"" iconcls=""icon-Relation"">
    <div class=""easyui-layout"" fit=""true"" border=""false"">
        <div region=""center"">
            <table id=""tblConnList"">
            </table>
        </div>
        <div region=""south"" split=""true"" style=""height: 220px; padding: 10px;"" title=""操作说明""
             iconcls=""icon-help"">
            <p>
                1. “新增连接”，“删除连接”，“设置连接”用于维护连接记录。
            </p>
            <p>
                2. “打开连接”将根据选择的连接，打开 Database 对象浏览页面。
            </p>
        </div>
    </div>
</div>
<");
                WriteLiteral("div id=\"divConnectionDialog\" style=\"padding: 10px; width: 420px; height: 320px; display: none;\" title=\"新增/编辑 数据库连接信息\">\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf12402", async() => {
                    WriteLiteral(@"
        <table cellpadding=""4"" cellspacing=""0"" style=""width: 100%; border: 0px;"">
            <tr>
                <td>
                    服务器IP/Name
                </td>
                <td>
                    <input id=""txtServerIP"" name=""ServerIP"" type=""text"" class=""myTextbox"" style=""width: 220px""/>
                </td>
            </tr>
            <tr>
                <td>
                    登录方式
                </td>
                <td>
                    <select id=""cboSSPI"" name=""SSPI"" style=""width: 222px"" panelwidth=""222"">
                        ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf13276", async() => {
                        WriteLiteral("用户名／密码");
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_2.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                        ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e4b4f44232a4d8e9bfac736af58ca7fffc94e2cf14588", async() => {
                        WriteLiteral("Windows连接");
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = (string)__tagHelperAttribute_3.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral(@"
                    </select>
                </td>
            </tr>
            <tr>
                <td>
                    登录名
                </td>
                <td>
                    <input id=""txtUserName"" name=""UserName"" type=""text"" class=""myTextbox"" style=""width: 220px""/>
                </td>
            </tr>
            <tr>
                <td>
                    登录密码
                </td>
                <td>
                    <input id=""txtPassword"" name=""Password"" type=""text"" class=""myTextbox"" style=""width: 220px""/>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <input id=""hfConnectionId"" name=""ConnectionId"" type=""hidden""");
                    BeginWriteAttribute("value", " value=\"", 4828, "\"", 4836, 0);
                    EndWriteAttribute();
                    WriteLiteral("/>\r\n                </td>\r\n            </tr>\r\n        </table>\r\n        <div>\r\n            <span id=\"spanWait\" style=\"display: none;\" class=\"waitText\">请稍后......</span>\r\n        </div>\r\n    ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_5.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n</div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
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
