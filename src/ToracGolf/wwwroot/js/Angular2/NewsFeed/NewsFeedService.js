System.register(['angular2/core', 'rxjs/add/operator/map', '../Common/httpinterceptor'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, httpinterceptor_1;
    var NewsFeedService, NewsFeedItem;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (_1) {},
            function (httpinterceptor_1_1) {
                httpinterceptor_1 = httpinterceptor_1_1;
            }],
        execute: function() {
            NewsFeedService = (function () {
                function NewsFeedService(httpInterceptor) {
                    this.HttpInterceptorSvc = httpInterceptor;
                }
                NewsFeedService.prototype.NewFeedGet = function () {
                    return this.HttpInterceptorSvc.Post('NewsFeedsGetPost', '');
                };
                NewsFeedService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [httpinterceptor_1.HttpInterceptor])
                ], NewsFeedService);
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