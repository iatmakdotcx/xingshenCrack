document.write("<script src='/js/pinyinFL.js'></script>");

if (!Date.prototype.Format) {
    Date.prototype.Format = function (fmt) {
        var o = {
            "M+": this.getMonth() + 1,
            "d+": this.getDate(),
            "h+": this.getHours(),
            "H+": this.getHours(),
            "m+": this.getMinutes(),
            "s+": this.getSeconds(),
            "q+": Math.floor((this.getMonth() + 3) / 3),
            "S": this.getMilliseconds()
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }
}
function Delconfirm() {
    if (confirm("删除？")) {
        location.href = location.href + '&action=del'
    }
}
function cDeletebtn() {
    $("div.frmbottom24680").html($("div.frmbottom24680").html() + "<input type=\"button\" class=\"btn violet\" value=\" 删 除 \" onclick=\"Delconfirm()\" />");
}
//可以自动关闭的提示，基于lhgdialog插件
function jsprint(msgtitle, url, msgcss, callback) {
    var iconurl = "";
    switch (msgcss) {
        case "Success":
            iconurl = "32X32/succ.png";
            break;
        case "Error":
            iconurl = "32X32/fail.png";
            break;
        default:
            iconurl = "32X32/hits.png";
            break;
    }
    top.artDialog.tips(msgtitle, 2, iconurl);
    //$.dialog.tips(msgtitle, 2, iconurl);
    if (url == "back") {
        if (frames["rptdisplayer"]) {
            frames["rptdisplayer"].history.back(-1);
        } else
            frames[0].history.back(-1);
    } else if (url != "") {
        if (frames["rptdisplayer"]) {
            frames["rptdisplayer"].location.href = url;
        } else
            frames[0].location.href = url;
    }
    //执行回调函数
    if (arguments.length == 4) {
        sessionStorage.setItem("OldUrl", url);
        setTimeout(function () {
            callback();
        }, 2000);
    }
}
//Tab控制函数
function tabs(tabObj) {
    var tabNum = $(tabObj).parent().index()
    //设置点击后的切换样式
    $(tabObj).parent().parent().find("li a").removeClass("selected");
    $(tabObj).addClass("selected");
    //根据参数决定显示内容
    $(".tab-content").hide();
    $(".tab-content").eq(tabNum).show();
}
//只允许输入数字
function checkNumber(e) {
    if (isFirefox = navigator.userAgent.indexOf("Firefox") > 0) {  //FF 
        if (!((e.which >= 48 && e.which <= 57) || (e.which >= 96 && e.which <= 105) || (e.which == 8) || (e.which == 46)))
            return false;
    } else {
        if (!((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || (event.keyCode == 8) || (event.keyCode == 46)))
            event.returnValue = false;
    }
}

$.fn.InitDataQuery = function (options) {
    var defaults = {
        url: "",           //ajax检索路径
        data: [],          //检索json
        AutoUsePy: false,  //如果true自动转拼音码
        width: "200px",
        height: "200px",
        InitListcallBack: function (json) {
            var html = "";
            for (var i = 0; i < json.length; i++) {
                var list = json[i];
                html += "<tr>";
                html += '<td style="width: 50px;">' + list[0] + '</td>';
                html += '<td style="width: 150px;">' + list[1] + '</td>';
                html += "</tr>";
            }
            return html;
        }, //初始化显示表格
        selectcallBack: function (json) { }    //选中行的回调
    };
    var settings = $.extend({}, defaults, options);
    var Ridx = 0;
    var QueryjsonData = {};
    var sender = this;
    var hasData = false;

    function dataQueryShowBox(c, htmldata) {
        var X = $(c).offset().top;
        var Y = $(c).offset().left;
        if ($("#div_gridshow").length == 0) {
            $('body').append('<div id="div_gridshow" style="overflow: auto;z-index: 1000;border: 1px solid #A8A8A8;border-top: 0px solid #A8A8A8;width:' + settings.width + ';height:' + settings.height + ';position: absolute; background-color: #fff; display: none;"></div>');
        } else {
            $("#div_gridshow").height(settings.height);
            $("#div_gridshow").width(settings.width);
        }
        $("#div_gridshow").html("");
        var sbhtml = '<table class="tableobj">';
        if (htmldata != "" && htmldata != undefined) {
            sbhtml += '<tbody>' + htmldata + '</tbody>';
        } else {
            sbhtml += '<tbody><tr><td style="color:red;text-align:center;width:' + settings.width + ';">没有找到您要的相关数据！</td></tr></tbody>';
        }
        sbhtml += '</table>';
        $("#div_gridshow").html(sbhtml);
        $("#div_gridshow").css("left", Y).css("top", X + 25).show();
        $("#div_gridshow").scrollTop(0);
        sender.focus();
        if (QueryjsonData.length > 0) {
            hasData = true;
            //默认选中第一行
            $("#div_gridshow").find('tbody tr').eq(0).addClass('selected');

            $("#div_gridshow").find('tbody tr').hover(function () {
                $("#div_gridshow").find('tbody tr').removeClass('selected');
                $(this).addClass("selected");
            }, function () {
                $(this).removeClass("selected");
            }).click(function () {
                onSelect();
            });
        }
    }

    function onSelect() {
        var dataIdx = $("#div_gridshow tbody tr.selected").index();
        if (dataIdx != -1) {
            settings.selectcallBack(QueryjsonData[dataIdx], sender);
        }
        $("#div_gridshow").hide();
    }

    return this.on({
        keydown: function (e) {
            switch (e.keyCode) {
                case 8, 27:/*删除或esc键*/
                    $("#div_gridshow").hide();
                    break;
                case 38: /* 方向键上*/

                    if (hasData && Ridx > 0) {
                        Ridx--;
                        $("#div_gridshow").find('tbody tr').removeClass('selected');
                        $("#div_gridshow").find('tbody tr').eq(Ridx).addClass('selected');

                        var sctl = $("#div_gridshow").find('tbody tr.selected');
                        if ((sctl.position().top + $("#div_gridshow .tableobj").position().top) < 0) {
                            var tv = sctl.position().top;
                            $("#div_gridshow").scrollTop(tv);
                        }
                    }
                    return false;
                case 40: /* 方向键下*/
                    var tindex = $("#div_gridshow").find('tbody tr').length;
                    if (hasData && Ridx < tindex - 1) {
                        Ridx++;
                        $("#div_gridshow").find('tbody tr').removeClass('selected');
                        $("#div_gridshow").find('tbody tr').eq(Ridx).addClass('selected');

                        var sctl = $("#div_gridshow").find('tbody tr.selected');
                        if ((sctl.position().top + sctl.height() + $("#div_gridshow table").position().top) >= $("#div_gridshow").height()) {
                            var tv = sctl.position().top - ($("#div_gridshow").height() - sctl.height());
                            $("#div_gridshow").scrollTop(tv);
                        }
                    }
                    return false;
                case 13:  /*回车键*/
                    if (hasData && !$("#div_gridshow").is(":hidden")) {
                        onSelect();
                    } else {
                        if ($(this).is(":text[readonly]")) {
                            return;
                        }
                        if ($(this).is(":disabled")) {
                            return;
                        }
                        sender = this;
                        Ridx = 0;
                        QueryjsonData = {};
                        hasData = false;                        

                        var value = $(this).val();
                        if (settings.url != "" && value != "") {
                            var param = "";
                            if (typeof (settings.url) == "function") {
                                param = settings.url();
                            } else {
                                param = settings.url;
                            }
                            if (param.indexOf("?") > 0) {
                                param += "&search=" + escape(value);
                            } else {
                                param += "?search=" + escape(value);
                            }
                            $.ajax({
                                type: 'get',
                                dataType: "json",
                                url: param,
                                cache: false,
                                async: false,
                                success: function (msg) {
                                    //QueryjsonData = eval("(" + msg + ")");
                                    QueryjsonData = msg;
                                    dataQueryShowBox(sender, settings.InitListcallBack(QueryjsonData));
                                }
                            });
                        }
                    }
                    return false;
                    break;
                default:

                    break;
            }
        },
        keyup: function (e) {
            if ((e.keyCode >= 48 || e.keyCode == 8) && settings.data.length > 0) {
                $(this).click();
            }
        },
        click: function () {
            if (settings.data.length > 0) {
                //检索json
                sender = this;
                var value = $(this).val().toLowerCase();
                var filterJson = [];
                for (var i = 0; i < settings.data.length; i++) {
                    if (settings.AutoUsePy && settings.data[i]["_pinyin_"] == undefined) {
                        //自动生成全部字段的拼音码
                        var arowallldata = "";
                        for (var k in settings.data[i]) {
                            arowallldata += settings.data[i][k];
                        }
                        settings.data[i]["_pinyin_"] = pinyinUtil.getFirstLetter(arowallldata, true).toString().toLowerCase();
                    }
                    for (var k in settings.data[i]) {
                        if (settings.data[i][k] != undefined && settings.data[i][k].toLowerCase().indexOf(value) > -1) {
                            filterJson.push(settings.data[i]);
                            break;
                        }
                    }
                    if (filterJson.length > 200) {
                        break;
                    }
                }
                QueryjsonData = filterJson;
                dataQueryShowBox(sender, settings.InitListcallBack(QueryjsonData));
            }
        },
        blur: function () {
            if ($("#div_gridshow tbody tr.selected").length == 0) {
                $("#div_gridshow").hide();
            }
        }
    });
}

//========================基于Validform插件========================
//初始化验证表单
$.fn.initValidform = function () {
    var checkValidform = function (formObj) {
        $(formObj).Validform({
            tiptype: function (msg, o, cssctl) {
                /*msg：提示信息;
                o:{obj:*,type:*,curform:*}
                obj指向的是当前验证的表单元素（或表单对象）；
                type指示提示的状态，值为1、2、3、4， 1：正在检测/提交数据，2：通过验证，3：验证失败，4：提示ignore状态；
                curform为当前form对象;
                cssctl:内置的提示信息样式控制函数，该函数需传入两个参数：显示提示信息的对象 和 当前提示的状态（既形参o中的type）；*/
                //全部验证通过提交表单时o.obj为该表单对象;
                if (!o.obj.is("form")) {
                    //定位到相应的Tab页面
                    if (o.obj.is(o.curform.find(".Validform_error:first"))) {
                        var tabobj = o.obj.parents(".tab-content"); //显示当前的选项
                        var tabindex = $(".tab-content").index(tabobj); //显示当前选项索引
                        if (!$(".content-tab ul li").eq(tabindex).children("a").hasClass("selected")) {
                            $(".content-tab ul li a").removeClass("selected");
                            $(".content-tab ul li").eq(tabindex).children("a").addClass("selected");
                            $(".tab-content").hide();
                            tabobj.show();
                        }
                    }
                    //页面上不存在提示信息的标签时，自动创建;
                    if (o.obj.parents("dd").find(".Validform_checktip").length == 0) {
                        o.obj.parents("dd").append("<span class='Validform_checktip' />");
                        o.obj.parents("dd").next().find(".Validform_checktip").remove();
                    }
                    var objtip = o.obj.parents("dd").find(".Validform_checktip");
                    cssctl(objtip, o.type);
                    objtip.text(msg);
                }
            },
            showAllError: true
        });
    };
    return $(this).each(function () {
        checkValidform($(this));
    });
}
//======================以上基于Validform插件======================

//智能浮动层函数
$.fn.smartFloat = function () {
    var position = function (element) {
        var top = element.position().top;
        var pos = element.css("position");
        $(window).scroll(function () {
            var scrolls = $(this).scrollTop();
            if (scrolls > top) {
                if (window.XMLHttpRequest) {
                    element.css({
                        position: "fixed",
                        top: 0
                    });
                } else {
                    element.css({
                        top: scrolls
                    });
                }
            } else {
                element.css({
                    position: pos,
                    top: top
                });
            }
        });
    };
    return $(this).each(function () {
        position($(this));
    });
};

//复选框
$.fn.ruleSingleCheckbox = function () {
    var singleCheckbox = function (parentObj) {
        //查找复选框
        var checkObj = parentObj.find('input:checkbox').eq(0);
        parentObj.children().hide();
        //添加元素及样式
        var lbl_yes = "是";
        var lbl_no = "否";
        if (parentObj.children().is("span") && parentObj.children().attr("label")) {
            var ll = parentObj.children().attr("label");
            var llarr = ll.split("|");
            if (llarr.length == 2) {
                lbl_yes = llarr[0]
                lbl_no = llarr[1];
            }
        }
        var newObj = $('<a href="javascript:;">'
            + '<i class="off">' + lbl_no + '</i>'
            + '<i class="on">' + lbl_yes + '</i>'
            + '</a>').prependTo(parentObj);
        parentObj.addClass("single-checkbox");
        //判断是否选中
        if (checkObj.prop("checked") == true) {
            newObj.addClass("selected");
        }
        //检查控件是否启用
        if (checkObj.prop("disabled") == true) {
            newObj.css("cursor", "default");
            return;
        }
        //绑定事件
        $(newObj).click(function () {
            if (checkObj.is(":disabled")) return false;
            if ($(this).hasClass("selected")) {
                $(this).removeClass("selected");
                //checkObj.prop("checked", false);
            } else {
                $(this).addClass("selected");
                //checkObj.prop("checked", true);
            }
            checkObj.trigger("click"); //触发对应的checkbox的click事件
        });
        $(checkObj).change(function () {
            if ($(this).prop("checked")) {
                $(this).prev().addClass("selected");
            } else {
                $(this).prev().removeClass("selected");
            }
        });
    };
    return $(this).each(function () {
        singleCheckbox($(this));
    });
};

//多项复选框
$.fn.ruleMultiCheckbox = function () {
    var multiCheckbox = function (parentObj) {
        parentObj.addClass("multi-checkbox"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find(":checkbox").each(function () {
            var indexNum = parentObj.find(":checkbox").index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                if (parentObj.find(':checkbox').eq(indexNum).is(":disabled")) return false;
                if ($(this).hasClass("selected")) {
                    $(this).removeClass("selected");
                    //parentObj.find(':checkbox').eq(indexNum).prop("checked",false);
                } else {
                    $(this).addClass("selected");
                    //parentObj.find(':checkbox').eq(indexNum).prop("checked",true);
                }
                parentObj.find(':checkbox').eq(indexNum).trigger("click"); //触发对应的checkbox的click事件
                //alert(parentObj.find(':checkbox').eq(indexNum).prop("checked"));
            });
        });
    };
    return $(this).each(function () {
        multiCheckbox($(this));
    });
}

//多项选项PROP
$.fn.ruleMultiPorp = function () {
    var multiPorp = function (parentObj) {
        parentObj.addClass("multi-porp"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<ul></ul>').prependTo(parentObj); //前插入一个DIV
        parentObj.find(":checkbox").each(function () {
            var indexNum = parentObj.find(":checkbox").index(this); //当前索引
            var liObj = $('<li></li>').appendTo(divObj)
            var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a><i></i>').appendTo(liObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                liObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                if (parentObj.find(':checkbox').eq(indexNum).is(":disabled")) return false;
                if ($(this).parent().hasClass("selected")) {
                    $(this).parent().removeClass("selected");
                } else {
                    $(this).parent().addClass("selected");
                }
                parentObj.find(':checkbox').eq(indexNum).trigger("click"); //触发对应的checkbox的click事件
                //alert(parentObj.find(':checkbox').eq(indexNum).prop("checked"));
            });
        });
    };
    return $(this).each(function () {
        multiPorp($(this));
    });
}

