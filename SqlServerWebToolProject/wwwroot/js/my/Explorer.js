
var g_tabIndex = 0;
var g_node_root = "root";
var g_node_sp = "sp";
var g_node_tbl = "tbl";
var g_node_view = "view";
var g_node_func = "func";


$(function() {
    $("#tblConnectionInfo input:text").addClass("myTextbox2");
    $("#tblConnectionInfo input:password").addClass("myTextbox2");
    $("#ulTree")
        .tree({ checkbox: true, animate: true, onClick: TreeNode_Click, onContextMenu: BindTreeNodeContextMenu });

    BindMenuEvent()
    BindTabEvent();
    LoadConnectionParameters();
    $("#btnRefresh").click(btnRefresh_click);

    $("#chkEnableGutter")
        .change(function() {
            var checked = $("#chkEnableGutter").is(":checked");
            SyntaxHighlighter.defaults['gutter'] = checked;
            $.cookie("msv_EnableGutter", (checked ? "checked" : "no"), { expires: 700, path: '/' });
        })
        .each(function() {
            var cookieVal = $.cookie("msv_EnableGutter");
            if (cookieVal == null || cookieVal == "checked")
                $(this).attr("checked", "checked");
        })
        .change();
});

function LoadConnectionParameters() {
    $.ajax({
        cache: false,
        dataType: "json",
        type: "GET",
        url: "/AjaxService/GetConnectionInfoByURL",
        data: { url: window.location.href },
        success: function(json) {
            $("#hidden_ConnectionId").val(json.ConnectionId);
            $('#txtServer').val(json.ServerIP);
            $('#txtUserName').val(json.UserName);
            $('#txtPassword').val(json.Password);

            document.title += " (" + json.ServerIP + " - " + json.UserName + ")";
            $("#chkTable").parent().attr("title", $("#chkTable").parent().attr("title") + "（这个操作比较耗时，请慎重选择）");
        }
    });
}

function GetConnectionId() {
    return $("#hidden_ConnectionId").val();
}

// 判断节点是不是“叶子节点”，它应该表示一个数据库的 表，视图，存储过程，自定义函数
function NodeIsLeaf(node) {
    // 说明：不想使用 return $("#ulTree").tree("isLeaf", node.target);
    return (node.attributes.NodeFlag == g_node_sp ||
        node.attributes.NodeFlag == g_node_tbl ||
        node.attributes.NodeFlag == g_node_view ||
        node.attributes.NodeFlag == g_node_func);
}

// 判断节点是不是一个目录节点。它应该是包含 “叶子节点” 的节点。但不是“数据库”节点
function NodeIsCategory(node) {
    return (node.attributes.NodeFlag == "Tables" ||
        node.attributes.NodeFlag == "Views" ||
        node.attributes.NodeFlag == "Procedures" ||
        node.attributes.NodeFlag == "Functions");
}

function btnRefresh_click() {
    if ($('#hidden_ConnectionId').val().length == 0) {
        $.messager.alert(g_MsgBoxTitle, '没有指定连接参数。', 'error');
        return false;
    }
    $("#ulTree").tree("loadData", []);
    $(this).hide();
    $(".waitMessageStyle").show();

    $.ajax({
        type: "GET",
        dataType: "json",
        cache: false,
        url: "/AjaxService/GetTreeNodes",
        data: { connectionId: $('#hidden_ConnectionId').val() },
        complete: function() {
            $(".waitMessageStyle").hide();
            $("#btnRefresh").show();
        },
        success: function(json) {
            if (json.dbList == null || json.dbList.length == 0) {
                $.messager.alert(g_MsgBoxTitle, '没有找到任何可用的数据库。', 'warning');
            } else {
                /* 2011-04-30 决定不显示加载失败的消息了。
                if( json.ErrorMessage != null && json.ErrorMessage.length > 0 )
                    $.messager.alert(g_MsgBoxTitle, json.ErrorMessage, 'error');
                */
                // 重新加载树节点
                $("#ulTree").tree("loadData", json.dbList);
                $("#ulTree").tree("options").url = "/AjaxService/GetDataBaseChildren";
            }
        }
    });

    return false;
}

// 根据一个“叶子节点”，返回对应的“数据库”节点，如果不是“叶子节点”，则返回null
function GetDataBaseNodeByLeftNode(node) {
    var isLeaf = NodeIsLeaf(node);
    if (isLeaf == false) return null;

    // 这一层应该是“Tables”或“Procedures”节点
    var parent = $("#ulTree").tree("getParent", node.target);
    if (parent == null) return null;

    // 继续取父节点
    var root = $("#ulTree").tree("getParent", parent.target);
    return root;
}

