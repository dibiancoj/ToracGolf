(function (app) {
    document.addEventListener('DOMContentLoaded', function () {
        ng.platform.browser.bootstrap(app.NewsFeedPage);
    });
})(window.app || (window.app = {}));