<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStore.aspx.cs" Inherits="HsBusiness.Stores.AddStore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="../css/initial.css" />
    <link rel="stylesheet" type="text/css" href="../css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/Newly.css" />
    <link href="../js/control/css/zyUpload.css" rel="stylesheet" />
</head>
<body>
    <div class="Standard">
        <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="fa fa-align-justify"></i></span>
                <p>编辑</p>
            </div>
            <div class="widget-content nopadding">
                <form action="#" method="get" class="form-horizontal">
                    <div class="span6-box">
                        <div class="span6">
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span><span id="mdname">&nbsp;</span>门店名称 :</label>
                                <div class="controls">
                                    <input type="text" class="span4" placeholder="请输入门店名称" id="storename">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red" id="mdaddress" hidden="hidden">*&nbsp;</span>门店地址 :</label>
                                <div class="controls">
                                    <input type="text" class="span4" placeholder="请输入门店地址" id="mendaddress" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red" id="hzyys" hidden="hidden">*&nbsp;</span>合作运营商 :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="operator">
                                        <option value="">请选择运营商</option>
                                        <option value="移动">移动</option>
                                        <option value="联通">联通</option>
                                        <option value="电信">电信</option>
                                        <option value="无">无</option>
                                    </select>
                                </div>
                            </div>
                            <div class="control-group" id="kddiv">
                                <label class="control-label"><span class="red" id="price" hidden="hidden">*&nbsp;</span>宽带价格（元） :</label>
                                <div class="controls">
                                    <input type="text" class="span2" placeholder="请输入宽带价格" id="kdprice">
                                </div>
                            </div>
                            <div class="control-group" id="otherywdiv">
                                <label class="control-label"><span class="red" id="otheryw" hidden="hidden">*&nbsp;</span>其他业务 :</label>
                                <div class="controls">
                                    <input type="text" class="span2" placeholder="请输入其他业务" id="YW" />
                                </div>
                            </div>
                            <div class="control-group" id="kdovertime">
                                <label class="control-label"><span class="red" id="overtimes" hidden="hidden">*&nbsp;</span>宽带到期时间 :</label>
                                <div class="controls">                                    
                                    <input type="text" placeholder="请选择宽带到期时间" name="sbirth" id="overtime" data-birth="123" class="input_text_200" onclick="WdatePicker()" />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red" id="linkmans" hidden="hidden">*&nbsp;</span>联系人 :</label>
                                <div class="controls">
                                    <input type="text" class="span2" placeholder="请输入姓名" id="linkman">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red" id="tels" hidden="hidden">*&nbsp;</span>联系电话 :</label>
                                <div class="controls">
                                    <input type="text" class="span3" placeholder="请输入手机号" id="tel" maxlength="11" />
                                </div>
                            </div>
                            <div id="demo" class="demo"></div>
                        </div>
                    </div>
                    <div id="vis">
                        <div class="Subtitle" id="box-c">
                            <p>走访记录</p>
                            <a href="javascript:void(0);" id="Takeup-3">点击收起</a>
                            <a href="javascript:void(0);" id="Open-3" style="display: none;">点击展开</a>
                        </div>
                        <div class="span6-box box-C">
                            <div class="span6">
                                <div class="control-group">
                                    <label class="control-label">是否是否有业务需求 :</label>
                                    <div class="controls">
                                        <input type="radio" name="IsNeed" value="1" checked="checked" />是
                                        <input type="radio" name="IsNeed" value="0" />否
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">本次交流情况 :</label>
                                    <div class="controls">
                                        <input type="text" class="span3" placeholder="本次交流情况" id="VisitContent">
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">是否需要再次拜访 :</label>
                                    <div class="controls">
                                        <input type="radio" name="IsAgain" value="1" checked="checked" />是
                                <input type="radio" name="IsAgain" value="0" />否
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">下次拜访时间 :</label>
                                    <div class="controls">
                                        <input type="text" placeholder="请选择下次拜访时间" name="sbirth" id="NextTime" data-birth="123" class="input_text_200" onclick="WdatePicker()"  />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-actions">
                        <button class="btn btn-success" onclick="alertFormDialog()">提交</button>
                        <button class="btn btn-danger" id="cancel">取消</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
