<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rbts.aspx.cs" Inherits="telegramSvr.xingshen.rbts" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <meta name="renderer" content="webkit"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0"/>    
    <link rel="stylesheet" href="../js/layui/css/layui.css" />
    <style>
        @media (min-width: 1000px) {
           body {
                padding: 20px;
                max-width: 1000px;
                margin: 0 auto;
            }
        }
        .layui-table {
            margin: 0
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">
                <div class="layui-btn-container">
                   <input type="button" class="layui-btn" id="btn_CreateJob" value="开始任务" />
                </div>
            </div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" id="btn_Createnew" value="创建新账号" />
                <input type="button" class="layui-btn" id="btn_new" value="添加" />
            </div>
        </div>
        <table class="layui-table alluids" lay-skin="line">
            <thead>
                <tr>
                    <th>用户名</th>
                    <th>uuid</th>
                    <th title="Android">Ard</th>
                    <th>baned</th>
                    <th>商会令</th>
                </tr>
            </thead>
            <tbody>
                <%foreach (var item in Web.Model.XingshenUser_BLL.GetGroup(Mak.Common.MakRequest.GetInt("gid", 1))){%>
                <tr>
                    <td><%=item.user_name %></td>
                    <td><%=item.uuid %></td>
                    <td><%=item.isAndroid?"√":"" %></td>
                    <td><%=item.isBanned?"√":"" %></td>
                    <td><%=item.shl %></td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </form>
    <div class="dialog">
        <div class="newdlg" style="display: none">
            <ul class="layui-form" style="margin: 10px;">
                <li class="layui-form-item">
                    <label class="layui-form-label">账号个数</label>
                    <div class="layui-input-block">
                        <input type="text" name="cnt" value="1" autocomplete="off" class="layui-input" />
                        <input type="hidden" name="groupid" value="<%=Mak.Common.MakRequest.GetInt("gid",1) %>"" />
                    </div>
                </li>
                <li class="layui-form-item">
                    <label class="layui-form-label">平台</label>
                    <div class="layui-input-block">
                        <select name="platform">
                            <option value="0">Android</option>
                            <option value="1">IOS</option>
                        </select>
                    </div>
                </li>
                <li class="layui-form-item" style="text-align: center;">
                    <button type="submit" lay-submit lay-filter="*" class="layui-btn">提交</button>
                </li>
            </ul>
        </div>
        <div class="process" style="display: none;height:100%;">
           <div style="text-align:center"><span class="posi">0</span> / <span class="max">0</span></div>
           <textarea style="height: calc(100% - 30px);width: 99%;margin: 5px;resize:none;"></textarea>
        </div>
    </div>
    <script src="../js/layui/layui.min.js"></script>
    <script>
        var player_data, player_data_bak;
        layui.use(['layer', 'element', "form"], function () {
            var layer = layui.layer, $ = layui.$, form = layui.form;
            $(".alluids tbody tr").click(function () {
                location.href = "rbtctl.aspx?uid=" + $(this).find("td:eq(1)").text();
            });

            $("#btn_Createnew").click(function () {
                layer.open({
                    type: 1
                    , title:"新增"
                    , resize: false
                    , content: $('.dialog .newdlg')
                    , success: function (layero) {
                        layero.find('.layui-layer-content').css('overflow', 'visible');
                    }
                });
            });
            $("#btn_CreateJob").click(function () {
                layer.prompt({ title: '宗门id' }, function (sid, index) {
                    layer.close(index);
                    layer.load(2);
                    $.ajax({
                        url: "<%=Request.Path%>?a=cj&uid=<%=Request["uid"]%>&gid=<%=Mak.Common.MakRequest.GetInt("gid",1)%>&sid="+sid,
                        async: true,
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            layer.closeAll('loading');
                            if (data.ok) {
                                showProcessDlg(sid)
                            } else {
                                layer.msg(data.msg);
                            }
                        },
                        error: function (err) {
                            layer.closeAll('loading');
                            layer.msg(err.responseText, { icon: 2 });
                        }
                    });
                });
            });
            form.render().on('submit(*)', function (data) {
                layer.load(2);
                $.ajax({
                    type: "POST",
                    url: "<%=Request.Path%>?a=rc",
                    data: JSON.stringify(data.field),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            location.reload();
                        } else {
                            layer.msg(data.msg);
                        }
                    },
                    error: function (err) {
                        layer.closeAll('loading');
                        layer.msg(err.responseText, { icon: 2 });
                    }
                })
            });

            function showProcessDlg(sid) {
                layer.open({
                    type: 1
                    , title:"进度"
                    , resize: false
                    , closeBtn: 0
                    , area: ['95%', '95%']
                    , content: $('.dialog .process')
                    , success: function (layero) {
                        layero.find('.layui-layer-content').css('overflow', 'visible');
                    }
                });
                var siid = setInterval(function () {
                    $.ajax({
                        url: "<%=Request.Path%>?a=ji&uid=<%=Request["uid"]%>&gid=<%=Mak.Common.MakRequest.GetInt("gid",1)%>&sid=" + sid,
                        async: true,
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            if (data.ok) {
                                $(".process .posi").html(data.data.posi);
                                $(".process .max").html(data.data.max);
                                $(".process textarea").val(data.data.msg);
                                $('.process textarea').scrollTop($('.process textarea').prop("scrollHeight"), 10);
                                if (data.data.finish) {
                                    clearInterval(siid);
                                }
                            } else {
                                clearInterval(siid);
                            }
                        },
                        error: function (err) {
                            clearInterval(siid);
                        }
                    });
                }, 1000);
            }
            <% if(sect_id>0){%>
            showProcessDlg(<%=sect_id%>);
            <% }%>
        });
    </script>

</body>

    

</html>
