<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddPerson.aspx.cs" Inherits="HsBusiness.PersonManage.AddPerson" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../css/initial.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Newly.css" rel="stylesheet" />
    <link href="../css/dialog.css" rel="stylesheet" />
</head>
<body>
   
	<div class="Standard">
		<div class="widget-box">
        <div class="widget-title"> <span class="icon"> <i class="fa fa-align-justify"></i> </span>
          <p>个人信息</p>
        </div>
        <div class="widget-content nopadding">
			<form action="#" method="get" class="form-horizontal">
				<div class="control-group">
					<label class="control-label"><span class="red">*&nbsp;</span>姓名 :</label>
					<div class="controls">
						<input type="text" id="name" class="span2" placeholder="请输入姓名" maxlength="50"/>
					</div>
				</div>
					<div class="control-group">
					<label class="control-label"><span class="red">*&nbsp;</span>手机号 :</label>
					<div class="controls">
						<input type="text" id="tel" class="span3" placeholder="请输入手机号" maxlength="11"/>
					</div>
				</div>
				<div class="control-group">
					<label class="control-label"><span class="red">*&nbsp;</span>角色 :</label>
					<div class="controls">
						<select class="input-small inline form-control" id="role" name="role">
							<option value="" id="checkrole">请选择角色</option>
							<%--<option value="1">公司领导</option>
							<option value="2">政企部主管</option>
							<option value="3">网格助理</option>
							<option value="4">区县经理</option>
							<option value="5">客户经理</option>--%>
						</select>
					</div>
				</div>
				<div class="control-group" id="area">
					<label class="control-label"><span class="red">*&nbsp;</span>所属区县 :</label>
					<div class="controls">
						<select class="input-small inline form-control" id="county" name="county">
							<option value="" id="checkcounty">请选择区县</option>
                           
							<%--<option value="1" id="hscity">衡水市</option>
							<option value="2" id="apcounty">安平县</option>
							<option value="3" id="rycounty">饶阳县</option>
							<option value="4" id="szcounty">深州市</option>
							<option value="5" id="wqcounty">武强县</option>
							<option value="6" id="fccounty">阜城县</option>
							<option value="7" id="wicounty">武邑县</option>
							<option value="8" id="jxcounty">景县</option>
							<option value="9" id="jzcounty">冀州县</option>
							<option value="10" id="zqcounty">枣强县</option>
							<option value="11" id="gccounty">故城县</option>--%>
						</select>
                      
					</div>
				</div>
                <div class="control-group" id="grid">
					<label class="control-label"><span class="red">*&nbsp;</span>所属网格 :</label>
					<div class="controls">
						<select class="input-small inline form-control" id="grids" name="grids">
					<option value="" id="checkgrids">请选择网格</option>
						</select>
                      
					</div>
				</div>
                <div class="control-group">
					<label class="control-label">岗位 :</label>
					<div class="controls">
						<input type="text" id="post" class="span3" placeholder="请输入岗位名称" />
					</div>
				</div>
				<div class="control-group">
					<label class="control-label">备注 :</label>
					<div class="controls">
						<textarea class="span6" id="remark" maxlength="50"></textarea>
					</div>
				</div>
				<div class="form-actions">
					<button type="button" class="btn btn-success" onclick="alertFormDialog()">提交</button>
					<button type="button" class="btn btn-danger" id="cancel">取消</button>
				</div>
			</form>
        	</div>
		</div>
	</div>
</body>
</html>
<script src="../js/jquery.min.js"></script>
 <script src="../js/browser.min.js"></script>
