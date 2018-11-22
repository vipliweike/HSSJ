<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonTable.aspx.cs" Inherits="HsBusiness.PersonManage.PersonTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/initial.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/uniform.css" rel="stylesheet" />
    <link href="../css/dialog.css" rel="stylesheet" />
    <link href="../css/table.css" rel="stylesheet" />
    <link href="../css/kkpager_blue.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/kkpager.js"></script>
    <script type="text/javascript">
        $(function () {
            //关键字查询
            $("#search").click(function () {

                GetExcelTable(1);
            })

                //批量删除
                $("#del").click(function () {
                    // 判断是否至少选择一项   
                    var checkedNum = $("input[name='subChk']:checked").length;
                    if (checkedNum == 0) {
                        alert("请选择要删除的行！");
                        return;
                    }
                    if (window.confirm('你确定要删除吗？')) {

                        //声明一个数组
                        var checkedList = new Array();
                        //遍历选中的值
                        $("input[name='subChk']:checked").each(function () {
                            checkedList.push($(this).val());
                        });
                        $.ajax({
                            url: 'PersonTable.aspx/BatchDel',
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json;charset=utf-8",
                            data: JSON.stringify({ ids: checkedList.toString() }),
                            success: function (result) {
                                $("[name ='subChk']:checkbox").attr("checked", false);
                                var result = $.parseJSON(result.d);
                                alert(result.msg);
                                window.location.reload();
                            }
                        });

                    }
                    else {
                        return false;

                    }

                })
               
        });

        function get() {
            $("tbody").empty();
            var data = {
                Areas: $("#counties option:selected").val(),
                Post: $("#post option:selected").val(),
                Name: $("#Name").val()

            };
            $.ajax({
                url: 'PersonTable.aspx/Get',
                type: 'post',
                dataType: 'JSON',
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify(data),
                success: function (data) {
                    var result = '';
                    //var d = JSON.parse(data.d);
                    var d = $.parseJSON(data.d);

                    for (var i = 0; i < d.length; i++) {
                        result += `<tr>
							<td><div class ="checker" id="uniform-undefined"><input name="subChk" type="checkbox" class ="ID" hidden="hidden" value="${d[i].ID}" /></div></td>
                
							<td>${d[i].Areas}</td>
							<td>${d[i].Post}</td>
							<td>${d[i].Name}</td>
							<td>${d[i].Mobile}</td>
							<td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='AddPerson.aspx?id=${d[i].ID}'">编辑</button><button class="btn btn-success btn-mini" onclick="ResetPwd(${d[i].ID})">重置密码</button><button class ="btn btn-danger btn-mini delete" onclick="Del(${d[i].ID})">删除</button></td>
						</tr>`;
                    }
                    $("#tbody").append(result);
                    $('input[type=checkbox],input[type=radio],input[type=file]').uniform();
                }


            })


        }
    
        //单个删除
        function Del(id) {
          
            if (window.confirm('你确定要删除吗？')) {
                var data = { id: id }
                $.ajax({
                    url: 'PersonTable.aspx/Delete',
                    data: JSON.stringify(data),
                    dataType: 'json',
                    type: 'post',
                    contentType: "application/json;charset=utf-8",
                    success: function (data) {
                        var data = $.parseJSON(data.d);
                        if (data.state == 1) {
                            alert(data.msg)
                        }
                        window.location.reload();
                    }
                });
            }

            else {
                return false;
            }

        }

        //重置密码
        function ResetPwd(id)
        {

            var data = { id: id }
            $.ajax({
                url: 'PersonTable.aspx/ResetPwd',
                data: JSON.stringify(data),
                dataType: 'json',
                type: 'post',
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    var data = $.parseJSON(data.d);
                    if (data.state == 1) {
                        alert(data.msg)
                    }
                   // window.location.reload();
                }
            });
        }
   
    </script>
</head>
<body>
    <div class="Standard">
        <div class="widget-box">
            <div class="widget-title">
                <p>控制台</p>
                <div class="control1">
                    <select class="input-small inline form-control" id="counties">
                        <option value="">请选择区县</option>
                        <%--<option value="衡水市">衡水市</option>--%>
                    </select>
                    <select class="input-small inline form-control" id="post">
                        <option value="">请选择角色</option>
                        <option value="公司领导">公司领导</option>
                        <option value="政企部主管">政企部主管</option>
                        <option value="网格助理">网格助理</option>
                        <option value="区县经理">区县经理</option>
                        <option value="客户经理">客户经理</option>
                        <option value="行业经理">行业经理</option>
                    </select>
                    <div class="search">
                        <input type="text" placeholder="请输入关键字" id="Name" />
                    </div>
                    <button class="btn btn-info btn-mini" id="search">确定</button>
                </div>
                <div class="control2">
                    <button class="btn btn-danger btn-mini" id="del">删除</button>
                   <button class="btn btn-primary btn-mini btn-14">模板导入</button>
                    
                </div>
            </div>
            <div class="widget-content nopadding">
                <table class="table table-bordered table-striped with-check">
                    <thead>
                        <tr>
                            <th>
                                <div class="checker" id="uniform-title-table-checkbox">
                                <input type="checkbox" id="title-table-checkbox" name="title-table-checkbox"/>
                                </div>
                            </th>
                            <th>区县</th>
                            <th>岗位</th>
                            <th>负责人</th>
                            <th>联系电话</th>
                            <th class="operation">操作</th>
                        </tr>
                    </thead>
                    <tbody id="tbody">
                    </tbody>
                </table>
            </div>
        </div>
       <div class="pagination alternate">
        <div id="kkpager">

        </div>
        </div>
      
    
   
             
    </div>