//多项单选
$.fn.ruleMultiRadio = function () {
    var multiRadio = function (parentObj) {
        parentObj.addClass("multi-radio"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find('input[type="radio"]').each(function () {
            var indexNum = parentObj.find('input[type="radio"]').index(this); //当前索引
            var newObj = $('<a href="javascript:;">' + parentObj.find('label').eq(indexNum).text() + '</a>').appendTo(divObj); //查找对应Label创建选项
            if ($(this).prop("checked") == true) {
                newObj.addClass("selected"); //默认选中
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                newObj.css("cursor", "default");
                return;
            }
            //绑定事件
            $(newObj).click(function () {
                if (parentObj.find('input[type="radio"]').eq(indexNum).is(":disabled")) return false;
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected");
                parentObj.find('input[type="radio"]').prop("checked", false);
                parentObj.find('input[type="radio"]').eq(indexNum).prop("checked", true);
                parentObj.find('input[type="radio"]').eq(indexNum).trigger("click"); //触发对应的radio的click事件
                //alert(parentObj.find('input[type="radio"]').eq(indexNum).prop("checked"));
            });
        });
    };
    return $(this).each(function () {
        multiRadio($(this));
    });
}

//单选下拉框
$.fn.ruleSingleSelect = function () {
    var singleSelect = function (parentObj) {
        parentObj.addClass("single-select"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        //创建元素
        var titObj = $('<a class="select-tit" href="javascript:;"><span></span><i></i></a>').appendTo(divObj);
        var itemObj = $('<div class="select-items"><ul></ul></div>').appendTo(divObj);
        var arrowObj = $('<i class="arrow"></i>').appendTo(divObj);
        var selectObj = parentObj.find("select").eq(0); //取得select对象
        //遍历option选项
        selectObj.find("option").each(function (i) {
            var indexNum = selectObj.find("option").index(this); //当前索引
            var liObj = $('<li>' + $(this).text() + '</li>').appendTo(itemObj.find("ul")); //创建LI
            if ($(this).prop("selected") == true) {
                liObj.addClass("selected");
                titObj.find("span").text($(this).text());
            }
            //检查控件是否启用
            if ($(this).prop("disabled") == true) {
                liObj.css("cursor", "default");
                return;
            }
            //绑定事件
            liObj.click(function () {
                $(this).siblings().removeClass("selected");
                $(this).addClass("selected"); //添加选中样式
                selectObj.find("option").prop("selected", false);
                selectObj.find("option").eq(indexNum).prop("selected", true); //赋值给对应的option
                titObj.find("span").text($(this).text()); //赋值选中值
                arrowObj.hide();
                itemObj.hide(); //隐藏下拉框
                selectObj.trigger("change"); //触发select的onchange事件
                //alert(selectObj.find("option:selected").text());
            });
        });
        //设置样式
        //titObj.css({ "width": titObj.innerWidth(), "overflow": "hidden" });
        //itemObj.children("ul").css({ "max-height": $(document).height() - titObj.offset().top - 62 });

        //检查控件是否启用
        if (selectObj.prop("disabled") == true) {
            titObj.css("cursor", "default");
            return;
        }
        //绑定单击事件
        titObj.click(function (e) {
            if (selectObj.is(":disabled")) return false;
            e.stopPropagation();
            if (itemObj.is(":hidden")) {
                //隐藏其它的下位框菜单
                $(".single-select .select-items").hide();
                $(".single-select .arrow").hide();
                //位于其它无素的上面
                arrowObj.css("z-index", "10");
                itemObj.css("z-index", "10");
                //显示下拉框
                arrowObj.show();
                itemObj.show();
            } else {
                //位于其它无素的上面
                arrowObj.css("z-index", "");
                itemObj.css("z-index", "");
                //隐藏下拉框
                arrowObj.hide();
                itemObj.hide();
            }
        });
        //绑定页面点击事件
        $(document).click(function (e) {
            selectObj.trigger("blur"); //触发select的onblure事件
            arrowObj.hide();
            itemObj.hide(); //隐藏下拉框
        });
    };
    return $(this).each(function () {
        singleSelect($(this));
    });
}

//多项下拉选择
$.fn.ruleMultiSelect = function () {
    var multiSelect = function (parentObj) {
        parentObj.addClass("multi-checkbox"); //添加样式
        parentObj.children().hide(); //隐藏内容
        var divObj = $('<div class="boxwrap"></div>').prependTo(parentObj); //前插入一个DIV
        parentObj.find(":checkbox").each(function () {
            //todo:有时间再搞
        });
    };
    return $(this).each(function () {
        multiSelect($(this));
    });
}

$.fn.fff = function () {
    var singleSelectRefresh = function (parentObj) {
        if (parentObj.parents(".single-checkbox").length) {
            if (parentObj.is(":checked")) {
                parentObj.parents(".single-checkbox").find("a").addClass("selected");
            } else {
                parentObj.parents(".single-checkbox").find("a").removeClass("selected");
            }            
        } else {
            if (parentObj.is("select:hidden") && parentObj.prev().hasClass("boxwrap")) {
                var boxwrap = parentObj.prev();
                boxwrap.find("li").removeClass("selected");
                var indexNum = parentObj.find("option:selected").index(); //当前索引
                boxwrap.find("li").eq(indexNum).addClass("selected");
                boxwrap.find(".select-tit span").text(parentObj.find("option:selected").text());
            }
        }
    };
    return $(this).each(function () {
        singleSelectRefresh($(this));
    });
}
var p_Mak_checkDatatype = {
    float: function (gets, obj, param) {
        var result = false;
        var precision = param.prec || 2;
        var reg = "/^[\\-]?\\d+$|^[\\-]?\\d+\\.\\d{1," + precision + "}$/";
        if (eval(reg).test(gets)) {
            var ff = parseFloat(gets);
            if (!isNaN(ff)) {

                if (param.min != undefined && typeof param.min == "number" && ff < param.min) {
                    result = false
                } else if (param.max != undefined && typeof param.max == "number" && ff > param.max) {
                    result = false
                } else
                    result = true;
            }
        }
        if (!result) {
            var tips = "必须是整数或" + precision + "小数！";
            if (param.min != undefined && typeof param.min == "number") {
                tips += "  最小值:" + param.min;
            }
            if (param.max != undefined && typeof param.max == "number") {
                tips += "  最大值:" + param.max;
            }
            obj.attr("errormsg", tips);
        }
        return result
    },
    int: function (gets, obj, param) {
        var result = false;
        var reg = "/^[\\-]?\\d+$/";
        if (eval(reg).test(gets)) {
            var ff = parseInt(gets);
            if (!isNaN(ff)) {
                if (param.min != undefined && typeof param.min == "number" && ff < param.min) {
                    result = false
                } else if (param.max != undefined && typeof param.max == "number" && ff > param.max) {
                    result = false
                } else
                    result = true;
            }
        }
        if (!result) {
            var tips = "必须是整数!";
            if (param.min != undefined && typeof param.min == "number") {
                tips += "  最小值:" + param.min;
            }
            if (param.max != undefined && typeof param.max == "number") {
                tips += "  最大值:" + param.max;
            }
            obj.attr("errormsg", tips);
        }
        return result
    }

}

var Validform_datatype = {};
var vf;
$(function () {
    $(".ltable tr:nth-child(odd)").addClass("odd_bg"); //隔行变色
    $("#floatHead").smartFloat();
    $(".rule-single-checkbox").ruleSingleCheckbox();
    $(".rule-multi-checkbox").ruleMultiCheckbox();
    $(".rule-multi-radio").ruleMultiRadio();
    $(".rule-single-select").ruleSingleSelect();
    $(".rule-multi-porp").ruleMultiPorp();

    $("select[data-ov]").change(function () {
        var data_odc = $("#" + $(this).attr("data-odc"));
        if (data_odc) {
            if ($(this).attr("data-ov") == $(this).val()) {
                data_odc.show();
            } else {
                data_odc.hide();
            }
        }
    }).change();
    //滑块
    $(".input-range input[type=range]").each(function () {
        var timer = null;
        var tooltip = $(this).next();
        if (tooltip.is("span.iptrange-tooltip")) {
            tooltip.hide();
            var showTip = function () {
                tooltip.show();
                var offsetLeft = this.offsetLeft;
                var width = this.offsetWidth - 28;
                var tooltipWidth = tooltip.width();
                var distince = Math.abs(this.max - this.min);
                var scaleWidth = (width / distince) * Math.abs(this.value - this.min);

                tooltip.css("left", (14 + offsetLeft + scaleWidth - tooltipWidth / 2) + 'px');
                tooltip.html(this.value);
                if (timer) {
                    clearTimeout(timer);
                }
                timer = setTimeout(function () {
                    tooltip.hide()
                }, 1000);
            };
            this.addEventListener('input', showTip);
        }
    });

    if (typeof ($.fn.Validform) == "function") {
        Validform_datatype = $.extend({}, $.Datatype, {
            "h*": function (gets, obj, curform, regxp) {
                if (obj[0].id == 'tzjk') {
                    if (gets || $(obj.parent()).is(":hidden")) {
                        return true;
                    } else
                        return false;
                } else if (gets || $(obj).is(":hidden")) {
                    return true;
                } else {
                    return false;
                }
            },
            extcheck: function (gets, obj, curform, regxp) {
                var param = eval("(" + obj.attr("data-vfp") + ")");
                if (param != undefined && param.t != undefined) {
                    var af = p_Mak_checkDatatype[param.t];
                    if (typeof af == "function") {
                        return af(gets, obj, param);
                    }
                }
                obj.attr("errormsg", "data-vfp.t未定义");
                return false;
            }
        });
        //初始化表单验证
        vf = $("#form1").Validform({
            showAllError: true,
            datatype: Validform_datatype,
            tiptype: function (msg, o, cssctl) {
                var objCL = null;
                var dobj = null;
                if ($(o.obj).is("input")) {
                    dobj = o.obj;
                } else if ($(o.obj).is("select")) {
                    dobj = o.obj.parents(".rule-single-select");
                }
                if (dobj && dobj.is(":visible")) {
                    objCL = dobj.get(0).classList;
                    if (o.type == 2 || o.type == 4) {
                        if (o.type == 4) {
                            objCL.remove("Validform_error");
                            objCL.remove("Validform_succ");
                        } else {
                            objCL.remove("Validform_error");
                            objCL.add("Validform_succ");
                        }
                        $(dobj).nextAll("span.Validform_checktip1").hide();
                    } else {
                        objCL.remove("Validform_succ");
                        objCL.add("Validform_error");
                        //msg = o.obj.attr("errormsg");
                        if (msg && msg.trim()) {
                            var tip = $(dobj).nextAll("span.Validform_checktip1");
                            if (!tip.length) {
                                $(dobj).parent().append("<span class='Validform_checktip1'></span>");
                                tip = $(dobj).nextAll("span.Validform_checktip1");
                            }
                            if (tip.length) {
                                tip.html(msg);
                                if (tip.parent().css("position") == "relative") {
                                    tip.css({
                                        'left':10,
                                        'top': 0
                                    });
                                } else {
                                    tip.css({
                                        'left': $(dobj).offset().left + 10,
                                        'top': $(dobj).offset().top
                                    });
                                }

                                tip.show();
                                if (tip.offset().top < 0) {
                                    tip.addClass("sbl");
                                }
                            }
                        }
                    }
                }
            }
        });
        $(document).click(function (a) {
            if ($(a.target).is("span.Validform_checktip1")) {
                $(a.target).hide();
            }
        });
    }
    $(document).click(function () {       
        if ($(this).is("#div_gridshow") || $(this).parents("#div_gridshow").length > 1) {
        } else {
            $("#div_gridshow").hide();
        }
    })
});