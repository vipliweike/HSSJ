<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HsBusiness.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登陆</title>
    <link href="css/initial.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/login.css" rel="stylesheet" />
    <link href="css/dialog.css" rel="stylesheet" />
    
   
    <style>
        .code 
        {
            float:left;
        }

    </style>
</head>
      
    <body onkeydown="keylogin()">
    <form id="form1" runat="server" defaultbutton="Button1">   
    <div id="loginbox">  
        <div class="control-group normal_text"> 
            <p style="color:#B2DFEE;font-size:28px;">衡水商机管理平台</p>
        </div> 
         
            <div class="control-group">
                <label class="control-label">登陆账号</label>
                <div class="controls">
                    <div class="main_input_box">
                        <span class="add-on bg_lg"><i class="icon-user" style="font-size:16px;"></i></span><asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="control-group">
                <label class="control-label">登陆密码</label>
                <div class="controls">
                    <div class="main_input_box">
                        <span class="add-on bg_ly"><i class="icon-lock" style="font-size:16px;"></i></span><asp:TextBox ID="txtPwd" runat="server" TextMode="Password" ></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="control-group2">
                <label class="control-label">验证码</label>
                <div class="controls">
                    <div class="main_input_box Verification">
                       <%-- <input id="code_input" type="text" style="width: 160px;float: left;" />--%>
                        <asp:TextBox ID="txtCode" runat="server" Width="160px" MaxLength="6" CssClass="code"></asp:TextBox>
                      <%--  <div id="v_container" style="width: 240px;height: 38px;float: right;"></div>--%>
                         <asp:Image ID="Image1" runat="server" Height="38px" ImageUrl="~/Ccode.aspx" onclick="this.src=this.src+'?';"
                        Width="240px" />
                    </div>
                </div>
            </div>
            <div class="form-actions">
                <div class="form-actions">
                   <%-- <input id="checkBtn" class="btn btn-success" value=" 登&nbsp;&nbsp;录"/>--%>
                  <%--  <asp:LinkButton ID="LinkButton1" CssClass="btn btn-success"  runat="server" OnClick="LinkButton1_Click">登陆</asp:LinkButton>--%>
                    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="登陆" OnClick="Button1_Click" />
                </div>
            </div>
            <div class="control-group normal_text">
                <div style="font-size:14px;color:#f2f2f2;">推荐使用webkit内核浏览器，如chrome等</div>
            </div>
     
    </div>
      </form>
</body>
      
</html>
<script src="js/gVerify.js"></script>
<script src="js/jquery.min.js"></script>
<script src="js/jquery.md5.js"></script>
<script type="text/javascript">

    //防止页面后退
    history.pushState(null, null, document.URL);
    window.addEventListener('popstate', function () {
        history.pushState(null, null, document.URL);
    });
    $(function () {
        
        $("#Button1").click(function () {
            $("#txtPwd").val($.md5($("#txtPwd").val()).toUpperCase())

        });

    })

    //回车登陆
    function keylogin()
    {
        if (event.keyCode == 13)   //回车键的键值为13
            document.getElementById("checkBtn").click();  //调用登录按钮的登录事件
    }
</script>
