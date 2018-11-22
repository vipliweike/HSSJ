<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessData.aspx.cs" Inherits="HsBusiness.ReportsManage.BusinessData" %>

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
    <style type="text/css">
    .table.with-check tr th:first-child, .table.with-check tr td:first-child {
	    width: auto;
	}
    </style>
</head>
<body>
	<div class="Standard">
		<div class="widget-box">
			<div class="widget-title">
				<p>控制台</p>
				<div class="control1">
					<div class="search">
						 <input type="text" style="width: 200px;" placeholder="请输入6位月份，例如：201805" id="Month"/>
					</div>
					<div class="search">
						<button class="btn btn-info btn-mini" style="float: right;" id="btnSearch">确定</button>
						<input type="text" placeholder="请输入上传人员" id="Search"/>
					</div>
				</div>
				<div class="control2">
					<button class="btn btn-primary btn-mini btn-14" id="ImportTable">模板导入</button>
				</div>
			</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>月份</th>
							<th>业务人员</th>
							<th>上传时间</th>
							<th>上传人员</th>
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
            //搜索
            $("#btnSearch").click(function () {
                GetExcelTable(1);
            });
            GetExcelTable(1);
            $("#ImportTable").click(function () {
                $.dialog({
                    type: 'confirm',
                    buttonText: {
                        titleText: '模板导入',
                        ok: '导入',
                        cancel: '取消'
                    },
                    onClickOk: function () {
                        Import();
                    },
                    contentHtml: '<p>1、请先<a class="red"  href="/Interface/DownLoad.ashx?action=reports">下载模板</a>，然后填入数据。</p><p>2、选择填入数据的模板文件，点击导入。</p><p><input type="file" id="files" style="padding:0 2px;border: 1px solid #ccc;"></p>'


                });
            });
       

        })

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
                url: "/Interface/ImportReports.ashx",
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
        //获取数据
        function GetExcelTable(pageindex) {
            $("tbody").empty();
            var data = {
                Month: $("#Month").val(),
                Search: $("#Search").val(),
                pageIndex: pageindex
            };
            $.ajax({
                url: 'BusinessData.aspx/Get',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    if (data.pagecount == 0 || data.data.length == 0) {
                        $("#tbody").empty();
                        $("#tbody").html("<tr><td colspan='5'><span style='text-align:center; color:red'>暂无数据</span></td></tr>");
                        $("#kkpager").hide();
                        return;
                    }
                    var result = '';

                    $("#kkpager").show();
                    for (var i = 0; i < data.data.length; i++) {
                        result += `<tr>
							<td>${data.data[i].Month || ""}</td>
							<td>${data.data[i].SalesManNumber || ""}</td>
							<td>${data.data[i].AddTime || ""}</td>
							<td>${data.data[i].Name || ""}</td>
							<td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='RepportsTable.aspx?mouth=${data.data[i].Month}'">查看详情</button></td>
						</tr>`;
                    }
                    $("#tbody").append(result);
                
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
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }

    </script>
</body>
</html>
