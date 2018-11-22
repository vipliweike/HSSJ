<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Password-Modification.aspx.cs" Inherits="HsBusiness.Password_Modification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/initial.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/Newly.css" rel="stylesheet" />
</head>
<body>
	<div class="Standard">
		<div class="widget-box">
        <div class="widget-title"> <span class="icon"> <i class="fa fa-align-justify"></i> </span>
          <p>修改密码</p>
        </div>
        <div class="widget-content nopadding">
			<form action="#" method="get" class="form-horizontal">
				<div class="control-group">
					<label class="control-label">用户 :</label>
					<div class="controls">
						<%--<input type="text" id="username" class="ui-wizard-content" value="<%=HttpContext.Current.Session["CurrentUerName"]%>"" disabled="disabled"/>--%>
                       <input type="text" id="username" class="ui-wizard-content" value="<%= Common.TCContext.Current.OnlineRealName%>"" disabled="disabled"/>
					</div>
				<div class="control-group">
					<label class="control-label">密码 :</label>
					<div class="controls" >
						<input type="password" id="oldpwd" name="password" class="ui-wizard-content" /><span for="password" id="dispwd" generated="true" class="help-inline" style="display: none;">密码不为空</span><span for="password" id="dispwd1" generated="true" class="help-inline" style="display: none;">密码不正确</span>
					</div>
                </div>
                <div class="control-group error">
					<label class="control-label">新密码 :</label>
					<div class="controls">
						<input type="password" id="newpwd" name="password" class="ui-wizard-content"/><span for="password" id="pwdyanzheng" generated="true" class="help-inline" style="display: none;">新密码不为空</span><span for="password" id="pwdyanzheng1" generated="true" class="help-inline" style="display: none;">请设置密码长度8-18个字符，同时包含字母和数字</span>
					</div>
                </div>
                <div class="control-group error">
					<label class="control-label">确认密码 :</label>
					<div class="controls">
						<input type="password" name="password2" id="renrenpwd" class="ui-wizard-content"/><span for="password" id="pwd1" generated="true" class="help-inline" style="display: none;">输入的密码不为空</span><span for="password" generated="true" id="pwd2" class="help-inline" style="display: none;">输入的密码不一致</span>
					</div>
                </div>
				<div class="form-actions">
					<button type="button" class="btn btn-success" id="save">保存</button>
					<button type="button" class="btn btn-danger" id="qx">取消</button>
				</div>
			</form>
        	</div>
		</div>
	</div>
</body>
</html>
<script src="js/jquery.min.js"></script>
<script type="text/javascript">
    var reg = {
        //密码长度不能小于8个字符或大于16个字符且至少包含大小写字母、数字、特殊字符四种组合中的三种组合
        pwd: /^(?!([a-zA-Z\d]*|[\d!@#\$%_\.]*|[a-zA-Z!@#\$%_\.]*)$)[a-zA-Z\d!@#\$%_\.]{8,16}$/,

        //pwd:/^([A-Za-z]|[0-9])+$/
    }
    $("#qx").click(function () {
        parent.refreshFrame();
    });

    $("#save").click(function () {
        var oldpwd = $("#oldpwd").val();
        var newpwd = $("#newpwd").val();
        var renrenpwd = $("#renrenpwd").val();
        var data = {
            pwd: renrenpwd,
            oldpwd:oldpwd
        }
        if (oldpwd == "" || oldpwd == null) {
            $("#dispwd").css('display', 'block');//显示错误信息
            return;
        } else {
            $("#dispwd").css('display', 'none');
        }
        if (newpwd == "" || newpwd == null) {
            $("#pwdyanzheng").css('display', 'block');
            return;
        } else {
            $("#pwdyanzheng").css('display', 'none');
        } 
     
        if (renrenpwd==""||renrenpwd==null) {
            $("#pwd1").css('display', 'block');
            return;
        } else {
            $("#pwd1").css('display', 'none');
        }
    
        if (renrenpwd != newpwd) {
            $("#pwd2").css('display', 'block');
            return;
        } else {
            $("#pwd2").css('display', 'none');
        }
        if (!reg.pwd.test(renrenpwd)) {
            alert("密码长度不能小于8个字符或大于16个字符且至少包含大小写字母、数字、特殊符号四种组合中的三种组合");
            return;
        }
        else {
            $("#pwd1").css('display', 'none');
            $("#pwdyanzheng").css('display', 'none');
            $.ajax({
                url: 'Password-Modification.aspx/ModifyPwd',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {

                    var data = $.parseJSON(data.d);
                    if (data.state == 1) {
                        alert("修改成功，请重新登录");
                        window.top.location.href = "/login.aspx";
                        //alert(data.msg);
                    }
                    if (data.state==0) {
                        alert(data.msg);
                    }
                    if (data.state==2) {
                        alert(data.msg);
                    }
                }
            })


        }

    })
    //防止页面后退
    history.pushState(null, null, document.URL);
    window.addEventListener('popstate', function () {
        history.pushState(null, null, document.URL);
    });
</script>
