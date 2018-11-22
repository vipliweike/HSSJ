<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreTable.aspx.cs" Inherits="HsBusiness.Stores.StoreTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link rel="stylesheet" type="text/css" href="../css/initial.css"/>
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/uniform.css"/>
    <link rel="stylesheet" type="text/css" href="../css/dialog.css"/>
    <link rel="stylesheet" type="text/css" href="../css/table.css"/>
    <link href="../css/kkpager_blue.css" rel="stylesheet" />
</head>
<body>
	<div class="Standard">
		<div class="widget-box">
			<div class="widget-title">
				<p>控制台</p>
                  <form id="form1" runat="server">
				<div class="control1">
					<select class="input-small inline form-control" id="Areas" name="Areas">
						<option value="">请选择区县</option>
						<%-- <option value="衡水市">衡水市</option>--%>
					</select>
					<select class="input-small inline form-control" id="Operator" name="Operator">
						<option value="">请选择运营商</option>
						<option value="电信">电信</option>
						<option value="移动">移动</option>
						<option value="联通">联通</option>
						<option value="4">暂无</option>
					</select>
					<div class="search">
						<input type="text" placeholder="请选择更新开始时间" name="sbirth" id="sbirth" data-birth="123" onclick="WdatePicker()" >
                        <span>至</span>
                        <input style="margin-left: 0;" type="text" placeholder="请选择更新结束时间" name="sbirth2" id="sbirth2" data-birth="123" onclick="WdatePicker()">
					</div>
					<div class="search">
						<a class="btn btn-info btn-mini alink" style="float: right;" id="search">确定</a>
						<input type="text" placeholder="请输入关键字" id="Search" name="Search">
					</div>
				</div>
				<div class="control2">
					<button class="btn btn-danger btn-mini" id="del" type="button">删除</button>
                    <button class="btn btn-primary btn-mini" onclick="Export()">导出</button>
                    <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none" class="btn btn-primary btn-mini" OnClick="LinkButton1_Click">导出</asp:LinkButton>
				</div>
                      </form>
			</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th><div class="checker" id="uniform-title-table-checkbox"><input type="checkbox" id="title-table-checkbox" name="title-table-checkbox"></div></th>
							<th>区县</th>
							<th>门店名称</th>                            
							<th>宽带运营商</th>
							<th>宽带价格（元）</th>
							<th>到期时间</th>
							<th>负责人</th>
							<th>到期提醒</th>
							<th>客户拜访</th>
							<th>拜访次数</th>
							<th>最近更新时间</th>
							<th class="operation">操作</th>
						</tr>
					</thead>
					<tbody id="tbody">
					
					</tbody>
				</table>
			</div>
		</div>
		  <div class="pagination alternate">
            <div id="kkpager">
            </div>
        </div>
	</div>
    <script src="../js/jquery.min.js"></script>
    <script src="../js/browser.min.js"></script>
	<script type="text/javascript" src="../js/dialog.js"></script>
	<script type="text/javascript" src="../js/example.js"></script>
	<script type="text/javascript" src="../js/jquery.uniform.js"></script>
	<script type="text/javascript" src="../js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="../js/matrix.tables.js"></script>
    <script src="../js/My97DatePicker/WdatePicker.js"></script>
    <script src="../js/kkpager.js"></script>
     <script>
        $(function () {
            //get();
            $("#search").click(function () {
                var starttime = $("#sbirth").val();
                var endtime = $("#sbirth2").val();

                if (starttime > endtime) {
                    alert('查询开始时间不能大于结束时间');
                    return;
                }

                GetExcelTable(1);
            })
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
                        url: 'StoreTable.aspx/BatchDel',
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


    </script>
     <script type="text/javascript">
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        function GetExcelTable(pageindex) {
            $("tbody").empty();
            var data = {
                Areas: $("#Areas").val(),
                Operator: $("#Operator").val(),
                starttime: $("#sbirth").val(),
                endtime: $("#sbirth2").val(),
                Search: $("#Search").val(),
                pageIndex: pageindex
            };
            $.ajax({
                url: 'StoreTable.aspx/Get',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var d = $.parseJSON(data.d);
                    if (d.pagecount == 0 || d.data.length == 0) {
                        $("#tbody").empty();
                        $("#tbody").html("<tr><td colspan='12'><span style='text-align:center; color:red'>暂无数据</span></td></tr>");
                        $("#kkpager").hide();
                        return;
                    }
                    var result = '';

                    $("#kkpager").show();
                    for (var i = 0; i < d.data.length; i++) {
                        result += `
                            <tr>
						    <td><div class ="checker" id="uniform-undefined"><input name="subChk" type="checkbox" class ="ID" hidden="hidden" value="${d.data[i].ID}" /></div></td>
							<td>${d.data[i].Areas || ""}</td>
                            <td>${d.data[i].StoreName || ""}</td>
							<td>${d.data[i].Broadband || ""}</td>
							<td>${d.data[i].Price || ""}</td>
							<td>${d.data[i].OverTime || ""}</td>
                            <td>${d.data[i].UserName || ""}</td>
                            <td><a class ="${d.data[i].State == 0 ? "black": d.data[i].State == 1 ? "red" : d.data[i].State==2 ? "green":""}">${d.data[i].State==0?"正常": d.data[i].State==1?"已提醒": d.data[i].State==2?"已回执": ""}</td>
                            <td><a class ="${d.data[i].State == 0 ? "black": d.data[i].HzState == 1 ? "red" : d.data[i].HzState==2 ? "green":""}">${d.data[i].State==0?"正常": d.data[i].HzState==1?"已提醒": d.data[i].HzState==2?"已回执": ""}</td>
                            <td>${d.data[i].VisitNum || 0}</td>
                            <td>${d.data[i].LastTime || ""}</td>
                            <td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='AddStore.aspx?id=${d.data[i].ID}'">编辑</button><button class="btn btn-success btn-mini" onclick="window.parent.test.location.href='StoreDetails.aspx?SID=${d.data[i].ID}'">查看详情</button><button class ="btn btn-danger btn-mini" onclick="del(${d.data[i].ID})">删除</button></td>
							</tr>`
              
                    }
                    $("#tbody").append(result);
                    $('input[name=subChk]').uniform();
                    //定义分页样式
                    var totalCount = parseInt(d.pagecount);

                    var pageSize = parseInt(d.pagesize);

                    var pageNo = getParameter('pageIndex');//该参数为插件自带，不设置好，调用数据的时候当前页码会一直显示在第一页

                    if (!pageNo) {
                        pageNo = pageindex;
                    }
                    var totalPages = totalCount % pageSize == 0 ? totalCount / pageSize : (parseInt(totalCount / pageSize) + 1);
                    kkpager.generPageHtml({
                        pno: pageNo,
                        total: totalPages,
                        totalRecords: totalCount,
                        mode: 'click',
                        click: function (n) {
                            this.selectPage(pageNo);
                            searchPage(n);
                            return false;
                        }
                    }, true);
                }, error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }
        //init
        $(function () {
            getRegion();
          
            GetExcelTable(1);
          

        });


        function getRegion() {
            $.ajax({
                url: '/PersonManage/PersonTable.aspx/GetRegion',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `
                        <option value="${data[i].Name}">${data[i].Name}</option>
                        `;
                    }
                    $("#Areas").append(result);
                }
            });
        }



        //ajax翻页
        function searchPage(n) {
            GetExcelTable(n);
        }
        //单个删除
        function del(id) {
            if (window.confirm('你确定要删除吗？')) {
                var data = { id: id }
                $.ajax({
                    url: 'StoreTable.aspx/Delete',
                    data: JSON.stringify(data),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        if (data.state == 1) {
                            alert(data.msg)
                        }
                        window.location.reload();
                    }
                });
            }
            else {
                return false;
            }
        }
        function Export() {
            document.getElementById("LinkButton1").click();
           
        }
    </script>
</body>
</html>