// 根据一个“任意节点”，返回对应的“数据库”节点
function GetDataBaseNodeByAnyNode(node) {
    if (node == null) return null;
    if (node.attributes.NodeFlag == g_node_root) return node;

    var parent = node;
    while (parent != null) {
        parent = $("#ulTree").tree("getParent", node.target);

        if (parent == null) {
            if (node.attributes.NodeFlag == g_node_root) return node;
            else return null;
        }
        node = parent;
    }
    return null;
}

// 根据树控件中当前选择的节点，返回对应的“数据库”节点
function GetSelectedDataBaseNode() {
    var node = $("#ulTree").tree("getSelected");
    return GetDataBaseNodeByAnyNode(node);
}

function TreeNode_Click(node, showInNewTab) {
    var root = GetDataBaseNodeByLeftNode(node);
    if (root == null) return false;

    if (typeof(showInNewTab) != "boolean") showInNewTab = false;

/*	if( $('#tabs').tabs('exists', node.text) ){
		 $('#tabs').tabs('close', node.text);
		 showInNewTab = true;
	}
*/
    if ($('#tabs').tabs('exists', node.text)) {
        $('#tabs').tabs('select', node.text);
        showInNewTab = false;
    }
    if (showInNewTab == false)
        showInNewTab = ($('#tabs').tabs("tabs").length == 0);

    var flag = node.attributes.NodeFlag;
    if (flag == g_node_sp) {
        ShowResultInTab("/AjaxService/GetStoreProcedureCode",
            "text",
            { connectionId: GetConnectionId(), dbName: root.text, spName: node.text },
            node.text,
            "Procedure",
            showInNewTab);
    } else if (flag == g_node_func) {
        ShowResultInTab("/AjaxService/GetFunctionCode",
            "text",
            { connectionId: GetConnectionId(), dbName: root.text, funcName: node.text },
            node.text,
            "Function",
            showInNewTab);
    } else if (flag == g_node_view) {
        ShowResultInTab("/AjaxService/GetViewCode",
            "text",
            { connectionId: GetConnectionId(), dbName: root.text, viewName: node.text },
            node.text,
            "View",
            showInNewTab);
    } else if (flag == g_node_tbl) {
        ShowResultInTab("/AjaxDataTable/TableDescribe",
            "html",
            { connectionId: GetConnectionId(), dbName: root.text, tableName: node.text },
            node.text,
            "Table",
            showInNewTab);
    }
}

// 显示结果到一个“标签窗口”中
function ShowResultInTab(url, dataType, param, title, nodeType, showInNewTab) {
    $(".waitMessageStyle").show();
    $.ajax({
        type: "GET",
        dataType: dataType,
        cache: false,
        url: url,
        data: param,
        complete: function() { $(".waitMessageStyle").hide(); },
        success: function(result) {
            var html = (nodeType == "Table")
                ? "<div class='Table'><div class='h3Title'>(Table) " + title + "</div>" + result + "</div>"
                : "<div class='h3Title'>(" +
                nodeType +
                ") " +
                title +
                "</div><pre class='brush: sql;' name='code'>" +
                result.replace(/</g, "&lt;") +
                "</pre>";
            if (showInNewTab)
                $('#tabs').tabs('add', { title: title, content: html, closable: true });
            else {
                var tab = $('#tabs').tabs("getSelected");
                if (tab == null)
                    $('#tabs').tabs('add', { title: title, content: html, closable: true });
                else
                    $('#tabs').tabs('update', { tab: tab, options: { title: title, content: html } });
            }
            BindTabEvent();
            if (nodeType == "Table") SetGridViewColor();
            SyntaxHighlighter.highlight();
        }
    });
}

function SetGridViewColor() {
    $("table.myGridVew").each(function() {
        $(this).removeClass("myGridVew").addClass("GridView")
            .find(">thead>tr").addClass("GridView_HeaderStyle").end()
            .find(">tbody>tr")
            .filter(':odd').addClass("GridView_AlternatingRowStyle").end()
            .filter(':even').addClass("GridView_RowStyle");
    });
}

function BindTabEvent() {
    //$('#tabs').tabs({onContextMenu: function(e, title){	// 不打算使用这个事件。
    $('.tabs-inner').has('span.tabs-closable')
        .unbind("dblclick")
        .dblclick(function() {
            var subtitle = $(this).children("span").text();
            $('#tabs').tabs('close', subtitle);
        })
        .unbind("contextmenu")
        .bind('contextmenu',
            function(e) {
                var subtitle = $(this).children("span").text();
                $('#tabs').tabs('select', subtitle);

                $('#mm_tab').menu('show', { left: e.pageX, top: e.pageY });
                $('#mm_tab').data("curtab", subtitle);
                return false;
            });
}

