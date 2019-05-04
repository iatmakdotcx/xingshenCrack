var player_data, player_data_bak;
var selectedRow;

layui.use(['layer', 'element', 'table'], function () {
    var layer = layui.layer, $ = layui.$, table = layui.table;
        player_data = JSON.parse(playerdata.data.player_data);
    player_data_bak = JSON.parse(playerdata.data.player_data);

    layui.util.fixbar({
        bar1: '&#xe60A;'
        , bar2: '&#xe612;'
        , css: { bottom: "50%" }
        , click: function (type) {
            if (type == "bar1") {
                layer.open({
                    content: $('.dialog .finditemdlg'),
                    id: 'adminPopupR',
                    title: "物品查询",
                    anim: 2,
                    isOutAnim: false,
                    closeBtn: false,
                    offset: 'r',
                    shadeClose: true,
                    area: '650px',
                    skin: 'layui-layer-adminRight',
                    type: 1,
                    end: function () {
                        layui.$(".finditemdlg tr").remove()
                    }
                });
            } else if (type == "bar2") {
                layer.open({
                    content: $('.dialog .findroledlg'),
                    id: 'adminPopupR',
                    title: "人物查询",
                    anim: 2,
                    isOutAnim: false,
                    closeBtn: false,
                    offset: 'r',
                    shadeClose: true,
                    area: '80%',
                    skin: 'layui-layer-adminRight',
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
    setInterval(function () {
        $.ajax({
            url: window.location.pathname + "?a=refreshwarn&uid=" + uid,
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
            url: window.location.pathname + "?a=delwarning&uid=" + uid+"&wid=" + id,
            async: true,
            type: "POST",
            dataType: "json",
            success: function (data) {
                layer.closeAll('loading');
                if (data.ok) {
                    if (obj && obj.tr.siblings().length > 0) {
                        obj.del();
                    } else
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
                var ts = (Math.round(new Date().getTime() / 1000) - player_data.playerDict.firstPlayTime) / (60 * 60 * 24);
                $(this).val(Math.round(ts));
            } else
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
            url: window.location.pathname + "?a=save&uid=" + uid,
            async: true,
            type: "POST",
            data: JSON.stringify(player_data),
            dataType: "json",
            success: function (data) {
                layer.closeAll('loading');
                if (data.ok) {
                    layer.msg("保存成功");
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
    $("#btn_downfirst").click(function () {
        layer.load(2);
        $.ajax({
            url: window.location.pathname + "?a=downfirst&uid="+uid,
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
            url: window.location.pathname + "?a=rdt&uid="+uid,
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
            , title: "警告"
            , btn: ['上传', '取消']
        }, function () {
            layer.closeAll("dialog");
            layer.load(2);
            $.ajax({
                url: window.location.pathname +"?a=upload&uid="+uid,
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
            url: window.location.pathname + "?a=sign&uid="+uid,
            async: true,
            type: "POST",
            data: $(".signdata textarea.data").val(),
            dataType: "json",
            success: function (data) {
                layer.closeAll('loading');
                if (data.ok) {
                    var rr = "Sign: " + data.Sign + "\r\n";
                    rr += "Server-Time: " + data.ServerTime + "\r\n";
                    rr += "Sign: " + data.Sign2;
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
    return lop(player_data, player_data_bak, result);
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

    var save_link = document.createElementNS("http://www.w3.org/1999/xhtml", "a");
    save_link.href = urlObject.createObjectURL(downloadData);
    save_link.download = name;
    fake_click(save_link);
}
function htt() {
    var data = [{ "t": 5, "i": 59 }, { "t": 6, "i": 60 }, { "t": 11, "i": 59 }, { "t": 7, "i": 63 }, { "t": 0, "i": 163 }, { "t": 1, "i": 60 }, { "t": 2, "i": 64 }, { "t": 3, "i": 70 }, { "t": 4, "i": 60 }, { "t": 2, "i": 64 }, { "t": 4, "i": 60 }];
    var maxid = parseInt(player_data.playerDict.itemID);
    for (var i in data) {
        player_data.playerDict.packageArr.push({
            itemType: data[i].t.toString(),
            childType: data[i].i.toString(),
            addDict: {},
            itemID: (maxid + parseInt(i) + 1).toString(),
            qianghuaLv: "10"
        });
    }
    player_data.playerDict.itemID = (maxid + data.length + 2).toString();
}