
$(document).ready(function() {
    SetChoiceDbControls();
    $("#btnRun").click(btnRun_click);
    $("#btnRefresh").click(btnRefresh_click);
    SetDataGrid();
});


function SetDataGrid() {
    $('#tblSpList').datagrid({
        fit: true,
        rownumbers: true,
        border: false,
        columns: [
            [
                { field: 'ck', checkbox: true },
                { field: 'Name', title: '存储过程名称', width: 330 }
            ]
        ]
    });
    $('#tblViewList').datagrid({
        fit: true,
        rownumbers: true,
        border: false,
        columns: [
            [
                { field: 'ck', checkbox: true },
                { field: 'Name', title: '视图名称', width: 400 }
            ]
        ]
    });
    $('#tblFuncList').datagrid({
        fit: true,
        rownumbers: true,
        border: false,
        columns: [
            [
                { field: 'ck', checkbox: true },
                { field: 'Name', title: '自定义函数名称', width: 240 }
            ]
        ]
    });
}


function btnRefresh_click() {
    if ($("#hfSrcConnId").val().length == 0) {
        $.messager.alert(g_MsgBoxTitle, '请选择数据源的连接帐号。', 'warning');
        return false;
    }
    if ($("#cboSrcDB").combobox("getValue").length == 0) {
        $.messager.alert(g_MsgBoxTitle, '请选择数据源的数据库。', 'warning');
        return false;
    }

    $(this).hide();
    $(".waitMessageStyle").show();
    $.ajax({
        cache: false,
        dataType: "json",
        type: "GET",
        url: "/AjaxService/GetDbSpViewFuncList",
        data: { connectionId: $("#hfSrcConnId").val(), dbName: $("#cboSrcDB").combobox("getValue") },
        complete: function() {
            $(".waitMessageStyle").hide();
            $("#btnRefresh").show();
        },
        success: function(json) {
            $('#tblSpList').datagrid("loadData", json[0]);
            $('#tblViewList').datagrid("loadData", json[1]);
            $('#tblFuncList').datagrid("loadData", json[2]);
        }
    });
}


function btnRun_click() {
    if (Check2ConnDB() == false) return false;

    var array1 = $('#tblSpList').datagrid("getSelections");
    var array2 = $('#tblViewList').datagrid("getSelections");
    var array3 = $('#tblFuncList').datagrid("getSelections");

    if (array1.length + array2.length + array3.length == 0) {
        $.messager.alert(g_MsgBoxTitle, '请选择要复制的项目。', 'warning');
        return false;
    }

    var spNames = '';
    for (var i = 0; i < array1.length; i++) spNames += array1[i].Name + ';';

    var viewNames = '';
    for (var i = 0; i < array2.length; i++) viewNames += array2[i].Name + ';';

    var funcNames = '';
    for (var i = 0; i < array3.length; i++) funcNames += array3[i].Name + ';';

    $(this).hide();
    $(".waitMessageStyle").show();
    $("#divExecuteResult").text('请稍后......');

    $.ajax({
        cache: false,
        dataType: "text",
        type: "POST",
        url: '/AjaxService/CopyProcedures',
        data: {
            srcConnId: $("#hfSrcConnId").val(),
            destConnId: $("#hfDestConnId").val(),
            srcDB: $("#cboSrcDB").combobox("getValue"),
            destDB: $("#cboDestDB").combobox("getValue"),
            spNames: spNames,
            viewNames: viewNames,
            funcNames: funcNames
        },
        complete: function() {
            $(".waitMessageStyle").hide();
            $("#btnRun").show();
        },
        success: function(text) {
            $("#divExecuteResult").text(text);
        }
    });

}