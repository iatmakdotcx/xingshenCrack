<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="telegramSvr.xingshen.login" %>
<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <title>登入</title>
  <meta name="renderer" content="webkit">
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0">
  <link rel="stylesheet" href="js/layui/css/layui.css" />  
  <link rel="stylesheet" href="assets/login.css" media="all">
</head>
<body>

  <div class="layadmin-user-login layadmin-user-display-show" id="LAY-user-login" style="display: none;">
    <div class="layadmin-user-login-main">
      <div class="layadmin-user-login-box layadmin-user-login-header">
        <h2>Hi</h2>
        <p></p>
      </div>
      <div class="layadmin-user-login-box layadmin-user-login-body layui-form">
        <div class="layui-form-item">
          <label class="layadmin-user-login-icon layui-icon layui-icon-username" for="LAY-user-login-username"></label>
          <input type="text" name="user_name" id="LAY-user-login-username" lay-verType="tips" lay-verify="required" placeholder="用户名" class="layui-input">
        </div>
        <div class="layui-form-item">
          <label class="layadmin-user-login-icon layui-icon layui-icon-password" for="LAY-user-login-password"></label>
          <input type="password" name="password" id="LAY-user-login-password" lay-verType="tips" lay-verify="required" placeholder="密码" class="layui-input">
        </div>
        <hr />
        <div class="layui-form-item">
          <button class="layui-btn layui-btn-fluid" lay-submit lay-filter="LAY-user-login-submit">登 入</button>
        </div>
        <div class="layui-trans layui-form-item layadmin-user-login-other">
          <label></label>
          <a href="javascript:;"><i class="layui-icon layui-icon-login-qq"></i></a>
          <a href="javascript:;"><i class="layui-icon layui-icon-login-wechat"></i></a>
          <a href="javascript:;"><i class="layui-icon layui-icon-login-weibo"></i></a>
          
          <a href="reg.html" style="display:none" class="layadmin-user-jump-change layadmin-link">注册帐号</a>
        </div>
      </div>
    </div>       
  </div>

  <script src="js/layui/layui.js"></script>
  <script>
  layui.use(['form'], function(){
    var $ = layui.$
          , form = layui.form

    form.render().on('submit(LAY-user-login-submit)', function(obj){
        layer.load(2);
        $.ajax({
            type: "POST",
            url: "<%=Request.Path%>?a=login",
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
    
    layer.msg('Welcome', {
      offset: '15px'
      ,icon: 1
    });
    
  });
  </script>
</body>
</html>