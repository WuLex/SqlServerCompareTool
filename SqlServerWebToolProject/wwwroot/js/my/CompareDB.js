
$(document).ready(function(){
	SetChoiceDbControls();
	$("#btnCompareDB").click(btnCompareDB_click);

	$("#divCompareOptions :checkbox")
		.change(function(){ 
			var checked = $(this).is(":checked");
			$.cookie($(this).attr("id"), (checked ? "checked" : "no"), {expires: 700, path: '/'});
		})
		.each(function(){
			var cookieVal = $.cookie( $(this).attr("id") );
			if( cookieVal == null || cookieVal == "checked" )
				$(this).attr("checked", "checked" );
		});
});

function btnCompareDB_click(){
	var flag = '';
	$("#divCompareOptions :checkbox:checked").each(function(){ flag += $(this).parent().text(); });
	if( flag.length == 0 ){
		$.messager.alert(g_MsgBoxTitle, '请选择要比较的数据库对象类别（右上角）。','warning'); return false;
	}
	
	if( Check2ConnDB() == false ) return false;	
	
	$(this).hide();
	$("#divCompareResult").html(__waitHTML);
	
	$.ajax({
		cache: false, dataType: "json", type: "GET",
		url: '/AjaxService/CompareDB.cspx',
		data:{  srcConnId: $("#hfSrcConnId").val(), 
				destConnId: $("#hfDestConnId").val(), 
				srcDB: $("#cboSrcDB").combobox("getValue"), 
				destDB: $("#cboDestDB").combobox("getValue") ,
				flag: flag
		},
		complete: function(){ $("#btnCompareDB").show(); },
		success: function (json) {
			$("#divCompareResult").html("<p><b>共发现　" + json.length + "　不同的地方。</b></p>");
			$.each(json, function(i, item){
				if( i > 0 )  $("#divCompareResult").append($("<hr />"));
				var html = 
					"<table class='CompareResult' cellpadding='3' cellspacing='0'>" +
					"	<tr><td style='width: 80px'>对象名称</td><td class='boldText'><span class='redText'>" + 
							item.ObjectName + "</span>　(" + item.ObjectType + ")</td></tr>" +
					"	<tr><td>差异行数</td><td>" + (item.LineNumber > 0 ? "第 <b>" + item.LineNumber + "</b> 行" : "") + "<span style='padding-left:25px'></span>" +
							item.Reason + "</td></tr>";
				if( item.SrcLine.length > 0 )
					html = html + 
					"	<tr><td colspan='2'>源对象代码</td></tr>" +
					"	<tr><td colspan='2'><pre class='brush: sql; first-line: " + item.SrcFirstLine + "; highlight: [" + item.LineNumber + "]' name='code'>" +
							item.SrcLine +
					"		</pre></td></tr>" ;
				if( item.DestLine.length > 0 )
					html = html + 
					"	<tr><td colspan='2'>目标对象代码</td></tr>" +
					"	<tr><td colspan='2'><pre class='brush: sql; first-line: " + item.DestFirstLine + "; highlight: [" + item.LineNumber + "]' name='code'>" +
							item.DestLine +
					"		</pre></td></tr>";
				
				html = html + "</table>";
				$("#divCompareResult").append($(html));
			});
			SyntaxHighlighter.highlight();
		}
	});
}

