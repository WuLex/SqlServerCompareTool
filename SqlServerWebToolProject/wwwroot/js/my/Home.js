
$(function(){
	SetDataGrid();
	SetConnectionDialog();
});

function SetDataGrid(){
	$('#tblConnList').datagrid({
	    url: '/AjaxService/GetAllConnectionInfo.cspx?__t=' + Math.random(),
		fit: true, rownumbers:true, singleSelect: true, border: false,
		loadMsg: "正在加载数据，请稍后......",
		columns:[[
			{field:'ServerIP', title:'服务器IP/Name', width:220},
			{field:'SSPI', title:'登录方式', width:150,
				formatter:function(value,rec){ return (value ? "Windows连接" : "用户名／密码"); }
			},
			{field:'UserName', title:'登录名', width:150},			
			{field:'Password', title:'Password', width:10}
		]],
		toolbar:[{
			id:'btnAdd',
			text:'新增连接',
			iconCls:'icon-Add2',
			handler: AddConnection
		},{
			id:'btnDelete',
			text:'删除连接',
			iconCls:'icon-delete2',
			handler: DeleteConnection
		},{
			id:'btnEdit',
			text:'设置连接',
			iconCls:'icon-edit2',
			handler: EditConnection
		},{
			id:'btnOpenConnection',
			text:'打开连接',
			iconCls:'icon-go',
			handler: OpenConnection
		}],
		onDblClickRow: function(rowIndex, rowData){ EditConnection(); }
	});
	$('#tblConnList').datagrid('hideColumn', "Password");
}



function SetConnectionDialog(){
	$("#cboSSPI").SetComboBox().change(function() {
		var isSSPI = ($("#cboSSPI").combobox('getValue') == "true");
		$("#txtUserName, #txtPassword").attr("disabled", isSSPI);
	});
}

function GetSelectedRow(){
	var row = $('#tblConnList').datagrid('getSelected');
	if( row )
		return row;
	else{
		$.messager.alert(g_MsgBoxTitle, '请选择一条连接的记录。','warning');
		return null;
	}
}

function ShowConnectionDialog(onOpenFunc){
	$('#divConnectionDialog').show().dialog({
		modal: true, resizable: true,
		buttons:[
		{
			text:'测试连接', iconCls:'icon-Breakpoint', plain: true,
			handler: TestConnection
		},{
			text:'确定保存', iconCls:'icon-ok', plain: true,
			handler: SubmitConnectionForm
		},{
			text:'取消', iconCls: "icon-cancel", plain: true,
			handler:function(){
				$('#divConnectionDialog').dialog('close');
			}
		}],
		onOpen: onOpenFunc
	});
}

function ValidateForm(){
	if( CheckTextboxIsInputed("txtServerIP", "数据库服务器IP/Name 不能为空。") == false ) return false;
	if( $("#cboSSPI").combobox('getValue') == "false" )
		if( CheckTextboxIsInputed("txtUserName", "登录名 不能为空。") == false ) return false;

	//if( CheckTextboxIsInputed("txtPassword", "登录密码 不能为空。") == false ) return false;
	return true;
}

function AddConnection(){
	ShowConnectionDialog(function(){
		$("#hfConnectionId").val("");
		$("#txtServerIP").val("");
		$("#txtUserName").val("");
		$("#txtPassword").val("");
		$("#cboSSPI").combobox("setValue", "true").change();
	});
}

function EditConnection(){
	var row = GetSelectedRow();
	if( row == null ) return false;
	
	ShowConnectionDialog(function(){
		$("#txtServerIP").val(row.ServerIP);
		$("#txtUserName").val(row.UserName);
		$("#txtPassword").val(row.Password);
		$("#hfConnectionId").val(row.ConnectionId);
		$("#cboSSPI").SetComboBoxValue(row.SSPI ? "true" : "false").change();
	});
}

function SubmitConnectionForm(){
	if( ValidateForm() == false ) return false;
	$("#formConnection").ajaxSubmit({
		beforeSubmit: function(formData, jqForm, options) { $("#spanWait").show(); },
		complete: function() { $("#spanWait").hide(); },
		success: function(responseText, statusText) {			
			if (responseText == "update OK" ){
				$('#divConnectionDialog').dialog('close');
				//$('#tblConnList').datagrid("reload");	// 这是一种最简单的方法，但需要刷新整个网格。
				// 下面的方法虽然复杂，却可以只更新所修改的行。为了学习，采用下面的方法
				var row = GetSelectedRow();
				UpdateDataRowFromDialog(row);
				$('#tblConnList').datagrid('acceptChanges');
				
				var index = $('#tblConnList').datagrid('getRowIndex', row);
				$('#tblConnList').datagrid('refreshRow', index);
			}
			else if( responseText.length == 36 ){
				$('#divConnectionDialog').dialog('close');
				//$('#tblConnList').datagrid("reload");
				
				var row = {	ConnectionId: responseText	};
				UpdateDataRowFromDialog(row);
				$('#tblConnList').datagrid('appendRow', row);
			}
			else
				$.messager.alert(g_MsgBoxTitle, "调用失败。<br />" + responseText ,'error');
		}
	});
	
	var UpdateDataRowFromDialog = function(row){
		row.ServerIP = $("#txtServerIP").val();
		row.SSPI = $("#cboSSPI").combobox("getValue");
		if( row.SSPI == "false" ){
			row.UserName = $("#txtUserName").val();
			row.Password = $("#txtPassword").val();
		}
		else{	// 不成功的控件，值不会提交回去。
			row.UserName = "";
			row.Password = "";
		}
	}	
}


function DeleteConnection(){
	var row = GetSelectedRow();
	if( row == null ) return false;

	$.messager.confirm(g_MsgBoxTitle, '确定要删除选择的连接记录吗？？', function(dlgResult){
		if (dlgResult){
			$.ajax({
				cache: false, dataType: "text", type: "GET",
				url: "/AjaxService/DeleteConnection.cspx",
				data: { connectionId: row.ConnectionId },
				success: function (responseText) {
						var index = $('#tblConnList').datagrid('getRowIndex', row);
						$('#tblConnList').datagrid('deleteRow', index);
				}
			});
		}
	});
}


function OpenConnection(){
	var row = GetSelectedRow();
	if( row == null ) return false;
	//window.open("Explorer.htm?id=" + row.ConnectionId, "_blank"); return false;
	$("#btnOpenConnection").attr("target", "_blank").attr("href", "Explorer.htm?id=" + row.ConnectionId); return true;
}

function TestConnection(){
	if( ValidateForm() == false ) return false;
	
	$("#formConnection").ajaxSubmit({
	    cache: false, url: "/AjaxService/TestConnection.cspx",
		beforeSubmit: function(formData, jqForm, options) { $("#spanWait").show(); },
		complete: function() { $("#spanWait").hide(); },
		success: function(responseText, statusText) {
			if (responseText == "ok" ) $.messager.alert(g_MsgBoxTitle,'连接参数有效。','info');
			else $.messager.alert(g_MsgBoxTitle, '指定的连接参数不能连接到服务器，<br />服务器返回错误代码或消息：' + responseText,'error');
		}
	});
}


