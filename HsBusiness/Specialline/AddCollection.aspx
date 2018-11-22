<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddCollection.aspx.cs" Inherits="HsBusiness.Specialline.AddCollection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/initial.css" />
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css" />
    <link href="../js/control/css/zyUpload.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../css/Newly.css" />
    <style type="text/css">
        .comments {
            width: 100%; /*自动适应父布局宽度*/
            overflow: auto;
            word-break: break-all;
            /*在ie中解决断行问题(防止自动变为在一行显示，主要解决ie兼容问题，ie8中当设宽度为100%时，文本域类容超过一行时，
当我们双击文本内容就会自动变为一行显示，所以只能用ie的专有断行属性“word-break或word-wrap”控制其断行)*/
        }
    </style>

</head>
<body>
    <div class="Standard">
        <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="fa fa-align-justify"></i></span>
                <p>编辑</p>
            </div>
            <div class="widget-content nopadding">
                <form action="#" method="get" class="form-horizontal">
                    <div class="span6-box">
                        <div class="span6">
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>单位名称 :</label>
                                <div class="controls">
                                    <input type="text" class="span4" placeholder="请输入单位名称" id="CompanyName">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>单位地址 :</label>
                                <div class="controls">
                                    <input type="text" class="span4" placeholder="请输入单位地址" id="CompanyAddress">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>电脑台数（台） :</label>
                                <div class="controls">
                                    <input type="text" class="span2" placeholder="请输入电脑台数" id="ComputerNumber">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">状态 :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="state">
                                        <option value="">请选择状态</option>
                                        <option value="0">跟进</option>
                                        <option value="1">落单</option>
                                        <option value="2">放弃</option>
                                    </select>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">&nbsp;</span>备注 :</label>
                                <div class="controls">
                                    <%--<input type="text" class="span2" placeholder="请输入备注" id="remark"/>--%>
                                    <textarea rows="8" cols="8" style="width: 300px; height: 150px" id="remark"></textarea>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>关键人1 :</label>
                                <div class="controls">
                                    <input type="text" class="span2" placeholder="请输入姓名" id="Name1">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>岗位 :</label>
                                <div class="controls">
                                    <input type="text" class="span2" placeholder="请输入岗位" id="Post1">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>手机号 :</label>
                                <div class="controls">
                                    <input type="text" class="span3" placeholder="请输入负责人联系号码" id="Tel1" maxlength="11">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"></label>
                                <div class="controls">
                                    <button class="btn btn-info addContacts" onclick="initContacts(2,'Contacts2')">继续添加关键人</button>
                                </div>
                            </div>
                            <div id="Contacts2" class="Contacts">
                            </div>
                            <div id="Contacts3" class="Contacts">
                            </div>
                        </div>

                    </div>
                    <div id="info">
                        <div id="Dedicate1" class="Dedicate1">
                            <div class="Subtitle" id="box-1">
                                <p>信息1(专线，电路或云业务)</p>
                                <a href="javascript:void(0);" id="Takeup-1">点击收起</a>
                                <a href="javascript:void(0);" id="Open-1" style="display: none;">点击展开</a>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>类型 :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="lrtype1">
                                        <option value="1">专线</option>
                                        <option value="2">电路</option>
                                        <option value="3">云业务</option>
                                    </select>
                                </div>
                            </div>
                            <div class="span6-box BOX-1">
                                <input style="display: none;" id="PlInfoID1" />
                                <div class="control-group" id="DivOperator1">
                                    <label class="control-label"><span class="red">*&nbsp;</span>合作运营商 :</label>
                                    <div class="controls">
                                        <select class="input-small inline form-control" id="Operator1">
                                            <option value="">请选择运营商</option>
                                            <option value="移动">移动</option>
                                            <option value="联通">联通</option>
                                            <option value="电信">电信</option>
                                            <option value="广电">广电</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group" id="DivWeekprice1">
                                    <label class="control-label"><span class="red">*&nbsp;</span>周价（元） :</label>
                                    <div class="controls">
                                        <input type="text" class="span2" placeholder="请输入周价" id="weekprice1">
                                    </div>
                                </div>
                                <div class="control-group" id="DivPaytype1">
                                    <label class="control-label"><span class="red">*&nbsp;</span>付费方式 :</label>
                                    <div class="controls">
                                        <select class="input-small inline form-control" id="paytype1">
                                            <option value="">请选择运营商</option>
                                            <option value="月缴">月缴</option>
                                            <option value="季度缴">季度缴</option>
                                            <option value="年缴">年缴</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group" id="DivOvertime1">
                                    <label class="control-label"><span class="red">*&nbsp;</span>到期时间 :</label>
                                    <div class="controls">
                                        <input type="text" placeholder="请选择到期时间" name="sbirth" data-birth="123" class="input_text_200" onclick="WdatePicker()" readonly="readonly" id="overtime1" />
                                    </div>
                                </div>
                                <div class="control-group" id="DivBroadband1">
                                    <label class="control-label"><span class="red">*&nbsp;</span>带宽（M） :</label>
                                    <div class="controls">
                                        <input type="text" class="span2" placeholder="请输入带宽" id="broadband1">
                                    </div>
                                </div>
                                <div class="control-group" id="DivServerBerSys1" hidden="hidden">
                                    <label class="control-label"><span class="red">*&nbsp;</span>服务器承载系统 :</label>
                                    <div class="controls">
                                        <input type="text" class="span2" placeholder="服务器承载系统" id="ServerBerSys1" />
                                    </div>
                                </div>
                                <div class="control-group" id="DivServerUsingTime1" hidden="hidden">
                                    <label class="control-label"><span class="red">*&nbsp;</span>现服务器开始使用时间 :</label>
                                    <div class="controls">
                                        <input type="text" placeholder="请选择现服务器开始使用时间" name="sbirth" data-birth="123" class="input_text_200" onclick="selectMonth()" readonly="readonly" id="ServerUsingTime1" />
                                    </div>
                                </div>
                                <div class="control-group" id="DivIsCloudPlan1" hidden="hidden">
                                    <label class="control-label"><span class="red">*&nbsp;</span>是否有上云计划 :</label>
                                    <div class="controls">
                                        <select class="input-small inline form-control" id="IsCloudPlan1">
                                            <option value="">请选择</option>
                                            <option value="0">否</option>
                                            <option value="1">是</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label"></label>
                                    <div class="controls">
                                        <button class="btn btn-info addDedicate">继续添加专线，电路或云业务信息</button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div id="vis">
                        <div class="Subtitle" id="box-f">
                            <p>走访记录</p>
                            <a href="javascript:void(0);" id="Takeup-6">点击收起</a>
                            <a href="javascript:void(0);" id="Open-6" style="display: none;">点击展开</a>
                        </div>
                        <div class="span6-box box-F">
                            <div class="span6">
                                <div class="control-group">
                                    <label class="control-label">是否有业务需求 :</label>
                                    <div class="controls">
                                        <input type="radio" name="IsNeed" value="1" checked="checked" />是
                            <input type="radio" name="IsNeed" value="0" />否
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">是否需要再次拜访 :</label>
                                    <div class="controls">
                                        <input type="radio" name="IsAgain" value="1" checked="checked" />是
                            <input type="radio" name="IsAgain" value="0" />否
                                    </div>
                                </div>
                                <div class="control-group" id="next">
                                    <label class="control-label">下次拜访时间 :</label>
                                    <div class="controls">
                                        <input type="text" placeholder="请选择下次拜访时间" name="sbirth" id="NextTime" data-birth="123" class="input_text_200" onclick="WdatePicker()" readonly="readonly" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-actions">
                        <button class="btn btn-success" onclick="save();">提交</button>
                        <button class="btn btn-danger" id="cancel">取消</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
    <input hidden="hidden" type="button" id="hideBtn" />
    <script src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="../js/My97DatePicker/WdatePicker.js"></script>
    <script src="../js/core/zyFile.js"></script>
    <script type="text/javascript" src="../js/control/js/zyUpload.js"></script>
    <script type="text/javascript" src="../js/core/jq22.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#box-f").click(function () {
                $(".box-F").slideToggle("slow");
                $("#Takeup-6").toggle();
                $("#Open-6").toggle();
            });
        });
        $(document).ready(function () {
            $(document).on("click", "#box-5", function () {
                $(".BOX-5").slideToggle("slow");
                $("#Takeup-5").toggle();
                $("#Open-5").toggle();
            });
        });
        $(document).ready(function () {
            $(document).on("click", "#box-4", function () {
                $(".BOX-4").slideToggle("slow");
                $("#Takeup-4").toggle();
                $("#Open-4").toggle();
            });
        });
        $(document).ready(function () {
            $(document).on("click", "#box-3", function () {
                $(".BOX-3").slideToggle("slow");
                $("#Takeup-3").toggle();
                $("#Open-3").toggle();
            });
        });
        $(document).ready(function () {
            $(document).on("click", "#box-2", function () {
                $(".BOX-2").slideToggle("slow");
                $("#Takeup-2").toggle();
                $("#Open-2").toggle();
            });
        });
        $(document).ready(function () {
            $("#box-1").click(function () {
                $(".BOX-1").slideToggle("slow");
                $("#Takeup-1").toggle();
                $("#Open-1").toggle();
            });

        });
    </script>
    <script type="text/javascript">

        var id = 0;
        var ContactsNum = 1;//联系人数量
        var DedicateNum = 1;//专线数量
        //初始化
        $(function () {
            $("form").submit(function () { return false; })
            id = getQueryString("id");
            if (id != 0) {
                getOne(id);
                $("#vis").hide();
            }
            $("#cancel").click(function () {
                location.href = "/Specialline/CollectionTable.aspx";
            });
            //判断所选类型（专线，电路，云业务）
            $(document).on('change', 'select[id^="lrtype"]', function () {
                var n = this.id.substr("lrtype".length, this.id.length - "lrtype".length);//获取id

                var Operator = $("#Operator" + n + " option[value='广电']")               
                if (this.value == 1) {
                    Operator.show()
                } else {
                    Operator.hide()
                }


                if (this.value == "3") {
                    $('#DivOperator' + n).hide();
                    $('#DivWeekprice' + n).hide();
                    $('#DivPaytype' + n).hide();
                    $('#DivOvertime' + n).hide();
                    $('#DivBroadband' + n).hide();

                    $('#Operator' + n).val('');
                    $('#weekprice' + n).val('');
                    $('#paytype' + n).val('');
                    $('#overtime' + n).val('');
                    $('#broadband' + n).val('');

                    $('#DivServerBerSys' + n).show();
                    $("#DivServerUsingTime" + n).show();
                    $("#DivIsCloudPlan" + n).show();
                }
                if (this.value == "1" || this.value == "2") {
                    $('#DivServerBerSys' + n).hide();
                    $("#DivServerUsingTime" + n).hide();
                    $("#DivIsCloudPlan" + n).hide();

                    $('#ServerBerSys' + n).val('');
                    $("#ServerUsingTime" + n).val('');
                    $("#IsCloudPlan" + n).val('');
                    $('#DivOperator' + n).show();
                    $('#DivWeekprice' + n).show();
                    $('#DivPaytype' + n).show();
                    $('#DivOvertime' + n).show();
                    $('#DivBroadband' + n).show();
                }
            }

            )
            $("input[name='IsAgain']").change(function () {

                var isagain = $("input[name='IsAgain']:checked").val();
                if (isagain == "1") {
                    $("#next").show();
                }
                else {
                    $("#next").hide();
                }
            });
            //判断当前选择时间是否小于是当前实际时间
            function contrastTime(start) {
                var evalue = $("#NextTime").val();
                var dB = new Date(evalue.replace(/-/g, "/"));//获取当前选择日期
                var d = new Date();
                var str = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();//获取当前实际日期
                if (Date.parse(str) > Date.parse(dB)) {//时间戳对比

                    return 1;
                }
                return 0;
            }
            //作业开始时间失去焦点验证
            $('#NextTime').blur(function () {
                var ret = contrastTime("NextTime");//获取返回值
                if (ret == 1) {
                    alert("下次拜访时间不能小于当前时间");
                    $(this).val('').focus();
                    return;
                }
            });
            $(document).on("click", ".addDedicate", function () {

                if (DedicateNum < 15) {
                    initDedicate(DedicateNum + 1);
                }
                else {
                    alert("最多有15条专线，电路或云业务信息");
                }

            });
            //作业开始时间失去焦点验证
            $('#overtime1').blur(function () {
                var ret = contrastTime("overtime1");//获取返回值
                if (ret == 1) {
                    alert("到期时间不能小于当前时间");
                    $(this).val('').focus();
                    return;
                }
            });
            //2
            $(document).on("blur", "#overtime2", function () {
                var ret = contrastTime("#overtime2");//获取返回值
                if (ret == 1) {
                    alert("到期时间不能小于当前时间");
                    $(this).val('').focus();
                    return;
                }
            });
            $(document).on("blur", "#overtime3", function () {
                var ret = contrastTime("#overtime3");//获取返回值
                if (ret == 1) {
                    alert("到期时间不能小于当前时间");
                    $(this).val('').focus();
                    return;
                }
            });
            $(document).on("blur", "#overtime4", function () {
                var ret = contrastTime("#overtime4");//获取返回值
                if (ret == 1) {
                    alert("到期时间不能小于当前时间");
                    $(this).val('').focus();
                    return;
                }
            });
            $(document).on("blur", "#overtime5", function () {
                var ret = contrastTime("#overtime5");//获取返回值
                if (ret == 1) {
                    alert("到期时间不能小于当前时间");
                    $(this).val('').focus();
                    return;
                }
            });
        });
        //日期控件只显示到月份
        function selectMonth() {
            WdatePicker({ dateFmt: 'yyyy-MM', isShowToday: false, isShowClear: false });
        }
        //判断当前选择时间是否小于是当前实际时间
        function contrastTime(start) {
            var evalue = $(start).val();
            var dB = new Date(evalue.replace(/-/g, "/"));//获取当前选择日期
            var d = new Date();
            var str = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();//获取当前实际日期
            if (Date.parse(str) > Date.parse(dB)) {//时间戳对比
                return 1;
            }
            return 0;
        }
        //查询
        function getOne(id) {

            var data = { ID: id };
            $.ajax({
                url: 'AddCollection.aspx/GetOne',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    $("#CompanyName").val(data.CompanyName);
                    $("#CompanyAddress").val(data.CompanyAddress);
                    $("#ComputerNumber").val(data.CompanyScale);
                    $("#remark").val(data.Remark);//备注
                    $("#state").val(data.State);//状态
                    //联系人
                    $("#Name1").val(data.Contacts[0].Name);
                    $("#Tel1").val(data.Contacts[0].Tel);
                    $("#Post1").val(data.Contacts[0].Post);
                    for (var i = 1; i < data.Contacts.length; i++) {
                        console.log("Contacts" + (i + 1));
                        initContacts(i + 1, "Contacts" + (i + 1));
                        $("#Name" + (i + 1)).val(data.Contacts[i].Name)
                        $("#Tel" + (i + 1)).val(data.Contacts[i].Tel)
                        $("#Post" + (i + 1)).val(data.Contacts[i].Post)
                    }
                    ContactsNum = data.Contacts.length;
                    lxrAddOrDel();
                    //专线
                    $("#PlInfoID1").val(data.Dedicate[0].ID);
                    $("#Operator1").val(data.Dedicate[0].Operator);
                    $("#weekprice1").val(data.Dedicate[0].WeekPrice);
                    $("#broadband1").val(data.Dedicate[0].BandWidth);
                    $("#paytype1").val(data.Dedicate[0].PayType);
                    $("#overtime1").val(data.Dedicate[0].OverTime);
                    $("#lrtype1").val(data.Dedicate[0].Type);
                    $("#ServerBerSys1").val(data.Dedicate[0].ServerBerSys);
                    $("#ServerUsingTime1").val(data.Dedicate[0].ServerUsingTime);
                    $("#IsCloudPlan1").val(data.Dedicate[0].IsCloudPlan);
                    $("#lrtype1").change();
                    for (var i = 1; i < data.Dedicate.length; i++) {
                        console.log("Dedicate" + (i + 1));
                        initDedicate(i + 1, "Dedicate" + (i + 1));
                        $("#PlInfoID" + (i + 1)).val(data.Dedicate[i].ID);
                        $("#Operator" + (i + 1)).val(data.Dedicate[i].Operator);
                        $("#weekprice" + (i + 1)).val(data.Dedicate[i].WeekPrice);
                        $("#broadband" + (i + 1)).val(data.Dedicate[i].BandWidth);
                        $("#paytype" + (i + 1)).val(data.Dedicate[i].PayType);
                        $("#overtime" + (i + 1)).val(data.Dedicate[i].OverTime);
                        $("#lrtype" + (i + 1)).val(data.Dedicate[i].Type);
                        $("#ServerBerSys" + (i + 1)).val(data.Dedicate[i].ServerBerSys);
                        $("#ServerUsingTime" + (i + 1)).val(data.Dedicate[i].ServerUsingTime);
                        $("#IsCloudPlan" + (i + 1)).val(data.Dedicate[i].IsCloudPlan);
                        $("#lrtype" + (i + 1)).change();
                        DedicateNum = data.Dedicate.length;
                        zxAddOrDel();
                    }

                }
            });


        }
        //监听联系人删除/添加
        document.addEventListener("click", function () {
            lxrAddOrDel();

            //禁用不是最后一个的删除按钮
            var dom = $(".delContact");
            dom.hide();
            dom.last().show();

        });
        //delDedicate
        document.addEventListener("click", function () {
            console.log(DedicateNum);
            zxAddOrDel();
            //禁用不是最后一个的删除按钮
            var dom = $(".delDedicate");
            dom.hide();
            dom.last().show();

        })
        function lxrAddOrDel() {

            var dom = $(".addContacts");//联系人
            dom.hide();
            if (dom.length >= 3) {
                dom.hide();
            } else {
                dom.last().show();
            }
        }
        function zxAddOrDel() {

            var dom1 = $(".addDedicate");//专线
            dom1.hide();
            if (dom1.length >= 15) {
                dom1.hide();
            } else {
                dom1.last().show();
            }
        }
        //保存
        function save() {
            var data = {
                ID: id,
                CompanyName: $("#CompanyName").val(),
                CompanyAddress: $("#CompanyAddress").val(),
                CompanyScale: $("#ComputerNumber").val(),
                Remark: $("#remark").val(),
                State: $("#state").val()
            }
            console.log(ContactsNum);
            if (data.CompanyName == "") { alert("请输入单位名称"); return false; }
            if (data.CompanyAddress == "") { alert("请输入单位地址"); return false; }
            //联系人
            var Contacts = [];
            for (var i = 0; i < ContactsNum; i++) {
                var cont = {
                    Name: $("#Name" + (i + 1)).val(),
                    Tel: $("#Tel" + (i + 1)).val(),
                    Post: $("#Post" + (i + 1)).val(),
                }
                if (cont.Name == "") { alert("请输入关键人" + (i + 1) + "姓名"); return false; }
                if (cont.Tel == "") { alert("请输入关键人" + (i + 1) + "手机号"); return false; }

                Contacts.push(cont);
            }
            //专线信息
            var Dedicate = [];
            for (var i = 0; i < DedicateNum; i++) {
                var cont = {
                    ID: $("#PlInfoID" + (i + 1)).val() == "" ? 0 : $("#PlInfoID" + (i + 1)).val(),
                    Operator: $("#Operator" + (i + 1)).val(),
                    WeekPrice: $("#weekprice" + (i + 1)).val(),
                    BandWidth: $("#broadband" + (i + 1)).val(),
                    PayType: $("#paytype" + (i + 1)).val(),
                    OverTime: $("#overtime" + (i + 1)).val(),
                    Type: $("#lrtype" + (i + 1)).val(),
                    ServerBerSys: $("#ServerBerSys" + (i + 1)).val(),
                    ServerUsingTime: $("#ServerUsingTime" + (i + 1)).val(),
                    IsCloudPlan: $("#IsCloudPlan" + (i + 1)).val()

                }
                var type = $('#lrtype' + (i + 1)).val();
                if (type == "1" || type == "2") {
                    if (cont.Type == "") {
                        alert("请选择" + (i + 1) + "录入类型");
                        return false;
                    }
                    if (cont.Operator == "") {
                        alert("请选择" + (i + 1) + "合作运营商");
                        return false;
                    }
                    if (cont.WeekPrice == "") {
                        alert("请输入" + (i + 1) + "周价");
                        return false;
                    }
                    if (cont.BandWidth == "") {
                        alert("请输入" + (i + 1) + "宽带");
                        return false;
                    }
                    if (cont.PayType == "") {
                        alert("请选择" + (i + 1) + "付费方式");
                        return false;
                    }
                    if (cont.OverTime == "") {
                        alert("请选择" + (i + 1) + "到期时间");
                        return false;
                    }
                }
                if (type == "3") {
                    if (cont.ServerBerSys == "") {
                        alert("请选择" + (i + 1) + "服务器承载系统");
                        return false;
                    }
                    if (cont.IsCloudPlan == "") {
                        alert("请选择" + (i + 1) + "是否上云计划");
                        return false;
                    }
                    if (cont.ServerUsingTime == "") {
                        alert("请选择" + (i + 1) + "现服务器开始使用时间");
                        return false;
                    }
                }
                //录入类型（专线，电路，云业务）
                Dedicate.push(cont);
            }

            //修改
            if (id != 0) {
                $.ajax({
                    url: 'AddCollection.aspx/Modify',
                    data: JSON.stringify({ data: JSON.stringify(data), lxr: JSON.stringify(Contacts), zx: JSON.stringify(Dedicate) }),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        alert(data.msg);
                        if (data.state == 1) {
                            location.href = '/Specialline/CollectionTable.aspx';
                        }
                    }
                });
            }
            else {//添加

                var visit = {
                    IsNeed: $("input[name='IsNeed']:checked").val(),
                    IsAgain: $("input[name='IsAgain']:checked").val(),
                    NextTime: $("#NextTime").val()
                }
                //if (visit.NextTime == "") { alert("请选择下次拜访时间"); return false; }

                if (visit.IsAgain == "1") {
                    if (visit.NextTime == "") {
                        alert("请选择下次拜访时间");
                        return false;
                    }

                }
                $.ajax({
                    url: 'AddCollection.aspx/Add',
                    data: JSON.stringify({ data: JSON.stringify(data), lxr: JSON.stringify(Contacts), vis: JSON.stringify(visit), zx: JSON.stringify(Dedicate) }),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        alert(data.msg);
                        if (data.state == 1) {
                            location.href = '/Specialline/CollectionTable.aspx';
                            //parent.refreshFrame();
                        }
                    }
                });
            }
        }

        //删除联系人
        function delContacts(el) {
            $("#" + el).html("");
            ContactsNum--;
        }
        //删除专线
        function delDedicate(n) {
            if (DedicateNum > 1) {
                $("#Dedicate" + n).remove();
                DedicateNum--;
            }
        }
        //初始化联系人(n是索引)(el添加的位置)
        function initContacts(n, el) {

            var result =

                    `<div class="control-group">
							<label class ="control-label">关键人${n}: </label>
							<div class="controls">
								<input type="text" class ="span2" placeholder="请输入姓名" id="Name${n}">
                                <button class ="btn btn-danger btn-mini delContact" style="margin-left: 5px" onclick="delContacts('${el}')">删除</button>
							</div>
						</div>
						<div class="control-group">
							<label class="control-label">关键人岗位 :</label>
							<div class="controls">
							<input type="text" class="span2" placeholder="岗位" id="Post${n}">
							</div>
						</div>
						<div class="control-group">
							<label class="control-label">手机号 :</label>
							<div class="controls">
								<input type="text" class ="span3" maxlength="11" placeholder="请输入负责人联系号码" id="Tel${n}">
							</div>
						</div>
                        <div class ="control-group">
                                <label class ="control-label"></label>
                                <div class ="controls">
                                    <button class ="btn btn-info addContacts" onclick="initContacts(3,'Contacts3')" >继续添加关键人</button>
                                </div>
                        </div>
                        `

            $("#" + el).append(result);
            ContactsNum++;
        }

        //初始化专线信息
        function initDedicate(n) {

            var result = `
                <div id="Dedicate${n}" class ="Dedicate${n}">
                 <div class ="Subtitle" id="box-${n}">
                    <p>专线，电路或云业务信息${n}</p>
                    <a href="javascript:void(0);" id="Takeup-${n}">点击收起</a>
                    <a href="javascript:void(0);" id="Open-${n}"+ style="display: none;">点击展开</a>
                </div>
                 <div class ="control-group">
                                    <label class ="control-label"><span class ="red">*&nbsp; </span>类型 :</label>
                                    <div class ="controls">
                                        <select class ="input-small inline form-control" id="lrtype${n}">
                                            <option value="1">专线</option>
                                            <option value="2">电路</option>
                                            <option value="3">云业务</option>
                                        </select>
                                    </div>
                                </div>
                <div class ="span6-box BOX-${n}" id="DivOperator${n}">
                <input style="display: none;" id="PlInfoID${n}" />
                    <div class ="control-group">
						<label class ="control-label"><span class ="red">*&nbsp; </span>合作运营商 :</label>
						<div class ="controls">
							<select class ="input-small inline form-control" id="Operator${n}">
								<option value="">请选择运营商</option>
								<option value="移动">移动</option>
								<option value="联通">联通</option>
								<option value="电信">电信</option>
                                <option value="广电">广电</option>
							</select>
						</div>
					</div>
                    	</div>
					<div class ="control-group" id="DivWeekprice${n}">
						<label class ="control-label"><span class ="red">*&nbsp; </span>周价（元） :</label>
						<div class ="controls">
							<input type="text" class ="span2" placeholder="请输入周价" id="weekprice${n}">
						</div>
					</div>
                    	<div class ="control-group" id="DivPaytype${n}">
						<label class ="control-label"><span class ="red">*&nbsp; </span>付费方式 :</label>
						<div class ="controls">
							<select class ="input-small inline form-control" id="paytype${n}">
								<option value="">请选择付费方式</option>
								<option value="月缴">月缴</option>
								<option value="季度缴">季度缴</option>
								<option value="年缴">年缴</option>
							</select>
						</div>
					</div>
                    <div class ="control-group" id="DivOvertime${n}">
                        <label class ="control-label"><span class ="red">*&nbsp; </span>到期时间 :</label>
                        <div class ="controls">
                            <input type="text" placeholder="请选择到期时间" name="sbirth" data-birth="123" class ="input_text_200" onclick="WdatePicker()" readonly="readonly" id="overtime${n}" />
                        </div>
                    </div>
					<div class ="control-group" id="DivBroadband${n}" >
						<label class ="control-label"><span class ="red">*&nbsp; </span>带宽（M） :</label>
						<div class ="controls">
							<input type="text" class ="span2" placeholder="请输入带宽" id="broadband${n}">
						</div>
					</div>


                        <div class ="control-group" id="DivServerBerSys${n}" hidden="hidden">
                                    <label class ="control-label"><span class ="red">*&nbsp; </span>服务器承载系统 :</label>
                                    <div class ="controls">
                                        <input type="text" class ="span2" placeholder="服务器承载系统" id="ServerBerSys1" />
                                    </div>
                                </div>
                                <div class ="control-group" id="DivServerUsingTime${n}" hidden="hidden">
                                    <label class ="control-label"><span class ="red">*&nbsp; </span>现服务器开始使用时间 :</label>
                                    <div class ="controls">
                                        <input type="text" placeholder="请选择现服务器开始使用时间" name="sbirth" data-birth="123" class ="input_text_200" onclick="selectMonth()" readonly="readonly" id="ServerUsingTime1" />
                                    </div>
                                </div>
                                <div class ="control-group" id="DivIsCloudPlan${n}" hidden="hidden">
                                    <label class ="control-label"><span class ="red">*&nbsp; </span>是否有上云计划 :</label>
                                    <div class ="controls">
                                        <select class ="input-small inline form-control" id="IsCloudPlan1">
                                            <option value="">请选择</option>
                                            <option value="0">否</option>
                                            <option value="1">是</option>
                                        </select>
                                    </div>
                                </div>
                    <div class ="control-group">
						<label class ="control-label"></label>
						<div class ="controls">
							<button class ="btn btn-danger delDedicate" onclick="delDedicate(${n})">删除</button>
							<button class ="btn btn-info addDedicate" style="margin-left: 5px">继续添加专线，电路或云业务信息</button>
						</div>
					</div>
                </div>
                </div>
                `
            $("#info").append(result);
            DedicateNum++;
        }

        //获取url的参数
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>
</body>
</html>
