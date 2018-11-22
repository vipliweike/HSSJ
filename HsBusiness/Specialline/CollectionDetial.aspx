<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CollectionDetial.aspx.cs" Inherits="HsBusiness.Specialline.CollectionDetial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/initial.css"/>
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/dialog.css"/>
    <link rel="stylesheet" type="text/css" href="../css/details-all.css"/>
</head>
<body>
	<div class="Standard">
		<div class="widget-title" style="background-color: #fff">
			<p id="companyname"></p>
			<%--<div class="control2 control-left">
				<p>
					<span>地址：</span><span id="companyaddress"></span>
				</p>
			</div>
            <div class="control2 control-left">
				<p>
					<span>备注：</span><span id="remark"></span>
				</p>
			</div>--%>
			<div class="control2">
				<p>
					<span>最近更新时间：</span><span id="updatetime"></span>
				</p>
			</div>
		</div>
        <div class="Subtitle" id="box-e">
			<p>基础信息</p>
			<a href="javascript:void(0);" id="Takeup-5">点击收起</a>
			<a href="javascript:void(0);" id="Open-5" style="display: none;">点击展开</a>
		</div>
		<div class="box-E">
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>公司名称</th>
							<th>公司地址</th>
							<th>备注</th>
							<th>建档时间</th>
						</tr>
					</thead>
					<tbody id="info">
						
					</tbody>
				</table>
			</div>
		</div>
		<div class="Subtitle" id="box-a">
			<p>联系人信息</p>
			<a href="javascript:void(0);" id="Takeup-1">点击收起</a>
			<a href="javascript:void(0);" id="Open-1" style="display: none;">点击展开</a>
		</div>
		<div class="box-A">
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>序号</th>
							<th>关键人</th>
							<th>岗位</th>
							<th>手机号</th>
						</tr>
					</thead>
					<tbody id="baseinfo">
						
					</tbody>
				</table>
			</div>
		</div>
		<div class="Subtitle" id="box-b">
			<p>专线或电路信息</p>
			<a href="javascript:void(0);" id="Takeup-2">点击收起</a>
			<a href="javascript:void(0);" id="Open-2" style="display: none;">点击展开</a>
		</div>

		<div class="box-B">
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>序号</th>
							<th>合作运营商</th>
							<th>周价（元）</th>
							<th>带宽（M）</th>
							<th>付费方式</th>
							<th>到期时间</th>
                            <th>类型</th>
							<th>状态</th>
						</tr>
					</thead>
					<tbody id="specialInfo">
						<%--<tr>
							<td>1</td>
							<td>电信</td>
							<td>100</td>
							<td>10</td>
							<td>月缴</td>
							<td>2018-04-06 12:30</td>
							<td class="red">已提醒</td>
						</tr>
						<tr>
							<td>2</td>
							<td>联通</td>
							<td>5000</td>
							<td>100</td>
							<td>月缴</td>
							<td>2018-04-06 12:30</td>
							<td class="red">已提醒</td>
						</tr>
						<tr>
							<td>3</td>
							<td>移动</td>
							<td>0</td>
							<td>1</td>
							<td>季度缴</td>
							<td>2018-04-06 12:30</td>
							<td class="red">已提醒</td>
						</tr>
						<tr>
							<td>4</td>
							<td>电信</td>
							<td>200</td>
							<td>20</td>
							<td>年缴</td>
							<td>2018-04-06 12:30</td>
							<td class="red">已提醒</td>
						</tr>
						<tr>
							<td>5</td>
							<td>电信</td>
							<td>500</td>
							<td>50</td>
							<td>年缴</td>
							<td>2018-04-06 12:30</td>
							<td class="red">已提醒</td>
						</tr>--%>
					</tbody>
				</table>
			</div>
		</div>
        	<div class="Subtitle" id="box-f">
			<p>云业务信息</p>
			<a href="javascript:void(0);" id="Takeup-6">点击收起</a>
			<a href="javascript:void(0);" id="Open-6" style="display: none;">点击展开</a>
		</div>

		<div class="box-F">
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>序号</th>
							<th>服务器承载系统</th>
							<th>现服务器开始使用时间</th>
							<th>是否有上云计划</th>
						</tr>
					</thead>
					<tbody id="TyyInfo">
					
					</tbody>
				</table>
			</div>
		</div>
		<div class="Subtitle" id="box-c">
			<p>走访记录</p>
			<a href="javascript:void(0);" id="Takeup-3">点击收起</a>
			<a href="javascript:void(0);" id="Open-3" style="display: none;">点击展开</a>
		</div>
		<div class="box-C">
			<div class="Contacts">
				<div>
					<span class="Name"></span>
					<button class="btn btn-success btn-mini" style="float: left;" id="addVisit">新增走访</button>
				</div>
			</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>拜访序数</th>
							<th>是否有业务需求</th>
							<th>是否需要二次拜访</th>
							<th>拜访时间</th>
						</tr>
					</thead>
					<tbody id="visitInfo">
						<%--<tr>
							<td>5</td>
							<td>是</td>
							<td>是</td>
							<td>2018-04-06</td>
						</tr>
						<tr>
							<td>4</td>
							<td>是</td>
							<td>是</td>
							<td>2018-04-06</td>
						</tr>
						<tr>
							<td>3</td>
							<td>是</td>
							<td>是</td>
							<td>2018-04-06</td>
						</tr>
						<tr>
							<td>2</td>
							<td>是</td>
							<td>是</td>
							<td>2018-04-06</td>
						</tr>
						<tr>
							<td>1</td>
							<td>是</td>
							<td>是</td>
							<td>2018-04-06</td>
						</tr>--%>
					</tbody>
				</table>
			</div>
		</div>
		<div class="Subtitle" id="box-d">
			<p>回执记录</p>
			<a href="javascript:void(0);" id="Takeup-4">点击收起</a>
			<a href="javascript:void(0);" id="Open-4" style="display: none;">点击展开</a>
		</div>
		<div class="box-D">
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check">
					<thead>
						<tr>
							<th>提醒内容</th>
							<th>提醒时间</th>
							<th>回执内容</th>
							<th>回执时间</th>
						</tr>
					</thead>
					<tbody id="remindinfo">
						
					</tbody>
				</table>
			</div>
		</div>
	</div>
	<div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
	<script type="text/javascript" src="../js/dialog.js"></script>
	<script type="text/javascript" src="../js/example.js"></script>
	<script type="text/javascript">
	    $(document).ready(function () {
	        $("#box-f").click(function () {
	            $(".box-F").slideToggle("slow");
	            $("#Takeup-6").toggle();
	            $("#Open-6").toggle();
	        });
	    });
	    $(document).ready(function () {
	        $("#box-e").click(function () {
	            $(".box-E").slideToggle("slow");
	            $("#Takeup-5").toggle();
	            $("#Open-5").toggle();
	        });
	    });
	$(document).ready(function(){
	$("#box-d").click(function(){
	    $(".box-D").slideToggle("slow");
	    $("#Takeup-4").toggle();
	    $("#Open-4").toggle();
	  });
	});
	$(document).ready(function(){
	$("#box-c").click(function(){
	    $(".box-C").slideToggle("slow");
	    $("#Takeup-3").toggle();
	    $("#Open-3").toggle();
	  });
	});
	$(document).ready(function(){
	$("#box-b").click(function(){
	    $(".box-B").slideToggle("slow");
	    $("#Takeup-2").toggle();
	    $("#Open-2").toggle();
	  });
	});
	$(document).ready(function(){
	$("#box-a").click(function(){
	    $(".box-A").slideToggle("slow");
	    $("#Takeup-1").toggle();
	    $("#Open-1").toggle();
	  });
	});

	var PLID = 0;
	$(function () {
	    PLID = getQueryString("PLID");
	    GetBasisInfo();//基础信息
	    GetInfo();
	    SpecialInfo();
	    TyyInfo();
	    VisitInfo();
	    RemindInfo();
	    $("#addVisit").click(function () {
	        window.parent.test.location.href = '/Specialline/AddVisit.aspx?PLID=' + PLID;

	    });
	})
        //基础信息
	function GetBasisInfo() {
	    var data = {
	        PLID: PLID
	    }
	    $.ajax({
	        url: 'CollectionDetial.aspx/GetBasisInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            //$("#companyname").html(data[0].CompanyName);
	            //$("#companyaddress").html(data[0].CompanyAddress);
	            //$("#updatetime").html(data[0].LastTime);
	            //$("#remark").html(data[0].Remark);
	            var result = '';
	            for (var i = 0; i < data.length; i++) {
	                result += `
                        <tr>
							<td>${data[i].CompanyName || ""}</td>
							<td>${data[i].CompanyAddress || ""}</td>
							<td>${data[i].Remark || ""}</td>
							<td>${data[i].AddTime || ""}</td>
						</tr>
                        `

	            }
	            $("#info").append(result);

	        }
	    });
	}
	//联系人信息
	function GetInfo() {
	    var data = {
	        PLID: PLID
	    }
	    $.ajax({
	        url: 'CollectionDetial.aspx/BasicInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data.Contacts;
	            $("#companyname").html(data[0].CompanyName);
	            $("#companyaddress").html(data[0].CompanyAddress);
	            $("#updatetime").html(data[0].LastTime);
	            //$("#remark").html(data[0].Remark);
	            var result = '';
	            for (var i = 0; i < data.length; i++) {
	                result += `
                        <tr>
							<td>${data[i].ID||""}</td>
							<td>${data[i].Name||""}</td>
							<td>${data[i].Post||""}</td>
							<td>${data[i].Tel||""}</td>
						</tr>
                        `
                 
	            }
	            $("#baseinfo").append(result);

	        }
	    });
	}
    //专线信息
	function SpecialInfo() {
	    var data = {
	        PLID: PLID
	    }
	    $.ajax({
	        url: 'CollectionDetial.aspx/SpecialInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            var result = '';
	            for (var i = 0; i < data.length; i++) {
	                result += `
                       <tr>
							<td>${data[i].ID||""}</td>
							<td>${data[i].Operator||""}</td>
							<td>${data[i].WeekPrice||""}</td>
							<td>${data[i].BandWidth||""}</td>
							<td>${data[i].PayType||""}</td>
							<td>${data[i].OverTime||""}</td>
                            <td>${data[i].Type||""}</td>
							<td class ="red">${data[i].State||""}</td>
						</tr>
                        `

	            }
	            $("#specialInfo").append(result);

	        }
	    });
	}
	    //云业务信息
	function TyyInfo()
	{
	    var data = {
	        PLID: PLID
	    }
	    $.ajax({
	        url: 'CollectionDetial.aspx/TyyInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            var result = '';
	            for (var i = 0; i < data.length; i++) {
	                result += `
                       <tr>
							<td>${data[i].ID || ""}</td>
							<td>${data[i].ServerBerSys || ""}</td>
							<td>${data[i].ServerUsingTime || ""}</td>
							<td>${data[i].IsCloudPlan || ""}</td>
						</tr>
                        `
	            }
	            $("#TyyInfo").append(result);
	        }
	    });
	}
        //走访信息
	function VisitInfo() {
	    var data = {
	        PLID: PLID
	    }
	    $.ajax({
	        url: 'CollectionDetial.aspx/VisitInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            var result = '';
	            for (var i = data.length - 1; i >= 0; i--) {

	                result += `
                        <tr>
                            <td>${i + 1}</td>
                            <td>${data[i].IsNeed || ""}</td>
                            <td>${data[i].IsAgain || ""}</td>
                            <td>${data[i].VisitTime || ""}</td>
                           
                        </tr>
                        `;
	            }
	            $("#visitInfo").append(result);

	        }
	    });
	}
        //回执记录
	function RemindInfo() {
	    var data = {
	        PLID: PLID
	    }
	    $.ajax({
	        url: 'CollectionDetial.aspx/RemindInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            var result = '';
	            for (var i = 0; i < data.length; i++) {
	                result += `
                        <tr>
							<td>${data[i].Type || ""}</td>
							<td>${data[i].AddTime || ""}</td>
							<td>${data[i].Contents||""}</td>
							<td>${data[i].ReceiptTime||""}</td>
						</tr>
                        `
	            }
	            $("#remindinfo").append(result);

	        }
	    });
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
