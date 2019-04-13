<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rbts.aspx.cs" Inherits="telegramSvr.xingshen.rbts" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <link rel="stylesheet" href="../js/layui/css/layui.css" />
    <style>
        body {
            padding: 20px;
            max-width: 1000px;
            margin: 0 auto;
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
                </tr>
            </thead>
            <tbody>
                <%foreach (var item in Web.Model.XingshenUser_BLL.GetGroup(Mak.Common.MakRequest.GetInt("gid", 1))){%>
                <tr>
                    <td><%=item.user_name %></td>
                    <td><%=item.uuid %></td>
                    <td><%=item.isAndroid?"√":"" %></td>
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
                        $("input[name=uuid]").closest("li").hide();
                        $("input[name=mac]").closest("li").show();
                    }
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
        });
    </script>

</body>

    

</html>
