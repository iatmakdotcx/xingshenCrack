layui.define(['config', 'admin', 'layer', 'laytpl', 'element', 'form'], function (exports) {
    var $ = layui.$;
    var config = layui.config;
    var admin = layui.admin;
    var layer = layui.layer;
    var laytpl = layui.laytpl;
    var element = layui.element;
    var form = layui.form;

    var index = {        
        // 页面元素绑定事件监听
        bindEvent: function () {
            // 退出登录
            $('#btnLogout').click(function () {
                location.replace('login.aspx?logout=1');
            });
            // 修改密码
            $('#setPsw').click(function () {
                admin.popupRight('components/chgpassword.aspx');
            });
            // 个人信息
            $('#setInfo').click(function () {

            });
            // 消息
            $('#btnMessage').click(function () {
                admin.popupRight('components/message.html');
            });
            $(".layui-side a[lay-href]").click(function () {
                index.openTabWindow(this.innerText, this.getAttribute("lay-href"), this.innerText);
            });
        },
        openTabWindow: function (menuId, menuPath, menuName) {
            admin.showLoading();
            if (config.pageTabs) {
                if ($(".layui-body ul.layui-tab-title>li[lay-id='" + menuId + "']").length == 0) {
                    element.tabAdd('admin-pagetabs', {
                        title: menuName,
                        content: '<iframe src="' + menuPath + '" frameborder=\"0\" class=\"layadmin-iframe\" id="' + menuId + '"></iframe>',
                        id: menuId
                    });
                    $("iframe#" + menuId).on("load", function () {
                        admin.removeLoading();
                    });
                } else {
                    admin.removeLoading();
                }
                // 切换tab关闭表格内浮窗
                $('.layui-table-tips-c').trigger('click');
                element.tabChange('admin-pagetabs', menuId);
            } else {
                var mainiframe = $(".layadmin-iframe");
                if (mainiframe.length == 0) {
                    mainiframe = $("<iframe frameborder=\"0\" class=\"layadmin-iframe\"></iframe>")
                    $(".layui-body").append(mainiframe);
                    mainiframe.on("load",function () {
                        admin.removeLoading();
                    });
                }
                mainiframe.attr("src", menuPath);
            }
        },
        // 检查多标签功能是否开启
        checkPageTabs: function () {
            // 加载主页
            if (config.pageTabs) {
                $('.layui-layout-admin').addClass('open-tab');
                this.openTabWindow("home", "components/home.aspx", '<i class="layui-icon layui-icon-home"></i>');
            } else {
                $('.layui-layout-admin').removeClass('open-tab');
            }
        },
        // 关闭选项卡
        closeTab: function (menuId) {
            element.tabDelete('admin-pagetabs', menuId);
        }
    };

    exports('index', index);
});
