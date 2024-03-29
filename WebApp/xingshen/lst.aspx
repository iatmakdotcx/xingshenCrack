﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lst.aspx.cs" Inherits="telegramSvr.xingshen.lst" %>

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
                    <input type="button" class="layui-btn" onclick="location.href='rbts.aspx?gid=9'" value="G9" />
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
                    <th>Andorid</th>
                    <%if(ShowBanned){ %><th>Banned</th><%} %>
                    <%if(ShowRobots){ %><th>isRobot</th><%} %>
                </tr>
            </thead>
            <tbody>
                <%foreach (var item in Web.Model.XingshenUser_BLL.GetNormalALL(ShowBanned, ShowRobots)){%>
                <tr>
                    <td><%=item.user_name %></td>
                    <td><%=item.uuid %></td>
                    <td><%=item.isAndroid?"√":"" %></td>
                    <%if(ShowBanned){ %><td><%=item.isBanned?"√":"" %></td><%} %>
                    <%if(ShowRobots){ %><td><%=item.RobotGroup%></td><%} %>
                </tr>
                <% } %>
            </tbody>
        </table>
    </form>
    <div class="dialog">
        <div class="newdlg" style="display: none">
            <ul class="layui-form" style="margin: 10px;">
                <li class="layui-form-item">
                    <label class="layui-form-label">用户名</label>
                    <div class="layui-input-block">
                        <input type="text" name="user_name" lay-verify="required" required lay-verType="tips" autocomplete="off" class="layui-input" />
                    </div>
                </li>
                <li class="layui-form-item">
                    <label class="layui-form-label">密码</label>
                    <div class="layui-input-block">
                        <input type="text" name="password" lay-verify="required" required lay-verType="tips" autocomplete="off" class="layui-input" />
                    </div>
                </li>
                <li class="layui-form-item">
                    <label class="layui-form-label">UUID</label>
                    <div class="layui-input-block">
                        <input type="text" name="uuid" placeholder="选填" autocomplete="off" class="layui-input" />
                    </div>
                </li>
                <li class="layui-form-item">
                    <label class="layui-form-label">DeviceID</label>
                    <div class="layui-input-block">
                        <input type="text" name="mac" placeholder="选填" autocomplete="off" class="layui-input" />
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
    </div>
    <script src="../js/layui/layui.min.js"></script>
    <script>
        var player_data, player_data_bak;
        layui.use(['layer', 'element', "form"], function () {
            var layer = layui.layer, $ = layui.$, form = layui.form;
            $(".alluids tbody tr").click(function () {
                location.href = "accinfo.aspx?uid=" + $(this).find("td:eq(1)").text();
            });

            $("#btn_new").click(function () {
                layer.open({
                    type: 1
                    , title:"新增"
                    , resize: false
                    , content: $('.dialog .newdlg')
                    , success: function (layero) {
                        layero.find('.layui-layer-content').css('overflow', 'visible');
                        $("input[name=uuid]").closest("li").show();
                        $("input[name=mac]").closest("li").hide();
                    }
                });

            });

            $("#btn_Createnew").click(function () {
                layer.open({
                    type: 1
                    , title:"新增"
                    , resize: false
                    , content: $('.dialog .newdlg')
                    , success: function (layero) {
                        layero.find('.layui-layer-content').css('overflow', 'visible');
                        $("input[name=uuid]").closest("li").hide();
                        $("input[name=mac]").closest("li").show();
                    }
                });
            });
            form.render().on('submit(*)', function (data) {
                layer.load(2);
                var url = "<%=Request.Path%>?a=";
                if ($("input[name=uuid]").closest("li").is(":visible")) {
                    url += "new";
                } else {
                    url += "create";
                }
                $.ajax({
                    type: "POST",
                    url: url,
                    data: JSON.stringify(data.field),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            location.href = "accinfo.aspx?uid=" + data.uid;
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
        });
    </script>

</body>

    

</html>