</body>
</html>

<script src="../js/jquery.min.js"></script>
<script src="../js/browser.min.js"></script>
<script src="../js/dialog.js"></script>
<script type="text/javascript" src="/js/My97DatePicker/WdatePicker.js"></script>
<script src="/js/control/js/zyUpload.js"></script>
<script src="/js/core/zyFile.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#box-c").click(function () {
            $(".box-C").slideToggle("slow");
            $("#Takeup-3").toggle();
            $("#Open-3").toggle();
        });
    });
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
            getOne(id);
        }
        //取消
        $("#cancel").click(function () {
            window.location.href = "/Stores/StoreTable.aspx"

        });
        $("input[name='IsAgain']").change(function () {
            var $val = $("input[name='IsAgain']:checked").val();
            if ($val == "1") {
                $("#NextTime").parent().parent().show();
              
            } else {
                $("#NextTime").parent().parent().hide();
                $("#NextTime").val("");
            }
        });
        //下次拜访时间大于当前时间
        $("#NextTime").on("blur", function () {
            var $val = $("#NextTime").val();
            console.log($val);
            var date = getDate();
            console.log(date)
            if ($val < date) {
                $(this).val(date);
                alert("下次拜访时间不能小于当前时间")
            }
        });
        //到期时间
        $("#overtime").on("blur",function () {
            var $val = $("#overtime").val();
            console.log($val);
            var date = getDate();
            console.log(date)
            if ($val < date) {
                $(this).val(date);
                alert("到期时间不能小于当前时间")
            }
        });
        //判断是否有运营商
        $("#operator").change(function () {
            var yunysname = $(this).val();
            if (yunysname == "无") {
                $("#kddiv").hide();
                $("#kdovertime").hide();
                $("#linkman").empty();
                $("#tel").empty();

            }
            else {

                $("#kddiv").show();
                $("#kdovertime").show();
            }
        });

       

        $("#demo").zyUpload({
            width: "650px",                 // 宽度
            height: "",                 // 宽度
            itemWidth: "120px",                 // 文件项的宽度
            itemHeight: "100px",                 // 文件项的高度
            url: "",  // 上传文件的路径
            multiple: true,                    // 是否可以多个文件上传
            dragDrop: false,                    // 是否可以拖动上传文件
            del: true,                    // 是否可以删除文件
            finishDel: false,  				  // 是否在上传文件完成后删除预览
            /* 外部获得的回调接口 */
            onSelect: function (files, allFiles) {                    // 选择文件的回调方法
               
                console.info("当前选择了以下文件：");
                console.info(files);
                console.info("之前没上传的文件：");
                console.info(allFiles);
            },
            onDelete: function (file, surplusFiles) {                     // 删除一个文件的回调方法
                console.info("当前删除了此文件：");
                console.info(file);
                console.info("当前剩余的文件：");
                console.info(surplusFiles);
            },
            onSuccess: function (file) {                    // 文件上传成功的回调方法
                console.log(file.length)
                console.info("此文件上传成功：");
                console.info(file);
            },
            onFailure: function (file) {                    // 文件上传失败的回调方法
                console.info("此文件上传失败：");
                console.info(file);
            },
            onComplete: function (responseInfo) {           // 上传完成的回调方法
                console.info("文件上传完成");
                console.info(responseInfo);
            }
        });



        // 监听input的change事件，通过e拿到文件
        $('#fileImage').change(e => {
            const files = e.target.files || e.dataTransfer.files;
            if (files.length > 6) {
                e.preventDefault();
                alert("不要超过6张");
            }
        });
        
        
    })


    
    
    function getDate() {
        var myDate = new Date();//获取当前日期
        // 获取当前日期
        var date = new Date();

        // 获取当前月份
        var nowMonth = date.getMonth() + 1;

        // 获取当前是几号
        var strDate = date.getDate();

        // 添加分隔符“-”
        var seperator = "-";

        // 对月份进行处理，1-9月在前面添加一个“0”
        if (nowMonth >= 1 && nowMonth <= 9) {
            nowMonth = "0" + nowMonth;
        }

        // 对月份进行处理，1-9号在前面添加一个“0”
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }

        // 最后拼接字符串，得到一个格式为(yyyy-MM-dd)的日期
        var nowDate = date.getFullYear() + seperator + nowMonth + seperator + strDate;

        return nowDate;
    }
    //查询
    function getOne(id) {
        var data = { ID: id };
        $.ajax({
            url: 'StoreTable.aspx/GetOne',
            data: JSON.stringify(data),
            dataType: 'json',
            type: 'post',
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                $("#storename").val(data.StoreName);
                $("#operator").val(data.Broadband);
                $("#kdprice").val(data.Price);
                $("#overtime").val(data.OverTime);
                $("#linkman").val(data.ContactName);
                $("#tel").val(data.ContactTel);
                $("#mendaddress").val(data.StoreAddress)
            }
        });
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
    //增加，修改操作（1，增加，2，修改）
    function alertFormDialog() {
        var data = {
            ID: id || 0,
            StoreName: $("#storename").val(),
            Broadband: $("#operator").val(),
            Price: $("#kdprice").val(),
            OverTime: $("#overtime").val(),
            ContactName: $("#linkman").val(),
            ContactTel: $("#tel").val(),
            StoreAddress: $("#mendaddress").val(),
            OtherNeeds: $("#YW").val()

        };
        //添加保存
        if (id == null) {

            if ($("#storename").val() == "") {
                $("#mdname").show();
                alert('请填写门店名称');
                return;
            }
            else {
                $("#mdname").hide();
            }
            if ($("#mendaddress").val() == "") {
                $("#mdaddress").show();
                alert('请填写门店地址');
                return;
            }
            else {
                $("#mdaddress").hide();
            }
            if ($("#operator").val() == "") {
                $("#hzyys").show();
                alert('请选择运营商');
                return;

            }
            else {
                $("#hzyys").hide();
            }
            //if ($("#kdprice").val() == "") {
            //    $("#price").show();

            //}
            //if ($("#overtime").val() == "") {
            //    $("#overtimes").show();

            //}
            if ($("#linkman").val() == "") {
                $("#linkmans").show();
                alert('请填写联系人');
                return;
            }
            else {
                $("#linkmans").hide();
            }
            if ($("#tel").val() == "") {
                $("#tels").show();
                alert('请填写联系人电话');
                return;
            }
            else {
                $("#tels").hide();
            }
            if (checkPhone()) {
                var ImgFiles = ZYFILE.funReturnNeedFiles();
                if (ZYFILE.funReturnNeedFiles().length > 6) {
                    alert("图片不能超过6张");
                    return false;
                }
                console.log(ImgFiles)
                var formdata = new FormData();
                formdata.append("data", JSON.stringify(data));
                for (var i = 0; i < ImgFiles.length; i++) {
                    formdata.append("files", ImgFiles[i]);
                }

                var storeVisit = {
                    IsNeed: $("input[name='IsNeed']:checked").val(),
                    IsAgain: $("input[name='IsAgain']:checked").val(),
                    VisitContent: $("#VisitContent").val(),
                    NextTime: $("#NextTime").val(),
                }
                if (storeVisit.IsAgain == "1") { if (storeVisit.NextTime == "") { alert("请选择下次拜访时间"); return false; } }
                formdata.append("storeVisit", JSON.stringify(storeVisit))

                $.ajax({
                    url: '/Interface/AddStoreVisit.ashx',
                    data: formdata,
                    dataType: 'json',
                    async: false,
                    type: 'post',
                    cache: false,
                    // 告诉jQuery不要去处理发送的数据
                    processData: false,
                    // 告诉jQuery不要去设置Content-Type请求头
                    contentType: false,
                    success: function (data) {
                        alert(data.msg);
                        window.location.href = "/Stores/StoreTable.aspx";

                    }
                });
            }
        }
            //修改保存
        else {
            $.ajax({
                url: 'StoreTable.aspx/Edit',
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
                    window.location.href = "/Stores/StoreTable.aspx";
                }
            })
        }
    }
    //绑定监听事件
    function addEventHandler(target, type, fn) {
        if (target.addEventListener) {
            target.addEventListener(type, fn);
        } else {
            target.attachEvent("on" + type, fn);
        }
    }
</script>
