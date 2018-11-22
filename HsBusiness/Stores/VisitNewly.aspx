<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitNewly.aspx.cs" Inherits="HsBusiness.Stores.VisitNewly" %>

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
                <form action="#" method="get" class="form-horizontal">                    
                    <div id="vis">
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
                                        <input type="text" placeholder="请选择下次拜访时间" name="sbirth" id="NextTime" data-birth="123" class="input_text_200" onclick="WdatePicker()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-actions">
                        <button class="btn btn-success" onclick="add();">提交</button>
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
    <script type="text/javascript">
        var StoreID;
        $(function () {
            StoreID = getParameter("StoreID");           

            $("form").submit(function () { return false; });

            $("input[name='IsAgain']").change(function () {
                var $val = $("input[name='IsAgain']:checked").val();
                console.log($val);
                if ($val == "1") {
                    $("#NextTime").parent().parent().show();
                } else {
                    $("#NextTime").parent().parent().hide();
                }
            });
            
        });
        
        function qx() {            
            location.href = "/Stores/StoreTable.aspx";
        }

        function add() {
            var data = {
                StoreID:StoreID,
                IsNeed: $("input[name='IsNeed']:checked").val(),
                IsAgain: $("input[name='IsAgain']:checked").val(),
                VisitContent: $("#VisitContent").val(),
                NextTime: $("#NextTime").val(),
            }

            $.ajax({
                url: 'VisitNewly.aspx/Add',
                data: JSON.stringify({ data: JSON.stringify(data) }),
                dataType: 'json',
                contentType: "application/json;charset=utf-8",
                type: 'post',
                success: function (data) {
                    var data = $.parseJSON(data.d)
                    alert(data.msg)
                    if (data.state == 1) {
                        qx();
                    }
                }

            });

        }

        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>
</body>
</html>
