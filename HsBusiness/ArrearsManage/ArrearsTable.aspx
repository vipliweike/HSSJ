<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArrearsTable.aspx.cs" Inherits="HsBusiness.ArrearsManage.ArrearsTable" %>

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
</head>
<body>
    <div class="Standard">
        <div class="widget-box">
            <div class="widget-title">
                <p>控制台</p>
                <div class="control1">
                    <select class="input-small inline form-control" id="Areas" name="Areas">
                        <option value="">请选择区县</option>
                    </select>
                    <select class="input-small inline form-control" id="State">
                        <option value="">请选择状态</option>
                        <option value="0">未读</option>
                        <option value="1">已读</option>
                    </select>
                    <div class="search">
                        <input type="text" placeholder="请输入客户名称" id="CusName" />
                    </div>
                    <div class="search">
                        <input type="text" placeholder="请输入维系人姓名" id="FzrName" />
                    </div>
                    <div class="search">
                        <button class="btn btn-info btn-mini" style="float: right;" id="btnSearch">确定</button>
                        <input type="text" placeholder="请输入搜索内容" id="Search" />
                    </div>
                </div>
                <div class="control2">
                    <button class="btn btn-danger btn-mini" id="del">删除</button>
                    <button class="btn btn-danger btn-mini" id="delall">全部删除</button>
                    <button class="btn btn-primary btn-mini btn-14" id="ImportTable" runat="server">模板导入</button>
                </div>
            </div>
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>
                                <div class="checker" id="uniform-title-table-checkbox">
                                    <input type="checkbox" id="title-table-checkbox" name="title-table-checkbox">
                                </div>
                            </th>
                            <th>用户号码</th>
                            <th>客户名称</th>
                            <th>维系人</th>
                            <th>责任区域</th>
                            <%--<th>用户类型大项</th>--%>
                            <th>入网时间</th>
                            <th>欠费帐期</th>
                            <th>联系电话</th>
                            <th>服务状态</th>
                            <th>欠费金额</th>
                            <%--<th>缴费期</th>--%>
                            <th>发布时间</th>
                            <th>状态</th>
                            <th>操作</th>
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
    <script src="../js/kkpager.js"></script>
    <script type="text/javascript">

        $(function () {
            //模板导入
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
                    contentHtml: '<p>1、请先<a class="red"  href="/Interface/DownLoad.ashx?action=arrears">下载模板</a>，然后填入数据。</p><p>2、选择填入数据的模板文件，点击导入。</p><p><input type="file" id="files" style="padding:0 2px;border: 1px solid #ccc;"></p>'
                });
            });

            $("#btnSearch").click(function () {
                GetExcelTable(1);
            });

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
                        url: 'ArrearsTable.aspx/BatchDel',
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
            //全部删除
            $("#delall").click(function () {
                if (window.confirm('你确定要全部删除吗(请谨慎操作！！)？')) {
                    $.ajax({
                        url: 'ArrearsTable.aspx/DeleteAll',
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        // data: JSON.stringify({ ids: checkedList.toString() }),
                        success: function (result) {
                            $("[name ='subChk']:checkbox").attr("checked", false);
                            var result = $.parseJSON(result.d);
                            alert(result.msg);
                            window.location.reload();
                        }
                    });

                }

            })
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

        //回执记录
        function remind(smid) {
            var data = {
                BaID: smid
            }
            $.ajax({
                url: 'ArrearsTable.aspx/GetRemind',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (result) {
                    var data = $.parseJSON(result.d).data;

                    $.dialog({
                        titleText: '已回执',
                        contentHtml: `<div><span>回执内容：</span><span>${data.ReceiptContent}</span></div><p><span>回执时间：</span><span>${data.ReceiptTime}</span></p>`
                    });
                }

            })
        }
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
                url: "/Interface/ImportArrears.ashx",
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
                    url: 'ArrearsTable.aspx/Delete',
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
        //获取数据
        function GetExcelTable(pageindex) {
            $("tbody").empty();
            var data = {
                Areas: $("#Areas").val(),
                State: $("#State").val(),
                CusName: $("#CusName").val(),
                FzrName: $("#FzrName").val(),
                Search: $("#Search").val(),
                pageIndex: pageindex
            };
            $.ajax({
                url: 'ArrearsTable.aspx/Get',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    if (data.pagecount == 0 || data.data.length == 0) {
                        $("#tbody").empty();
                        $("#tbody").html("<tr><td colspan='18'><span style='text-align:center; color:red'>暂无数据</span></td></tr>");
                        $("#kkpager").hide();
                        return;
                    }
                    var result = '';

                    $("#kkpager").show();
                    for (var i = 0; i < data.data.length; i++) {
                        result += `<tr>
							<td><div class ="checker" id="uniform-undefined"><input name="subChk" type="checkbox" class ="ID" hidden="hidden" value="${data.data[i].ID}" /></div></td>
							<td>${data.data[i].UserNumber || ""}</td>
                            <td>${data.data[i].CustomerName || ""}</td>
                            <td>${data.data[i].UserName || ""}</td>
                            <td>${data.data[i].Area || ""}</td>
                            <!--<td>${data.data[i].UserTypeItem || ""}</td>-->
                            <td>${data.data[i].InNetTime || ""}</td>
                            <td>${data.data[i].Period || ""}</td>
                            <td>${data.data[i].ContactTel || ""}</td>
                            <td>${data.data[i].ServiceStatus || ""}</td>
                            <td>${data.data[i].AmountOwed || ""}</td>
                            <!--<td>${data.data[i].Payment || ""}</td>-->
                            <td>${data.data[i].AddTime || ""}</td>
                            <!--<td><a href="javascript:void(0);"class ="${data.data[i].State == 0 ? "black" : data.data[i].State == 1 ? "red" : data.data[i].State == 2 ? "green" : ""}" onclick="remind(${data.data[i].ID})" >${data.data[i].State == 0 ? "正常" : data.data[i].State == 1 ? "已提醒" : data.data[i].State == 2 ? "已回执" : ""}</a></td>-->
                            <td class ="${data.data[i].IsRead == 0 ? 'red' : 'green'}">${data.data[i].IsRead == 0 ? '未读' : '已读'}</td>
                            <td>
                            <button class ="btn btn-success btn-mini" onclick="window.parent.test.location.href = 'ArrearsDetails.aspx?ArrID=${data.data[i].ID}&state=${data.data[i].IsRead}'">查看详情</button>
                            <button class ="btn btn-danger btn-mini delete" onclick="Del(${data.data[i].ID})">删除</button></td>

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
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        function Export() {
            document.getElementById("LinkButton1").click();

        }

    </script>
</body>
</html>
