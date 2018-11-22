<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformationTable.aspx.cs" Inherits="HsBusiness.Businesss.InformationTable" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>衡水商机</title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css">
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="/css/uniform.css">
    <link rel="stylesheet" type="text/css" href="/css/dialog.css">
    <link rel="stylesheet" type="text/css" href="/css/table.css">
  
</head>
<body>
	<div class="Standard">
		<div class="widget-box">
			<div class="widget-title">
				<p>控制台</p>
				<div class="control1">
					<select class="input-small inline form-control" id="Areas">
						<option value="">请选择区县</option>
						<option value="衡水市">衡水市</option>
						<option value="2">安平县</option>
						<option value="3">饶阳县</option>
						<option value="4">深州市</option>
						<option value="5">武强县</option>
						<option value="6">阜城县</option>
						<option value="7">武邑县</option>
						<option value="8">景县</option>
						<option value="9">冀州县</option>
						<option value="10">枣强县</option>
						<option value="11">故城县</option>
					</select>
					<select class="input-small inline form-control" id="Industry">
						<option value="">请选择行业</option>
						<option value="企业">企业</option>
						<option value="2">党政</option>
						<option value="3">医疗</option>
						<option value="4">校园</option>
						<option value="5">军警</option>
						<option value="6">其他</option>
					</select>
					<select class="input-small inline form-control" id="Operator">
						<option value="">请选择运营商</option>
						<option value="电信">电信</option>
						<option value="2">移动</option>
						<option value="3">联通</option>
						<option value="4">暂无</option>
					</select>
					<select class="input-small inline form-control" id="Scale">
						<option value="">请选择规模</option>
						<option value="50人">50人</option>
						<option value="100人">100人</option>
						<option value="3">200人</option>
						<option value="4">500人</option>
						<option value="5">1000人</option>
					</select>
					<select class="input-small inline form-control"id="State">
						<option value="">请选择状态</option>
						<option value="1">派单</option>
						<option value="2">已回执</option>
						<option value="3">已提醒</option>
						<option value="4">已落单</option>
					</select>
					<button class="btn btn-info btn-mini" id="search">确定</button>
				</div>
				<div class="control2">
					<button class="btn btn-danger btn-mini" id="del">删除</button>
				
				</div>
			</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th><div class="checker" id="uniform-title-table-checkbox"><input type="checkbox" id="title-table-checkbox" name="title-table-checkbox"></div></th>
							<th>区县</th>
							<th>客户名称</th>
							<th>行业归属</th>
							<th>合作运营商</th>
							<th>用户规模</th>
							<th>移动业务</th>
							<th>固网业务</th>
							<th>拜访次数</th>
							<th>最近拜访时间</th>
							
							<th>操作</th>
						</tr>
					</thead>
					<tbody id="tbody">
                      
					</tbody>
				</table>
			</div>
		</div>
		<div class="pagination alternate">
			<ul>
				<li class="disabled"><a href="#">上一页</a></li>
				<li class="active"> <a href="#">1</a> </li>
				<li><a href="#">2</a></li>
				<li><a href="#">3</a></li>
				<li><a href="#">4</a></li>
				<li><a href="#">下一页</a></li>
			</ul>
		</div>
	</div>
	<script src="/js/jquery.min.js"></script>
	<script src="/js/dialog.js"></script>
	<script src="/js/example.js"></script>
	<script src="/js/jquery.uniform.js"></script>
	<script src="/js/jquery.dataTables.min.js"></script>
    <script src="../js/matrix.tables.js"></script>
    <script>
        $(function () {
           get();
            $("#search").click(function () { get(); })            
            $("#del").click(function () {
                // 判断是否至少选择一项   
                var checkedNum = $("input[name='subChk']:checked").length;
                if (checkedNum == 0) {
                    alert("请选择要删除的行！");
                    return;
                }
                if (window.confirm('你确定要删除吗？')) {

                    var checkedList = new Array();

                    $("input[name='subChk']:checked").each(function () {
                        checkedList.push($(this).val());
                    });
                    $.ajax({
                        url: 'InformationTable.aspx/BatchDel',
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        data: JSON.stringify({ ids: checkedList.toString() }),
                        success: function (result) {
                            $("[name ='subChk']:checkbox").attr("checked", false);
                            var result = $.parseJSON(result.d);
                            alert(result.msg);
                            window.location.reload();
                        }
                    });

                }
                else {
                    return false;

                }
         
            })
        });
        //查询
        function get() {
            $("tbody").empty();
            var data = {
                Areas: $("#Areas").val(),
                Industry: $("#Industry").val(),
                Operator: $("#Operator").val(),
                Scale: $("#Scale").val(),
                State: $("#State").val(),
                PageIndex:1
            };
            $.ajax({
                url: 'InformationTable.aspx/Get',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",

                success: function (data) {
                    var d = $.parseJSON(data.d);
                    var result = "";
                    for (var i = 0; i < d.length; i++) {
                        result += `
                        <tr>
							<td><div class ="checker" id="uniform-undefined"><input name="subChk" type="checkbox" class ="ID" hidden="hidden" value="${d[i].ID}" /></div></td>
							<td>${d[i].Areas ||""}</td>
							<td>${d[i].CompanyName}</td>
							<td>${d[i].Industry}</td>
							<td>${d[i].MOperator || ""}</td>
							<td>${d[i].CustomerScale}</td>
							<td>`
                            if (d[i].IsMove) { result += `<a href="javascript:void(0);" onclick="getMove(${d[i].ID})">查看</a>` }else{result+="暂无"}
                            result+=`</td>
							<td>`
                            if (d[i].IsFixed) { result += `<a href="javascript:void(0);"  onclick="getFixed(${d[i].ID})">查看</a>` } else {result+="暂无" }
                            result+=`</td>
							<td><a class ="red" href="javascript:void(0);">${d[i].VisitNum || 0}</a></td>
							<td>${d[i].AddTime}</td>
							<td class ="red"></td>
							<td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='/Businesss/InformationNewly.aspx?id=${d[i].ID}'">编辑</button><button class="btn btn-danger btn-mini" onclick="del(${d[i].ID})">删除</button></td>

						</tr>
                        `.trim();
                    }
                    $("#tbody").append(result);
                    $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
                }
            })
        }
        //查询移动业务
        function getMove(id) {
            var data = { ID: id }
            $.ajax({
                url: 'InformationTable.aspx/GetMove',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    if (data) {
                        $.dialog({
                            titleText: '移动业务',
                            contentHtml: `<table class="table table-bordered table-striped with-check example-table">
					<thead>
						<tr>
							<th>手机卡用途</th>
							<th>可推业务</th>
							<th>主合作运营商</th>
							<th>占比</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>${data.MCardUse||""}</td>
							<td>${data.MPushWork||""}</td>
							<td>${data.MOperator||""}</td>
							<td>${data.MRatio||""}</td>
						</tr>
					</tbody>
				</table>
				<table class="table table-bordered table-striped with-check example-table">
					<thead>
						<tr>
							<th>使用政策</th>
							<th>使用月消费</th>
							<th>年收入预测</th>
							<th>有无在用其他业务</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>${data.MPolicy||""}</td>
							<td>${data.MMonthFee||""}</td>
							<td>${data.MIncome||""}</td>
							<td>${data.MOtherWork||""}</td>
						</tr>
					</tbody>
				</table>`
                        });
                    } else {
                        $.dialog({
                            type: 'info',
                            infoText: '查看失败',
                            infoIcon: '/images/icon/fail.png',
                            autoClose: 1000
                        });
                    }
                },
                error: function () {
                    $.dialog({
                        type: 'info',
                        infoText: '查看失败',
                        infoIcon: '/images/icon/fail.png',
                        autoClose: 1000
                    });
                }
            });

        }
        //查询固网业务
        function getFixed(id) {            
            var data = {
                ID:id
            }
            $.ajax({
                url: 'InformationTable.aspx/GetFixed',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    if (data) {
                        $.dialog({
                            titleText: '固网业务',
                            contentHtml: `<table class="table table-bordered table-striped with-check example-table">
					<thead>
						<tr>
							<th>业务类别</th>
							<th>规模</th>
							<th>是否跨域</th>
							<th>预计周价</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>${data.FPushWork||""}</td>
							<td>${data.FScale||""}</td>
							<td>${data.FIsDomain ? "是" : "否"}</td>
							<td>${data.FPreWeekPrice||""}</td>
						</tr>
					</tbody>
				</table>
				<table class="table table-bordered table-striped with-check example-table">
					<thead>
						<tr>
							<th>预计入网月份</th>
							<th>年收益</th>
						</tr>
					</thead>
					<tbody>
						<tr>
							<td>${data.FPreInNetMouth||""}</td>
							<td>${data.FPreAnlIncome||""}</td>
						</tr>
					</tbody>
				</table>`
                        });
                    } else {
                        $.dialog({
                            type: 'info',
                            infoText: '查看失败',
                            infoIcon: '/images/icon/fail.png',
                            autoClose: 1000
                        });
                    }                    
                },
                error: function () {
                    $.dialog({
                        type: 'info',
                        infoText: '查看失败',
                        infoIcon: '/images/icon/fail.png',
                        autoClose: 1000
                    });
                }
            });
                
        }
        //单个删除
        function del(id) {
            if (window.confirm('你确定要删除吗？')) {
                var data = { ids: id }
                $.ajax({
                    url: 'InformationTable.aspx/Delete',
                    data: JSON.stringify(data),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        if (data.state == 1) {
                            alert(data.msg)
                        }
                        parent.location.reload();
                    }
                });
            }
            else {
                return false;
            }
        }
      
    </script>

</body>
</html>