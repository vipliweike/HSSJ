<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="HsBusiness.Welcome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link rel="stylesheet" type="text/css" href="../css/initial.css"/>
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="../css/welcome.css"/>
    <%--<script src="../js/jquery.min.js"></script>--%>
    <script src="js/jquery.min.js"></script>
   <script type="text/javascript">
    $(function () {

        BusStatistics();
        SJSum();//商机数量
        StoreStatisc();
        ZXNum();
        ProjectAss();
        BalanceStatic();

    })
       //商机上报统计
    function BusStatistics()
    {
        $.ajax({
            url: 'Welcome.aspx/BusStatistics',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d).data;
                var result = '';
                for (var i = 0; i < data.length; i++) {
                   
                    result += `
                        <tr>
						<td>${i+1}</td>
						<td>${data[i].Key||""}</td>
						<td>${data[i].Value}</td>
			           </tr>
                        `
                }
                $("#businfo").append(result);
            }
        });

    }
       //商机上报数量
    function SJSum()
    {
        $.ajax({
            url: 'Welcome.aspx/BusNum',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d).data;
                $("#SSum").html(data);
            }
        });

    }
       //专线上报统计
    function StoreStatisc() {
        $.ajax({
            url: 'Welcome.aspx/PlStatisc',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d).data;
                var result = '';
                for (var i = 0; i < data.length; i++) {
                    result += `
                        <tr>
						<td>${i + 1}</td>
						<td>${data[i].Key || ""}</td>
						<td>${data[i].Value}</td>
					</tr>

                        `
                }
                $("#storeinfo").prepend(result);
            }
        });

    }
     //专线数量
    function ZXNum()
    {
        $.ajax({
            url: 'Welcome.aspx/ZXNum',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d).data;
                $("#zxnum").html(data);
               
            }
        });
    }
     //项目派单
    function ProjectAss()
    {

        $.ajax({
            url: 'Welcome.aspx/ProjectAss',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                var result = '';
              
                    result += `
                        <tr>
						<td>${data.Procount}</td>
						<td>${data.Reportcount }</td>
						<td>${data.Reportrate || ""}</td>
					</tr>

                        `
                
                $("#proinfo").append(result);
            }
        });
    }
       //小余额
    function BalanceStatic()
    {
        $.ajax({
            url: 'Welcome.aspx/BalanceStatic',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                var result = '';
               
                    result += `
                        <tr>
						<td>${data.Procount}</td>
						<td>${data.Reportcount}</td>
						<td>${data.Reportrate || 0}</td>
					</tr>

                        `
               
                $("#balaceinfo").append(result);
            }
        });
    }
</script>
</head>
<body>
	<div class="Standard">
	    <div class="Standard-box1">
			<div>
				<div class="widget-box">
					<div class="widget-title bg_lb"> <span class="icon"> <i class="fa fa-shopping-cart fa-fw"></i> </span>
						<p style="color: #fff">本月项目派单</p>
					</div>
					<div class="widget-content nopadding">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th>项目派单</th>
									<th>回执情况</th>
									<th>回执率</th>
								</tr>
							</thead>
							<tbody id="proinfo">
								
							</tbody>
						</table>
					</div>
				</div>
			</div>
		</div>
		<div class="Standard-box2">
			<div>
				<div class="widget-box">
					<div class="widget-title bg_lg"> <span class="icon"> <i class="fa fa-pie-chart fa-fw"></i> </span>
						<p style="color:#fff">本月存量小余额派单</p>
					</div>
					<div class="widget-content nopadding">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th>到期派单</th>
									<th>已读</th>
									<th>未读</th>
								</tr>
							</thead>
							<tbody id="balaceinfo">
								
							</tbody>
						</table>
					</div>
				</div>
			</div>
	    </div>
	</div>
	<div class="Standard">
		<div class="Standard-box1">
			<div>
				<div class="widget-box">
					<div class="widget-title bg_ly"> <span class="icon"> <i class="fa fa-list-ol fa-fw"></i> </span>
						<p style="color: #fff">本月商机统计</p>
					</div>
					<div class="widget-content nopadding">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th>排名</th>
									<th>区县</th>
									<th>本月上报</th>
								</tr>
                                
							</thead>
							<tbody id="businfo">
                                 
							</tbody>
                            <tbody>
                                 <tr style="background-color:#fffeec">
									<td style="font-weight:600" colspan="2">本月商机合计</td>
									<td style="font-weight:600" id="SSum"></td>
								</tr>
                            </tbody>

						</table>
					</div>
				</div>
			</div>
		</div>
		<div class="Standard-box2">
			<div>
				<div class="widget-box">
					<div class="widget-title bg_lo"> <span class="icon"> <i class="fa fa-map-marker fa-fw"></i> </span>
						<p style="color: #fff">本月专线统计</p>
					</div>
					<div class="widget-content nopadding">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th>排名</th>
									<th>区县</th>
									<th>本月上报</th>
								</tr>
                                
							</thead>
							<tbody id="storeinfo">
								
							</tbody>
                            <tbody>

                                 <tr style="background-color:#fffeec">
									<td style="font-weight:600" colspan="2">本月专线合计</td>
									<td style="font-weight:600" id="zxnum"></td>
								</tr>
                            </tbody>
						</table>
					</div>
				</div>
			</div>
	    </div>
	</div>
</body>
</html>

