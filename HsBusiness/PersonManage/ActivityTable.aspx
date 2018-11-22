<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityTable.aspx.cs" Inherits="HsBusiness.PersonManage.ActivityTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/initial.css" />
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/uniform.css" />
    <link rel="stylesheet" type="text/css" href="../css/dialog.css" />
    <link rel="stylesheet" type="text/css" href="../css/table.css" />
    <link href="../css/kkpager_blue.css" rel="stylesheet" />
    <script src="../js/kkpager.js"></script>
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
                <form id="form1" runat="server">
                    <div class="control1">
                        <select class="input-small inline form-control" id="areas" name="areas">
                            <option value="">请选择区县</option>

                        </select>
                        <select class="input-small inline form-control" id="post" name="post">
                            <option value="">请选择岗位</option>
                            <option value="公司领导">公司领导</option>
                            <option value="政企部主管">政企部主管</option>
                            <option value="网格主管">网格主管</option>
                            <option value="区县经理">区县经理</option>
                            <option value="客户经理">客户经理</option>
                            <option value="行业经理">行业经理</option>
                        </select>
                        <div class="search">
                            <input type="text" placeholder="请选择开始时间" name="sbirth" id="sbirth" data-birth="123" onclick="WdatePicker()">
                            <span>至</span>
                            <input style="margin-left: 0;" type="text" placeholder="请选择结束时间" name="sbirth2" id="sbirth2" data-birth="123" onclick="WdatePicker()">
                        </div>
                        <div class="search">
                            <input type="text" placeholder="请输入搜索内容" id="Search" name="Search" />
                            <a class="btn btn-info btn-mini alink" style="float: right;" id="search">确定</a>
                        </div>
                    </div>
                    <div class="control2">
                        <button class="btn btn-primary btn-mini" onclick="Export()">导出</button>
                        <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none" class="btn btn-primary btn-mini" OnClick="LinkButton1_Click">导出</asp:LinkButton>
                    </div>
                </form>
            </div>
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            
                            <th>区县</th>
                            <th>岗位</th>
                            <th>负责人</th>
                            <th>联系电话</th>
                            <th>App登录次数</th>
                            <th>App登录最近时间</th>
                            <th>Web登录次数</th>
                            <th>Web登录最近时间</th>
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
    <script src="../js/dialog.js"></script>
    <script src="../js/example.js"></script>
    <script src="../js/jquery.uniform.js"></script>
    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/matrix.tables.js"></script>
    <script src="../js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        function GetExcelTable(pageindex) {
            $("tbody").empty();
            var data = {
                Areas: $("#areas option:selected").val(),
                Post: $("#post option:selected").val(),
                starttime: $("#sbirth").val(),
                endtime: $("#sbirth2").val(),
                Search: $("#Search").val(),
                LogType: $("#logintype").val(),
                pageIndex: pageindex
            };
            $.ajax({
                url: 'ActivityTable.aspx/GetInfo',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var d = $.parseJSON(data.d);
                    if (d.pagecount == 0 || d.data.length == 0) {
                        $("#tbody").empty();
                        $("#tbody").html("<tr><td colspan='8'><span style='text-align:center; color:red'>暂无数据</span></td></tr>");
                        $("#kkpager").hide();
                        return;
                    }
                    var result = '';

                    $("#kkpager").show();
                    for (var i = 0; i < d.data.length; i++) {
                        result += `<tr>
                            <td>${d.data[i].Areas || ""}</td>
							<td>${d.data[i].Post || ""}</td>
							<td>${d.data[i].Name || ""}</td>
							<td>${d.data[i].Mobile || ""}</td>
							<td>${d.data[i].AppLogCount || 0}</td>
							<td>${d.data[i].AppLogTime || ""}</td>
							<td>${d.data[i].WebLogCount || 0}</td>
							<td>${d.data[i].WebLogTime || ""}</td>

						</tr>`;
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
            $("#search").click(function () {
                var starttime = $("#sbirth").val();
                var endtime = $("#sbirth2").val();

                if (starttime > endtime) {
                    alert('查询开始时间不能大于结束时间');
                    return;
                }
                GetExcelTable(1);
            })
            getRegion();
            GetExcelTable(1);

        });
        //查询区县
        function getRegion() {
            $.ajax({
                url: 'ActivityTable.aspx/GetRegion',
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
                    $("#areas").append(result);
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
