<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArrearsDetails.aspx.cs" Inherits="HsBusiness.ArrearsManage.ArrearsDetails" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>衡水商机</title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css">
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="/css/dialog.css">
    <link rel="stylesheet" type="text/css" href="/css/details-all.css">
</head>
<body>
    <div class="Standard">
        <div class="widget-title" style="background-color: #fff">
            <p id="CustomerName"></p>
            <%--<div class="control2 control-left">
				<p>
					<span>地址：</span><span id=""></span>
				</p>
			</div>--%>
            <div class="control2">
                <p>
                    <span>发布时间：</span><span id="AddTime"></span>
                </p>
            </div>
        </div>
        <div class="Subtitle" id="box-a">
            <p>基础信息</p>
            <a href="javascript:void(0);" id="Takeup-1">点击收起</a>
            <a href="javascript:void(0);" id="Open-1" style="display: none;">点击展开</a>
        </div>
        <div class="box-A">
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>用户号码</th>
                            <th>维系人</th>
                            <th>责任区域</th>
                            <th>用户类型大项</th>
                            <th>入网时间</th>
                            <th>欠费帐期</th>                            
                        </tr>
                    </thead>
                    <tbody id="tbody1">
                    </tbody>
                </table>
            </div>
        </div>
        <div class="Subtitle" id="box-d">
            <p>详细信息</p>
            <a href="javascript:void(0);" id="Takeup-4">点击收起</a>
            <a href="javascript:void(0);" id="Open-4" style="display: none;">点击展开</a>
        </div>
        <div class="box-D">
            <div class="Contacts">
                <div>
                    <%--<span class="Name">提醒时间:</span><span class="Name-data" id="RemindTime"></span>--%>
                    <span class="Name">状态:</span><span class="Name-data"><a class="red" href="javascript:void(0);" id="State"></a></span>
                </div>
            </div>
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>联系电话</th>
                            <th>服务状态</th>
                            <th>欠费金额</th>
                            <th>缴费期</th>
                            <th>装机地址</th>
                        </tr>
                    </thead>
                    <tbody id="tbody2">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/dialog.js"></script>
    <script type="text/javascript" src="/js/example.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#box-e").click(function () {
                $(".box-E").slideToggle("slow");
                $("#Takeup-5").toggle();
                $("#Open-5").toggle();
            });
        });
        $(document).ready(function () {
            $("#box-d").click(function () {
                $(".box-D").slideToggle("slow");
                $("#Takeup-4").toggle();
                $("#Open-4").toggle();
            });
        });
        $(document).ready(function () {
            $("#box-a").click(function () {
                $(".box-A").slideToggle("slow");
                $("#Takeup-1").toggle();
                $("#Open-1").toggle();
            });
        });
        var ArrID = 0;
        var state = 0;
        $(function () {
            ArrID = getParameter("ArrID");
            state = getParameter("state");
            getOne(ArrID);
            read(ArrID, state);
        });
        function read(id, state) {
            if (state == 0) {
                var data = {
                    ArrID: id,
                }
                $.ajax({
                    url: 'ArrearsTable.aspx/Read',
                    data: JSON.stringify(data),
                    dataType: 'json',
                    contentType: "application/json;charset=utf-8",
                    type: 'post',
                    success: function (data) {
                        //var data = $.parseJSON(data.d);
                        //if (data.state == 1) {
                        //    window.parent.test.location.href = 'BalanceDetails.aspx?BalID=' + SmBaID;
                        //}
                    }
                });
            } else {
                //window.parent.test.location.href = 'BalanceDetails.aspx?BalID=' + SmBaID;
            }
        }
        function getOne(ArrID) {
            var data = {
                ArrID: ArrID
            }
            $.ajax({
                url: 'ArrearsDetails.aspx/GetOne',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    $("#CustomerName").html(data.CustomerName);
                    $("#AddTime").html(data.AddTime)
                    var result1 = `
                        <tr>
							<td>${data.UserNumber}</td>
							<td>${data.UserName}</td>
							<td>${data.Area}</td>
							<td>${data.UserTypeItem}</td>
							<td>${data.InNetTime}</td>
							<td>${data.Period}</td>
						</tr>
                    `;
                    $("#tbody1").append(result1);
                    $("#RemindTime").html(data.RemindTime);
                    $("#State").html(`${data.IsRead == 0 ? "未读" : data.IsRead == 1 ? "已读" : ""}`);
                    var result2 = `
                        <tr>
                            <td>${data.ContactTel}</td>
							<td>${data.ServiceStatus}</td>
							<td>${data.AmountOwed}</td>
							<td>${data.Payment}</td>
							<td>${data.InstalledAddress}</td>
						</tr>
                    `;
                    $("#tbody2").append(result2);

                }
            });
        }
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>
</body>
</html>
