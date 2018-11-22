<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetReminder.aspx.cs" Inherits="HsBusiness.SetReminder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/initial.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/Newly.css" rel="stylesheet" />
</head>
<body>
    <div class="Standard">
        <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="fa fa-align-justify"></i></span>
                <p>提醒设置</p>
            </div>
            <div class="widget-content nopadding">
                <form action="#" method="get" class="form-horizontal">
                    <div class="control-group" id="reminder">
                        <%--<label class="control-label">用户:</label>
					<div class="controls">
                       <input type="text" class="ui-wizard-content" value=""/>
					</div>--%>
                    </div>
                    <div class="form-actions">
                        <button type="button" class="btn btn-success" id="save" onclick="save1();">保存</button>
                        <button type="button" class="btn btn-danger" id="cancel">取消</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
</body>
</html>
<script src="js/jquery.min.js"></script>
<script type="text/javascript">
    $(function () {

        reminder();
    })
    function reminder() {

        $.ajax({
            url: 'SetReminder.aspx/GetReminder',
            dataType: 'json',
            type: 'post',
            async: false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                var result = '';
                for (var i = 0; i < data.length; i++) {
                    result += `<label class="control-label">${data[i].Name}:</label>
					<div class="controls">
                       <input type="text" class ="ui-wizard-content" id="${data[i].EnlishName}" value="${data[i].Numerical}"/><span>小时</span>
					</div>`;
                }
                $("#reminder").append(result);
            }
        });
    }
    //修改
    function save1() {
        var data =
            {
                data: JSON.stringify({
                    MExpire: $("#MExpire").val(),
                    FExpire: $("#FExpire").val(),
                    VExpire: $("#VExpire").val(),
                    StoreExpire: $("#StoreExpire").val(),
                    StoreVExpire: $("#StoreVExpire").val(),
                    ProjectVExpire: $("#ProjectVExpire").val(),
                    PlExpire: $("#PlExpire").val(),
                    PlVExpire: $("#PlVExpire").val(),
                })
            }
        $.ajax({
            url: 'SetReminder.aspx/Edit',
            dataType: 'json',
            type: 'post',
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                alert(data.msg);
                parent.refreshFrame();
            }
        });
    }
    //取消
    $("#cancel").click(function () {
        window.location.reload(true);


    });
</script>
