<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="translate.aspx.cs" Inherits="telegramSvr.i18n.translate" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>翻译</title>
    <link rel="stylesheet" href="../js/layui/css/layui.css" />
    
<style>
body{padding: 20px;max-width:1000px;margin: 0 auto;}
.layui-table-page{padding:0;height:auto;border-width:0;right:10px;float:right;width:auto}
.filter-toolbar{
    display :inline;
    float:left;
    padding-left:10px;
    line-height: 30px;
}

    table.translations th.priority {
        width: 2em;
        text-align: center;
    }

    table.translations th.original,
    table.translations th.translation {
        width: 50%;
    }

    table.translations th.actions {
        min-width: 30px;
        text-align: center;
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
        min-width:160px
    }
.filter-toolbar span.layui-icon {color: #FFB800;}
.filter-current{font-weight:bold;}
</style>
</head>
<body>
    <form id="form1" runat="server"></form>
        <blockquote class="layui-elem-quote"><%=LI("将 {0} 翻译为 {1} ",lang_src,lang_dst)%></blockquote>
        <div id="paging" class="paging layui-table-page"></div>
<div class="filter-toolbar">
    <a href="<%=baseurl%>" class="revealing filter open">Filter ↑</a> <span class="separator">•</span>
    <a href="<%=baseurl%>" class="revealing sort">Sort ↓</a> <strong class="separator">•</strong>
    <a href="<%=baseurl%>" <%=filterstar == 0 ? "class=\"filter-current\"" : ""%>>All&nbsp;(<%=AllRecordCount %>)</a> <span class="separator">•</span>
    <a href="<%=baseurl%>&filter_s=5" <%=filterstar == 5 ? "class=\"filter-current\"" : ""%>><span class="layui-icon layui-icon-rate-solid">5</span>(<%=s1_5[4] %>)</a> <span class="separator">•</span>
    <a href="<%=baseurl%>&filter_s=4" <%=filterstar == 4 ? "class=\"filter-current\"" : ""%>><span class="layui-icon layui-icon-rate-solid">4</span>(<%=s1_5[3] %>)</a> <span class="separator">•</span>
    <a href="<%=baseurl%>&filter_s=3" <%=filterstar == 3 ? "class=\"filter-current\"" : ""%>><span class="layui-icon layui-icon-rate-solid">3</span>(<%=s1_5[2] %>)</a> <span class="separator">•</span>
    <a href="<%=baseurl%>&filter_s=2" <%=filterstar == 2 ? "class=\"filter-current\"" : ""%>><span class="layui-icon layui-icon-rate-solid">2</span>(<%=s1_5[1] %>)</a> <span class="separator">•</span>
    <a href="<%=baseurl%>&filter_s=1" <%=filterstar == 1 ? "class=\"filter-current\"" : ""%>><span class="layui-icon layui-icon-rate-solid">1</span>(<%=s1_5[0] %>)</a>
</div>
        <asp:Repeater ID="rptList" runat="server">
            <HeaderTemplate>
                <table class="layui-table translations" lay-skin="line">
                    <thead>
                        <tr>                           
                            <th class="original"><%=LI("原始字符串")%></th>
                            <th class="translation"><%=LI("翻译")%></th>
                            <th class="actions"><%=LI("评分")%></th>
                            <%if(includeSys){ %><th class="actions">sys</th><%} %> 
                        </tr>
                    </thead>
                    <tbody>
                    <tr class="editor layui-bg-gray" style="display:none">
                    <td colspan="3">
                        <div class="strings">
                            <p class="original"></p>
                            <div class="textareas">
                                <blockquote><em><small class="translation"></small></em></blockquote>
                                <textarea class="layui-textarea translation"></textarea>
                            </div>
                            <div class="actions"><button class="layui-btn"><%=LI("确定")%></button></div>
                        </div>
                        <div class="meta"><dl><dt>评分:</dt><dd><div id="Scorecnt"></div></dd></dl>
                            <dl><dt>页面:</dt><dd class="layui-select"><a>a.aspx</a><br><a>b.aspx</a></dd></dl>
                        </div>
                    </td>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="preview" data-orig="<%#Eval("Original")%>" data-score="<%#Eval("score")%>" >
                    <td><%#Eval("lang1")%></td>
                    <td><%#Eval("lang2")%></td>
                    <td><%#Eval("Score")%></td>
                    <%if(includeSys){ %><td><%#(bool)Eval("isSyskey")?"√":""%></th><%} %> 
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"3\">暂无记录</td></tr>" : ""%>
                    </tbody>
                  </table>
            </FooterTemplate>
        </asp:Repeater>
    <div id="paging2" class="paging layui-table-page"></div>

    <script src="../js/layui/layui.min.js"></script>
    <script>
        var sx = null;
        layui.use(['laypage', 'layer', 'rate'], function () {
            var laypage = layui.laypage
                , layer = layui.layer
                , $ = layui.$;

            var selectedRow;            
            laypage.render({
                elem: 'paging'
                , count: <%=RecordCount%>
                , prev: '<em>←</em>'
                , next: '<em>→</em>'
                , layout: ['prev', 'page', 'next']
                , limit: <%=datalimit%>
                , curr: <%=pageidx%>
                , jump: function (obj, first) {
                    if (!first) {
                        location.href = "<%=pageUrl()%>&page=" + obj.curr;
                    }
                }
            });
            laypage.render({
                elem: 'paging2'
                , count: <%=RecordCount%>
                , prev: '<em>←</em>'
                , next: '<em>→</em>'
                , layout: ['prev', 'page', 'next']
                , limit: <%=datalimit%>
                , curr: <%=pageidx%>
                , jump: function (obj, first) {
                    if (!first) {
                        location.href = "<%=pageUrl()%>&page=" + obj.curr;
                    }
                }
            });
            sx = layui.rate.render({
                elem: '#Scorecnt'
                , length: 5
                , value: 5
            });

            $("tr.preview").click(function () {
                selectedRow = $(this).is("tr.preview") ? $(this) : $(this).parents("tr.preview");
                var editor = $("table.translations tr.editor");
                if (editor.is(":visible") && editor.is(selectedRow.next())) {
                    editor.hide();
                } else {
                    var orig = selectedRow.find("td:eq(0)").text();
                    var score = selectedRow.data("score") || 5;
                    var trans = selectedRow.find("td:eq(1)").text();

                    editor.find(".original").text(orig);
                    editor.find(".translation").text(trans);
                    editor.insertAfter(selectedRow).show();
                    sx.setvalue(score);
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
                            layer.msg("已保存", {time:1000});
                        } else {
                            layer.msg(data.msg);
                        }
                    },
                    error: function (err) {
                        layer.closeAll('loading');
                        layer.msg(err, { icon: 2 });
                    }
                });
                $("table.translations tr.editor").hide();
            });
        });
    </script>
</body>
</html>