function BindMenuEvent() {
    $('#mm_closeCurrent').click(function() {
        var currentTabTitle = $('#mm_tab').data("curtab");
        $('#tabs').tabs('close', currentTabTitle);
    })

    $('#mm_closeAll').click(function() {
        $('.tabs-inner span.tabs-title').filter(".tabs-closable").each(function(i, n) {
            $('#tabs').tabs('close', $(n).text());
        });
    });

    $('#mm_closeOthers').click(function() {
        var currentTabTitle = $('#mm_tab').data("curtab");
        $('.tabs-inner span.tabs-title').filter(".tabs-closable").each(function(i, n) {
            var title = $(n).text();
            if (title != currentTabTitle)
                $('#tabs').tabs('close', title);
        });
    });

    $('#mm_open').click(function() { MenuOpenClick(false); });

    $('#mm_openInNewTab').click(function() { MenuOpenClick(true); });

    $('#mm_CopyItemName, #mm_CopyItemName2').click(function() {
        var node = $("#ulTree").tree("getSelected");
        if (node == null) return;
        if (window.clipboardData)
            window.clipboardData.setData("Text", node.text); // IE专用方法。
        else
            $.messager.alert(g_MsgBoxTitle, '您的浏览器不支持此功能。', 'info');
    });

    $('#mm_ShowMultiItem, #mm_ShowMultiItem2, #mm_ShowMultiItem3').click(ShowMultiSelectedItemCode);

/*	$('#mm_DeleteSelectedItems1, #mm_DeleteSelectedItems2, #mm_DeleteSelectedItems3').click( function(){
		$.messager.confirm(g_MsgBoxTitle, '确定要删除所有已勾选的项目吗？？', function(dlgResult){ if( dlgResult ) DeleteSelectedItems(); });
	} );
*/
    $('#mm_DeleteSelectedItems1, #mm_DeleteSelectedItems2, #mm_DeleteSelectedItems3').click(DeleteSelectedItems);
    $('#mm_Search1, #mm_Search2, #mm_Search3').click(ShowSearchDialog);
    $("#mm_RefreshNodes").click(RefreshNodesForOneDB);
}


function RefreshNodesForOneDB() {
    var root = GetSelectedDataBaseNode();
    if (root == null) return false;
    $(".waitMessageStyle").show();

    $.ajax({
        type: "GET",
        dataType: "json",
        cache: false,
        url: "/AjaxService/GetTreeNodesByDbName",
        data: { connectionId: GetConnectionId(), dbName: root.text },
        complete: function() { $(".waitMessageStyle").hide(); },
        success: function(json) {
            var children = $("#ulTree").tree("getChildren", root.target);
            for (var i = 0; i < children.length; i++)
                if (NodeIsLeaf(children[i])) $("#ulTree").tree("remove", children[i].target);
            var children = $("#ulTree").tree("getChildren", root.target);
            for (var i = 0; i < children.length; i++) $("#ulTree").tree("remove", children[i].target);

            $("#ulTree").tree("append", { parent: root.target, data: json });
        }
    });
}


function ShowSearchDialog() {
    var root = GetSelectedDataBaseNode();
    if (root == null) return false;

    $("#spanCurrentSearchDB").text(root.text);

    $('#divSearchDialog').show().dialog({
        modal: true,
        resizable: true,
        buttons: [
            {
                text: '确定',
                iconCls: 'icon-ok',
                plain: true,
                handler: function() {
                    var str = $.trim($("#txtSearchWord").val());
                    if (str.length == 0) {
                        $.messager.alert(g_MsgBoxTitle,
                            '请输入要搜索的关键词。',
                            'warning',
                            function() { $("#txtSearchWord").focus(); });
                        return false;
                    }

                    var flag = '';
                    $("#tdSearchScope :checkbox:checked").each(function() { flag += $(this).parent().text(); });
                    if (flag.length == 0) {
                        $.messager.alert(g_MsgBoxTitle, '请选择要搜索的数据库对象类别范围。', 'warning');
                        return false;
                    }

                    var limit = $("#txtLimitCount").val(); // 这里不转换成数字，由服务端去处理。如果用户输入一个负数，我就让他多浪费点时间。

                    SearchDataBase(GetConnectionId(), root.text, str, flag, limit);
                    $('#divSearchDialog').dialog('close');
                }
            }, {
                text: '取消',
                iconCls: "icon-cancel",
                plain: true,
                handler: function() {
                    $('#divSearchDialog').dialog('close');
                }
            }
        ],
        onOpen: function() { $("#txtSearchWord").focus(); }
    });
}

