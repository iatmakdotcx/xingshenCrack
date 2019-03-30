layui.define(function (exports) {

    var config = {
        pageTabs: true,       // 是否开启多标签
    };
    if (localStorage.getItem("config_pageTabs") == "false")
        config.pageTabs = false; 

    exports('config', config);
});
