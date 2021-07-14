
var g_MsgBoxTitle = "SqlServerSmallTool - http://www.cnblogs.com/fish-li";
var __waitHTML = '<div style="padding: 20px;"><img src="Images/progress_loading.gif" /><span style="font-weight: bold;padding-left: 10px; color: #FF66CC;">请稍后......</span></div>';


$(function(){
	if( window.SyntaxHighlighter )  SyntaxHighlighter.defaults['toolbar'] = false;
	$.ajaxSetup({
		cache: false, 
		error: function (XMLHttpRequest, textStatus, errorThrown) {
			if( typeof(errorThrown) != "undefined" )
				$.messager.alert(g_MsgBoxTitle, "调用服务器失败。<br />" + errorThrown ,'error');
			else{
				var error = "<b style='color: #f00'>" + XMLHttpRequest.status + "  " + XMLHttpRequest.statusText + "</b>";
				var start = XMLHttpRequest.responseText.indexOf("<title>");
				var end = XMLHttpRequest.responseText.indexOf("</title>");
				if( start > 0 && end > start )
					error += "<br /><br />" + XMLHttpRequest.responseText.substring(start + 7, end);
					
				$.messager.alert(g_MsgBoxTitle, "调用服务器失败。<br />" + error ,'error');
			}
		}
	});
});



function ShowWaitMessageDialog(dlgTitle) {
	if( typeof(dlgTitle ) != "string" ) dlgTitle = "请求处理中";
	var j_dialog = $(__waitHTML);
	j_dialog.appendTo('body').show().dialog({
        height: 120, width: 350, modal: true, resizable: false, closable: false, title: dlgTitle
    });
	return j_dialog;
}

function HideWaitMessageDialog(j_dialog) {
	if( j_dialog == null ) 	return;	
	j_dialog.dialog('close');
	j_dialog.remove();
	j_dialog = null;
}


// 将一个select控件显示成Easy-UI的ComboBox样式
jQuery.fn.SetComboBox = function () {
	return this.each(function () {
		var jComboBox = $(this);
		var maxheight = 0;
		// 说明：使用 panelWidth 这个自定义标签实在是没有办法的事，因为如果下拉框放在隐藏的层中，Easy-UI在显示时，宽度会为2px
		//       panelHeight 的使用，也是类似的原因。
		if (jComboBox.is("[panelHeight]"))
			maxheight = parseInt(jComboBox.attr("panelHeight"));
		else {
			maxheight = $("option", this).length * 20;
			if (maxheight > 500) maxheight = 500;
			else maxheight += 15; // 不多加一点，有时会出现滚动条，不好看，尤其是IE
		}

		var pWidth = 0;
		if (jComboBox.is("[panelWidth]"))
			pWidth = parseInt(jComboBox.attr("panelWidth"));
		else {
			// 1.2.1 这样写没有问题，虽然看起来有点怪。
			pWidth = (jComboBox.width() < 10 ? parseInt(jComboBox.css("width")) : null);
		}
		$(this).data("originalVal", $(this).val());

		jComboBox.combobox({
			panelHeight: maxheight, panelWidth: pWidth,
			editable: jComboBox.is("[combobox=editable]"),
			onSelect: function () { jComboBox.val($(this).combobox('getValue')); /*$(this).combobox("hidePanel");*/jComboBox.change(); }
		});
	});
};

jQuery.fn.SetComboBoxValue = function(val){
	return this.each(function(){ $(this).val(val).combobox("setValue", val); });
}


function CheckTextboxIsInputed(textbox, errorMessage){
	if( $.trim($("#" + textbox).val()).length == 0 ) {
		$.messager.alert(g_MsgBoxTitle, errorMessage,'warning', function(){$("#" + textbox).focus();});
		return false;
	}
	else
		return true;
}


function SetSearchTextbox(textboxId, hiddenId, pickButtonClick) {
    var j_text = $('#' + textboxId);
	if( j_text.attr("readonly") == "readonly" || j_text.attr("disabled") == "disabled" ) return false;
	
	var width = j_text.width();
	var height = j_text.height() - 2;
	
    var j_div = $("<div></div>").insertBefore(j_text)
				.addClass(j_text.attr("class")).css("width", width).css("padding", "1px");
    
    j_text.removeClass("myTextbox").addClass("myTextboxReadonly").css("width", (width-42)).css("float", "left").css("border", "0px").css("height", height).attr("readonly", "readonly");	//	.attr("disabled", "disabled");
    j_div.append(j_text);
    
    $("<a></a>").attr("title", "选择").addClass("floatButton").addClass("searchButton").appendTo(j_div).click(pickButtonClick);
    $("<a></a>").attr("title", "清除").addClass("floatButton").addClass("clearButton").appendTo(j_div).click(function(){
		j_text.val("").change();
		$("#" + hiddenId).val("");
		return false;
	});
}