<script src="../js/dialog.js"></script>
<script type="text/javascript">

    //获取url的参数
    function getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
    var id = 0;
    $(function () {
       
        id = getQueryString("id");
        $("form").submit(function () { return false; });
        if (id != 0 && id != null) {
            //grid(id);
            getOne(id);
        }
        GetRegion();//得到区县
        GetRole();//得到角色
   
        //取消
        $("#cancel").click(function () {
            window.location.href = "/PersonManage/PersonTable.aspx"

        });
        //根据所属关系确认下拉选择
        $('select[name="role"]').change(function () {
            //当岗位为公司领导，政企部主管(1,2)
            if ($('select[name="role"]').val() == 1 || $('select[name="role"]').val() == 2) {
                $("#county").find("option:contains('请选择区县')").attr("selected", true);
                $("#grids").find("option:contains('请选择网格')").attr("selected", true);
                $("#county").attr("disabled", true);
                $("#grids").attr("disabled", true);
               
            }
            if ($('select[name="role"]').val() == 3) {
                //当角色为区县经理
                $("#county").attr("disabled", false);
                $("#grids").attr("disabled", true);
                $("#grids").empty();
            }
            //行业经理
            if ($('select[name="role"]').val()==6) {
                $("#county").find("option:contains('请选择区县')").attr("selected", true);
                $("#grids").find("option:contains('请选择网格')").attr("selected", true);
                $("#county").attr("disabled", true);
                $("#grids").attr("disabled", true);
                $("#grids").empty();
            }
            if ($('select[name="role"]').val() == 4 || $('select[name="role"]').val() == 5) {
                $("#grids").attr("disabled", false);
                $("#county").attr("disabled", false);
                $("#grids").empty();
               // grids();
            }
        })

        //根据区县和所属网格
        $('select[name="county"]').change(function () {
            if ($('select[name="role"]').val() == 4 || $('select[name="role"]').val() == 5) {
                $("#grids").attr("disabled", false);
                $("#county").attr("disabled", false);
                $("#grids").empty();
                grids();
            }
        
        })
        //
       
    })
    //修改时为网格赋值
    function grid(id) {
        
        $.ajax({
            url: 'AddPerson.aspx/GetGrids',
            data:JSON.stringify({userid:id}),
            dataType: 'json',
            type: 'post',
            async:false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                
                var data = $.parseJSON(data.d);
                var result = '';
                for (var i = 0; i < data.length; i++) {
                    result += `<option value="${data[i].ID}">${data[i].Name}</option>`;
                }
                $("#grids").append(result);
            }
        });
    }
    //增加时根据区县得到网格
    function grids() {
        var checkcountryid = $("#county").val();
        $.ajax({
            url: 'AddPerson.aspx/GetRegion1',
            data: JSON.stringify({ countyid: checkcountryid }),
            dataType: 'json',
            type: 'post',
            async:false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                
                var result = '';
                for (var i = 0; i < data.length; i++) {
                    result += `<option value="${data[i].ID}">${data[i].Name}</option>`;
                }
                $("#grids").append(result);
            }
        });
    }

    //查询
    function getOne(id) {
        var data = { ID: id };
        $.ajax({
            url: 'PersonTable.aspx/GetOne',
            data: JSON.stringify(data),
            dataType: 'json',
            type: 'post',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                $("#role").val(data.RoleID)
                if ($('select[name="role"]').val() == 1 || $('select[name="role"]').val() == 2) {
                    $("#county").find("option:contains('请选择区县')").attr("selected", true);
                    $("#grids").find("option:contains('请选择网格')").attr("selected", true);
                    $("#county").attr("disabled", true);
                    $("#grids").attr("disabled", true);

                }
                if ($('select[name="role"]').val() == 3) {
                    //当角色为区县经理
                    $("#county").attr("disabled", false);

                    $("#grids").attr("disabled", true);
                }
                //网格助理
                if ($('select[name="role"]').val() == 4 || $('select[name="role"]').val() == 5) {
                    $("#grids").attr("disabled", false);
                    $("#county").attr("disabled", false);
                    $("#grids").empty();
                }
                //行业经理
                if ($('select[name="role"]').val() == 6) {
                    $("#county").find("option:contains('请选择区县')").attr("selected", true);
                    $("#grids").find("option:contains('请选择网格')").attr("selected", true);
                    $("#county").attr("disabled", true);
                    $("#grids").empty();
                    $("#grids").attr("disabled", true);
                }
                $("#name").val(data.Name)
                $("#tel").val(data.Mobile)
                $('#post').val(data.Post)
                
                //$('#county').val(data.Areas),
                // $("#county").filter(function () { return $(this).text() == data.Areas; }).attr("selected", true);
                $("#county option").filter(function () {
                    return $(this).text() == data.Areas;
                }).prop("selected", true);
                grids();
                
                $("#grids option").filter(function () {
                    return $(this).text() == data.Grids;
                }).prop("selected", true);

                $("#remark").val(data.Remark);

                
            }
        });
    }

   
    //增加，修改操作（1，增加，2，修改）
    function alertFormDialog() {
        var data = {
            ID: id,
            Name: $("#name").val(),
            Mobile: $("#tel").val(),
            Post: $('#post').val(),//岗位
            Areas: $('#county option:selected').text(),//区县
            roleid: $("#role").val(),//角色
            Remark: $("#remark").val(),//备注
            Grids: $("#grids option:selected").text()

        };
        //添加方法

        function AddPerson() {
        
                $.ajax({
                    url: 'AddPerson.aspx/AddPersons',
                    data: JSON.stringify(data),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {

                        var data = $.parseJSON(data.d);
                        if (data.state == 1) {

                            alert(data.msg);
                            window.location.href = "/PersonManage/PersonTable.aspx";
                        }
                    }
                })
            
        }
        //添加保存
        if (id ==null) {
            if ($("#name").val()=="")
            {
                alert('请输入姓名');
                return;
            }
            if ($("#tel").val()=="") {
                alert('请输入手机号');
                return;

            }
            var phone = document.getElementById('tel').value;
            if (!(/^1[34578]\d{9}$/.test(phone))) {
                alert("手机号码有误，请重填");
                return;
            }
            if ($("#role").val()=="") {
                alert('请选择角色');
                return;
            }
            if ($('select[name="role"]').val() == 1 || $('select[name="role"]').val() == 2 || $('select[name="role"]').val()==6) {
                AddPerson();
            }
            if ($('select[name="role"]').val() == 3) {
                if ($("#county").val() == "") {
                    alert('请选择所属区县');
                    return;
                }
                else
                {
                    AddPerson();

                }
            }
            if ($('select[name="role"]').val() == 4 || $('select[name="role"]').val() == 5) {
                if ($("#county").val() == "") {
                    alert('请选择区县');
                    return;
                }
                if ($("#grids").val() == "") {
                    alert('请选择网格');
                    return;
                }
                else
                {
                    AddPerson();
                }
            }
            
           
            
        }
         //修改保存
        else {
                $.ajax({
                    url: 'PersonTable.aspx/Edit',
                    type: 'post',
                    dataType: 'json',
                    contentType: "application/json;charset=utf-8",
                    data: JSON.stringify(data),
                    success: function (data) {
                        var data = $.parseJSON(data.d);                        
                        if (data.state == 1) {
                            alert(data.msg);
                        }
                        else {
                            alert(data.msg);

                        }

                        window.location.href = "/PersonManage/PersonTable.aspx";
                    }
                })
            }
    }

   
    //验证手机号输入
    function checkPhone() {
        var phone = document.getElementById('tel').value;
        if (!(/^1[34578]\d{9}$/.test(phone))) {
            alert("手机号码有误，请重填");
            return false;
        }
        else {
            return true;
        }
    }
    //得到区县
    function GetRegion() {
        $.ajax({
            url: 'AddPerson.aspx/GetRegion',
            type: 'post',
            dataType: 'JSON',
            async:false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var result = '';
                //var d = JSON.parse(data.d);
                var d = $.parseJSON(data.d);

                for (var i = 0; i < d.length; i++) {
                    result += `<option value="${d[i].ID}">${d[i].Name}</option>`;
                }
                $("#county").append(result);
            }
        })
    }

    //得到角色
    function GetRole()
    {
        $.ajax({
            url: 'AddPerson.aspx/GetRole',
            type: 'post',
            dataType: 'JSON',
            async: false,
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var result = '';
                //var d = JSON.parse(data.d);
                var d = $.parseJSON(data.d);

                for (var i = 0; i < d.length; i++) {
                    result += `<option value="${d[i].ID}">${d[i].RoleName}</option>`;
                }
                $("#role").append(result);
            }
        })

    }
</script>
