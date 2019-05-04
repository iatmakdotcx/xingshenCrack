<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wp.aspx.cs" Inherits="telegramSvr.xingshen.wp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>物品查询</title>
    <meta name="renderer" content="webkit"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=0"/>    
    <link rel="stylesheet" href="../js/layui/css/layui.css" />
    <style>
        @media (min-width: 1000px) {
           body {
                padding: 10px;
                max-width: 80%;
                margin: 0 auto;
            }
        }
        .layui-table-body.layui-table-main{
            min-height:500px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="layui">
            <div class="layui-tab">
                <ul class="layui-tab-title">
                    <li class="layui-this">物品</li>
                    <li>人物</li>
                </ul>
                <div class="layui-tab-content">
                    <div class="layui-tab-item layui-show wp">
                        <div class="finditemdlg">
                            <div class="test-table-reload-btn" style="margin:0 10px;">
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
                    </div>
                    <div class="layui-tab-item rw">
                        <div class="findroledlg">
                            <div class="test-table-reload-btn" style="margin:0 10px;">
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
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="ALLFILE.aspx?v=<%=xingshenSvrHelper.svrHelper.Andorid_VERSION %>"></script>
    <script src="../js/layui/layui.min.js"></script>
    <script>
        layui.use(['layer', 'element', 'table'], function () {
            var layer = layui.layer, $ = layui.$, table = layui.table;

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
                    , { field: 'name', width: 150, title: '名称', sort: true }
                    , { field: 'miaoshu', title: '描述' }
                ]];
                table.render({
                    elem: '#item-table-all'
                    , cellMinWidth: 40
                    , cols: tablecols
                    , data: filterdata
                    , limit: 99999
                });
                table.render({
                    elem: '#item-table-l9'
                    , cellMinWidth: 40
                    , cols: tablecols
                    , data: filterdata_l9
                    , limit: 99999
                });
                table.render({
                    elem: '#item-table-e9'
                    , cellMinWidth: 40
                    , cols: tablecols
                    , data: filterdata_e9
                    , limit: 99999
                });
                table.render({
                    elem: '#item-table-b9'
                    , cellMinWidth: 40
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
                    , { field: 'miaoShu', width: 300, title: '描述' }
                ]];
                table.render({
                    elem: '#role-table-7'
                    , cellMinWidth: 40                    
                    , cols: tablecols
                    , data: filterdata_7
                    , limit: 99999
                });
                table.render({
                    elem: '#role-table-6'
                    , cellMinWidth: 40
                    , cols: tablecols
                    , data: filterdata_6
                    , limit: 99999
                });
                table.render({
                    elem: '#role-table-5'
                    , cellMinWidth: 40
                    , cols: tablecols
                    , data: filterdata_5
                    , limit: 99999
                });
                table.render({
                    elem: '#role-table-4-'
                    , cellMinWidth: 40
                    , cols: tablecols
                    , data: filterdata_4_
                    , limit: 99999
                });
            });
        });
    </script>
</body>
</html>
