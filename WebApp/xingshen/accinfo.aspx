<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="accinfo.aspx.cs" Inherits="telegramSvr.xingshen.accinfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta name="renderer" content="webkit"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0"/>
    <link rel="stylesheet" href="../js/layui/css/layui.css" />

    <style>
        .layui-fixbar{
               display:none;
           }
        @media (min-width: 1000px) {
           body {
                padding: 20px;
                max-width: 1000px;
                margin: 0 auto;
            }
           .layui-fixbar{
               display:block;
           }
        }
        @media (max-width: 400px) {
           .layui-table-tool-self{
               right: 0;
           }
           .layui-table-tool{
               padding:10px 0;
           }
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
        .editor  .meta .layui-select{
            height: auto;
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
        .layui-layer.layui-layer-adminRight {
            top: 50px !important;
            bottom: 0;
            box-shadow: 1px 1px 10px rgba(0, 0, 0, .1);
            border-radius: 0;
            overflow: auto;
        }
        .layui-nav-child{
            display:block;
            width:100px;
            min-width:100px;
        }
        .layui-tab-item.other{
            padding:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <blockquote class="layui-elem-quote" id="countdownlabel"></blockquote>
        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">
                 <%if(_optuser.isAdmin){ %>
                  <div class="layui-btn-container">
                    <input type="button" class="layui-btn" id="btn_back" onclick="history.go(-1);" value="<<" />
                    <input type="button" class="layui-btn" id="btn_AddEtime" value="ExpiryDate" />
                </div>
                 <%}%>
            </div>
            <div class="layui-table-tool-self layui-btn-group">
                <input type="button" class="layui-btn" id="btn_uploadData" value="上传存档" />
                <input type="button" class="layui-btn" id="btn_renewData" title="重新下载服务器存档" value="刷新存档" />
                <input type="button" class="layui-btn" id="btn_ok" value="暂存" />
            </div>
        </div>
        <%if(user!=null && user.isBanned ){ %>
        <blockquote class="layui-elem-quote" style="background-color: #f9c2c2;"><%=user.BanMsg %></blockquote>
        <%}else if(user!=null && user.isHold){ %>
        <blockquote class="layui-elem-quote" style="background-color: #f7d763;">此存档已标记！下次登录将使用修改后的存档！</blockquote>
        <%} %>
        <div class="layui">
            <div class="layui-tab">
                <ul class="layui-tab-title">
                    <li class="layui-this">数据</li>
                    <li>数据JSON</li>
                    <li>物品</li>
                    <li>仓库</li>
                    <li>人物</li>
                    <li>SignData</li>
                    <li>其他</li>
                </ul>
                <div class="layui-tab-content">
                    <div class="layui-tab-item layui-show base">
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">uid</label>
                                <div class="layui-form-mid layui-word-aux" data-key="uuid">11</div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">token</label>
                                <div class="layui-form-mid layui-word-aux" data-key="token">22</div>                                
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">username</label>
                                <div class="layui-form-mid layui-word-aux" data-key="user_name">33</div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">元宝</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="ybao" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">充值元宝</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="czJiFen" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">元宝消耗</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="ybaoXH" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">金币</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="coin" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">积分</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="hyJiFen" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline" style="display: none">
                                <label class="layui-form-label">积分消耗</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="hyjfxh" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <label class="layui-form-label">军团经验</label>
                            <div class="layui-input-inline">
                                <input type="text" name="juntuanExp" lay-verify="required" autocomplete="off" class="layui-input">
                            </div>
                            <div class="layui-form-mid layui-word-aux">等级：<span class="juntdj">1000</span></div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">书页</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="shuye" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">书页消耗</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="syXH" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">神魂</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="shNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">神魂消耗</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="shUseNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">试炼币</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="sldNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">试炼币消耗</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="sldXH" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">试炼层数</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="scslLv" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">兽灵</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="mjslNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">兽灵消耗</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="slXH" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">招募令</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="zhaomuling" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">深渊等级</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="syTGLV" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">游戏天数</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="firstPlayTime" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">神墓等级</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="smTGLV" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                       <div class="layui-form-item">
                            <div class="layui-inline">
                                <label class="layui-form-label">下品灵石消耗数量</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="xplsUseNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">神精消耗数量</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="sjUseNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                            <div class="layui-inline">
                                <label class="layui-form-label">下品仙晶消耗数量</label>
                                <div class="layui-input-inline">
                                    <input type="text" name="xpxjUseNum" lay-verify="required" autocomplete="off" class="layui-input">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="layui-tab-item basejson">
                        <textarea class="layui-textarea basefulldata" style="height: 550px;"></textarea>
                        <div class="actions">
                            <input type="button" class="layui-btn" style="display: none" value="确定" /></div>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-table package" lay-skin="line">
                            <thead>
                                <tr>
                                    <th>id</th>
                                    <th>名称</th>
                                    <th>类型</th>
                                    <th>备注</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="editor layui-bg-gray" style="display: none">
                                    <td colspan="4">
                                        <div class="strings">
                                            <div class="textareas">
                                                <textarea class="layui-textarea fulldata"></textarea>
                                            </div>
                                            <div class="actions">
                                                <input type="button" class="layui-btn" value="确定" />
                                            </div>
                                        </div>
                                        <div class="meta">
                                            <dl>
                                                <dt>:</dt>
                                                <dd></dd>
                                            </dl>
                                            <dl>
                                                <dt>附加属性:</dt>
                                                <dd class="layui-select"></dd>
                                            </dl>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-table cangku" lay-skin="line">
                            <thead>
                                <tr>
                                    <th>id</th>
                                    <th>名称</th>
                                    <th>类型</th>
                                    <th>备注</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="editor layui-bg-gray" style="display: none">
                                    <td colspan="4">
                                        <div class="strings">
                                            <div class="textareas">
                                                <textarea class="layui-textarea fulldata"></textarea>
                                            </div>
                                            <div class="actions">
                                                <input type="button" class="layui-btn" value="确定" />
                                            </div>
                                        </div>
                                        <div class="meta">
                                            <dl>
                                                <dt>:</dt>
                                                <dd></dd>
                                            </dl>
                                            <dl>
                                                <dt>附加属性:</dt>
                                                <dd class="layui-select"></dd>
                                            </dl>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-table roles" lay-skin="line">
                            <thead>
                                <tr>
                                    <th>参战</th>
                                    <th>名称</th>
                                    <th>类型</th>
                                    <th>备注</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="editor layui-bg-gray" style="display: none">
                                    <td colspan="4">
                                        <div class="strings">
                                            <div class="textareas">
                                                <textarea class="layui-textarea fulldata"></textarea>
                                            </div>
                                            <div class="actions">
                                                <input type="button" class="layui-btn" value="确定" /></div>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="layui-tab-item signdata">
                        <textarea class="layui-textarea data" style="height: 550px;"></textarea>
                        <input type="button" class="layui-btn" value="Sign" />
                        <textarea class="layui-textarea res" style="height: 50px;"></textarea>
                    </div>
                    <div class="layui-tab-item other">
                        <input type="button" class="layui-btn layui-btn-danger" id="btn_delete" value="删除" />
                        <input type="button" class="layui-btn" onclick="cfgdownload('a.txt', JSON.stringify(player_data.playerDict));" value="下载存档到本地" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <br />
                        <%if(_optuser.isAdmin){ %>
                        <input type="button" class="layui-btn layui-btn-danger" id="btn_clear" value="清空" />
                        <input type="button" class="layui-btn" id="btn_login" value="登录刷新token" />
                        <%}%>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <div class="dialog">
        <div class="finditemdlg" style="display:none">
            <div class="test-table-reload-btn" style="margin: 10px;">
                搜索：
              <div class="layui-inline">
                  <input class="layui-input" id="itemsearchkey" />
              </div>
                <input type="button" class="layui-btn" id="btn_itemsearch" value="搜索" />
            </div>
            <div class="layui-tab">
                <ul class="layui-tab-title">
                    <li class="layui-this">全部</li>
                    <li>装备</li>
                    <li>物品</li>
                    <li>技能书</li>
                </ul>
                <div class="layui-tab-content">
                    <div class="layui-tab-item layui-show">
                        <table class="layui-hide" id="item-table-all"></table>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-hide" id="item-table-l9"></table>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-hide" id="item-table-b9"></table>
                    </div>
                    <div class="layui-tab-item">
                       <table class="layui-hide" id="item-table-e9"></table>
                    </div>
                </div>
            </div>
        </div>
        <div class="findroledlg" style="display:none">
            <div class="test-table-reload-btn" style="margin: 10px;">
                搜索：
              <div class="layui-inline">
                  <input type="text" class="layui-input" id="rolesearchkey" />
              </div>
                <input type="button" class="layui-btn" id="btn_rolesearch" value="搜索" />
            </div>
            <div class="layui-tab">
                <ul class="layui-tab-title">
                    <li class="layui-this">7</li>
                    <li>6</li>
                    <li>5</li>
                    <li>4-</li>
                </ul>
                <div class="layui-tab-content">
                    <div class="layui-tab-item layui-show">
                        <table class="layui-hide" id="role-table-7"></table>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-hide" id="role-table-6"></table>
                    </div>
                    <div class="layui-tab-item">
                        <table class="layui-hide" id="role-table-5"></table>
                    </div>
                    <div class="layui-tab-item">
                       <table class="layui-hide" id="role-table-4-"></table>
                    </div>
                </div>
            </div>
        </div>
        <div class="warninglistdlg" style="display:none">
            <table id="warninglist" lay-filter="warninglist"></table>            
            <script type="text/html" id="toolsbar">              
              <a class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>              
            </script>
        </div>
        <%if (_optuser.isAdmin){%>
        <div class="newExpiryDate" style="display: none">
            <ul class="layui-form" style="margin: 10px;">
                <li class="layui-form-item">
                    <label class="layui-form-label">时长</label>
                    <div class="layui-input-block">
                        <input type="text" name="timeval" value="1" autocomplete="off" class="layui-input" />
                    </div>
                </li>
                <li class="layui-form-item">
                    <label class="layui-form-label">单位</label>
                    <div class="layui-input-block">
                        <select name="units">
                            <option value="0">分钟</option>
                            <option value="1" selected="selected">小时</option>
                            <option value="2">天</option>
                        </select>
                    </div>
                </li>
                <li class="layui-form-item" style="text-align: center;">
                    <button type="submit" lay-submit lay-filter="ExpiryDate" class="layui-btn">提交</button>
                </li>
            </ul>
        </div><%}%>
    </div>
    <script src="ALLFILE.aspx?v=<%=xingshenSvrHelper.svrHelper.Andorid_VERSION %>"></script>
    <script>        
        var playerdata = <%=playerdata.ToString(Newtonsoft.Json.Formatting.None)%>;
        var itemlx = {
            "0": "武器",
            "1": "衣服",
            "2": "戒指",
            "3": "项链",
            "4": "手镯",
            "5": "头盔",
            "6": "鞋子",
            "7": "腰带",
            "8": "道具",
            "9": "技能书",
            "10": "坐骑",
            "11": "盾牌",
            "12": "宝物",
            "13": "图纸",
            "14": "宝石",
            "15": "阵",
            "16": "功法",
        }
        var warningdata = <%=warningdata.ToString(Newtonsoft.Json.Formatting.None)%>;
    </script>

    <script src="../js/layui/layui.min.js"></script>
    <script>      
        var uid = "<%=Request["uid"]%>";
        var thisTimer = null;
        layui.use(["util", "element", "form"], function () {
            var $ = layui.$;
            var endTime = new Date("<%=user.ExpiryDate.ToString("yyyy-MM-dd HH:mm:ss")%>"), serverTime = new Date("<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>");
            clearTimeout(thisTimer);
            layui.util.countdown(endTime, serverTime, function (date, serverTime, timer) {
                if (date[0] || date[1] || date[2] || date[3]) {
                    var str = date[1] + ':' + date[2] + ':' + date[3];
                    if (date[0] > 0) {
                        str = date[0] + '天' + str;
                    }
                    layui.$('#countdownlabel').html("修改时限：" + str);
                } else {
                    layui.$('#countdownlabel').html("修改时限已过:<%=user.ExpiryDate.ToString("yyyy-MM-dd HH:mm:ss")%>").css("background-color", "#f9c2c2");
                }
                thisTimer = timer;
            });
            <%if (_optuser.isAdmin){%>
            //clearTimeout(thisTimer);
            $("#btn_AddEtime").click(function () {
                layer.open({
                    type: 1
                    , title: "ExpiryDate"
                    , resize: false
                    , content: $('.dialog .newExpiryDate')
                    , success: function (layero) {
                        layero.find('.layui-layer-content').css('overflow', 'visible');
                    }
                });
            });
            layui.form.render().on('submit(ExpiryDate)', function (data) {
                layer.load(2);
                var url = "<%=Request.Path%>?a=aed&uid=" + uid;
                $.ajax({
                    type: "POST",
                    url: url,
                    data: JSON.stringify(data.field),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.closeAll();
                            layer.msg("ok", { time: 1000 });
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
            $("#btn_login").click(function () {
                $.ajax({
                    type: "POST",
                    url: "<%=Request.Path%>?a=login&uid=" + uid,
                    data: "",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.closeAll();
                            layer.msg("ok", { time: 1000 });
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
            <%}%>
        });
        
    </script>
    <script src="assets/accinfo.min.js?v=0.1"></script>
</body>
</html>
