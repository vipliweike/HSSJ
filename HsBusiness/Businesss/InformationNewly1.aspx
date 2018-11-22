<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformationNewly.aspx.cs" Inherits="HsBusiness.Businesss.InformationNewly" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>衡水商机</title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css">
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="/css/Newly.css">
    <link href="/css/dialog.css" rel="stylesheet" />
</head>
<body>
	<div class="Standard">
		<div class="widget-box">
        <div class="widget-title"> <span class="icon"> <i class="fa fa-align-justify"></i> </span>
          <p>编辑</p>
        </div>
        <div class="widget-content nopadding">
			<form action="#" method="get" class="form-horizontal">
				<div class="span6-box">
				<div class="span6">
				<div class="control-group">
					<label class="control-label">单位名称 :</label>
					<div class="controls">
						<input type="text" class="span4" placeholder="请输入单位名称" id="CompanyName">
					</div>
				</div>				
				<div class="control-group">
					<label class="control-label">所属区县 :</label>
					<div class="controls">
						<select class="input-small inline form-control" id="Areas">
							<option value="0">请选择区县</option>
							<option value="衡水市">衡水市</option>
							<option value="安平县">安平县</option>
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
				</div>
				<div class="control-group">
					<label class="control-label">行业归属 :</label>
					<div class="controls">
						<select class="input-small inline form-control" id="Industry">
							<option value="">请选择行业</option>
							<option value="企业">企业</option>
							<option value="党政">党政</option>
							<option value="3">医疗</option>
							<option value="4">校园</option>
							<option value="5">军警</option>
							<option value="6">其他</option>
						</select>
					</div>
				</div>
				<div class="control-group">
					<label class="control-label">用户规模 :</label>
					<div class="controls">
						<select class="input-small inline form-control" id="CustomerScale">
							<option value="0">请选择规模</option>
							<option value="50人">50人</option>
							<option value="100人">100人</option>
							<option value="3">200人</option>
							<option value="4">500人</option>
							<option value="5">1000人</option>
						</select>
					</div>
				</div>
				</div>
				<div class="span6" id="Contacts">

				</div>
				</div>
				<div class="control-group">
					<label class="control-label">备注 :</label>
					<div class="controls">
						<textarea class="span6 left" id="Remark"></textarea>
					</div>
				</div>
				<div class="form-actions">
					<button class="btn btn-success" id="submit">提交</button>
					<button class="btn btn-danger" id="cancel">取消</button>
				</div>
			</form>
        	</div>
		</div>
	</div>
    <script src="/js/jquery.min.js"></script>
    <script src="/js/dialog.js"></script>
    <script>
        var id = 0;
        var ContactsNum = 0;
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
                    ID:id,
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
                        }else {
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
            getOne(id);
        });
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

                    $("#CompanyName").val(data.CompanyName)
                    $("#Areas").val(data.Areas);
                    $("#Industry").val(data.Industry);
                    $("#CustomerScale").val(data.CustomerScale)
                    $("#Remark").val(data.Remark)
                    for (var i = 0; i < data.Contacts.length; i++) {
                        initContacts(i + 1);
                        $("#Name" + (i + 1)).val(data.Contacts[i].Name)
                        $("#Tel" + (i + 1)).val(data.Contacts[i].Tel)
                    }
                    ContactsNum = data.Contacts.length;
                }
            });
        }

        //初始化联系人(n是索引)
        function initContacts(n) {
            
            var result = `
                <div class ="control-group">
					<label class ="control-label">关键人${n}: </label>
					<div class ="controls">
						<input type="text" class ="span2" placeholder="请输入姓名" id="Name${n}">
					</div>
				</div>
				<div class ="control-group">
					<label class ="control-label">手机号: </label>
					<div class ="controls">
						<input type="text" class ="span3" placeholder="请输入负责人联系号码" id="Tel${n}">
					</div>
				</div>
                `;
            $("#Contacts").append(result);
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
