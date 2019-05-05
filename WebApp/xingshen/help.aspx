<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="help.aspx.cs" Inherits="telegramSvr.xingshen.help" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>如何使用</title>
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
        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 30px;">
  <legend>Help</legend>
</fieldset>
<div class="layui-collapse" lay-filter="x">
  <div class="layui-colla-item">
    <h2 class="layui-colla-title">修改数据</h2>
    <div class="layui-colla-content">
     <div class="layui-card-body">
            <ul class="layui-timeline">
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <h3 class="layui-timeline-title">1.游戏中“注销”</h3>
                  <p></p>
                </div>
              </li>
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <h3 class="layui-timeline-title">2.刷新存档</h3>
                  <p><img src="help/1.1.png" /></p>
                </div>
              </li>
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <h3 class="layui-timeline-title">3.修改数据</h3>
                  <p><img src="help/1.3.png" /></p>
                  <blockquote class="layui-elem-quote"><span style="color:red">修改数据值过分可能导致被封.</span></blockquote>
                </div>
              </li>
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <h3 class="layui-timeline-title">4.修改物品</h3>
                  <p><img src="help/1.4.png" /></p>
                  <blockquote class="layui-elem-quote">“物品大类”和“物品小类”可以通过<a href="/xingshen/wp.aspx" target="_blank">物品查询</a>获取，修改之后即会变为另一种物品。
                      <br />
                      <span style="color:red">修改出“先天”、“神器”账号容易被封。</span>
                  </blockquote>
                </div>
              </li>
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <h3 class="layui-timeline-title">5.修改人物</h3>
                  <p><img src="help/1.5.png" /></p>
                  <blockquote class="layui-elem-quote">“人物类型id”可以通过<a href="/xingshen/wp.aspx" target="_blank">人物查询</a>获取，修改之后即会变为另一个人物。</blockquote>
                </div>
              </li>
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <h3 class="layui-timeline-title">6.上传存档</h3>
                  <p><img src="help/1.6.png" /></p>
                  <blockquote class="layui-elem-quote">提示“成功”说明万事大吉！</blockquote>
                </div>
              </li>
              <li class="layui-timeline-item">
                <i class="layui-icon layui-timeline-axis"></i>
                <div class="layui-timeline-content layui-text">
                  <div class="layui-timeline-title">7.手机登录游戏</div>
                    <p>然后……开始浪</p>
                </div>
              </li>
            </ul> 
          </div>
    </div>
  </div>
  <div class="layui-colla-item">
    <h2 class="layui-colla-title">如何去除1小时登录限制</h2>
    <div class="layui-colla-content">
       <ul class="layui-timeline">
            <li class="layui-timeline-item">
            <i class="layui-icon layui-timeline-axis"></i>
            <div class="layui-timeline-content layui-text">
                <h3 class="layui-timeline-title">1.手机必须已经连接上WIFI</h3>
                <p></p>
            </div>
            </li>
            <li class="layui-timeline-item">
            <i class="layui-icon layui-timeline-axis"></i>
            <div class="layui-timeline-content layui-text">
                <h3 class="layui-timeline-title">2.设置代理</h3>
                <p><img src="help/2.1.png" /></p>
                <p><img src="help/2.2.png" /></p>
                <p><img src="help/2.3.png" /></p>
                <br />
                <blockquote class="layui-elem-quote" style="background-color:#ffc7c7">警告：设置代理后你手机上的流量全都会先流向代理服务器。<br />虽然我们不会拦截或监听您游戏之外的数据。但是我们强烈建议你设置代理前，关闭所有游戏外的应用，使用完后及时取消代理设置。</blockquote>
            </div>
            </li>
        <li class="layui-timeline-item">
            <i class="layui-icon layui-timeline-axis"></i>
            <div class="layui-timeline-content layui-text">
                <div class="layui-timeline-title">3.手机登录游戏</div>
                <p>然后……开始浪</p>
            </div>
            </li>
        </ul> 
    </div>
  </div>
  <div class="layui-colla-item">
    <h2 class="layui-colla-title">游戏不“注销”的修改</h2>
    <div class="layui-colla-content">
     <ul class="layui-timeline">
        <li class="layui-timeline-item">
        <i class="layui-icon layui-timeline-axis"></i>
        <div class="layui-timeline-content layui-text">
            <h3 class="layui-timeline-title">1.手机已登录游戏</h3>
            <p></p>
        </div>
        </li>
        <li class="layui-timeline-item">
        <i class="layui-icon layui-timeline-axis"></i>
        <div class="layui-timeline-content layui-text">
            <h3 class="layui-timeline-title">2.设置代理</h3>
            <p><img src="help/2.1.png" /></p>
            <p><img src="help/2.2.png" /></p>
            <p><img src="help/2.3.png" /></p>
            <br />
            <blockquote class="layui-elem-quote" style="background-color:#ffc7c7">警告：设置代理后你手机上的流量全都会先流向代理服务器。<br />虽然我们不会拦截或监听您游戏之外的数据。但是我们强烈建议你设置代理前，关闭所有游戏外的应用，使用完后及时取消代理设置。</blockquote>
        </div>
        </li>
         <li class="layui-timeline-item">
        <i class="layui-icon layui-timeline-axis"></i>
        <div class="layui-timeline-content layui-text">
            <h3 class="layui-timeline-title">2.礼包兑换</h3>
            <p><img src="help/3.1.png" /></p>
            <ul>
                <li>jinbi:10000</li>
                <li>yuanbao:10000</li>
                <li>yueka:10</li>
                <li>wp:0,0,100</li>
            </ul>
            <br />
            <blockquote class="layui-elem-quote" style="color:red">以上兑换码只在已设置代理的状态下使用！</blockquote>
        </div>
        </li>
        <li class="layui-timeline-item">
        <i class="layui-icon layui-timeline-axis"></i>
        <div class="layui-timeline-content layui-text">
            <div class="layui-timeline-title">3.手机登录游戏</div>
            <p>然后……开始浪</p>
        </div>
        </li>
    </ul> 
    </div>
  </div>
  <div class="layui-colla-item">
    <h2 class="layui-colla-title">还有啥？</h2>
    <div class="layui-colla-content layui-show">
      <p>。。。</p>
    </div>
  </div>
</div>
    </form>
<script src="../js/layui/layui.min.js"></script>
<script>
layui.use(['element', 'layer'], function(){});
</script>
</body>
</html>
