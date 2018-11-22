<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="HsBusiness.main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>衡水商机</title>
    <link href="css/initial.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/leftnav.css" media="screen" type="text/css" />
</head>
<body>
    <div class="pg-header">
        <div class="logo">
            <img src="images/logo.png" height="100%">
            <p>衡水商机</p>
        </div>
        <div class="header-right" onclick="exit()">
            <div class="user"><i class="fa fa-power-off"></i>退出登录</div>
        </div>
        <div class="header-right">
            <div class="user">
                <i class="fa fa-user"></i><span>欢迎您,<%= Common.TCContext.Current.OnlineRealName%></span>
                <%-- <ul>
                    <li>
                        <a href="Administrators.html" target="test"><i class="fa fa-user"></i> 基本信息</a>
                    </li>
                    <li>
                        <a href="Password-modification.html" target="test"><i class="fa fa-key"></i> 修改密码</a>
                    </li>
                </ul>--%>
            </div>
        </div>
        <!--
        <div class="header-right">
            <div class="user"><i class="fa fa-envelope"></i>消息<span class="Number">4</span></div>
        </div>
    -->
        <div class="header-right">
            <div class="user" id="ref"><i class="fa fa-refresh"></i>刷新</div>
        </div>
    </div>
    <div class="pg-content">
        <div class="menu">
            <div class="account-l fl">
                <!--
                <a class="list-title" style="color: #f2f2f2;background-color: #333e4a;border-left: 2px solid #A4D3EE;">商机概览</a>
            -->
                <ul id="accordion" class="accordion">

                    <li class="open" id="welcome" runat="server">
                        <a href="Welcome.aspx" target="test">
                            <div class="link"><i class="fa fa-globe fa-fw"></i>商机概览</div>
                        </a>
                    </li>
                    <li class="open" id="news" runat="server">
                        <a href="/Information/News.aspx" target="test">
                            <div class="link"><i class="fa fa-globe fa-fw"></i>消息中心</div>
                        </a>
                    </li>
                    <!--
                    <li>
                        <div class="link"><i class="fa fa-bell"></i>消息管理<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li id="mymsg"><a>我的消息</a></li>
                        </ul>
                    </li>
                -->
                    <li id="person" runat="server">
                        <div class="link"><i class="fa fa-user-circle fa-fw"></i>人员管理<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li><a href="../PersonManage/AddPerson.aspx" target="test">人员新增</a></li>
                            <li><a href="../PersonManage/PersonTable.aspx" target="test">人员列表</a></li>
                            <li><a href="/PersonManage/ActivityTable.aspx" target="test">活跃度统计</a></li>
                        </ul>
                    </li>
                    <li id="business" runat="server">
                        <div class="link"><i class="fa fa-list-ol fa-fw"></i>商机管理<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li><a href="/Businesss/InformationNewly.aspx?id=0" target="test">新增商机</a></li>
                            <li><a href="/Businesss/InformationTable.aspx" target="test">商机信息表</a></li>
                        </ul>
                    </li>
                    <li id="project" runat="server">
                        <a href="/ProjectManage/ProjectTable.aspx" target="test">
                            <div class="link"><i class="fa fa-shopping-cart fa-fw"></i>项目派单</div>
                        </a>
                    </li>
                    <li id="balance" runat="server">
                        <div class="link"><i class="fa fa-pie-chart fa-fw"></i>到期派单<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li><a href="/BalanceManage/BalanceTable.aspx" target="test">存量派单</a></li>
                            <li><a href="/ArrearsManage/ArrearsTable.aspx" target="test">欠费派单</a></li>
                        </ul>
                    </li>
                    <%-- <li id="store" runat="server">
                        <div class="link"><i class="fa fa-map-marker fa-fw"></i>门店管理<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li><a href="../Stores/AddStore.aspx" target="test">新增管理</a></li>
                            <li><a href="../Stores/StoreTable.aspx" target="test">门店列表</a></li>
                        </ul>
                    </li>--%>
                    <li>
                        <div class="link"><i class="fa fa-hdd-o fa-fw"></i>专线建档<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li><a href="/Specialline/AddCollection.aspx?id=0" target="test">新增专线</a></li>
                            <li><a href="/Specialline/CollectionTable.aspx" target="test">专线列表</a></li>
                        </ul>
                    </li>
                    <li id="busShow" runat="server">
                        <a href="/ReportsManage/BusinessData.aspx" target="test">
                            <div class="link"><i class="fa fa-bar-chart-o fa-fw"></i>业务展示</div>
                        </a>
                    </li>

                    <li id="safe" runat="server">
                        <div class="link"><i class="fa fa-unlock-alt fa-fw"></i>安全管理<i class="fa fa-chevron-down"></i></div>
                        <ul class="submenu">
                            <li id="passwordmodify"><a href="Password-Modification.aspx" target="test" id="pwd">修改密码</a></li>
                            <li id="reminder" runat="server"><a href="SetReminder.aspx" target="test">设置提醒</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
        <div class="content">
            <div id="content-header">
                <div id="breadcrumb"><a href="?act=index.main" title="系统" class="tip-bottom"><i class="fa fa-home"></i>系统</a><a href="#" title="商机预览" id="currentPage" class="current" runat="server">商机预览</a></div>
            </div>
            <%if ((Common.TCContext.Current.OnlineUserType == "公司领导" && Common.TCContext.Current.OnlineRealName == "admin") || Common.TCContext.Current.OnlineUserType == "公司领导" || Common.TCContext.Current.OnlineUserType == "政企部主管" || Common.TCContext.Current.OnlineUserType == "区县经理")
                {
            %>
            <iframe src="Welcome.aspx" name="test" id="iframe-main" frameborder="0"></iframe>
            <%} %>

            <%if (Common.TCContext.Current.OnlineUserType == "客户经理" || Common.TCContext.Current.OnlineUserType == "网格助理" || Common.TCContext.Current.OnlineUserType == "行业经理")
                {
            %>
            <iframe src="Information/News.aspx" name="test" id="iframe-main" frameborder="0"></iframe>
            <%} %>
        </div>
    </div>
    <div class="pg-footer"></div>
    <script src="js/jquery.min.js"></script>
    <script src='js/leftnav.js'></script>
    <script type="text/javascript">
        var opr;
        $(function () {
            opr = getParameter("opr");
            //刷新
            $("#ref").click(function () {
                refreshFrame();
            });

            $(".submenu li a").click(function () {
                $("#currentPage").html($(this).html());
            });

            $("#welcome").click(function () {
                $("#currentPage").html("商机概览");
            });
            $("#news").click(function () {
                $("#currentPage").html("消息中心");
            });
            $("#project").click(function () {
                $("#currentPage").html("项目派单");
            });
            //$("#balance").click(function () {
            //    $("#currentPage").html("存量小余额");
            //});
            $("#busShow").click(function () {
                $("#currentPage").html("业务展示");
            });
            console.log(opr);
            if (opr == "pwd") {
                $("#iframe-main").removeAttr("src").attr("src", "Password-Modification.aspx")
            }
        });
        //刷新
        function refreshFrame() {
            document.getElementById('iframe-main').contentWindow.location.reload(true);
        }
        //退出登陆
        function ExitLogin() {
            $.ajax({
                url: "/Login.aspx/ExitLogin",
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    window.location.href = "/Login.aspx";
                }

            })
        }
        function exit() {
            if (confirm('您是否要退出系统') == true) {
                $.ajax({
                    url: "/Login.aspx/ExitLogin",
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        window.location.href = "/Login.aspx";
                    }

                })
            }
        }
        function getParameter(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        }
    </script>

</body>
</html>
