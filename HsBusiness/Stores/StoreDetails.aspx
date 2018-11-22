<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreDetails.aspx.cs" Inherits="HsBusiness.Stores.StoreDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/initial.css" />
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/dialog.css" />
    <link rel="stylesheet" type="text/css" href="../css/details-all.css" />
</head>
<body>
    <div class="Standard">
        <div class="widget-title" style="background-color: #fff">
            <p id="StoreName"></p>
            <div class="control2 control-left">
                <p>
                    <span>地址：</span><span id="StoreAddress"></span>
                </p>
            </div>
            <div class="control2">
                <p>
                    <span>最近更新时间：</span><span id="LastTime"></span>
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
                            <th>区县</th>
                            <th>宽带运营商</th>
                            <th>宽带价格（元）</th>
                            <th>到期时间</th>
                            <th>客户经理</th>
                            <th>联系人</th>
                            <th>联系电话</th>
                            <th>状态</th>
                        </tr>
                    </thead>
                    <tbody id="SInfo">
                        <%--	<tr>
							<td>安平</td>
							<td>移动</td>
							<td>700元/年</td>
							<td>2018-09-31</td>
							<td>李刚</td>
							<td>张三</td>
							<td>13712345678</td>
						</tr>--%>
                    </tbody>
                </table>
            </div>
            <div class="Contacts project-details">
                <div>
                    <p class="unit">门店照片：</p>
                    <p class="unit-text" id="storeimg">
                    </p>
                </div>
            </div>
        </div>
        <div class="Subtitle" id="box-d">
            <p>走访记录</p>
            <a href="javascript:void(0);" id="Takeup-4">点击收起</a>
            <a href="javascript:void(0);" id="Open-4" style="display: none;">点击展开</a>
        </div>
        <div class="box-D">
            <div class="Contacts">
                <div>
                    <span class="Name">预约时间:</span><span class="Name-data" id="BookTime"></span>
                    <span class="Name">状态:</span><span class="Name-data" id="state"></span>
                    <button class="btn btn-success btn-mini" id="addStoreVisit">新增走访</button>
                </div>
            </div>
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check ">
                    <thead>
                        <tr>
                            <th>拜访序数</th>
                            <th>沟通内容</th>
                            <th>负责人</th>
                            <th>手机号码</th>
                            <th>是否有业务需求</th>
                            <th>是否需要二次拜访</th>
                            <th>拜访时间</th>
                        </tr>
                    </thead>
                    <tbody id="vistinfo">
                    </tbody>
                </table>
            </div>
        </div>
        <div class="Subtitle" id="box-e">
            <p>回执记录</p>
            <a href="javascript:void(0);" id="Takeup-5">点击收起</a>
            <a href="javascript:void(0);" id="Open-5" style="display: none;">点击展开</a>
        </div>
        <div class="box-E">
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>提醒内容</th>
                            <th>提醒时间</th>
                            <th>回执内容</th>
                            <th>负责人</th>
                            <th>手机号码</th>
                            <th>回执时间</th>
                        </tr>
                    </thead>
                    <tbody id="receiptinfo">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/dialog.js"></script>
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
        var StoreId = 0;//门店id
        $(function () {
            StoreId = getQueryString("SID");//商机ID

            GetInfo();
            GetVist();
            GetRemind();
            $("#addStoreVisit").click(function () {
                location.href = 'VisitNewly.aspx?StoreID=' + StoreId;
            });
        })
        //基础信息
        function GetInfo() {
            var data = {
                ID: StoreId
            }
            $.ajax({
                url: 'StoreDetails.aspx/GetInfo',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    $("#StoreName").html(data.StoreName);
                    $('#StoreAddress').html(data.StoreAddress)
                    $("#LastTime").html(data.LastTime);
                    var result = `
                        <tr>
                            <td>${data.Areas || ""}</td>
                            <td>${data.Broadband || ""}</td>
                            <td>${data.Price || ""}</td>
                            <td>${data.OverTime || ""}</td>
                            <td>${data.CusManage || ""}</td>
                            <td>${data.ContactName || ""}</td>
                            <td>${data.ContactTel || ""}</td>
                            <td><a class ="${data.State == 0 ? "" : data.State == 1 ? "red" : data.State == 2 ? "green" : ""}"
                                    href="javascript:void(0);"  onclick="alertReceipt(${data.RemindID});"}">
                            ${data.State == 0 ? "正常" : data.State == 1 ? "已派单" : data.State == 2 ? "已回执" : ""}</a></td>
                        </tr>
                        `;

                    $("#SInfo").append(result);
                    var imginfo = '';
                    for (var i = 0; i < data.Img.length; i++) {
                        //124.239.149.190:8001
                        imginfo += `
                            <a href="http://192.168.100.144:8282/UploadFile/Images/${data.Img[i]}" target="_blank"><img src="http://192.168.100.144:8282/UploadFile/Images/${data.Img[i]}" width="200px" height="200px"></a>

                            `
                    }
                    $("#storeimg").append(imginfo);
                }
            });
        }
        //走访记录
        function GetVist() {
            var data = {
                ID: StoreId
            }
            $.ajax({
                url: 'StoreDetails.aspx/GetVistInfo',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.length > 0) {
                        $("#BookTime").html(data[data.length - 1].NextTime);
                        var state = data[data.length - 1].State;
                        var result = "";
                        result += `<a class ="red" href="javascript:void(0);" onclick="alertReceipt(${data[data.length - 1].RemindID})" >${state == 0 ? "正常" : state == 1 ? "已提醒" : state == 2 ? "已回执" : ''}</a>`;
                        $("#state").append(result);
                    }

                    var result = '';
                    //console.log(data.length)
                    for (var i = data.length - 1; i >= 0; i--) {

                        result += `
                        <tr>
                            <td>${i + 1}</td>
                            <td>${data[i].VisitContent || ""}</td>
                            <td>${data[i].FZR || ""}</td>
                            <td>${data[i].FZrTel || ""}</td>
                            <td>${data[i].IsNeed == 1 ? "是" : "否"}</td>
                            <td>${data[i].IsAgain == 1 ? "是" : "否"}</td>
                            <td>${data[i].VisitTime || ""}</td>
                        </tr>
                        `;
                    }
                    $("#vistinfo").append(result);

                }
            });
        }
        //回执记录
        function GetRemind() {
            var data = {
                ID: StoreId
            }
            $.ajax({
                url: 'StoreDetails.aspx/GetRemind',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data.Remind;

                    var result = "";
                    for (var i = 0; i < data.length; i++) {
                        result += `
                        <tr>
                            <td>${data[i].Type == 1 ? "宽带到期" : data[i].Type == 2 ? "拜访预约" : ""}</td>
                            <td>${data[i].AddTime || ""}</td>
                            <td>${data[i].Contents || ""}</td>
                            <td>${data[i].UserName || ""}</td>
                            <td>${data[i].UserTel || ""}</td>
                            <td>${data[i].ReceiptTime || ""}</td>

                        </tr>
                        `;
                    }
                    $("#receiptinfo").append(result);
                }
            });
        }


        //回执弹框
        function alertReceipt(RemindID) {
            console.log(RemindID);
            if (RemindID == 0) {
                alert("无需回执")
            } else {
                $.dialog({
                    type: 'confirm',
                    buttonText: {
                        titleText: '提醒回执',
                        ok: '提交',
                        cancel: '取消'
                    },
                    onClickOk: function () {
                        receipt(RemindID);
                    },
                    contentHtml: '<input type="text" class="span5" placeholder="请输入回执内容" id="Contents" />'
                });
            }
        }

        //回执
        function receipt(RemindID) {
            var data = {
                RemindID: RemindID,
                Contents: $("#Contents").val(),
            }
            $.ajax({
                url: 'StoreDetails.aspx/ReminderReceipt',
                data: JSON.stringify(data),
                contentType: "application/json;charset=utf-8",
                dataType: 'json',
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d)
                    alert(data.msg);
                    parent.refreshFrame();
                },
            })
        }
        //获取url
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>
</body>
</html>
