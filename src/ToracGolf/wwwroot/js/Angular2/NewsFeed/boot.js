System.register(['angular2/platform/browser', './NewsFeedApp', 'angular2/core', 'angular2/http', '../Common/httpinterceptor'], function(exports_1) {
    var browser_1, NewsFeedApp_1, core_1, http_1, httpinterceptor_1;
    return {
        setters:[
            function (browser_1_1) {
                browser_1 = browser_1_1;
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
            browser_1.bootstrap(NewsFeedApp_1.NewsFeedApp, [http_1.HTTP_PROVIDERS, httpinterceptor_1.HttpInterceptor]);
        }
    }
});
//# sourceMappingURL=boot.js.map