System.register([], function(exports_1) {
    var NewsFeedService, NewsFeedItem;
    return {
        setters:[],
        execute: function() {
            NewsFeedService = (function () {
                function NewsFeedService() {
                    this.NewsFeeds = [];
                    this.NewsFeeds.push({ Month: 'January', Day: 5 }, { Month: 'December', Day: 10 });
                }
                NewsFeedService.prototype.NewFeedGet = function () {
                    return this.NewsFeeds;
                };
                return NewsFeedService;
            })();
            exports_1("NewsFeedService", NewsFeedService);
            NewsFeedItem = (function () {
                function NewsFeedItem() {
                }
                return NewsFeedItem;
            })();
            exports_1("NewsFeedItem", NewsFeedItem);
        }
    }
});
//# sourceMappingURL=NewsFeedService.js.map