function SearchDataBase(connId, dbName, str, scope, limit) {
    $(".waitMessageStyle").show();
    $.ajax({
        cache: false,
        dataType: "json",
        type: "GET",
        url: '/AjaxService/SearchDB',
        data: {
            connectionId: connId,
            dbName: dbName,
            searchWord: str,
            wholeMatch: ($("#chkWholeMatch").is(":checked") ? "1" : "0"),
            caseSensitive: ($("#chkCaseSensitive").is(":checked") ? "1" : "0"),
            searchScope: scope,
            limitCount: limit
        },
        complete: function() { $(".waitMessageStyle").hide(); },
        success: function(json) {
            var html = "<p><b>共搜索到　" + json.length + "　个符合项。</b></p>";
            $.each(json,
                function(i, item) {
                    if (i > 0) html = html + "<hr />";
                    html = html +
                        "<table class='SearchResult' cellpadding='3' cellspacing='0'>" +
                        "	<tr><td style='width: 80px'>对象名称</td><td class='boldText'><span class='redText'>" +
                        item.ObjectName +
                        "</span>　(" +
                        item.ObjectType +
                        ")</td></tr>" +
                        "	<tr><td colspan='2'><pre class='brush: sql; first-line: 1; highlight: [" +
                        item.LineNumber +
                        "]' name='code'>" +
                        item.SqlScript.replace(/</g, "&lt;") +
                        "		</pre></td></tr></table>";

                });

            $('#tabs').tabs('add',
                {
                    title: dbName + " SearchResult_" + (g_tabIndex++).toString(),
                    content: html,
                    closable: true
                });
            BindTabEvent();
            SyntaxHighlighter.highlight();
        }
    });
}


function BindTreeNodeContextMenu(e, node) {
    // 这里只能将右击的节点选中了，因为后续的操作中需要获取“节点”对象。
    $("#ulTree").tree("select", node.target);

    if (NodeIsLeaf(node))
        $('#mm_tree').menu('show', { left: e.pageX, top: e.pageY });
    else if (node.attributes.NodeFlag == g_node_root)
        $('#mm_tree2').menu('show', { left: e.pageX, top: e.pageY });
    else
        $('#mm_tree3').menu('show', { left: e.pageX, top: e.pageY });

    e.preventDefault();
}

function MenuOpenClick(showInNewTab) {
    var node = $("#ulTree").tree("getSelected");
    if (node == null) return;
    TreeNode_Click(node, showInNewTab);
}


function ShowMultiSelectedItemCode() {
    var nodes = $("#ulTree").tree("getChecked");
    if (nodes.length == 0) {
        $.messager.alert(g_MsgBoxTitle, '没有选择要操作的项目。', 'warning');
        return false;
    }

    var databaseName = null;
    var tblNames = "";
    var spNames = "";
    var viewNames = "";
    var funcNames = "";
    var count = 0;

    for (var i = 0; i < nodes.length; i++) {
        var node = nodes[i];
        var root = GetDataBaseNodeByAnyNode(node);
        if (root != null) {
            if (databaseName == null) {
                databaseName = root.text;
            } else if (databaseName != root.text) {
                $.messager.alert(g_MsgBoxTitle, '不能同时查看多个数据库的项目。', 'error');
                return false;
            }

            if (node.attributes.NodeFlag == g_node_sp) {
                spNames += ';' + node.text;
                count++;
            } else if (node.attributes.NodeFlag == g_node_tbl) {
                tblNames += ';' + node.text;
                count++;
            } else if (node.attributes.NodeFlag == g_node_view) {
                viewNames += ';' + node.text;
                count++;
            } else if (node.attributes.NodeFlag == g_node_func) {
                funcNames += ';' + node.text;
                count++;
            }
        }
    }
    if (databaseName == null || count == 0) return false;

    $(".waitMessageStyle").show();
    if (spNames == "" && viewNames == "" && funcNames == "")
        $.ajax({
            type: "POST",
            dataType: "json",
            cache: false,
            url: "/AjaxDataTable/MultiTableDescribe",
            data: { connectionId: GetConnectionId(), dbName: databaseName, tableNames: tblNames },
            complete: function() { $(".waitMessageStyle").hide(); },
            success: function(json) {
                var html = "";
                for (var i = 0; i < json.length; i++)
                    html += "<div class='Table'><div class='h3Title'>(Table) " +
                        json[i].TableName +
                        "</div>" +
                        json[i].Html +
                        "</div>";

                $('#tabs').tabs('add',
                    {
                        title: databaseName + " Code_" + (g_tabIndex++).toString(),
                        content: html,
                        closable: true
                    });
                BindTabEvent();
                SetGridViewColor();
            }
        });
    else
        $.ajax({
            type: "POST",
            dataType: "json",
            cache: false,
            url: "/AjaxService/GetSelectedItemCode",
            data: {
                connectionId: GetConnectionId(),
                dbName: databaseName,
                tblNames: tblNames,
                spNames: spNames,
                viewNames: viewNames,
                funcNames: funcNames
            },
            complete: function() { $(".waitMessageStyle").hide(); },
            success: function(json) {
                var html = '';
                for (var j = 0; j < json.length; j++) {
                    var node = json[j];
                    html += "<div class='subTitle'>--(" +
                        node.TypeText +
                        ") " +
                        node.Name +
                        "</div><pre class='brush: sql;' name='code'>" +
                        node.SqlScript.replace(/</g, "&lt;") +
                        "</pre>";
                }
                $('#tabs').tabs('add',
                    {
                        title: databaseName + " Code_" + (g_tabIndex++).toString(),
                        content: html,
                        closable: true
                    });
                BindTabEvent();
                SyntaxHighlighter.highlight();
            }
        });
}

