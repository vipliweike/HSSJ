<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectTable.aspx.cs" Inherits="HsBusiness.ProjectManage.ProjectTable" %>

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
                <form id="form1" runat="server">
                    <div class="control1">
                        <select class="input-small inline form-control" id="Areas" name="Areas">
                            <option value="">请选择区县</option>
                        </select>
                        <select class="input-small inline form-control" runat="server" id="State" name="State">
                            <option value="">请选择状态</option>
                            <option value="1">派单中</option>
                            <option value="2">已回执</option>
                        </select>
                        <div class="search">

                            <a class="btn btn-info btn-mini alink" style="float: right;" id="btnSearch">确定</a>
                            <input type="text" placeholder="请选择更新开始时间" name="sbirth" id="sbirth" data-birth="123" onclick="WdatePicker()" />
                            <span>至</span>
                            <input style="margin-left: 0;" type="text" placeholder="请选择更新结束时间" name="sbirth2" id="sbirth2" data-birth="123" onclick="WdatePicker()" />
                        </div>
                    </div>
                    <div class="control2">
                        <button class="btn btn-danger btn-mini" id="del" type="button">删除</button>
                        <button class="btn btn-primary btn-mini" type="button" onclick="Export()">导出</button>
                        <asp:LinkButton ID="LinkButton1" runat="server" Style="display: none" class="btn btn-primary btn-mini" OnClick="LinkButton1_Click">导出</asp:LinkButton>
                        <button class="btn btn-success btn-mini btn-14" id="ImportTable" type="button">模板导入</button>
                        <button class="btn btn-success btn-mini btn-14" id="ImportTopicsTable" type="button">专题模板导入</button>
                    </div>
                </form>
            </div>

            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check given-table">
                    <thead>
                        <tr>
                            <th>
                                <div class="checker" id="uniform-title-table-checkbox">
                                    <input type="checkbox" id="title-table-checkbox" name="title-table-checkbox" /></div>
                            </th>
                            <th class="given-th2">区县</th>
                            <th class="given-th3">项目简介</th>
                            <th class="given-th4">发布时间</th>
                            <th class="given-th5">最近更新时间</th>
                            <th class="given-th5">负责人</th>
                            <th class="given-th6">状态</th>
                            <th class="given-th6">类别</th>
                            <th class="given-th7">操作</th>
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
                    contentHtml: '<p>1、请先<a class="red"  href="/Interface/DownLoad.ashx?action=project">下载模板</a>，然后填入数据。</p><p>2、选择填入数据的模板文件，点击导入。</p><p><input type="file" id="files" style="padding:0 2px;border: 1px solid #ccc;"></p>'


                });
            });
            $("#ImportTopicsTable").click(function () {
                $.dialog({
                    type: 'confirm',
                    buttonText: {
                        titleText: '专题模板导入',
                        ok: '导入',
                        cancel: '取消'
                    },
                    onClickOk: function () {
                        ImportTopics();
                    },
                    contentHtml: '<p>1、请先<a class="red"  href="/Interface/DownLoad.ashx?action=topicsproject">下载模板</a>，然后填入数据。</p><p>2、选择填入数据的模板文件，点击导入。</p><p><input type="file" id="files1" style="padding:0 2px;border: 1px solid #ccc;"></p>'


                });
            });
            $("#btnSearch").click(function () {
                var starttime = $("#sbirth").val();
                var endtime = $("#sbirth2").val();

                if (starttime > endtime) {
                    alert('查询开始时间不能大于结束时间');
                    return;
                }
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
                        url: 'ProjectTable.aspx/BatchDel',
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
            getRegion();
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

        function ImportTopics() {
            var file = $("#files1").val();
            if (file == null || file.length == 0) {
                alert("请先选择上传文件");
                return false;
            }
            var formData = new FormData();
            formData.append("file", document.getElementById("files1").files[0]);
            var file = document.getElementById("files1").value;
            var form1 = document.getElementById("form");
            var ext = file.slice(file.lastIndexOf(".") + 1).toLowerCase();

            $.ajax({
                url: "/Interface/ImportTopicsProject.ashx",
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
                    url: 'ProjectTable.aspx/Delete',
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
                UpdateStartTime: $("#sbirth").val(),
                UpdateEndTime: $("#sbirth2").val(),
                pageIndex: pageindex
            };
            $.ajax({
                url: 'ProjectTable.aspx/Get',
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
							<td class ="ellipsis">${data.data[i].Areas || ""}</td>
                            <td class ="ellipsis">${data.data[i].Introduce || ""}</td>
                            <td>${data.data[i].AddTime.substr(0, 10) || ""}</td>
                            <td>${data.data[i].LastTime.substr(0, 10) || ""}</td>
                            <td>${data.data[i].UserName || ""}</td>                            
                            <td class ="${data.data[i].State == 0 ? "black" : data.data[i].State == 1 ? "red" : data.data[i].State == 2 ? "green" : ""}">${data.data[i].State == 0 ? "正常" : data.data[i].State == 1 ? "派单中" : data.data[i].State == 2 ? "已回执" : ""}</td>
                            <td>${data.data[i].Type == 1 ? "专题": "非专题"}</td>
                            <td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='ProjectDetails.aspx?ProID=${data.data[i].ID}'">查看详情</button><button class ="btn btn-danger btn-mini delete" onclick="Del(${data.data[i].ID})">删除</button></td>
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
