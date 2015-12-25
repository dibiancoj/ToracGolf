System.register(['angular2/platform/browser', './NewsFeedApp', 'angular2/core'], function(exports_1) {
    var browser_1, NewsFeedApp_1, core_1;
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
            }],
        execute: function() {
            core_1.enableProdMode();
            browser_1.bootstrap(NewsFeedApp_1.NewsFeedApp);
        }
    }
});
//# sourceMappingURL=boot.js.map