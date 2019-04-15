<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uf.aspx.cs" Inherits="telegramSvr.xingshen.uf" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">
                <div class="layui-btn-container">
                    <input type="button" class="layui-btn" id="btn_back" onclick="location.href = 'lst.aspx'" value="<<" />
                </div>
            </div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" id="btn_uploadData" value="上传存档" />
                <input type="button" class="layui-btn" id="btn_renewData" title="重新下载服务器存档" value="刷新存档" />
                <input type="button" class="layui-btn" id="btn_downfirst" value="Down" />
                <input type="button" class="layui-btn" id="btn_ok" value="确定" />
            </div>
        </div>
        <div class="layui">
            <div class="layui-tab">
                <ul class="layui-tab-title">
                    <li class="layui-this">数据</li>
                    <li>数据JSON</li>
                    <li>物品</li>
                    <li>仓库</li>
                    <li>人物</li>
                    <li>SignData</li>
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
                                            <%--<p class="original">数量：<span class="oldsl">10</span> >> <input type="text" id="newsl" value="" /></p>
                                <p class="original">强化：<span class="oldqh">10</span> >> <input type="text" id="newqh" value="" /></p>--%>
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
                    <div class="layui-tab-item signdata">
                        <textarea class="layui-textarea data" style="height: 550px;"></textarea>
                        <input type="button" class="layui-btn" value="Sign" />
                        <textarea class="layui-textarea res" style="height: 50px;"></textarea>
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
        var player_data, player_data_bak;
        var selectedRow;
        //var wzz;
        layui.use(['layer', 'element', 'table'], function () {
            var layer = layui.layer, $ = layui.$, table = layui.table;

            <% if (!string.IsNullOrEmpty(errMsg)){ %> 
            layui.layer.alert("<%=errMsg%>", {},
                function () {
                    location.href = "lst.aspx"
                });
            return;
            <%}%>

            player_data = JSON.parse(playerdata.data.player_data);
            player_data_bak = JSON.parse(playerdata.data.player_data);

            layui.util.fixbar({
                bar1: '&#xe60A;'
                , bar2: '&#xe612;'
                , css: {bottom: "50%"}
                , click: function(type){
                    if (type == "bar1") {
                        layer.open({
                            content : $('.dialog .finditemdlg'),
                            id : 'adminPopupR',
                            title : "物品查询",
                            anim : 2,
                            isOutAnim: false,
                            closeBtn : false,
                            offset : 'r',
                            shadeClose : true,
                            area : '650px',
                            skin : 'layui-layer-adminRight',
                            type: 1,
                            end: function () {
                                layui.$(".finditemdlg tr").remove()
                            }
                        });
                    } else if (type == "bar2") {
                        layer.open({
                            content : $('.dialog .findroledlg'),
                            id : 'adminPopupR',
                            title : "人物查询",
                            anim : 2,
                            isOutAnim : false,
                            closeBtn : false,
                            offset : 'r',
                            shadeClose: true,
                            area : '80%',
                            skin : 'layui-layer-adminRight',
                            type: 1,
                            end: function () {
                                layui.$(".findroledlg tr").remove()
                            }
                        });
                    }
                }
            });

            table.on('tool(warninglist)', function (obj) {
                if (obj.event == "del") {
                    warninglistDelete(obj.data.id, obj);
                }
            });
            var warndlgdiv;
            function showWarningdlg(data) {
                var initwarnDlg = function () {
                    var tablecols = [[
                        { field: 'id', width: 40, title: 'ID' }
                        , { field: 'jgrq', width: 160, title: '日期' }
                        , { field: 'jgxx', width: 320, title: '内容' }
                        , { width: 66, toolbar: '#toolsbar', title: "<a class=\"layui-btn layui-btn-danger layui-btn-xs warnclear\">全删</a>" }
                    ]];
                    table.render({
                        elem: '#warninglist'
                        , cellMinWidth: 40
                        , height: "400px"
                        , cols: tablecols
                        , data: data
                        , limit: 99999
                    });
                    $(".warnclear").click(function () {
                        warninglistDelete(-1);
                        return false;
                    });
                }
                if (data.length > 0) {
                    if (warndlgdiv && warndlgdiv.is(":visible")) {
                        initwarnDlg();
                    } else {
                        layer.open({
                            type: 1
                            , title: "警告信息"
                            , resize: false
                            , area: ['610px', '465px']
                            , content: $('.dialog .warninglistdlg')
                            , success: function (layero) {
                                warndlgdiv = layero;
                                initwarnDlg();
                            }
                        });
                    } 
                }
            }
            showWarningdlg(warningdata);
            //wzz = showWarningdlg;
            setInterval(function () {
                $.ajax({
                    url: "<%=Request.Path%>?a=refreshwarn&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (data.ok) {
                            showWarningdlg(data.data);
                        }
                    },
                    error: function (err) {
                    }
                });
            }, 5000);
            function warninglistDelete(id, obj) {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=delwarning&uid=<%=Request["uid"]%>&wid=" + id,
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            if (obj && obj.tr.siblings().length > 0) {
                                obj.del();
                            }else
                                layer.closeAll("page")
                        } else {
                            layer.msg(data.msg);
                        }
                    },
                    error: function (err) {
                        layer.closeAll('loading');
                        layer.msg(err.responseText, { icon: 2 });
                    }
                });
            }
            function buildTabledata(d) {
                var html = "";
                for (var i in d) {
                    html += "<tr class=\"data\" data-id=\"" + i + "\"><td>" + d[i].itemID + "</td>";
                    if (ALLFILE_Item["ITEMFILE" + d[i].itemType] && ALLFILE_Item["ITEMFILE" + d[i].itemType][d[i].childType]) {
                        html += "<td>" + ALLFILE_Item["ITEMFILE" + d[i].itemType][d[i].childType].name + "</td>";
                    } else {
                         html += "<td style=\"color:red\">未知物品</td>";
                    }
                    html += "<td>" + itemlx[d[i].itemType] + "</td>";
                    var bz = "";
                    if (typeof d[i].num != "undefined") {
                        bz += "，数量：" + d[i].num + ""
                    } if (typeof d[i].qianghuaLv != "undefined") {
                        bz += "，强化：" + d[i].qianghuaLv
                    }
                    html += "<td>" + bz.substr(1) + "</td>";
                    html += "</tr>";
                }
                return html
            }
            $(".package tbody").append(buildTabledata(player_data.playerDict.packageArr));
            $(".cangku tbody").append(buildTabledata(player_data.playerDict.cangkuArr));
            (function initfujian() {
                var datasss = "";
                for (var i in ALLFILE_Item["KEYSTRNAME"]) {
                    datasss += i + ":" + ALLFILE_Item["KEYSTRNAME"][i].name + ";";
                }
                $("table.package .meta .layui-select").html(datasss);
                $("table.cangku .meta .layui-select").html(datasss);
            })();
            (function buildRolesdata() {
                var html = "";
                for (var i in player_data.playerDict.battleRolesArr) {
                    html += "<tr class=\"data\" data-t=\"1\" data-id=\"" + i + "\"><td>√</td>";
                    html += "<td>" + ALLFILE_Item["ROLEFILE"][player_data.playerDict.battleRolesArr[i].ID].roleName + "</td>";
                    html += "<td>" + "</td>";
                    html += "<td>" + "</td>";
                    html += "</tr>";
                }
                for (var i in player_data.playerDict.rolesArr) {
                    html += "<tr class=\"data\" data-t=\"2\" data-id=\"" + i + "\"><td></td>";
                    html += "<td>" + ALLFILE_Item["ROLEFILE"][player_data.playerDict.rolesArr[i].ID].roleName + "</td>";
                    html += "<td>" + "</td>";
                    html += "<td>" + "</td>";
                    html += "</tr>";
                }
                $(".roles tbody").append(html);
            })();
            (function initBaseData() {
                $(".layui-tab-item.base input").each(function () {
                    if (this.name == "firstPlayTime") {
                        var ts = (Math.round(new Date().getTime() / 1000) - player_data.playerDict.firstPlayTime) / (60*60*24);
                        $(this).val(Math.round(ts));
                    }else
                        $(this).val(player_data.playerDict[this.name]);
                });
                $(".layui-tab-item.base div[data-key]").each(function () {
                    $(this).html(player_data.playerDict[$(this).data("key")]);
                });
                var juntjy = player_data.playerDict.juntuanExp || 1;
                for (var i in ALLFILE_Item.EXPLEVEL) {
                    if (parseInt(ALLFILE_Item.EXPLEVEL[i]) <= juntjy) {
                        $(".layui-tab-item.base .juntdj").text(parseInt(i) + 1);
                    }
                }
                var tmpjs = {};
                for (var i in player_data.playerDict) {
                    if (typeof player_data.playerDict[i] != "object") {
                        //var s = $(".layui-tab-item.base input[name=" + i + "]");
                        //if (s.length > 0) {
                        //    var bz = s.parent().siblings(".layui-form-label").text();
                        //    if (bz != "") {
                        //        tmpjs[i + "$" + bz] = player_data.playerDict[i];
                        //        continue;
                        //    }
                        //}
                        tmpjs[i] = player_data.playerDict[i];
                    }
                }
                $(".layui-tab-item.basejson .basefulldata").val(JSON.stringify(tmpjs, null, "\t"));
            })();
            $("table.package tr.data").click(function () {
                selectedRow = $(this);
                var editor = $("table.package tr.editor");
                if (editor.is(":visible") && editor.is(selectedRow.next())) {
                    editor.hide();
                } else {
                    var data = player_data.playerDict.packageArr[selectedRow.data("id")];
                    //if (data.num != undefined) {
                    //    editor.find(".oldsl").text(data.num);
                    //    editor.find("#newsl").val(data.num).show();
                    //} else {
                    //    editor.find(".oldsl").text("");
                    //    editor.find("#newsl").hide();
                    //}
                    //if (data.qianghuaLv != undefined) {
                    //    editor.find(".oldqh").text(data.qianghuaLv);
                    //    editor.find("#newqh").val(data.qianghuaLv).show();
                    //} else {
                    //    editor.find(".oldqh").text("");
                    //    editor.find("#newqh").hide();
                    //}
                    editor.find(".fulldata").val(JSON.stringify(data, null, "\t"));
                    editor.insertAfter(selectedRow).show();
                }
            });
            $("table.cangku tr.data").click(function () {
                selectedRow = $(this);
                var editor = $("table.cangku tr.editor");
                if (editor.is(":visible") && editor.is(selectedRow.next())) {
                    editor.hide();
                } else {
                    var data = player_data.playerDict.cangkuArr[selectedRow.data("id")];
                    editor.find(".fulldata").val(JSON.stringify(data, null, "\t"));
                    editor.insertAfter(selectedRow).show();
                }
            });
            $("table.roles tr.data").click(function () {
                selectedRow = $(this);
                var editor = $("table.roles tr.editor");
                if (editor.is(":visible") && editor.is(selectedRow.next())) {
                    editor.hide();
                } else {
                    var data;
                    if (selectedRow.data("t") == "1") {
                        data = player_data.playerDict.battleRolesArr[selectedRow.data("id")];
                    } else {
                        data = player_data.playerDict.rolesArr[selectedRow.data("id")];
                    }
                    editor.find(".fulldata").val(JSON.stringify(data, null, "\t"));
                    editor.insertAfter(selectedRow).show();
                }
            });
            $("table.translations tr.editor .layui-btn").click(function () {
                var data = {
                    orig: selectedRow.data("orig"),
                    score: sx.config.value,
                    trans: $("tr.editor .foreign-text").val()
                }
                console.log(JSON.stringify(data));
                layer.load(2);
                layui.$.ajax({
                    type: "POST",
                    url: location.href,
                    data: JSON.stringify(data),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("已保存", { time: 1000 });
                        } else {
                            layer.msg(data.msg);
                        }
                    },
                    error: function (err) {
                        layer.closeAll('loading');
                        layer.msg(err.responseText, { icon: 2 });
                    }
                });
                $("table.translations tr.editor").hide();
            });
            $(".layui-tab-item.base input").change(function () {
                var v = parseInt($(this).val());
                if (!isNaN(v)) {
                    player_data.playerDict[this.name] = v.toString();
                }
                $(this).val(player_data.playerDict[this.name]);
                if (this.name == "juntuanExp") {
                    var juntjy = player_data.playerDict.juntuanExp || 1;
                    for (var i in ALLFILE_Item.EXPLEVEL) {
                        if (parseInt(ALLFILE_Item.EXPLEVEL[i]) <= juntjy) {
                            $(".layui-tab-item.base .juntdj").text(parseInt(i) + 1);
                        }
                    }
                } else if (this.name == "firstPlayTime") {
                    player_data.playerDict.firstPlayTime = (Math.round(new Date().getTime() / 1000) - v * (60 * 60 * 24)).toString();
                }
            });

            $("#btn_ok").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=save&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    data: JSON.stringify(player_data),
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("保存成功");
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
            $("#btn_downfirst").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=downfirst&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    data: JSON.stringify(player_data),
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("ok");
                            cfgdownload("a.txt", data.data);
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
            $("#btn_renewData").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=rdt&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
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
                });
            });
            //物品编辑
            $(".package .editor .layui-btn").click(function () {
                changeitemData(player_data.playerDict.packageArr, $(".package .editor .fulldata").val(), this);
            });
            $(".cangku .editor .layui-btn").click(function () {
                changeitemData(player_data.playerDict.cangkuArr, $(".cangku .editor .fulldata").val(), this);
            });
            function changeitemData(d, v, t) {
                var vv;
                try {
                    vv = JSON.parse(v);
                } catch (e) {
                    layer.msg("数据保存失败！请再次检查内容！");
                    return;
                }
                d[selectedRow.data("id")] = vv;
                $(t).closest(".editor").hide();
                layer.msg("ok", { time: 1000 });
            }
            //人物编辑
            $(".roles .editor .layui-btn").click(function () {
                var data;
                if (selectedRow.data("t") == "1") {
                    data = player_data.playerDict.battleRolesArr;
                } else {
                    data = player_data.playerDict.rolesArr;
                }
                changeitemData(data, $(".roles .editor .fulldata").val(), this);
            }); 
            $("#btn_itemsearch").click(function () {
                var keyw = $("#itemsearchkey").val();
                var filterdata = [];
                var filterdata_l9 = [];
                var filterdata_e9 = [];
                var filterdata_b9 = [];
                for (var i in ALLFILE_Item) {
                    if (i.startsWith("ITEMFILE")) {
                        var typeid = parseInt(i.substr(8));
                        for (var j in ALLFILE_Item[i]) {
                            if (keyw == "" || ALLFILE_Item[i][j].ID == keyw || ALLFILE_Item[i][j].name.indexOf(keyw) > -1 || (ALLFILE_Item[i][j].miaoshu || "").indexOf(keyw) > -1) {
                                ALLFILE_Item[i][j].itemtype = typeid;
                                filterdata.push(ALLFILE_Item[i][j]);
                                if (typeid < 9) {
                                    filterdata_l9.push(ALLFILE_Item[i][j]);
                                } else if (typeid == 9) {
                                    filterdata_e9.push(ALLFILE_Item[i][j]);
                                } else {
                                    filterdata_b9.push(ALLFILE_Item[i][j]);
                                }
                            }
                        }
                    }
                }
                var tablecols = [[
                    { field: 'itemtype', width: 66, title: 'type', sort: true }
                    , { field: 'ID', width: 40, title: 'ID' }
                    , { field: 'name', width: 100, title: '名称', sort: true }
                    , { field: 'miaoshu', width: 410, title: '描述' }
                ]];
                table.render({
                    elem: '#item-table-all'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata
                    , limit: 99999
                });
                table.render({
                    elem: '#item-table-l9'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_l9
                    , limit: 99999
                });
                table.render({
                    elem: '#item-table-e9'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_e9
                    , limit: 99999
                });
                table.render({
                    elem: '#item-table-b9'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_b9
                    , limit: 99999
                });
            });
            $("#btn_rolesearch").click(function () {
                var keyw = $("#rolesearchkey").val();
                var filterdata_7 = [];
                var filterdata_6 = [];
                var filterdata_5 = [];
                var filterdata_4_ = [];

                for (var j in ALLFILE_Item["ROLEFILE"]) {
                    var aRole = ALLFILE_Item["ROLEFILE"][j]
                    if (keyw == "" || aRole.ID == keyw || aRole.roleName.indexOf(keyw) > -1 || (aRole.miaoShu || "").indexOf(keyw) > -1) {                        
                        var star = parseInt(aRole.star);
                        if (star == 7) {
                            filterdata_7.push(aRole);
                        } else if (star == 6) {
                            filterdata_6.push(aRole);
                        } else if (star == 5) {
                            filterdata_5.push(aRole);
                        } else {
                            filterdata_4_.push(aRole);
                        }
                    }
                }
          
                var tablecols = [[
                    { field: 'ID', width: 70, title: 'ID', sort: true }
                    , { field: 'roleName', width: 100, title: '名称' }
                    , { field: 'HP', width: 100, title: 'HP', sort: true }
                    , { field: 'HPAdd', width: 100, title: 'HP成长', sort: true }
                    , { field: 'MP', width: 100, title: 'MP', sort: true }
                    , { field: 'MPAdd', width: 100, title: 'MP成长', sort: true }
                    , { field: 'atk', width: 100, title: '武力', sort: true }
                    , { field: 'atkAdd', width: 100, title: '武力成长', sort: true }
                    , { field: 'ts', width: 100, title: '统帅', sort: true }
                    , { field: 'tsAdd', width: 100, title: '统帅成长', sort: true }
                    , { field: 'yl', width: 100, title: '妖力', sort: true }
                    , { field: 'ylAdd', width: 100, title: '妖力成长', sort: true }
                    , { field: 'def', width: 100, title: '防御', sort: true }
                    , { field: 'defAdd', width: 100, title: '防御成长', sort: true }
                    , { field: 'SPEED', width: 100, title: '速度', sort: true }
                    , { field: 'speedAdd', width: 100, title: '速度成长', sort: true }
                    , { field: 'miaoShu', width: 100, title: '描述' }
                ]];
                table.render({
                    elem: '#role-table-7'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_7
                    , limit: 99999
                });
                table.render({
                    elem: '#role-table-6'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_6
                    , limit: 99999
                });
                table.render({
                    elem: '#role-table-5'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_5
                    , limit: 99999
                });
                table.render({
                    elem: '#role-table-4-'
                    , cellMinWidth: 40
                    , height: (layui.$(".layui-layer-adminRight").height() - 170)
                    , cols: tablecols
                    , data: filterdata_4_
                    , limit: 99999
                });
            });

            $("#btn_uploadData").click(function () {
                layer.confirm('上传存档前请注销游戏！！！！', {
                    icon: 3
                    , title:"警告"
                    , btn: ['上传', '取消']
                }, function () {
                    layer.closeAll("dialog");
                    layer.load(2);
                    $.ajax({
                        url: "<%=Request.Path%>?a=upload&uid=<%=Request["uid"]%>",
                        async: true,
                        type: "POST",
                        data: JSON.stringify(player_data),
                        dataType: "json",
                        success: function (data) {
                            layer.closeAll('loading');
                            if (data.ok) {
                                layer.msg("成功");
                            } else {
                                layer.msg(data.msg);
                            }
                        },
                        error: function (err) {
                            layer.closeAll('loading');
                            layer.msg(err.responseText, { icon: 2 });
                        }
                    });                   
                }, function () {
                    
                });
            });
            $(".signdata .layui-btn").click(function () {
                $.ajax({
                    url: "<%=Request.Path%>?a=sign&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    data: $(".signdata textarea.data").val(),
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            var rr = "Server-Time: " + data.ServerTime + "\r\n";
                            rr += "Sign: " + data.Sign;
                            $(".signdata textarea.res").val(rr);
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
        function modifyLog() {
            function lop(a, b, c) {
                for (var i in a) {
                    if (typeof a[i] == "object" && a[i] != null) {
                        if (a[i].push != undefined) {
                            var r1 = [];
                            lop(a[i], b[i], r1);
                            if (r1.length > 0) {
                                c[i] = r1;
                            }
                        } else {
                            var r2 = {};
                            lop(a[i], b[i], r2);
                            for (var r2i in r2) {
                                c[i] = r2;
                                break;
                            }
                        }
                    } else {
                        if (a[i] != b[i]) {
                            c[i] = a[i];
                        }
                    }
                }
                return c;
            }
            var result = {};
            return lop(player_data, player_data_bak, result)
        }
        function fake_click(obj) {
            var ev = document.createEvent("MouseEvents");
            ev.initMouseEvent(
                "click", true, false, window, 0, 0, 0, 0, 0
                , false, false, false, false, 0, null
            );
            obj.dispatchEvent(ev);
        }
        function cfgdownload(name, data) {
            var urlObject = window.URL || window.webkitURL || window;

            var downloadData = new Blob([data]);

            var save_link = document.createElementNS("http://www.w3.org/1999/xhtml", "a")
            save_link.href = urlObject.createObjectURL(downloadData);
            save_link.download = name;
            fake_click(save_link);
        }

    </script>
</body>
</html>
