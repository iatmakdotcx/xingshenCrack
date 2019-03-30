<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lst.aspx.cs" Inherits="telegramSvr.xingshen.lst" %>

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

        .editor .strings {
            width: 60%;
            float: left;
        }

        .editor .meta {
            background: #fff;
            border: 1px solid #eee;
            font-size: 13px;
            margin: 0;
            padding: 5px 10px;
            width: 39%;
            float: right;
            -webkit-box-sizing: border-box;
            -moz-box-sizing: border-box;
            box-sizing: border-box;
            min-width: 160px
        }

        .layui-tab-content {
            padding: 0;
        }

        .layui-table {
            margin: 0
        }

        .fulldata {
            height: 400px;
        }
        .layui-layer-content{
            overflow:visible;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">
                <div class="layui-btn-container">
                   <input type="button" class="layui-btn" id="btn_proxystatus" value="代理状态" />
                </div>
            </div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" id="btn_new" value="新增" />
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
                <%foreach (var item in Web.Model.XingshenUser_BLL.GetALL()){%>
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
    <script src="../js/layui/layui.js"></script>
    <script>
        var player_data, player_data_bak;
        layui.use(['layer', 'element', "form"], function () {
            var layer = layui.layer, $ = layui.$, form = layui.form;
            $(".alluids tbody tr").click(function () {
                location.href = "uf.aspx?uid=" + $(this).find("td:eq(1)").text();
            });

            $("#btn_new").click(function () {
                layer.open({
                    type: 1
                    , title:"新增"
                    , resize: false
                    , content: $('.dialog .newdlg')
                    , success: function (layero) {
                        layero.find('.layui-layer-content').css('overflow', 'visible');
                        form.render().on('submit(*)', function (data) {                            
                            layer.load(2);
                            $.ajax({
                                type: "POST",
                                url: "<%=Request.Path%>?a=new",
                                data: JSON.stringify(data.field),
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    layer.closeAll('loading');
                                    if (data.ok) {
                                        location.href = "uf.aspx?uid=" + data.uid;
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
                    }
                });

            });

            $("#btn_proxystatus").click(function () {
                layer.open({
                    type: 2
                    , title:"代理状态"
                    , resize: false
                    , area:['300px', '300px']
                    , content: "proxyStatus.aspx"
                    , success: function (layero) {
                       
                    }
                });

            });
        });
    </script>

</body>

    

</html>
