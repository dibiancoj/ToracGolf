/// <reference path="../../../../node_modules/angular2-in-memory-web-api/typings/browser.d.ts" />
System.register(['@angular/platform-browser-dynamic', './NewsFeedApp', '@angular/core', '@angular/http', '../Common/httpinterceptor'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var platform_browser_dynamic_1, NewsFeedApp_1, core_1, http_1, httpinterceptor_1;
    return {
        setters:[
            function (platform_browser_dynamic_1_1) {
                platform_browser_dynamic_1 = platform_browser_dynamic_1_1;
            },
            function (NewsFeedApp_1_1) {
                NewsFeedApp_1 = NewsFeedApp_1_1;
            },
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (httpinterceptor_1_1) {
                httpinterceptor_1 = httpinterceptor_1_1;
            }],
        execute: function() {
            core_1.enableProdMode();
            platform_browser_dynamic_1.bootstrap(NewsFeedApp_1.NewsFeedApp, [http_1.HTTP_PROVIDERS, httpinterceptor_1.HttpInterceptor]);
        }
    }
});
//# sourceMappingURL=boot.js.map