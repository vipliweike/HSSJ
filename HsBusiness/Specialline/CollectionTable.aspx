<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CollectionTable.aspx.cs" Inherits="HsBusiness.Specialline.CollectionTable" %>

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
					<div class="search">
						<select class="input-small inline form-control" id="timecontorl" name="timecontorl">
                            <option value="">请选择</option>
							<option value="1">建档时间</option>
							<option value="2">更新时间</option>
						</select>
						<input type="text" placeholder="请选择开始时间" name="sbirth" id="sbirth" data-birth="123" onclick="WdatePicker()"/>
                        <span>至</span>
                        <input style="margin-left: 0;" type="text" placeholder="请选择结束时间" name="sbirth2" id="sbirth2" data-birth="123" onclick="WdatePicker()"/>
                     
						 <input type="text" style="width: 200px;" placeholder="请输入过期时间，例如：201805" id="ovretime" name="ovretime"/>
				
					</div>
                    <div class="search">
						<select class="input-small inline form-control" id="state" name="state">
                             <option value="">请选择</option>
                             <option value="0">跟进</option>
                             <option value="1">落单</option>
                             <option value="2">放弃</option>
						</select>

					</div>
					<div class="search">
						<a class="btn btn-info btn-mini alink" style="float: right;" id="search">确定</a>
						<input type="text" placeholder="请输入搜索内容" id="Search" name="Search"/>
					</div>
				</div>
				<div class="control2">
					<button class="btn btn-danger btn-mini" id="del" type="button">删除</button>
					<button class="btn btn-primary btn-mini" type="button" onclick="Export()">导出</button>
                  <%--  <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none" class="btn btn-primary btn-mini">导出</asp:LinkButton>--%>
                    <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none" class="btn btn-primary btn-mini"  OnClick="LinkButton1_Click">LinkButton</asp:LinkButton>
				</div>
                </form>
			</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th><div class="checker" id="uniform-title-table-checkbox"><input type="checkbox" id="title-table-checkbox" name="title-table-checkbox"/></div></th>
							<th>单位名称</th>
							<th>单位地址</th>
							<th>专线条数</th>
							<th>负责人</th>
							<th>发布时间</th>
                            <th>更新时间</th>
                            <th>状态</th>
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
	<script type="text/javascript" src="../js/dialog.js"></script>
	<script type="text/javascript" src="../js/example.js"></script>
	<script type="text/javascript" src="../js/jquery.uniform.js"></script>
	<script type="text/javascript" src="../js/jquery.dataTables.min.js"></script>
	<script type="text/javascript" src="../js/matrix.tables.js"></script>
    <script src="../js/My97DatePicker/WdatePicker.js"></script>
    <script src="../js/kkpager.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#search").click(function () {
                var starttime = $("#sbirth").val();
                var endtime = $("#sbirth2").val();

                if (starttime > endtime) {
                    alert('查询开始时间不能大于结束时间');
                    return;
                }

                GetExcelTable(1);
            })
            //批量删除
            $("#del").click(function () {
                // 判断是否至少选择一项   
                var checkedNum = $("input[name='subChk']:checked").length;
                if (checkedNum == 0) {
                    alert("请选择要删除的行！");
                    return;
                }
                if (window.confirm('你确定要删除吗？')) {

                    //声明一个数组
                    var checkedList = new Array();
                    //遍历选中的值
                    $("input[name='subChk']:checked").each(function () {
                        checkedList.push($(this).val());
                    });
                    $.ajax({
                        url: 'CollectionTable.aspx/BatchDel',
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

            });
            GetExcelTable(1);

        });
        function Import() {
            var file = $("#files").val();
            if (file == null || file.length == 0) {
                alert("请先选择上传文件");
                return false;
            }
            var formData = new FormData();
            formData.append("file", document.getElementById("files").files[0]);
            var file = document.getElementById("files").value;
            var form1 = document.getElementById("form");
            var ext = file.slice(file.lastIndexOf(".") + 1).toLowerCase();

            $.ajax({
                url: "/Interface/ImportProject.ashx",
                dataType: 'json',
                type: 'post',
                //contentType: "application/json;charset=utf-8",
                contentType: false,
                processData: false,
                data: formData,
                success: function (data) {
                    alert(data.msg);
                    if (data.state == 1) {
                        window.parent.refreshFrame();
                    }
                },
            });

        }
        //单个删除
        function Del(id) {

            if (window.confirm('你确定要删除吗？')) {
                var data = { id: id }
                $.ajax({
                    url: 'CollectionTable.aspx/Delete',
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
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        //获取数据
        function GetExcelTable(pageindex) {
            $("tbody").empty();
            var data = {
                Time: $("#timecontorl").val(),
                StartTime: $("#sbirth").val(),
                EndTime: $("#sbirth2").val(),
                Search: $("#Search").val(),
                State:$("#state").val(),
                OverTime:$("#ovretime").val(),
                pageIndex: pageindex
            };
            $.ajax({
                url: 'CollectionTable.aspx/Get',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    if (data.pagecount == 0 || data.data.length == 0) {
                        $("#tbody").empty();
                        $("#tbody").html("<tr><td colspan='7'><span style='text-align:center; color:red'>暂无数据</span></td></tr>");
                        $("#kkpager").hide();
                        return;
                    }
                    var result = '';

                    $("#kkpager").show();
                    for (var i = 0; i < data.data.length; i++) {
                        result += `<tr>
							 <td><div class ="checker" id="uniform-undefined"><input name="subChk" type="checkbox" class ="ID" hidden="hidden" value="${data.data[i].ID}" /></div></td>
							<td>${data.data[i].CompanyName || ""}</td>
							<td>${data.data[i].CompanyAddress||""}</td>
							<td>${data.data[i].CompanyScale||""}</td>
							<td>${data.data[i].UserName||""}</td>
							<td title="${data.data[i].AddTimeDetial}">${data.data[i].AddTime||""}</td>
                            <td title="${data.data[i].LastTimeDetail}">${data.data[i].LastTime||""}</td>
                            <td>${data.data[i].State}</td>
							<td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='/Specialline/AddCollection.aspx?id=${data.data[i].ID}'">编辑</button><button class="btn btn-success btn-mini" onclick="window.parent.test.location.href='CollectionDetial.aspx?PLID=${data.data[i].ID}'">查看详情</button><button class ="btn btn-danger btn-mini" onclick="Del(${data.data[i].ID})">删除</button></td>
						</tr>`;
                    }
                    $("#tbody").append(result);
                    $('input[name=subChk]').uniform();
                    //定义分页样式
                    var totalCount = parseInt(data.pagecount);  

                    var pageSize = parseInt(data.pagesize);

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

        //ajax翻页
        function searchPage(n) {
            GetExcelTable(n);
        }

        function Export() {
            document.getElementById("LinkButton1").click();

        }
    </script>
</body>
</html>