function DeleteSelectedItems() {
    var nodes = $("#ulTree").tree("getChecked");
    if (nodes.length == 0) {
        $.messager.alert(g_MsgBoxTitle, '没有选择要操作的项目。', 'warning');
        return false;
    }

    var databaseName = null;
    var tblNames = "";
    var spNames = "";
    var viewNames = "";
    var funcNames = "";
    var count = 0;
    var root = null;

    for (var i = 0; i < nodes.length; i++) {
        var node = nodes[i];
        root = GetDataBaseNodeByAnyNode(node);
        if (root != null) {
            if (databaseName == null) {
                databaseName = root.text;
            } else if (databaseName != root.text) {
                $.messager.alert(g_MsgBoxTitle, '不能同时操作多个数据库的项目。', 'error');
                return false;
            }

            if (node.attributes.NodeFlag == g_node_sp) {
                spNames += ';' + node.text;
                count++;
            } else if (node.attributes.NodeFlag == g_node_tbl) {
                tblNames += ';' + node.text;
                count++;
            } else if (node.attributes.NodeFlag == g_node_view) {
                viewNames += ';' + node.text;
                count++;
            } else if (node.attributes.NodeFlag == g_node_func) {
                funcNames += ';' + node.text;
                count++;
            }
        }
    }
    if (databaseName == null || count == 0) return false;

    $.messager.confirm(g_MsgBoxTitle,
        '确定要删除所有已勾选的 ' + count + ' 个项目吗？？',
        function(dlgResult) {
            if (dlgResult) {
                $(".waitMessageStyle").show();
                $.ajax({
                    type: "POST",
                    dataType: "text",
                    cache: false,
                    url: "/AjaxService/DeleteSelectedItems",
                    data: {
                        connectionId: GetConnectionId(),
                        dbName: databaseName,
                        tblNames: tblNames,
                        spNames: spNames,
                        viewNames: viewNames,
                        funcNames: funcNames
                    },
                    complete: function() { $(".waitMessageStyle").hide(); },
                    success: function(text) {
                        for (var i = 0; i < nodes.length; i++) {
                            var node = nodes[i];

                            if (node.attributes.NodeFlag == g_node_sp ||
                                node.attributes.NodeFlag == g_node_tbl ||
                                node.attributes.NodeFlag == g_node_view ||
                                node.attributes.NodeFlag == g_node_func) {
                                $("#ulTree").tree("remove", node.target)
                            }
                        }
                        // 修改节点文本中的“子节点数量”
                        var children = $("#ulTree").tree("getChildren", root.target);
                        for (var i = 0; i < children.length; i++) {
                            var node = children[i];
                            if (NodeIsCategory(node)) {
                                var ccc = $("#ulTree").tree("getChildren", node.target);
                                $("#ulTree").tree("update",
                                    { target: node.target, text: node.attributes.NodeFlag + "(" + ccc.length + ")" });
                            }
                        }
                        $.messager.alert(g_MsgBoxTitle, '操作成功', 'info');
                    }
                });
            }
        });
}