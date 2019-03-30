<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="proxyStatus.aspx.cs" Inherits="telegramSvr.xingshen.proxyStatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>代理状态</title>
    <link rel="stylesheet" href="../js/layui/css/layui.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="layui-form-item">
                <label class="layui-form-label">状态</label>
                <%if (telegramSvr.xingshen.xingshenProxyMgr2.IsRunning()){%>
                    <div class="layui-form-mid" style="color:green">是</div>
                <%}else{ %>
                    <div class="layui-form-mid" style="color:red">否</div>
                <%} %>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">端口</label>
                <div class="layui-form-mid"><%=telegramSvr.xingshen.xingshenProxyMgr2.ProxyPort() %></div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">根证书</label>
                <div class="layui-form-mid"><a href="http://<%=Request.Url.Host %>:<%=telegramSvr.xingshen.xingshenProxyMgr2.ProxyPort() %>/FiddlerRoot.cer">点击下载</a></div>
            </div>
        </div>
    </form>
</body>
</html>
