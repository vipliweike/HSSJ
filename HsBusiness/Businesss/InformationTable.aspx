<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformationTable.aspx.cs" Inherits="HsBusiness.Businesss.PersonnelTable" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>衡水商机</title>
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
                <div class="control1" style="width: 700px;">
                    <select runat="server" class="input-small inline form-control" id="Areas" name="Areas">
                        <option value="">请选择区县</option>
                        <%--<option value="衡水市">衡水市</option>--%>
                    </select>
                    <select runat="server" class="input-small inline form-control" id="Industry" name="Industry">
                        <option value="">请选择行业</option>
                        <option value="企业">企业</option>
                        <option value="党政">党政</option>
                        <option value="医疗">医疗</option>
                        <option value="学校">学校</option>
                        <option value="军警">军警</option>
                        <option value="其他">其他</option>
                    </select>
                    <select runat="server" class="input-small inline form-control" id="Operator" name="Operator">
                        <option value="">请选择运营商</option>
                        <option value="电信">电信</option>
                        <option value="移动">移动</option>
                        <option value="联通">联通</option>
                        <option value="暂无">暂无</option>
                    </select>
                    <select runat="server" class="input-small inline form-control" id="Scale" name="Scale">
                        <option value="">请选择规模</option>
                        <option value="50以下">50以下</option>
                        <option value="50-100">50-100</option>
                        <option value="100-200">100-200</option>
                        <option value="200以上">200以上</option>
                    </select>
                    <div class="search">
                        <select class="input-small inline form-control" id="TimeType" name="TimeType">
							<option value="1">更新时间</option>
							<option value="2">创建时间</option>
						</select>
                        <input runat="server" type="text" placeholder="请选择更新开始时间" name="sbirth" id="sbirth" data-birth="123" class="input_text_200" onclick="WdatePicker()" />
                        <span>至</span>
                        <input runat="server" style="margin-left: 0;" type="text" placeholder="请选择更新结束时间" name="sbirth2" id="sbirth2" data-birth="123" class="input_text_200" onclick="WdatePicker()"/>
                        <script type="text/javascript" src="/js/My97DatePicker/WdatePicker.js"></script>
                    </div>

                    <div class="search">
                     <a class="btn btn-info btn-mini alink " style="float: right;" id="search" >确定</a>
                        <input runat="server" type="text" placeholder="请输入搜索内容" id="Search" name="Search" />
                     
                    </div>
                    
                </div>
              
                <div class="control2">
                    <button type="button" class="btn btn-danger btn-mini" id="del">删除</button>
                    	<button type="button" class="btn btn-primary btn-mini" onclick="Export()">导出</button>
               <%--  <form id="form1" runat="server">--%>
                    <%--<asp:Button id="btnExport" Visible="false" runat="server" class="btn btn-primary btn-mini" Text="导出" OnClick="btnExport_Click" />--%>
                        <asp:LinkButton ID="btnTest" style="display:none" class="btn btn-primary btn-mini" runat="server" OnClick="btnTest_Click">导出</asp:LinkButton>
                      <%--  <input hidden="hidden" runat="server" id="where" />--%>
              <%--     </form>--%>
                </div>
         </form>
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
                            <th>区县</th>
                            <th>客户名称</th>
                            <th>行业归属</th>
                            <th>用户规模</th>
                            <th>移动业务</th>
                            <th>固网业务</th>
                            <th>拜访次数</th>
                            <th>建档时间</th>
                            <th>最近更新时间</th>
                            <th>提交人</th>
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
     <script src="../js/browser.min.js"></script>
    <script src="../js/jquery.uniform.js"></script>
    <script src="../js/jquery.dataTables.min.js"></script>
    <script src="../js/matrix.tables.js"></script>
    <script src="../js/kkpager.js"></script>
    <script>
        $(function () {
            
            //get();
            $("#search").click(function () { GetExcelTable(1); })
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
                Industry: $("#Industry").val(),
                Operator: $("#Operator").val(),
                Scale: $("#Scale").val(),
                starttime: $("#sbirth").val(),
                endtime: $("#sbirth2").val(),
                Search: $("#Search").val(),
                pageIndex: pageindex,
                TimeType:$("#TimeType").val()
            };
           // $("#where").val(JSON.stringify(data));
            $.ajax({
                url: 'InformationTable.aspx/Get',
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
							<td>${d.data[i].CompanyName || ""}</td>
							<td>${d.data[i].Industry || ""}</td>
							
							<td>${d.data[i].CustomerScale || ""}</td>
							<td><a class ="${d.data[i].MState == 0 ? "" : d.data[i].MState == 1 ? "red" : d.data[i].MState == 2 ? "green" : ""}" ${d.data[i].IsMove==false?'style="color:black"': ""}  href="javascript:void(0);" onclick="window.parent.test.location.href='Details.aspx?BusID=${d.data[i].ID}&action=M'" >
                            `
                        if (d.data[i].IsMove) {
                            result += `
                                ${d.data[i].MState == 0 ? "正常" : d.data[i].MState == 1 ? "已提醒" : d.data[i].MState == "2" ? "已回执" : ""}
                                `;
                        } else {
                            result += `
                                暂无
                                `;
                        }

                        result += `</a></td>
							<td><a class ="${d.data[i].FState == 0 ? "" : d.data[i].FState == 1 ? "red" : d.data[i].FState == 2 ? "green" : ""}"  ${d.data[i].IsFixed==false?'style="color:black"': ""} href="javascript:void(0);" onclick="window.parent.test.location.href='Details.aspx?BusID=${d.data[i].ID}&action=F'">`
                        if (d.data[i].IsFixed) {
                            result += `
                            ${d.data[i].FState == 0 ? "正常": d.data[i].FState == 1 ? "已提醒": d.data[i].FState == "2" ? "已回执": ""}
                                `;
                        } else {
                            result += `
                                暂无
                                `;
                        }

                        result += `</a></td>
							<td><a href="javascript:void(0);" onclick="window.parent.test.location.href='Details.aspx?BusID=${d.data[i].ID}&action=V'">${d.data[i].VisitNum || 0}</a></td>
                            <td>${d.data[i].AddTime||""}</td>
							<td>${d.data[i].LastTime||""}</td>
                            <td>${d.data[i].UserName}</td>
							<td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='/Businesss/InformationNewly.aspx?id=${d.data[i].ID}'">编辑</button><button class="btn btn-danger btn-mini" onclick="del(${d.data[i].ID})">删除</button></td>
						</tr>
                            `
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

        //获取区县
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
                        window.location.reload();
                    }
                });
            }
            else {
                return false;
            }
        }
        //导出

        function Export()
        {
            document.getElementById("btnTest").click();
            //var data = {
            //    Areas: $("#Areas").val(),
            //    Industry: $("#Industry").val(),
            //    Operator: $("#Operator").val(),
            //    Scale: $("#Scale").val(),
            //    starttime: $("#sbirth").val(),
            //    endtime: $("#sbirth2").val(),
            //    Search: $("#Search").val()
            //   // pageIndex: pageindex
            //};
            //$.ajax({
            //    url: 'InformationTable.aspx/Export',
            //    dataType: "json",
            //    type: "POST",
            //    contentType: "application/json;charset=utf-8",
            //    data: JSON.stringify(data),
            //    success: function (data) {
            //        alert(123);
            //    }
         
            //});

        }

    </script>
</body>
</html>
