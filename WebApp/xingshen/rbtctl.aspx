<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rbtctl.aspx.cs" Inherits="telegramSvr.xingshen.rbtctl" %>

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
                    <input type="button" class="layui-btn" id="btn_back" onclick="history.go(-1);" value="<<" />
                </div>
            </div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" onclick="location.href = 'accinfo.aspx?uid=<%=Request["uid"]%>'" value=">>" />
            </div>
        </div>

        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">宗门</div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" id="btn_qrysects" value="查询" />
                <input type="button" class="layui-btn" id="btn_sectjoin" value="加入" />
                <input type="button" class="layui-btn" id="btn_donate" value="贡献" />
                <input type="button" class="layui-btn" id="btn_attack_damage" value="打Boss" />
                <input type="button" class="layui-btn" id="btn_quit" value="退出" />
            </div>
        </div>

        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">商会</div>
            <div class="layui-table-tool-self">
                <input type="button" class="layui-btn" id="btn_qryling" value="查询" />
                <input type="button" class="layui-btn" id="btn_addling" value="新增" />
                <input type="button" class="layui-btn" id="btn_shLst" value="列表" />
                <input type="button" class="layui-btn" id="btn_buyone" style="display:none" value="BuyFirst" />
            </div>
        </div>

        <div class="layui-table-tool layui-border-box">
            <div class="layui-table-tool-temp">
                <div class="layui-btn-container">
                    
                </div>
            </div>
            <div class="layui-table-tool-self">
            </div>
        </div>
    </form>
    <div class="dialog">
        <div class="shoplistdlg" style="display: none">
            <table id="shoplist" lay-filter="shoplist"></table>
            <script type="text/html" id="toolsbar">
                <a class="layui-btn layui-btn-xs" lay-event="buy"> - 买 - </a>
            </script>
        </div>
    </div>
    <script src="../js/layui/layui.min.js"></script>
    <script>
        var player_data, player_data_bak;
        var selectedRow;
        var uid ="<%=Request["uid"]%>";

        layui.use(['layer', 'element', 'table'], function () {
            var layer = layui.layer, $ = layui.$, table = layui.table;
            $("#btn_shLst").click(function () {
                layer.load(2);
                $.ajax({
                    url: window.location.pathname + "?a=sl&uid=" + uid,
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            var initShopDlg = function () {
                                var tablecols = [[
                                    { field: 'id', width: 100, title: 'ID' }
                                    , { field: 'item_name', width: 200, title: 'item_name' }
                                    , { field: 'price', width: 100, title: 'price' }
                                    , { width: 70, toolbar: '#toolsbar', title: "<a class=\"layui-btn layui-btn-danger layui-btn-xs buyall\">全买</a>" }
                                ]];
                                table.render({
                                    elem: '#shoplist'
                                    , cellMinWidth: 40
                                    , height: "400px"
                                    , cols: tablecols
                                    , data: data.data
                                    , limit: 99999
                                });
                                $(".buyall").click(function () {
                                    layer.msg("算了吧这样做影响太大了");
                                    return false;
                                });
                            }
                            layer.open({
                                type: 1
                                , title: "商会列表"
                                , resize: false
                                , area: ['610px', '465px']
                                , content: $('.dialog .shoplistdlg')
                                , success: function (layero) {
                                    warndlgdiv = layero;
                                    initShopDlg();
                                }
                            });
                        }
                    },
                    error: function (err) {
                        layer.closeAll('loading');
                        layer.msg(err.responseText, { icon: 2 });
                    }
                }); 
            });
            table.on('tool(shoplist)', function (obj) {
                if (obj.event == "buy") {
                    console.log(obj.data.id);
                    layer.load(2);
                    $.ajax({
                        url: "<%=Request.Path%>?a=slb1&uid=<%=Request["uid"]%>&id=" + obj.data.id,
                        async: true,
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            layer.closeAll('loading');
                            if (data.ok) {
                                layer.msg("ok:" + data.name);
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
            });
            $("#btn_attack_damage").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=bossinfo&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("level:"+data.level);
                            layer.prompt({ title: '打多少血？',value:data.hp+Math.ceil(Math.random()*1000000) }, function (sid, index) {
                                layer.close(index);
                                layer.load(2);
                                $.ajax({
                                    url: "<%=Request.Path%>?a=ad&uid=<%=Request["uid"]%>&sl=" + sid,
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
            $("#btn_addling").click(function () {
                layer.prompt({ title: '新增商会令数量' }, function (sid, index) {
                    layer.close(index);
                    layer.load(2);
                    $.ajax({
                        url: "<%=Request.Path%>?a=shl&uid=<%=Request["uid"]%>&sl=" + sid,
                        async: true,
                        type: "POST",
                        dataType: "json",
                        success: function (data) {
                            layer.closeAll('loading');
                            if (data.ok) {
                                layer.msg("商会令：" + data.shl);
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
            $("#btn_qryling").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=qshl&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg("商会令：" + data.shl);
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
            $("#btn_buyone").click(function () {
                layer.load(2);
                $.ajax({
                    url: "<%=Request.Path%>?a=bo&uid=<%=Request["uid"]%>",
                    async: true,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        layer.closeAll('loading');
                        if (data.ok) {
                            layer.msg(data.name);
                            console.log(data.name);
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
                            layer.msg("ok:" + data.name);
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
                        url: "<%=Request.Path%>?a=sj&uid=<%=Request["uid"]%>&sid=" + sid,
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
            $("#btn_quit").click(function () {
                layer.confirm('退出宗门？', {
                    icon: 3
                    , title: "警告"
                    , btn: ['退出宗门', '取消']
                }, function () {
                    layer.closeAll("dialog");
                    layer.load(2);
                    $.ajax({
                        url: "<%=Request.Path%>?a=qu&uid=<%=Request["uid"]%>",
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
                }, function () {

                });
            });
        });
        var iiii = 0;
        function byone() {
            layui.$.ajax({
                url: "<%=Request.Path%>?a=bo&uid=<%=Request["uid"]%>",
                async: true,
                type: "POST",
                dataType: "json",
                success: function (data) {
                    if (data.ok) {
                        console.log((iiii++) + "》" + data.name);
                    } else {
                        console.log((iiii++) + "》" + data.msg);
                    }
                },
                error: function (err) {
                    console.log(err.responseText);
                }
            });
        }

        function l100() {
            for (var i = 0; i < 100; i++) {               
                byone()
            }
        }
    </script>

</body>
</html>
