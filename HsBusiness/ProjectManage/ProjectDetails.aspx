<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectDetails.aspx.cs" Inherits="HsBusiness.ProjectManage.ProjectDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/initial.css"/>
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/details-all.css"/>
    
</head>
<body>
	<div class="Standard">
		<div class="widget-title" style="background-color: #fff">
			<p>项目详情</p>
			<div class="control2">
				<p>
					<span>最近更新时间：</span><span id="updatetime"></span>
				</p>
			</div>
		</div>
		<div class="Subtitle" id="box-a">
			<p>基础信息</p>
			<a href="javascript:void(0);" id="Takeup-1">点击收起</a>
			<a href="javascript:void(0);" id="Open-1" style="display: none;">点击展开</a>
		</div>
		<div class="box-A">
			<div class="Contacts project-details" id="info">
				
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
				<span class="Name">预约时间:</span><span class="Name-data" id="yytime"></span><span class="Name">状态:</span><span class="Name-data red" id="state"></span>
			</div>
		</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered table-striped with-check example-table">
					<thead>
						<tr>
							<th>拜访时间</th>
							<th>沟通内容</th>
							<th>图片信息</th>
							<th>负责人</th>
						</tr>
					</thead>
					<tbody id="tbody">
												
							
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
							<th>回执时间</th>
							<th>回执内容</th>
							<th>回执区县</th>
							<th>回执人</th>
				
						</tr>
					</thead>
					<tbody id="tbodys">
						
						
					</tbody>
				</table>
			</div>
		</div>
	</div>
	<div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
	<script type="text/javascript">
	$(document).ready(function(){
	$("#box-e").click(function(){
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
	$("#box-a").click(function(){
	    $(".box-A").slideToggle("slow");
	    $("#Takeup-1").toggle();
	    $("#Open-1").toggle();
	  });
	});
	var ProID = 0;
	$(function () {

	    ProID = getQueryString("ProID");//项目id
	    GetInfo();
	    GetVistInfo();
	    GetRemind();

	})
	//基础信息
	function GetInfo() {
	    var data = {
	        ID: ProID
	    }
	    $.ajax({
	        url: 'ProjectDetails.aspx/GetInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            for (var i = 0; i < data.length; i++) {
	                $("#updatetime").html(data[i].LastTime);
	                var result=`
                <div>
					<p class ="unit">提供日期：</p>
					<p class ="unit-text" >${data[i].AddTime||""}</p>
				</div>
				<div>
					<p class ="unit">项目详情：</p>
					<p class ="unit-text">${data[i].Introduce||""}</p>
				</div>
			    
				<div>
					<p class ="unit">商机区县：</p>
					<p class ="unit-text">${data[i].Areas}</p>
				</div>
                <div>
					<p class ="unit">领导批示：</p>
					<p class ="unit-text">${data[i].Instruction||""}</p>
				</div>
                 `
	            }
	            $("#info").append(result);
	           
	        }
	    });
	}
    //走访记录
	function GetVistInfo()
	{
	    var data = {
	        ID: ProID
	    }
	    $.ajax({
	        url: 'ProjectDetails.aspx/GetVistInfo',
	        data: JSON.stringify(data),
	        dataType: 'json',
	        contentType: "application/json;charset=utf-8",
	        type: 'post',
	        success: function (data) {
	            var data = $.parseJSON(data.d).data;
	            if (data.length > 0) {
	                $("#yytime").html(data[0].NextTime);
	                $("#state").html(data[0].State);
	            }	            
	            var result = "";
	            for (var i = 0; i < data.length; i++) {
	               
               
	                result += `
                        <tr>
							<td>${data[i].AddTime||""}</td>
							<td>${data[i].VisitContents || ""}</td>
							<td>`
                             for(var j = 0; j < data[i].Img.length; j++) {
                            result +=
                                `<a href="http://124.239.149.190:8001/UploadFile/Images/${data[i].Img[j]}" target="_blank">图片${j + 1}</a>`

                             }
                             result += `</td>
							<td>${data[i].FzrName || ""}</td>
						</tr>
                        
                        `
	            }
	            $("#tbody").append(result);
	        }
         
	    });
	}
    //回执记录
	function GetRemind()
	{
	    var data = {
	        ID: ProID
	    }
	    $.ajax({
	        url: 'ProjectDetails.aspx/GetRemind',
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
							<td>${data[i].ReceiptTime || ""}</td>
							<td>${data[i].RemindContents || ""}</td>
							<td>${data[i].Areas||""}</td>
							<td>${data[i].UserName || ""}</td>
						</tr>

                        `
	            }
	            $("#tbodys").append(result);
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
