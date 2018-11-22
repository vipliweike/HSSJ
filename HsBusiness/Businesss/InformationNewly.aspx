<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformationNewly.aspx.cs" Inherits="HsBusiness.Businesss.InformationNewly1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>衡水商机</title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css" />
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/Newly.css" />
    <style type="text/css">
     .comments {
     width:100%;/*自动适应父布局宽度*/
     overflow:auto;
     word-break:break-all;
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

                            <%--<div class="control-group">
                                <label class="control-label">所属区县 :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="Area">
                                        <option value="0">请选择区县</option>
									<option value="1">衡水市</option>
									<option value="2">安平县</option>
									<option value="3">饶阳县</option>
									<option value="4">深州市</option>
									<option value="5">武强县</option>
									<option value="6">阜城县</option>
									<option value="7">武邑县</option>
									<option value="8">景县</option>
									<option value="9">冀州县</option>
									<option value="10">枣强县</option>
									<option value="11">故城县</option>
                                    </select>
                                </div>
                            </div>--%>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>行业归属 :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="hygs">
                                    <option value="">请选择行业归属</option>
                                    </select>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>用户规模（人） :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="usergm">
                                    <option value="">请选择用户规模</option>
                                    </select>
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label">是否有员工通讯录 :</label>
                                <div class="controls">
                                    <input type="radio" name="sex" value="1">是
	                            <input type="radio" name="sex" value="0" checked="checked">否
                                </div>
                            </div>
                        </div>
                       <div class="control-group">
                              <label class="control-label"><span class="red">&nbsp;</span>备注 :</label>
                                <div class="controls">
                                    <%--<input type="text" class="span2" placeholder="请输入备注" id="remark"/>--%>
                                    <textarea id="remark" rows="8" cols="8" style="width:300px;height:150px"></textarea>
                                </div>
                        </div>
                        <%-- 联系人 --%>
                        <div class="span6">
                            <div id="Contacts1" class="Contacts">
                                <div class="control-group">
                                    <label class="control-label"><span class="red">*&nbsp;</span>关键人1 :</label>
                                    <div class="controls">
                                        <input type="text" class="span2" placeholder="请输入姓名" id="Name1">
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">关键人岗位 :</label>
                                    <div class="controls">
                                        <input type="text" class="span2" placeholder="岗位" id="Post1">
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label"><span class="red">*&nbsp;</span>手机号 :</label>
                                    <div class="controls">
                                        <input type="text" class="span3" maxlength="11" placeholder="请输入负责人联系号码" id="Tel1">
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label"></label>
                                    <div class="controls">
                                        <button class="btn btn-info addContacts" onclick="initContacts(2,'Contacts2')">继续添加关键人</button>
                                    </div>
                                </div>

                            </div>
                            <div id="Contacts2" class="Contacts">
                            </div>
                            <div id="Contacts3" class="Contacts">
                            </div>


                        </div>
                         
                        <%-- 移动 --%>
                        <div class="Subtitle" id="box-b">
                            <p>移动业务</p>
                            <a href="javascript:void(0);" id="Takeup-2">点击收起</a>
                            <a href="javascript:void(0);" id="Open-2" style="display: none;">点击展开</a>
                        </div>
                        <div class="span6-box box-B">
                            <div class="span6">
                                <div class="control-group">
                                    <label class="control-label">是否有移动业务 :</label>
                                    <div class="controls">
                                        <input type="radio" name="IsMove" value="true" checked="checked">是
	                                <input type="radio" name="IsMove" value="false">否
                                    </div>
                                </div>
                                <div class="move">
                                    <div class="control-group">
                                        <label class="control-label">手机卡用途 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="sjkuser">
                                                <option value="">请选择手机卡用途</option>
                                                <%--
									<option value="1">通话</option>
									<option value="2">设备使用</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">可推业务 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="ktyw">
                                                <option value="">请选择可推业务</option>
                                                <%--<option value="0">请选择业务</option>
									<option value="1">手机</option>
									<option value="2">物联网</option>
									<option value="3">移固融合</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">联通业务占比（%） :</label>
                                        <div class="controls">
                                            <input type="text" class="span2" placeholder="请输入运营商业务占比" id="ltzb">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">移动业务占比（%） :</label>
                                        <div class="controls">
                                            <input type="text" class="span2" placeholder="请输入运营商业务占比" id="ydzb">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">电信业务占比（%） :</label>
                                        <div class="controls">
                                            <input type="text" class="span2" placeholder="请输入运营商业务占比" id="dxzb">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">员工年龄段（岁） :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="agegroup">
                                                <option value="">请选择年龄段</option>
                                                <%--<option value="0">请选择龄段</option>
									<option value="1">20-40</option>
									<option value="2">40-50</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">是否有员工补贴 :</label>
                                        <div class="controls">
                                            <input type="radio" name="sex1" value="1" checked="checked">是
	                                        <input type="radio" name="sex1" value="0">否
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">补贴额度（元）:</label>
                                        <div class="controls">
                                            <input type="text" class="span3" placeholder="请输入补贴额度" id="bted">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="move">
                                    <div class="control-group">
                                        <label class="control-label">补贴范围 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="btfw">
                                                <option value="">请选择补贴范围</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">使用政策 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="syzc">
                                                <option value="">请选择使用政策</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">租机到期时间 :</label>
                                        <div class="controls">
                                            <input type="text" placeholder="请选择租机到期时间" name="sbirth" id="overtime" data-birth="123" class="input_text_200" onclick="WdatePicker()" disabled="true" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">使用月消费（元） :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="mouthxf">
                                                <option value="">请选择月消费</option>
                                                <%--<option value="0">请选择月消费</option>
									<option value="1">10-50</option>
									<option value="2">50-100</option>
									<option value="3">100-200</option>
									<option value="4">200以上</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">使用侧重 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="sycz">
                                                <option value="">请选择使用侧重</option>
                                                <%--<option value="0">请选择使用侧重</option>
									<option value="1">通话</option>
									<option value="2">上网</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">年收入预测（元）:</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="yearyc">
                                                <option value="">请选择收入预测</option>
                                                <%--<option value="0">请选择收入预测</option>
									<option value="1">1000以下</option>
									<option value="2">1000-5000</option>
									<option value="3">5000-2万</option>
									<option value="4">2万以上</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">有无在用其他业务 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="isothoryw">
                                                <option value="">请选择业务</option>
                                                <%--<option value="0">请选择业务</option>
									<option value="1">专线</option>
									<option value="2">固话</option>
									<option value="3">宽带</option>
									<option value="4">云</option>
									<option value="5">ICT</option>
									<option value="6">无</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%-- 固网 --%>
                        <div class="Subtitle" id="box-a">
                            <p>固网业务</p>
                            <a href="javascript:void(0);" id="Takeup-1">点击收起</a>
                            <a href="javascript:void(0);" id="Open-1" style="display: none;">点击展开</a>
                        </div>
                        <div class="span6-box box-A">
                            <div class="span6">
                                <div class="control-group">
                                    <label class="control-label">是否有固网业务 :</label>
                                    <div class="controls">
                                        <input type="radio" name="IsFixed" value="true" checked="checked">是
	                                <input type="radio" name="IsFixed" value="false">否
                                    </div>
                                </div>
                                <div class="fixed">
                                    <div class="control-group">
                                        <label class="control-label">可推业务 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="gwktyw">
                                                <option value="" selected="selected">请选择业务</option>
                                                <%--
									<option value="1">固话</option>
									<option value="2">专线</option>
									<option value="3">电路</option>
									<option value="4">宽带</option>
									<option value="5">天翼云</option>
									<option value="6">ICT</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group" id="FScaleDiv">
                                        <label class="control-label">规模:</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="FScale">
                                                <option value="">请选择规模</option>
                                                <%--<option value="0">请选择规模</option>
									<option value="1">10以下</option>
									<option value="2">10-20</option>
									<option value="3">20-50</option>
									<option value="4">50及以上</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group" id="FBandWidthDiv">
                                        <label class="control-label">带宽 :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入带宽" id="FBandWidth">
                                        </div>
                                    </div>                                   
                                    <div class="control-group">
                                        <label class="control-label">是否跨域 :</label>
                                        <div class="controls">
                                            <input type="radio" name="sex4" value="1" checked="checked">是
	                            <input type="radio" name="sex4" value="0">否
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">预计周价（元） :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入预计周价" id="yjzj">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">预计入网月份 :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入预计入网月份" id="rwyf">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">预计年收益（元） :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入预计年收益" id="yjnsy">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label"><span class="red">*&nbsp;</span>合作运营商 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="hzyys">
                                                <option value="">请选择运营商</option>
                                                
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label"><span class="red">*&nbsp;</span>在用业务 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="gwuseyw">
                                                <option value="">请选择业务</option>
                                                
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group" id="FUseScaleDiv">
                                        <label class="control-label">规模:</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="FUseScale">
                                                <option value="">请选择规模</option>
                                                
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group" id="FUseBandWidthDiv">
                                        <label class="control-label">带宽:</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入带宽" id="FUseBandWidth">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="span6">
                                <div class="fixed">
                                    <div class="control-group">
                                        <label class="control-label"><span class="red">*&nbsp;</span>周价（元） :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入周价" id="zj">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label"><span class="red">*&nbsp;</span>到期时间 :</label>
                                        <div class="controls">
                                            <input type="text" placeholder="请选择到期时间" name="sbirth" id="dqsj" data-birth="123" class="input_text_200" onclick="WdatePicker()" />
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">友商年收益（元） :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入友商年收益" id="yssy">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">电脑台数（台） :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入电脑台数" id="dnts">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">现用业务是否满意 :</label>
                                        <div class="controls">
                                            <input type="radio" name="sex2" value="1" checked="checked">是
	                                        <input type="radio" name="sex2" value="0">否
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">是否有服务器 :</label>
                                        <div class="controls">
                                            <input type="radio" name="sex3" value="1" checked="checked">是
	                                        <input type="radio" name="sex3" value="0">否
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">服务器承载平台 :</label>
                                        <div class="controls">
                                            <input type="text" class="span4" placeholder="请输入服务器承载平台" id="fwq">
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">有无在用其他业务 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="ywqtyw">
                                                <option value="">请选择其他业务</option>
                                                <%--<option value="0">请选择运业务</option>
									<option value="1">手机</option>
									<option value="2">物联网</option>
									<option value="3">移固融合</option>--%>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="vis">
                            <%-- 走访记录 --%>
                            <div class="Subtitle" id="box-c">
                                <p>走访记录</p>
                                <a href="javascript:void(0);" id="Takeup-3">点击收起</a>
                                <a href="javascript:void(0);" id="Open-3" style="display: none;">点击展开</a>
                            </div>
                            <div class="span6-box box-C">
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
                                    <div class="control-group">
                                        <label class="control-label">是否需要公司领导拜访 :</label>
                                        <div class="controls">
                                            <input type="radio" name="IsLeader" value="1" checked="checked" />是
	                                    <input type="radio" name="IsLeader" value="0" />否
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">需协同领导 :</label>
                                        <div class="controls">
                                            <select class="input-small inline form-control" id="Leader">
                                                <option value="">请选择领导</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="control-group" id="next">
                                        <label class="control-label">下次拜访时间 :</label>
                                        <div class="controls">
                                            <input type="text" placeholder="请选择下次拜访时间" name="sbirth" id="NextTime" data-birth="123" class="input_text_200" onclick="WdatePicker()" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-actions">
                            <button type="button" class="btn btn-success" onclick="save();">提交</button>
                            <button class="btn btn-danger" id="cancel">取消</button>
                        </div>
                </form>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
    <script src="../js/browser.min.js"></script>
    <script src="../js/My97DatePicker/WdatePicker.js"></script>
    <script>
        //GetArea();//区县
        GetHYGS();//行业归属
        GetUserGm();//用户规模
        GetRole();
        GetMUser();
        GetMKTYW();
        GetYGNL();
        GetBTFW();
        GetSYZC();
        GetYXF();
        GetSYCZ();
        GetNSRYC();
        GetOtherYW();
        GetFYW();
        GetFGM();
        GetZXGM();
        GetFYYS();
        GetFUserYW();
        GetFOthorYW();
        getDic('需协同领导', Leader);

        function GetHYGS() {
            $.ajax({
                url: 'InformationNewly.aspx/GetHYGS',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                data: JSON.stringify({ name: "行业归属" }),
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#hygs").append(result);
                }
            });
        }
        //区县
        function GetArea() {
            $.ajax({
                url: 'InformationNewly.aspx/GetRegion1',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#Area").append(result);
                }
            })
        }
        //用户规模
        function GetUserGm() {
            $.ajax({
                url: 'InformationNewly.aspx/GetYHGM',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async:false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#usergm").append(result);
                }
            })
        }
        //角色
        function GetRole() {
            $.ajax({
                url: 'InformationNewly.aspx/GetRole',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#role").append(result);
                }
            })
        }

        //手机卡用途
        function GetMUser() {
            $.ajax({
                url: 'InformationNewly.aspx/GetMUser',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#sjkuser").append(result);
                }
            })
        }

        //可推业务
        function GetMKTYW() {
            $.ajax({
                url: 'InformationNewly.aspx/GetMKTYW',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#ktyw").append(result);
                }
            })
        }
        //年龄段
        function GetYGNL() {
            $.ajax({
                url: 'InformationNewly.aspx/GetYGNL',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#agegroup").append(result);
                }
            })
        }
        //补贴范围
        function GetBTFW() {
            $.ajax({
                url: 'InformationNewly.aspx/GetBTFW',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#btfw").append(result);
                }
            })
        }
        //使用政策
        function GetSYZC() {
            $.ajax({
                url: 'InformationNewly.aspx/GetSYZC',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#syzc").append(result);
                }
            })
        }
        //使用月消费
        function GetYXF() {
            $.ajax({
                url: 'InformationNewly.aspx/GetYXF',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#mouthxf").append(result);
                }
            })
        }
        //使用侧重
        function GetSYCZ() {
            $.ajax({
                url: 'InformationNewly.aspx/GetSYCZ',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#sycz").append(result);
                }
            })
        }
        //得到年收入预测yearyc
        function GetNSRYC() {
            $.ajax({
                url: 'InformationNewly.aspx/GetNSRYC',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#yearyc").append(result);
                }
            })
        }
        //移动其他业务
        function GetOtherYW() {
            $.ajax({
                url: 'InformationNewly.aspx/GetOtherYW',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#isothoryw").append(result);
                }
            })
        }
        //GetFYW固网可推业务
        function GetFYW() {
            $.ajax({
                url: 'InformationNewly.aspx/GetFYW',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#gwktyw").append(result);
                }
            })
        }
        //得到固话规模
        function GetFGM() {
            $.ajax({
                url: 'InformationNewly.aspx/GetFGM',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#FScale").append(result);
                }
            })
        }
        //专线规模
        function GetZXGM() {
            $.ajax({
                url: 'InformationNewly.aspx/GetZXGM',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#zxgm").append(result);
                }
            })
        }
        //得到合作运营商
        function GetFYYS() {
            $.ajax({
                url: 'InformationNewly.aspx/GetFYYS',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#hzyys").append(result);
                }
            })
        }
        //固网再用业务
        function GetFUserYW() {
            $.ajax({
                url: 'InformationNewly.aspx/GetFUserYW',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#gwuseyw").append(result);
                }
            })
        }
        //得到固网其他业务
        function GetFOthorYW() {
            $.ajax({
                url: 'InformationNewly.aspx/GetFOthorYW',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                type: "POST",
                async: false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `<option value="${data[i].Name}">${data[i].Name}</option>`;
                    }
                    $("#ywqtyw").append(result);
                }
            });            
        }
        //获取字典
        function getDic(name, el) {
            $(el).empty();
            $.ajax({
                url: 'InformationNewly.aspx/GetDic',
                data: JSON.stringify({ DicName: name }),
                dataType: 'json',
                type: 'post',
                async: false,
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = ``;
                    for (var i = 0; i < data.length; i++) {
                        result += `
                            <option value="${data[i].Name}">${data[i].Name}</option>
                            `;
                    }
                    $(el).append($("<option value=''>请选择"+name+"</option>"))
                    $(el).append(result);
                }
            })
        }
    </script>
    <script type="text/javascript">
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
        $(document).ready(function () {
            $("#box-c").click(function () {
                $(".box-C").slideToggle("slow");
                $("#Takeup-3").toggle();
                $("#Open-3").toggle();
            });
        });

        $(function () {
            $("form").submit(function () { return false; })

            //是否有移动业务
            $("input[name='IsMove']").change(function () {
                var $val = $("input[name='IsMove']:checked").val();
                if ($val == "true") {
                    $(".move").show();
                } else {
                    $(".move").hide();
                }
            });
            //是否有固网业务
            $("input[name='IsFixed']").change(function () {
                var $val = $("input[name='IsFixed']:checked").val();
                if ($val == "true") {
                    $(".fixed").show();
                } else {
                    $(".fixed").hide();
                }
            });
            //是否需要再次拜访
            $("input[name='IsAgain']").change(function () {

                var isagain = $("input[name='IsAgain']:checked").val();
                if (isagain == "1") {
                    $("#next").show();
                }
                else {
                    $("#next").hide();
                }
            });
            //下次拜访时间大于当前时间
            //$("#NextTime").blur(function () {
            //    var $val = $("#NextTime").val();
            //    console.log($val);
            //    var date = getDate();

            //    if ($val < date) {
            //        $(this).val(date);
            //        alert("下次拜访时间不能小于当前时间");

            //    }

            //});
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

            //是否需领导协同
            $("input[name='IsLeader']").change(function () {
                var $val = $("input[name='IsLeader']:checked").val();
                if ($val == "1") {
                    $("#Leader").parent().parent().show();
                } else {
                    $("#Leader").parent().parent().hide();
                }
            });

            //是否有员工补贴
            $(document).on("change", "input[name='sex1']", function (el) {
                var $val = $("input[name='sex1']:checked").val()
                if ($val == "1") {
                    $("#bted").removeAttr("disabled");
                    $("#btfw").removeAttr("disabled");
                }
                if ($val == "0") {
                    $("#bted").attr("disabled", "true");
                    $("#btfw").attr("disabled", "true");
                    $("#bted").val("");
                    $("#btfw").val("");
                }
            })
            //使用政策
            $(document).on("change", "#syzc", function () {
                var $val = $("#syzc").val();
                if ($val == "单卡") {
                    $("#overtime").attr("disabled", "true");
                    $("#overtime").val("");
                }
                if ($val == "租机") {
                    $("#overtime").removeAttr("disabled");
                }
            });

            //是否有服务器
            $("input[name='sex3']").change(function () {
                var $val = $("input[name='sex3']:checked").val();
                if ($val == "0") {
                    $("#fwq").attr("disabled", "true")
                    $("#fwq").val("");
                }
                if ($val == "1") {
                    $("#fwq").removeAttr("disabled");
                }
            });

            //固网可推业务
            $("#gwktyw").change(function () {

                var yw = $("#gwktyw").val();
                var gm = $("#FScale");//规模
                var gmDiv = $("#FScaleDiv");//规模Div
                var dk = $("#FBandWidth");//带宽
                var divdkDiv = $("#FBandWidthDiv");//带宽Div

                getDic(yw + '规模', FScale);

                if (yw == "固话") {
                    gmDiv.show();
                    divdkDiv.hide();
                } else if (yw == "专线") {
                    gmDiv.show();
                    divdkDiv.show();
                } else if (yw == "电路") {
                    gmDiv.show();
                    divdkDiv.show();
                } else if (yw == "宽带") {
                    gmDiv.show();
                    divdkDiv.show();
                } else if (yw == "天翼云") {
                    gmDiv.show();
                    divdkDiv.hide();
                } else if (yw == "ICT") {
                    gmDiv.hide();
                    divdkDiv.hide();
                } else {
                    gmDiv.show();
                    divdkDiv.show();
                }
            });
            //固网再用业务
            $("#gwuseyw").change(function () {

                var yw = $("#gwuseyw").val();
                var gm = $("#FUseScale");//规模
                var gmDiv = $("#FUseScaleDiv");//规模Div
                var dk = $("#FUseBandWidth");//带宽
                var divdkDiv = $("#FUseBandWidthDiv");//带宽Div
                
                getDic(yw + '规模', FUseScale);

                if (yw == "固话") {
                    gmDiv.show();
                    divdkDiv.hide();
                } else if (yw == "专线") {
                    gmDiv.show();
                    divdkDiv.show();
                } else if (yw == "电路") {
                    gmDiv.show();
                    divdkDiv.show();
                } else if (yw == "宽带") {
                    gmDiv.show();
                    divdkDiv.show();
                } else if (yw == "天翼云") {
                    gmDiv.show();
                    divdkDiv.hide();
                } else if (yw == "ICT") {
                    gmDiv.hide();
                    divdkDiv.hide();
                } else {
                    gmDiv.show();
                    divdkDiv.show();
                }
            })

            ////下次拜访时间大于当前时间
            //$("#NextTime").on("onblur ", function () {
            //    var $val = $("#NextTime").val();
            //    console.log($val);
            //    var date = getDate();
            //    console.log(date)
            //    if ($val < date) {
            //        $(this).val(date);
            //        alert("下次拜访时间不能小于当前时间")
            //    }
            //});

           
        });
        

    </script>
    <script type="text/javascript">


        //保存
        function save() {
            var data = {
                ID: id,
                CompanyName: $("#CompanyName").val(),
                CompanyAddress: $("#CompanyAddress").val(),
                Remark: $("#remark").val(),//备注
                //Areas: $("#Area").val(),
                Industry: $("#hygs").val(),
                CustomerScale: $("#usergm").val(),
                IsHavePhoneList: $("input[name='sex']:checked").val(),
                MCardUse: $("#sjkuser").val(),
                MPushWork: $("#ktyw").val(),
                MUnicom: $("#ltzb").val(),
                MMobile: $("#ydzb").val(),
                MTelecom: $("#dxzb").val(),
                MAgeGroup: $("#agegroup").val(),
                MIsSubsidy: $("input[name='sex1']:checked").val(),
                MQuota: $("#bted").val(),
                MRange: $("#btfw").val(),
                MPolicy: $("#syzc").val(),
                MOverTime: $("#overtime").val(),
                MMonthFee: $("#mouthxf").val(),
                MFocus: $("#sycz").val(),
                MIncome: $("#yearyc").val(),
                MOtherWork: $("#isothoryw").val(),
                FPushWork: $("#gwktyw").val(),
                FOperator: $("#hzyys").val(),//合作运营商
                FUseBandWidth: $("#FUseBandWidth").val(),//在用业务带宽
                FScale: $("#FScale").val(),
                FUseWork: $("#gwuseyw").val(),
                FUseScale: $("#FUseScale").val(),
                FWeekPrice: $("#zj").val(),
                FOverTime: $("#dqsj").val(),
                FAlsAnlIncome: $("#yssy").val(),
                FComputerNum: $("#dnts").val(),
                FIsSatisfy: $("input[name='sex2']:checked").val(),
                FIsServer: $("input[name='sex3']:checked").val(),
                FIsDomain: $("input[name='sex4']:checked").val(),
                FPreWeekPrice: $("#yjzj").val(),
                FPreInNetMouth: $("#rwyf").val(),
                FPreAnlIncome: $("#yjnsy").val(),
                FOtherWork: $("#ywqtyw").val(),
                FPlatform: $("#FPlatform").val(),
                FBandWidth: $("#FBandWidth").val(),
                FPlatform: $("#fwq").val(),
                //Remark: $("#Remark").val(),
                IsFixed: $("input[name='IsFixed']:checked").val(),
                IsMove: $("input[name='IsMove']:checked").val(),
            }
            console.log(ContactsNum);
            if (data.CompanyName == "") { alert("请输入单位名称"); return false; }
            if (data.CompanyAddress == "") { alert("请输入单位地址"); return false; }
            if (data.Industry == "") { alert("请输入行业归属"); return false; }
            if (data.CustomerScale == "") { alert("请输入用户规模"); return false; }

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

            if (data.IsFixed == "true") {
                if (data.FWeekPrice == "") { alert("请输入周价"); return false; }
                if (data.FOverTime == "") { alert("请输入到期时间"); return false; }
            }
            //修改
            if (id != 0) {
                $.ajax({
                    url: 'InformationNewly.aspx/Modify',
                    data: JSON.stringify({ data: JSON.stringify(data), lxr: JSON.stringify(Contacts) }),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        alert(data.msg);
                        if (data.state == 1) {
                            location.href = '/Businesss/InformationTable.aspx';
                        }
                    }
                });
            } else {//添加

                var visit = {
                    IsNeed: $("input[name='IsNeed']:checked").val(),
                    IsAgain: $("input[name='IsAgain']:checked").val(),
                    IsLeader: $("input[name='IsLeader']:checked").val(),
                    Leader: $("#Leader").val(),
                    NextTime: $("#NextTime").val()
                }
                if (visit.IsAgain == "1")
                {
                    if (visit.NextTime== "")
                    {
                        alert("请选择下次拜访时间");
                        return false;
                    }

                }
               
                if (visit.IsLeader == "1") { if (visit.Leader == "") { alert("请选择需协同领导"); return; } }
                if (visit.IsLeader == "0") { visit.Leader = ""; }

                $.ajax({
                    url: 'InformationNewly.aspx/Add',
                    data: JSON.stringify({ data: JSON.stringify(data), lxr: JSON.stringify(Contacts), vis: JSON.stringify(visit) }),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        alert(data.msg);
                        if (data.state == 1) {
                            location.href = '/Businesss/InformationTable.aspx';
                            //parent.refreshFrame();
                        }
                    }
                });
            }
        }



    </script>
    <script>

        var id = 0;
        var ContactsNum = 1;
        $(function () {
            id = getQueryString("id");
            $("form").submit(function () { return false; });
            $("#submit").click(function () {
                var Contacts = [];
                for (var i = 0; i < ContactsNum; i++) {
                    var contact = {
                        Name: $("#Name" + (i + 1)).val(),
                        Tel: $("#Tel" + (i + 1)).val()
                    }
                    Contacts.push(contact);
                }
                var data = {
                    ID: id,
                    Areas: $("#Areas").val(),
                    CompanyName: $("#CompanyName").val(),
                    Industry: $("#Industry").val(),
                    CustomerScale: $("#CustomerScale").val(),
                    Remark: $("#Remark").val(),
                    Contacts: JSON.stringify(Contacts)
                };

                $.ajax({
                    url: 'InformationNewly.aspx/Modify',
                    data: JSON.stringify(data),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        if (data.state == 1) {
                            //$.dialog({
                            //    type: 'info',
                            //    infoText: '修改成功',
                            //    infoIcon: '/images/icon/success.png',
                            //    autoClose: 1500
                            //});
                            alert(data.msg);
                            location.href = "/Businesss/InformationTable.aspx";
                        } else {
                            $.dialog({
                                type: 'info',
                                infoText: '修改失败',
                                infoIcon: '/images/icon/fail.png',
                                autoClose: 1500
                            });
                        }
                    },
                    error: function () {
                        $.dialog({
                            type: 'info',
                            infoText: '修改失败',
                            infoIcon: '/images/icon/fail.png',
                            autoClose: 1500
                        });
                    }
                })
            });
            $("#cancel").click(function () {
                location.href = "/Businesss/InformationTable.aspx";
            });
            if (id != 0) {
                getOne(id);
                $("#vis").hide();
            }

        });

        //监听联系人删除/添加
        document.addEventListener("click", function () {
            lxrAddOrDel();

            //禁用不是最后一个的删除按钮
            var dom = $(".delContact");
            //dom.attr("disabled", "disabled");
            //dom.last().removeAttr("disabled");
            dom.hide();
            dom.last().show();

        });

        function lxrAddOrDel() {
            var dom = $(".addContacts");//联系人
            dom.hide();
            if (dom.length >= 3) {
                dom.hide();
            } else {
                dom.last().show();
            }
        }
        //查询
        function getOne(id) {
            var data = { ID: id };
            $.ajax({
                url: 'InformationNewly.aspx/GetOne',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);

                    $("#CompanyName").val(data.CompanyName);
                    $("#CompanyAddress").val(data.CompanyAddress);
                    //$("#Area").val(data.Areas);
                    $("#hygs").val(data.Industry);
                    $("#usergm").val(data.CustomerScale);
                    $("#remark").val(data.Remark);
                    //是否有移动业务
                    if (data.IsMove) {
                        $("input[name='IsMove']").eq(0).attr("checked", "checked");
                    }
                    else {
                        $("input[name='IsMove']").eq(1).attr("checked", "checked");
                    }
					$("input[name='IsMove']").trigger('change');//触发change
                    //是否有固网业务
                    if (data.IsFixed) {
                        $("input[name='IsFixed']").eq(0).attr("checked", "checked");
                    }
                    else {
                        $("input[name='IsFixed']").eq(1).attr("checked", "checked");
                    }
					$("input[name='IsFixed']").trigger('change');//触发change

                    if (data.IsHavePhoneList == 0) {
                        $("input[name='sex']").eq(1).attr("checked", "checked");
                    }
                    if (data.IsHavePhoneList == 1) {
                        $("input[name='sex']").eq(0).attr("checked", "checked");
                    }
                    $("#sjkuser").val(data.MCardUse);
                    $("#ktyw").val(data.MPushWork);
                    $("#ltzb").val(data.MUnicom);
                    $("#ydzb").val(data.MMobile);
                    $("#dxzb").val(data.MTelecom);
                    $("#agegroup").val(data.MAgeGroup);
                    if (data.MIsSubsidy == 0) {
                        $("input[name='sex1']").eq(1).attr("checked", "checked");
                    }
                    if (data.MIsSubsidy == 1) {
                        $("input[name='sex1']").eq(0).attr("checked", "checked");
                    }
                    $("input[name='sex1']").trigger('change');//触发change
                    $("#bted").val(data.MQuota);
                    $("#btfw").val(data.MRange);
                    $("#syzc").val(data.MPolicy).trigger('change');
                    $("#overtime").val(data.MOverTime);
                    $("#mouthxf").val(data.MMonthFee);
                    $("#sycz").val(data.MFocus);
                    $("#yearyc").val(data.MIncome);
                    $("#isothoryw").val(data.MOtherWork);

                    /*****************************/
                    $("#gwktyw").val(data.FPushWork);
                    $("#gwktyw").change();
                    $("#FScale").val(data.FScale);///////
                    $("#FBandWidth").val(data.FBandWidth);///////
                    /*********************************/
                    $("#gwuseyw").val(data.FUseWork);
                    $("#gwuseyw").change();
                    $("#FUseScale").val(data.FUseScale);///////
                    $("#FUseBandWidth").val(data.FUseBandWidth);///////
                    /*******************************/
                    $("#zj").val(data.FWeekPrice);
                    $("#dqsj").val(data.FOverTime);
                    $("#yssy").val(data.FAlsAnlIncome);
                    $("#dnts").val(data.FComputerNum);
                    $("#hzyys").val(data.FOperator)
                    if (data.FIsSatisfy == 0) {
                        $("input[name='sex2']").eq(1).attr("checked", "checked");
                    }
                    if (data.FIsSatisfy == 1) {
                        $("input[name='sex2']").eq(0).attr("checked", "checked");
                    }
                    if (data.FIsServer == 0) {
                        $("input[name='sex3']").eq(1).attr("checked", "checked");
                    }
                    if (data.FIsServer == 1) {
                        $("input[name='sex3']").eq(0).attr("checked", "checked");
                        $("#fwq").val(data.FPlatform);
                    }
                    $("input[name='sex3']").trigger("change");
                    if (data.FIsDomain == 0) {
                        $("input[name='sex4']").eq(1).attr("checked", "checked");
                    }
                    if (data.FIsDomain == 1) {
                        $("input[name='sex4']").eq(0).attr("checked", "checked");
                    }

                    $("#yjzj").val(data.FPreWeekPrice);
                    $("#rwyf").val(data.FPreInNetMouth);
                    $("#yjnsy").val(data.FPreAnlIncome);
                    $("#ywqtyw").val(data.FOtherWork);
                    $("#Remark").val(data.Remark);


                    $("#Name1").val(data.Contacts[0].Name)
                    $("#Tel1").val(data.Contacts[0].Tel)
                    $("#Post1").val(data.Contacts[0].Post)
                    for (var i = 1; i < data.Contacts.length; i++) {
                        console.log("Contacts" + (i + 1));
                        initContacts(i + 1, "Contacts" + (i + 1));
                        $("#Name" + (i + 1)).val(data.Contacts[i].Name)
                        $("#Tel" + (i + 1)).val(data.Contacts[i].Tel)
                        $("#Post" + (i + 1)).val(data.Contacts[i].Post)
                    }
                    ContactsNum = data.Contacts.length;
                    lxrAddOrDel();
                }
            });


        }

        //删除联系人
        function delContacts(el) {
            $("#" + el).html("");
            ContactsNum--;
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
        //获取当前时间
        function getDate() {
            var myDate = new Date();//获取当前日期
            // 获取当前日期
            var date = new Date();

            // 获取当前月份
            var nowMonth = date.getMonth() + 1;

            // 获取当前是几号
            var strDate = date.getDate();

            // 添加分隔符“-”
            var seperator = "-";

            // 对月份进行处理，1-9月在前面添加一个“0”
            if (nowMonth >= 1 && nowMonth <= 9) {
                nowMonth = "0" + nowMonth;
            }

            // 对月份进行处理，1-9号在前面添加一个“0”
            if (strDate >= 0 && strDate <= 9) {
                strDate = "0" + strDate;
            }

            // 最后拼接字符串，得到一个格式为(yyyy-MM-dd)的日期
            var nowDate = date.getFullYear() + seperator + nowMonth + seperator + strDate;

            return nowDate;
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
