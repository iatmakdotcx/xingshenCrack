<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rbtctl.aspx.cs" Inherits="telegramSvr.xingshen.rbtctl" %>

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
                    <input type="button" class="layui-btn" id="btn_back" onclick="location.href = 'rbts.aspx'" value="<<" />
                </div>
            </div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" id="btn_donate" value="宗门贡献" />
                <input type="button" class="layui-btn" id="btn_qrysects" value="宗门查询" />
                <input type="button" class="layui-btn" id="btn_sectjoin" value="宗门加入" />
                <input type="button" class="layui-btn" id="btn_ok" value="确定" />
            </div>
        </div>
    </form>
    <script src="../js/layui/layui.js"></script>
    <script>
        var player_data, player_data_bak;
        var selectedRow;

        layui.use(['layer', 'element', 'table'], function () {
            var layer = layui.layer, $ = layui.$, table = layui.table;

            $("#btn_donate").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=donate&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("ok");
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
            $("#btn_qrysects").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=qs&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("ok:"+data.name);
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
            $("#btn_sectjoin").click(function () {
                layer.prompt({ title: '宗门id' }, function (sid, index) {
                    layer.close(index);
                    layer.load(2);
                    $.ajax({
                        url: "<%=Request.Path%>?a=sj&uid=<%=Request["uid"]%>&sid="+sid,
                        async: true,
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            layer.closeAll('loading');
                            if (data.ok) {
                                layer.msg("ok");
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
        });
    </script>
        
</body>
</html>
