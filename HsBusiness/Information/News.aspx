<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="HsBusiness.Information.News" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css"/>
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="/css/exclusive.css"/>
</head>
<body>
	<div class="Standard">
	    <div class="Standard-box3">
			<div>
				<div class="widget-box">
					<div class="widget-title bg_ly"> <span class="icon"> <i class="fa fa-eye"></i> </span>
						<p style="color: #fff">当月派单处理</p>
					</div>
					<div class="widget-content nopadding">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th>名称</th>
									<th>实际派单量</th>
                                    <th>总提醒量</th>
									<th>已处理</th>
									<th>未处理</th>
								</tr>
							</thead>
							<tbody>
								<tr id="proinfo">
								<%--	<td>项目派单</td>
									<td>100</td>
									<td>20</td>
									<td>80</td>--%>
								</tr>
								<tr id="banceinfo">
									<%--<td>存量派单</td>
									<td>1000</td>
									<td>199</td>
									<td>801</td>--%>
								</tr>
								<tr id="businfo">
									<%--<td>商机派单</td>
									<td>5</td>
									<td>0</td>
									<td>5</td>--%>
								</tr>
								<tr id="spliceinfo">
								<%--	<td>门店派单</td>
									<td>50</td>
									<td>40</td>
									<td>10</td>--%>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</div>
	    </div>
	    <div class="Standard-box4">
			<div class="widget-box">
				<div class="widget-title">
					<ul class="nav nav-tabs" id="remind">
					<li class="active"><a data-toggle="tab" href="#tab1">商机提醒<span class="red" id="busremind"></span></a></li>
					<li class=""><a data-toggle="tab" href="#tab2">专线提醒<span class="red" id="zxremind"></span></a></li>
					<li class=""><a data-toggle="tab" href="#tab3">项目派单提醒<span class="red" id="proremind"></span></a></li>
					<%--<li class=""><a data-toggle="tab" href="#tab4">小余额提醒<span class="red" id="banceremind"></span></a></li>--%>
					</ul>
				</div>
				<div class="widget-content tab-content">
					<div id="tab1" class="tab-pane active">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th class="given-th1">客户名称</th>
									<th class="given-th2">类型</th>
									<th class="given-th3">提醒时间</th>
								</tr>
							</thead>
							<tbody id="buslist">
							
							</tbody>
						</table>
					</div>
					<div id="tab2" class="tab-pane">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th class="given-th1">公司名称</th>
									<th class="given-th2">类型</th>
									<th class="given-th3">提醒时间</th>
								</tr>
							</thead>
							<tbody id="specillist">
								
							</tbody>
						</table>
					</div>
					<div id="tab3" class="tab-pane">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th class="given-th1">项目内容</th>
									<th class="given-th2">类型</th>
									<th class="given-th3">提醒时间</th>
								</tr>
							</thead>
							<tbody id="prolist">
								
							</tbody>
						</table>
					</div>
					<div id="tab4" class="tab-pane">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th class="given-th1">客户名称</th>
									<th class="given-th2">余额（元）</th>
									<th class="given-th3">提醒时间</th>
								</tr>
							</thead>
							<tbody id="bancelist">
								<%--<tr>
									<td><a href="#">安平县第二实验小学</a></td>
									<td class="red">-322.58</td>
									<td>2018-04-03 14:30:00</td>
								</tr>
								<tr>
									<td><a href="#">安平县第二实验小学</a></td>
									<td class="red">-322.58</td>
									<td>2018-04-03 14:30:00</td>
								</tr>
								<tr>
									<td><a href="#">安平县第二实验小学</a></td>
									<td class="red">-322.58</td>
									<td>2018-04-03 14:30:00</td>
								</tr>
								<tr>
									<td><a href="#">安平县第二实验小学</a></td>
									<td class="red">-322.58</td>
									<td>2018-04-03 14:30:00</td>
								</tr>
								<tr>
									<td><a href="#">安平县第二实验小学</a></td>
									<td class="red">-322.58</td>
									<td>2018-04-03 14:30:00</td>
								</tr>--%>
							</tbody>
						</table>
					</div>
				</div>
			</div>
	    </div>
	</div>
	<div class="Standard">
		<div class="widget-box">
			<div class="widget-title bg_lb"> <span class="icon"> <i class="fa fa-list-ol fa-fw"></i> </span>
				<p style="color: #fff">当月报表详情</p>
			</div>
			<div class="widget-content nopadding">
				<table class="table table-bordered ranking">
					<thead>
						<tr>
							<th>排名</th>
							<th>存量收入</th>
							<th>新增收入</th>
							<th>收入总额</th>
							<th>存量收入保有率</th>
							<th>R0进度目标</th>
							<th>R0完成率（%）</th>
							<th>R2进度目标</th>
							<th>R2完成率（%）</th>
						</tr>
					</thead>
					<tbody id="report">
						
					</tbody>
				</table>
			</div>
		</div>
	</div>
    <script src="../js/jquery.min.js"></script>
	<script src="../js/bootstrap.min.js"></script> 
    <script type="text/javascript">
        $(function () {
            GetBusRemind();
            GetZxRemind();
            ProRemind();
            Balance();
            MonthlyProject();
            MonthlyStock();
            MonthlyBusiness();
            MonthlySplice();
            Report();
            PeperReport();
        })
        //商机提醒
        function GetBusRemind()
        {
            $.ajax({
                url: 'News.aspx/GetBusRemind',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.length>0) {
                        $("#busremind").html("（" + data[0].Number + "）");
                    }
                   
                    var result = '';
                    for (var i = 0; i < data.length; i++) {
                        result += `
                        <tr>
						<td><a href="/Businesss/Details.aspx?BusID=${data[i].ID}">${data[i].CompanyName||""}</a></td>
						<td>${data[i].Type || ""}</td>
						<td>${data[i].AddTime}</td>
					</tr>

                        `
                    }
                    $("#buslist").append(result);
                }
            });
        }
        //门店提醒
        function GetStoreRemind() {
            $.ajax({
                url: 'News.aspx/GetStoreRemind',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.length > 0) {
                        $("#storeremind").html("（" + data[0].Number + "）");
                    }
                    var result = '';
                    for (var i = 0; i < data.length; i++) {
                        result += `
                        <tr>
						<td><a href="/Stores/StoreDetails.aspx?SID=${data[i].StoreID}">${data[i].StoreName || ""}</a></td>
						<td>${data[i].Type || ""}</td>
						<td>${data[i].AddTime}</td>
					</tr>

                        `
                    }
                    $("#storelist").append(result);
                }
            });
        }
        //专线派单提醒
        function GetZxRemind() {
            $.ajax({
                url: 'News.aspx/GetZxRemind',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.length > 0) {
                        $("#zxremind").html("（" + data[0].Number + "）");
                    }
                    var result = '';
                    for (var i = 0; i < data.length; i++) {
                        result += `
                        <tr>
						<td><a href="#">${data[i].CompanyName}</a></td>
						<td>${data[i].Type || ""}</td>
						<td>${data[i].AddTime}</td>
					</tr>

                        `
                    }
                    $("#specillist").append(result);
                }
            });
        }

        //项目派单提醒
        function ProRemind() {
            $.ajax({
                url: 'News.aspx/ProRemind',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.length > 0) {
                        $("#proremind").html("（" + data[0].Number + "）");
                    }
                    var result = '';
                    for (var i = 0; i < data.length; i++) {
                        result += `
                        <tr>
						<td class ="ellipsis"><a href="#">${data[i].ProjectName || ""}</a></td>
						<td>${data[i].Type || ""}</td>
						<td>${data[i].AddTime}</td>
					</tr>

                        `
                    }
                    $("#prolist").append(result);
                }
            });
        }
        //小余额提醒
        function Balance() {
            $.ajax({
                url: 'News.aspx/Balance',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d).data;
                    if (data.length > 0) {
                        $("#banceremind").html('（' + data[0].Number + '）');
                    }
                    var result = '';
                    for (var i = 0; i < data.length; i++) {
                        result += `
                      <tr>
						<td><a href="/BalanceManage/BalanceDetails.aspx?BalID=${data[i].ID}&state=${data[i].IsRead}">${data[i].CustomerName}</a></td>
						<td class ="red">${data[i].Balance||0.00}</td>
						<td>${data[i].AddTime}</td>
					</tr>

                        `
                    }
                    $("#bancelist").append(result);
                }
            });
        }
        //当月项目派单处理
        function MonthlyProject()
        {
            $.ajax({
                url: 'News.aspx/MonthlyProject',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = '';
                    result += `
                        <td>${data.Name}</td>
						<td>${data.Procount || 0}</td>
                        <td>${data.CounReport||0}</td>
						<td>${data.Reportcount || 0}</td>
						<td>${data.NoneReport||0}</td>
                        `
                    $("#proinfo").append(result);
                }
            });
        }
        //当月存量派单处理
        function MonthlyStock()
        {
            $.ajax({
                url: 'News.aspx/MonthlyStock',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = '';
                    result += `
                        <td>${data.Name}</td>
						<td>${data.Procount || 0}</td>
                        <td>${data.Reportcount||0}</td>
						<td>${data.IsRead || 0}</td>
						<td>${data.NoneRead || 0}</td>
                        `
                    $("#banceinfo").append(result);
                }
            });
        }
        //当月商机派单
        function MonthlyBusiness()
        {
            $.ajax({
                url: 'News.aspx/MonthlyBusiness',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = '';
                    result += `
                        <td>${data.Name}</td>
						<td>${data.Buscount || 0}</td>
                        <td>${data.CounReport||0}</td>
						<td>${data.Reportcount || 0}</td>
						<td>${data.NoneReport || 0}</td>
                        `
                    $("#businfo").append(result);
                }
            });
        }
        //专线派单
        function MonthlySplice()
        {
            $.ajax({
                url: 'News.aspx/MonthlySpecil',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = '';
                    result += `
                        <td>${data.Name}</td>
						<td>${data.Specilcount || 0}</td>
                        <td>${data.CounReport || 0}</td>
						<td>${data.Reportcount || 0}</td>
						<td>${data.NoneReport || 0}</td>
                        `                                    
                    $("#spliceinfo").append(result);
                }
            });
        }
        //当月门店派单
        //function MonthlyStore()
        //{
        //    $.ajax({
        //        url: 'News.aspx/MonthlyStore',
        //        dataType: "json",
        //        type: "POST",
        //        contentType: "application/json;charset=utf-8",
        //        success: function (data) {
        //            var data = $.parseJSON(data.d);
        //            var result = '';
        //            result += `
        //                <td>${data.Name}</td>
		//				<td>${data.Storecount || 0}</td>
        //                <td>${data.CounReport||0}</td>
		//				<td>${data.Reportcount || 0}</td>
		//				<td>${data.NoneReport || 0}</td>
        //                `
        //            $("#storeinfo").append(result);
        //        }
        //    });
        //}
        //当月业务报表(全部的前三名)
        function Report()
        {
            $.ajax({
                url: 'News.aspx/Report',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                async:false,
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = '';
                    for (var i = 0; i < data.data.length; i++) {
                        result += `
                       <tr>`
                        result+=`
							<td>`
                        if(data.data[i].Rank == 1) {result+=`<img class ="tu" src="../images/main_leader_one.png" />`}
                        if(data.data[i].Rank == 2) {result+=`<img class ="tu" src="../images/main_leader_two.png" /> `}
                        if(data.data[i].Rank == 3) {result+=`<img class ="tu" src="../images/main_leader_three.png" />` }
                        result += `</td>`

                        result+=`
						    <td>${data.data[i].StockIncome}</td>
                            <td>${data.data[i].NewIncome}</td>
                            <td>${data.data[i].TotalIncome}</td>
                            <td>${data.data[i].StockIncomeRate}</td>
                            <td>${data.data[i].R0Aims}</td>
                            <td>${data.data[i].R0CompleteRate}</td>
                            <td>${data.data[i].R2Aims}</td>
                            <td>${data.data[i].R2CompleteRate}</td>
						</tr>
                        `
                    }
                    $("#report").append(result);
                }
            });
        }
        //个人当月业务报表
        function PeperReport()
        {
            $.ajax({
                url: 'News.aspx/PeperReport',
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    var result = '';
                    for (var i = 0; i < data.data.length; i++) {
                        result += `
                            <tr style="color: #ffb848">
                            <td>${data.data[i].Rank}</td>
						    <td>${data.data[i].StockIncome}</td>
                            <td>${data.data[i].NewIncome}</td>
                            <td>${data.data[i].TotalIncome}</td>
                            <td>${data.data[i].StockIncomeRate}</td>
                            <td>${data.data[i].R0Aims}</td>
                            <td>${data.data[i].R0CompleteRate}</td>
                            <td>${data.data[i].R2Aims}</td>
                            <td>${data.data[i].R2CompleteRate}</td>
					    </tr>
                        `
                    }
                    $("#report").append(result);
                }
            });
        }
    </script>
</body>

</html>
