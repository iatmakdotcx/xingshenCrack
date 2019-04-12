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
                <input type="button" class="layui-btn" id="btn_donate" value="贡献" />
                <input type="button" class="layui-btn" id="btn_renewData" title="重新下载服务器存档" value="刷新存档" />
                <input type="button" class="layui-btn" id="btn_downfirst" value="Down" />
                <input type="button" class="layui-btn" id="btn_ok" value="确定" />
            </div>
        </div>
    </form>
    <script src="../js/layui/layui.js"></script>
    <script>
        var player_data, player_data_bak;
        var selectedRow;

        layui.use(['layer', 'element', 'table'], function () {


        });
   </script>
        
</body>
</html>