function SetChoiceDbControls(){
	SetSearchTextbox("txtSrcConn", "hfSrcConnId", function(){ ShowSelectedConnAndFillDbComboBox("txtSrcConn", "hfSrcConnId", "cboSrcDB"); });
	SetSearchTextbox("txtDestConn", "hfDestConnId", function(){ ShowSelectedConnAndFillDbComboBox("txtDestConn", "hfDestConnId", "cboDestDB"); });
	$("#tblChoiceConn select").attr("panelHeight", "300").SetComboBox().combobox({ valueField:'Name', textField:'Name' });
	$("#txtSrcConn").change(function(){ if($("#txtSrcConn").val().length == 0)  $("#cboSrcDB").combobox("loadData", []).combobox("clear"); });
	$("#txtDestConn").change(function(){ if($("#txtDestConn").val().length == 0)  $("#cboDestDB").combobox("loadData", []).combobox("clear"); });
	
	// Create Choice Database Dialog
	var j_dialog = $('<div id="dlgChoiceDB" title="选择连接帐号"></div>');
	var j_tableDbList = $('<table id="tblDbListInDialog"></table>');
	j_tableDbList.appendTo(j_dialog).show();
	j_dialog.appendTo( $(document) );
	
	var _SetDataGrid = false;
	var SetDataGrid = function(){
		if( _SetDataGrid ) return;
		_SetDataGrid = true;
		
		j_tableDbList.datagrid({
			fit: true, rownumbers:true, singleSelect: true, border: false,
			columns:[[
				{field:'ServerIP', title:'服务器IP/Name', width:200},
				{field:'SSPI', title:'登录方式', width:100,
					formatter:function(value,rec){ return (value ? "Windows连接" : "用户名／密码"); }
				},
				{field:'UserName', title:'登录名', width:100},			
				{field:'ConnectionId', title:'ID', width:10}
			]]
		});
		j_tableDbList.datagrid('hideColumn', "ConnectionId");
	};
	
	var ShowSelectedConnAndFillDbComboBox = function(textboxId, hiddenId, cboDb){
		var list = null;
		$.ajax({
			async: false, dataType: "json", type: "GET",
			url: '/AjaxService/GetAllConnectionInfo',
			success: function (json) {
				list = json;
			}
		});
		if( list == null ) return false;
		
		j_dialog.show().dialog({
			height: 300, width: 570, modal: true, resizable: true, 
			buttons: [
				{ text: '确定', iconCls: 'icon-ok', plain: true,
					handler: function() {
						var row = j_tableDbList.datagrid('getSelected');
						if( row == null ){
							$.messager.alert(g_MsgBoxTitle, '请选择一条连接的记录。','warning'); return false;
						}
						else{
							$("#" + textboxId).val( row.ServerIP + ", " + row.UserName );
							$("#" + hiddenId).val(row.ConnectionId); 
							$("#" + cboDb).combobox("loadData", []).combobox("clear");
							FillDbComboBox(cboDb, row); 
							j_dialog.dialog('close');
						}
					}
				}, 
				{ text: '取消', iconCls: 'icon-cancel',  plain: true,
					handler: function() { 
						j_dialog.dialog('close');
					}
				}],
			onOpen: function() { 
				SetDataGrid();
				j_tableDbList.show();
				j_tableDbList.datagrid('loadData', list);
			}
		});
	}
	
	var FillDbComboBox = function(cboDb, row){
		$.ajax({
			dataType: "json", type: "GET",
			url: '/AjaxService/GetDbList',
			data: { connectionId: row.ConnectionId },
			success: function (json) {
				$("#" + cboDb).combobox("loadData", json);
			}
		});
	}	
}

function Check2ConnDB(){
	if( $("#hfSrcConnId").val().length == 0 ){
		$.messager.alert(g_MsgBoxTitle, '请选择数据源的连接帐号。','warning'); return false;
	}
	if( $("#cboSrcDB").combobox("getValue").length == 0 ){
		$.messager.alert(g_MsgBoxTitle, '请选择数据源的数据库。','warning'); return false;
	}
	if( $("#hfDestConnId").val().length == 0 ){
		$.messager.alert(g_MsgBoxTitle, '请选择数据目标的连接帐号。','warning'); return false;
	}
	if( $("#cboDestDB").combobox("getValue").length == 0 ){
		$.messager.alert(g_MsgBoxTitle, '请选择数据目标的数据库。','warning'); return false;
	}
	return true;
}




