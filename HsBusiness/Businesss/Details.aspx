<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="HsBusiness.Businesss.Details" %>

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
            <p id="CompanyName"></p>
            <div class="control2">
                <p>
                    <span>最近更新时间：</span><span id="LastTime"></span>
                </p>
            </div>
        </div>
        <div class="Subtitle" id="box-a">
            <p>基础信息</p>
            <a href="javascript:void(0);" id="Takeup-1" style="display: none;">点击收起</a>
            <a href="javascript:void(0);" id="Open-1">点击展开</a>
        </div>
        <div class="box-A info">
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>区县</th>
                            <th>客户名称</th>
                            <th>是否有员工通讯录</th>
                            <th>客户地址</th>
                            <th>行业归属</th>
                            <th>用户规模</th>
                            <th>备注</th>
                        </tr>
                    </thead>
                    <tbody id="Binfo">
                    </tbody>
                </table>
            </div>
            <div class="Contacts" id="Contacts">
            </div>
        </div>
        <div class="Subtitle" id="box-b">
            <p>移动业务</p>
            <a href="javascript:void(0);" id="Takeup-2" style="display: none;">点击收起</a>
            <a href="javascript:void(0);" id="Open-2">点击展开</a>
        </div>
        <div class="box-B info">
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>手机卡用途</th>
                            <th>可推业务</th>
                            <th>联通业务占比</th>
                            <th>移动业务占比</th>
                            <th>电信业务占比</th>
                            <th>员工年龄段</th>
                            <th>是否有员工补贴</th>
                            <th>使用政策</th>
                            <th>使用月消费</th>
                            <th>年收入预测</th>
                            <th>有无在用其他业务</th>
                            <th>到期时间</th>
                            <th>状态</th>
                        </tr>
                    </thead>
                    <tbody id="Minfo">
                    </tbody>
                </table>
            </div>
        </div>
        <div class="Subtitle" id="box-c">
            <p>固网业务</p>
            <a href="javascript:void(0);" id="Takeup-3" style="display: none;">点击收起</a>
            <a href="javascript:void(0);" id="Open-3">点击展开</a>
        </div>
        <div class="box-C info">
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>业务类别</th>
                            <th>规模</th>
                            <th>是否跨域</th>
                            <th>预计周价（元）</th>
                            <th>预计入网月份</th>
                            <th>年收益</th>
                            <th>合作运营商</th>
                            <th>在用业务</th>
                            <th>规模</th>
                            <th>周价（元）</th>
                            <th>到期时间</th>
                            <th>状态</th>
                        </tr>
                    </thead>
                    <tbody id="Finfo">
                    </tbody>
                </table>
            </div>
        </div>
        <div class="Subtitle" id="box-d">
            <p>走访记录</p>
            <a href="javascript:void(0);" id="Takeup-4" style="display: none;">点击收起</a>
            <a href="javascript:void(0);" id="Open-4">点击展开</a>
        </div>
        <div class="box-D info">
            <div class="Contacts">
                <div>
                    <span class="Name">预约时间:</span><span class="Name-data" id="NextTime"></span>
                    <span class="Name">状态:</span><span class="Name-data red" id="VisitState"></span>
                    <button class="btn btn-success btn-mini" id="addVisit" >新增走访</button>
                </div>

            </div>
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>拜访序数</th>
                            <th>沟通内容</th>
                            <th>负责人</th>
                            <th>手机号码</th>
                            <th>是否有业务需求</th>
                            <th>是否需要二次拜访</th>
                            <th>是否需要公司领导拜访</th>
                            <th>需要协同领导</th>
                            <th>拜访时间</th>
                            <th>图片信息</th>
                        </tr>
                    </thead>
                    <tbody id="Vinfo">
                    </tbody>
                </table>
            </div>
        </div>
        <div class="Subtitle" id="box-e">
            <p>回执记录</p>
            <a href="javascript:void(0);" id="Takeup-5" style="display: none;">点击收起</a>
            <a href="javascript:void(0);" id="Open-5">点击展开</a>
        </div>
        <div class="box-E info">
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
                    <tbody id="Rinfo">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
    <script src="../js/browser.min.js"></script>
    <script type="text/javascript" src="/js/dialog.js"></script>
    <script type="text/javascript" src="/js/example.js"></script>
    <script type="text/javascript">

        var BusID = 0;
        var action;
        var IsMove, IsFixed;//是否拥有移动、固网业务
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
            $("#box-c").click(function () {
                $(".box-C").slideToggle("slow");
                $("#Takeup-3").toggle();
                $("#Open-3").toggle();
            });
        });
        $(document).ready(function () {
            $("#box-b").click(function () {
                $(".box-B").slideToggle("slow");
                $("#Takeup-2").toggle();
                $("#Open-2").toggle();
            });
        });
        $(document).ready(function () {
            $("#box-a").click(function () {
                $(".box-A").slideToggle("slow");
                $("#Takeup-1").toggle();
                $("#Open-1").toggle();
            });
        });


        $(function () {
            BusID = getQueryString("BusID");//商机ID
            action = getQueryString("action");//动作
            $(".info").hide();
            getBasis();
            getMove();
            getFixed();
            getVisit();
            getRemind();
            if (action == "M") { $("#box-b").click(); }//移动
            if (action == "F") { $("#box-c").click(); }//固网
            if (action == "V") { $("#box-d").click(); }//回访

            $("#addVisit").click(function () {
                window.parent.test.location.href = 'VisitNewly.aspx?BusID=' + BusID;
            });

        });
        //监听点击
        document.addEventListener("click", function () {
            IshaveWork();
        });
        //判断是否拥有业务（移动、固网）
        function IshaveWork() {
            if (IsMove) {
                $("#box-b").click(function () { return false; })
            }
            if (IsFixed) {
                $("#box-c").click(function () { return false; });
            }
        }

        function addVisit() {
            alert("添加成功");
        }




        //基础信息
        function getBasis() {
            var data = {
                ID: BusID
            }
            $.ajax({
                url: 'Details.aspx/GetInfo',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    $("#CompanyName").html(data.CompanyName);
                    $("#LastTime").html(data.LastTime);
                    var result = `
                        <tr>
                            <td>${data.Areas || ""}</td>
                            <td>${data.CompanyName || ""}</td>
                            <td>${data.IsHavePhoneList == 1 ? "是" : "否"}</td>
                            <td>${data.CompanyAddress || ""}</td>
                            <td>${data.Industry || ""}</td>
                            <td>${data.CustomerScale || ""}</td>
                            <td>${data.Remark||""}</td>
                        </tr>

                        `;
                    $("#Binfo").append(result);
                    var Contacts = "";
                    for (var i = 0; i < data.Contacts.length; i++) {
                        Contacts += `
                            <div>
                                <span class ="Name">联系人&ensp;: </span><span class="Name-data">${data.Contacts[i].Name}</span><span class ="Name">岗位: </span><span class="Name-data">${data.Contacts[i].Post}</span><span class ="Name">联系电话: </span><span class="Name-data">${data.Contacts[i].Tel}</span>
                            </div>
                            `;
                    }
                    $("#Contacts").append(Contacts);
                }
            });
        }

        //移动
        function getMove() {
            var data = {
                ID: BusID
            }
            $.ajax({
                url: 'Details.aspx/GetMove',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.IsMove) {
                        var result = `
                        <tr>
                            <td>${data.MCardUse || ""}</td>
                            <td>${data.MPushWork || ""}</td>
                            <td>${data.MUnicom || ""}</td>
                            <td>${data.MMobile || ""}</td>
                            <td>${data.MTelecom || ""}</td>
                            <td>${data.MAgeGroup || ""}</td>
                            <td>${data.MIsSubsidy == 1 ? "是" : "否"}</td>
                            <td>${data.MPolicy || ""}</td>
                            <td>${data.MMonthFee || ""}</td>
                            <td>${data.MIncome || ""}</td>
                            <td>${data.MOtherWork || ""}</td>
                            <td>${data.MOverTime || ""}</td>
                             <td><a class ="${data.MState == 0 ? "" : data.MState == 1 ? "red" : data.MState == 2 ? "green" : ""}"
                                    href="javascript:void(0);"  onclick="alertReceipt(${data.RemindID});"}">
                            ${data.MState == 0 ? "正常": data.MState == 1 ? "已提醒": data.MState == 2 ? "已回执": ""}</a></td>
                        </tr>
                        `;
                        $("#Minfo").append(result);
                    }
                    else if (action == "M") {
                        alert("暂无移动业务");
                    }
                }

            });
        }
        //固网
        function getFixed() {
            var data = {
                ID: BusID
            }
            $.ajax({
                url: 'Details.aspx/GetFixed',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.IsFixed) {
                        var result = `
                       <tr>
                            <td>${data.FPushWork || ""}</td>
                            <td>${data.FScale || ""}</td>
                            <td>${data.FIsDomain == 1 ? "是" : "否"}</td>
                            <td>${data.FPreWeekPrice || ""}</td>
                            <td>${data.FPreInNetMouth || ""}</td>
                            <td>${data.FAlsAnlIncome || ""}</td>
                            <td>${data.FOperator || ""}</td>
                            <td>${data.FUseWork || ""}</td>
                            <td>${data.FUseScale || ""}</td>
                            <td>${data.FWeekPrice || ""}</td>
                            <td>${data.FOverTime || ""}</td>
                            <td><a class ="${data.FState == 0 ? "" : data.FState == 1 ? "red" : data.FState == 2 ? "green" : ""}"
                                    href="javascript:void(0);"  onclick="alertReceipt(${data.RemindID});">
                            ${data.FState == 0 ? "正常": data.FState == 1 ? "已提醒": data.FState == 2 ? "已回执": ""}</a></td>
                        </tr>
                        `;
                        $("#Finfo").append(result);
                    } else if (action == "F") {
                        alert("暂无固网业务");
                    }

                }

            });
        }

        //走访记录
        function getVisit() {
            var data = {
                ID: BusID
            }
            $.ajax({
                url: 'Details.aspx/GetVisit',
                data: JSON.stringify(data),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d).data.Visit;
                    if (data.length > 0) {
                        var da=data[data.length - 1];
                        $("#NextTime").html(data[data.length - 1].NextTime);
                        var state = data[data.length - 1].State;
                        var result = "";
                        result += `<a class ="red" href="javascript:void(0);" onclick="alertReceipt(${da.RemindID})" >${state == 0 ? "正常" : state == 1 ? "已提醒" : state == 2 ? "已回执" : ''}</a>`;
                        $("#VisitState").append(result);
                    }
                    var result = "";
                    for (var i = data.length - 1; i >= 0; i--) {
                        result += `
                        <tr>
                            <td>${i + 1}</td>
                            <td>${data[i].VisitContent || ""}</td>
                            <td>${data[i].UserName || ""}</td>
                            <td>${data[i].UserTel || ""}</td>
                            <td>${data[i].IsNeed == 1 ? "是" : "否"}</td>
                            <td>${data[i].IsAgain == 1 ? "是" : "否"}</td>
                            <td>${data[i].IsLeader == 1 ? "是" : "否"}</td>
                            <td>${data[i].Leader != null ? data[i].Leader : ""}</td>
                            <td>${data[i].VisitTime || ""}</td>
                            <td>`
                        for (var j = 0; j < data[i].Img.length; j++) {
                            result += `
                                <a href="http://124.239.149.190:8001/UploadFile/Images/${data[i].Img[j]}" target="_blank">图片${j + 1}</a>
                                `;
                        }
                        `
                        </tr>
                        `;
                    }

                    $("#Vinfo").append(result);
                }

            });
        }
        //获取回执
        function getRemind() {

            var data = {
                ID: BusID
            }
            $.ajax({
                url: 'Details.aspx/GetRemind',
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
                                <td>${data[i].Type == 1 ? "移动业务" : data[i].Type == 2 ? "固网业务" : data[i].Type == 3 ? "预约提醒" : ""}</td>
                                <td>${data[i].AddTime || ""}</td>
                                <td>${data[i].Contents || ""}</td>
                                <td>${data[i].UserName || ""}</td>
                                <td>${data[i].UserTel || ""}</td>
                                <td>${data[i].ReceiptTime || ""}</td>
                            </tr>
                            `;
                    }
                    $("#Rinfo").append(result);
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
                url: 'Details.aspx/ReminderReceipt',
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


        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>

</body>
</html>
