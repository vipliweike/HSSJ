<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessMap.aspx.cs" Inherits="HsBusiness.Businesss.BusinessMap" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>衡水商机</title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css">
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="/css/Business-map.css">
</head>
<body>
	<div class="Standard">
		<div class="widget-box">
        <div class="widget-title"> <span class="icon"> <i class="fa fa-align-justify"></i> </span>
          <p>衡水地图</p>
        </div>
        <div class="content">
			<div class="content-left">
				<img src="/img/map.png" border="0" usemap="#planetmap" alt="Planets" /> 
				<map name="planetmap" id="planetmap">
					<map name="planetmap" id="planetmap">
						<area shape="rect" coords="60,20,159,119" href ="#" alt="安平县" />
						<area shape="rect" coords="160,20,219,119" href ="#" alt="饶阳县" />
						<area shape="rect" coords="100,120,219,219" href ="#" alt="深州市" />
						<area shape="rect" coords="220,80,299,159" href ="#" alt="武强县" />
						<area shape="rect" coords="300,100,439,199" href ="#" alt="阜城县" />
						<area shape="rect" coords="100,220,219,279" href ="#" alt="衡水市辖区" />
						<area shape="rect" coords="220,160,299,279" href ="#" alt="武邑县" />
						<area shape="rect" coords="300,200,419,299" href ="#" alt="景县" />
						<area shape="rect" coords="60,280,199,379" href ="#" alt="冀州县" />
						<area shape="rect" coords="200,280,259,429" href ="#" alt="枣强县" />
						<area shape="rect" coords="260,320,379,439" href ="#" alt="故城县" />
					</map>
				</map>
			</div>
			<div class="content-right">
				<div class="widget-box">
					<div class="widget-title"> <span class="icon"> <i class="fa fa-eye"></i> </span>
						<p>深州市汇总</p>
					</div>
					<div class="widget-content nopadding">
						<table class="table table-bordered">
							<thead>
								<tr>
									<th colspan="2">本月数据</th>
									<th colspan="2">累计数据</th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td>本月新增客户数：<span>256</span></td>
									<td>排名：<span>12</span></td>
									<td>累计客户数：<span>3256</span></td>
									<td>排名：<span>12</span></td>
								</tr>
								<tr>
									<td>本月走访客户数：<span>123</span></td>
									<td>排名：<span>12</span></td>
									<td>累计走访客户数：<span>123</span></td>
									<td>排名：<span>12</span></td>
								</tr>
								<tr>
									<td>本月系统派单数：<span>123</span></td>
									<td>排名：<span>12</span></td>
									<td>累计系统派单数：<span>123</span></td>
									<td>排名：<span>12</span></td>
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
