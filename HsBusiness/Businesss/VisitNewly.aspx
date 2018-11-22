<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitNewly.aspx.cs" Inherits="HsBusiness.Businesss.VisitNewly" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>衡水商机</title>
    <link rel="stylesheet" type="text/css" href="/css/initial.css">
    <link rel="stylesheet" type="text/css" href="/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="/js/control/css/zyUpload.css">
    <link rel="stylesheet" type="text/css" href="/css/Newly.css">
</head>
<body>
    <div class="Standard">
        <div class="widget-box">
            <div class="widget-title">
                <span class="icon"><i class="fa fa-align-justify"></i></span>
                <p>编辑</p>
            </div>
            <div class="widget-content nopadding">
                <form action="#" method="get" class="form-horizontal" id="form1">
                    <div class="span6-box">
                        <div class="span6">
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>本次拜访日期 :</label>
                                <div class="controls">
                                    <input type="text" id="VisitTime" placeholder="请选择拜访日期" name="VisitTime" data-birth="123" class="input_text_200" onclick="WdatePicker()"  />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>本次洽谈内容 :</label>
                                <div class="controls">
                                    <input type="text" id="VisitContent" name="VisitContent" class="span4" placeholder="请输入洽谈内容">
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>下次预约日期 :</label>
                                <div class="controls">
                                    <input type="text" id="NextTime"  placeholder="请选择预约日期" name="NextTime" data-birth="123" class="input_text_200" onclick="WdatePicker()"  />
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>是否需要公司领导拜访 :</label>
                                <div class="controls">
                                    <input type="radio" name="IsLeader" value="1" checked="checked">是
	                            <input type="radio" name="IsLeader" value="0">否
                                </div>
                            </div>
                            <div class="control-group">
                                <label class="control-label"><span class="red">*&nbsp;</span>需协同领导 :</label>
                                <div class="controls">
                                    <select class="input-small inline form-control" id="Leader" name="Leader">
                                        <option value="">请选择领导</option>
                                        <option value="总经理">总经理</option>
                                        <option value="副总经理">副总经理</option>
                                        <option value="政企部主任">政企部主任</option>
                                        <option value="区县经理">区县经理</option>
                                    </select>
                                </div>
                            </div>

                        </div>
                    </div>                    
                    <div id="demo" class="demo"></div>
                    <div class="form-actions">
                        <button class="btn btn-success" id="save">提交</button>
                        <button class="btn btn-danger" onclick="qx();">取消</button>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <div style="height: 50px;"></div>
    <script src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="/js/My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="/js/core/zyFile.js"></script>
    <script type="text/javascript" src="/js/control/js/zyUpload.js"></script>
    <%--<script type="text/javascript" src="/js/core/jq22.js"></script>--%>
    <script>
        var BusID;
        $('form').submit(function () { return false; });

        $(function () {
            BusID=getParameter("BusID")
            $("#save").click(function () { add(); });
            // 初始化插件
            $("#demo").zyUpload({
                width: "650px",                 // 宽度
                height: "",                 // 宽度
                itemWidth: "120px",                 // 文件项的宽度
                itemHeight: "100px",                 // 文件项的高度
                url: "/Interface/AddVisit.ashx",  // 上传文件的路径
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

            $("input[name='IsLeader']").change(function () {
                var $val = $("input[name='IsLeader']:checked").val();
                if ($val == "1") {
                    $("#Leader").parent().parent().show();
                } else {
                    $("#Leader").parent().parent().hide();
                    $("#Leader").val("");
                }
            });

            //本次访问时间
            $("#VisitTime").blur(function () {
                var $val = $("#VisitTime").val();
                var date = getDate();
                if ($val > date) {
                    alert("本次访问时间不能超过当前时间")
                    $("#VisitTime").val(date);
                }
            });
            //下次预约时间
            $("#NextTime").blur(function () {
                var $val = $("#NextTime").val();
                var date = getDate();
                if ($val < date) {
                    alert("下次预约时间不能小于当前时间")
                    $("#NextTime").val(date);
                }
            });
        });
        
       
        //取消
        function qx() {
            location.href = "InformationTable.aspx";
            //self.location = document.referrer;//返回上一页并刷新
        }

        //添加
        function add() {
            
            var data = {
                BusID:BusID,
                VisitTime: $("#VisitTime").val(),
                VisitContent: $("#VisitContent").val(),
                NextTime:$("#NextTime").val(),
                IsLeader: $("input[name='IsLeader']:checked").val(),
                Leader: $("#Leader").val(),
            };
            if (data.VisitTime == "") { alert("请选择本次拜访时间"); return; }
            if (data.VisitContent == "") { alert("请输入洽谈内容"); return; }
            //if (data.NextTime == "") { alert("请选择下次预约时间"); return; }
            if (data.IsLeader == "1") { if (data.Leader == "") { alert("请选择需协同领导"); return; } }
            if (data.IsLeader == "0") { data.Leader = ""; }
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
                  
            $.ajax({
                url: '/Interface/AddVisit.ashx',
                data:formdata,
                dataType: 'json',
                async:false,
                type: 'post',
                cache:false,
                // 告诉jQuery不要去处理发送的数据
                processData: false,
                // 告诉jQuery不要去设置Content-Type请求头
                contentType: false,
                success: function (data) {                    
                    alert(data.msg);
                    if (data.state == 1) { qx();}
                }
            });
        }

        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
        //获取当前时间
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
    </script>
</body>
</html>