<script src="../js/dialog.js"></script>
<script src="../js/browser.min.js"></script>
<script src="../js/example.js"></script>
<script src="../js/jquery.uniform.js"></script>
<script src="../js/jquery.dataTables.min.js"></script>
<script src="../js/matrix.tables.js"></script>
<script type="text/javascript">
    function getParameter(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
    function GetExcelTable(pageindex) {
        $("tbody").empty();
        var data = {
            Areas: $("#counties option:selected").val(),
            Post: $("#post option:selected").val(),
            Search: $("#Name").val(),
            pageIndex: pageindex
        };
        $.ajax({
            url: 'PersonTable.aspx/Get',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            data:JSON.stringify(data),
            success: function (data) {
                var d = $.parseJSON(data.d);
                if (d.pagecount==0||d.data.length==0) {
                    $("#tbody").empty();
                    $("#tbody").html("<tr><td colspan='5'><span style='text-align:center; color:red'>暂无数据</span></td></tr>");
                    $("#kkpager").hide();
                    return;
                }
                var result = '';
                
                $("#kkpager").show();
                for (var i = 0; i < d.data.length; i++) {
                    result += `<tr>
							<td><div class ="checker" id="uniform-undefined"><input name="subChk" type="checkbox" class ="ID" hidden="hidden" value="${d.data[i].ID}" /></div></td>

							<td>${d.data[i].Areas||""}</td>
							<td>${d.data[i].Post||""}</td>
							<td>${d.data[i].Name||""}</td>
							<td>${d.data[i].Mobile||""}</td>
							<td><button class ="btn btn-info btn-mini" onclick="window.parent.test.location.href='AddPerson.aspx?id=${d.data[i].ID}'">编辑</button><button class="btn btn-success btn-mini" onclick="ResetPwd(${d.data[i].ID})">重置密码</button><button class ="btn btn-danger btn-mini delete" onclick="Del(${d.data[i].ID})">删除</button></td>
						</tr>`;
                }
                $("#tbody").append(result);
                $('input[name=subChk]').uniform();
                //定义分页样式
                var totalCount = parseInt(d.pagecount);
                
                var pageSize = parseInt(d.pagesize);

                var pageNo = getParameter('pageIndex');//该参数为插件自带，不设置好，调用数据的时候当前页码会一直显示在第一页

                if (!pageNo) {
                    pageNo = pageindex;
                }
                var totalPages = totalCount % pageSize == 0 ? totalCount / pageSize : (parseInt(totalCount / pageSize) + 1);
                kkpager.generPageHtml({
                    pno: pageNo,
                    total: totalPages,
                    totalRecords: totalCount,
                    mode: 'click',
                    click: function (n) {
                        this.selectPage(pageNo);
                        searchPage(n);
                        return false;
                    }
                }, true);
            }, error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
    //init
    $(function () {
        getRegion();
        GetExcelTable(1);
      
    });
    function getRegion() {
        $.ajax({
            url: 'PersonTable.aspx/GetRegion',
            dataType: "json",
            type: "POST",
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                var data = $.parseJSON(data.d);
                var result = ``;
                for (var i = 0; i < data.length; i++) {
                    result += `
                        <option value="${data[i].Name}">${data[i].Name}</option>
                        `;
                }
                $("#counties").append(result);
            }
        });
    }
    //ajax翻页
    function searchPage(n) {
        GetExcelTable(n);
    }
    $('.btn-14').click(function () {
        $.dialog({
            type: 'confirm',
            buttonText: {
                titleText: '模板导入',
                ok: '导入',
                cancel: '取消'
            },
            onClickOk: function () {
                Import();
            },
            ///<p>1、请先<button class="red" id="drmb" onclick="Drmb()">下载模板</button>，然后填入数据。</p>
            contentHtml: '<p>1、请先<a class="red" id="drmb"  href="/Interface/DownLoad.ashx?action=person">下载模板</a>，然后填入数据。</p><p>2、选择填入数据的模板文件，点击导入。</p><p><input type="file" id="files" style="padding:0 2px;border: 1px solid #ccc;"></p>'
        });
    });
</script>
 <script type="text/javascript">
     //下载模板
     function Drmb()
     {
         $.ajax({
             url: 'PersonTable.aspx/Template',
             dataType: 'json',
             type: 'post',
             contentType: "application/json;charset=utf-8",
             enctype:"multipart/form-data",
             success: function (data) {
                 var data = $.parseJSON(data.d);
                 if (data.state == 1) {
                     alert(data.msg)
                 }
                 // window.location.reload();
             }
         });
     }
    
     function Import()
     {
         
         var file = $("#files").val();
         if (file == null || file.length == 0) {
             alert("请先选择上传文件");
             return false;
         }
         var formData = new FormData();
         formData.append("file", document.getElementById("files").files[0]);
         var file = document.getElementById("files").value;
         var form1 = document.getElementById("form");
         var ext = file.slice(file.lastIndexOf(".") + 1).toLowerCase();    
         $.ajax({
             url: "/Interface/Import.ashx",
            dataType: 'json',
            type: 'post',
            //contentType: "application/json;charset=utf-8",
            contentType: false,
            processData: false,
            data:formData ,
            success: function (data) {
             //if (data.state == 1) {
             //    // $.messager.alert("成功", data.msg, "info");
             //    alert(data.msg);
             //    location.reload();
                //}
                alert(data.msg);
                if (data.state == 1) {
                    window.parent.refreshFrame();
                }
            
            
         },
       
     });
          
     }

 </script>
    
</body>
</html>
