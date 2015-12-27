System.register(['angular2/core', './NewsFeedService'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, NewsFeedService_1;
    var NewsFeedApp;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (NewsFeedService_1_1) {
                NewsFeedService_1 = NewsFeedService_1_1;
            }],
        execute: function() {
            NewsFeedApp = (function () {
                function NewsFeedApp(newsFeedService, ngZone) {
                    this.NewsFeedSvc = newsFeedService;
                    this.NgZoneSvc = ngZone;
                    var t = this;
                    setTimeout(function () {
                        //run outside of the angular zone for performance
                        t.NgZoneSvc.runOutsideAngular(function () {
                            //we have some data (get this from ajax return 
                            var data = [{ Month: 'January', Day: 5 }, { Month: 'Feb', Day: 10 }];
                            //now re-enter the zone so we can update the ui
                            t.NgZoneSvc.run(function () {
                                data.forEach(function (x) { return t.NewsFeedSvc.NewsFeeds.push(x); });
                            });
                        });
                    }, 3000);
                }
                ;
                NewsFeedApp = __decorate([
                    core_1.Component({
                        selector: 'NewsFeedPostContainer',
                        templateUrl: 'js/Angular2/NewsFeed/NewsFeedPostView.html',
                        bindings: [NewsFeedService_1.NewsFeedService]
                    }), 
                    __metadata('design:paramtypes', [NewsFeedService_1.NewsFeedService, core_1.NgZone])
                ], NewsFeedApp);
                return NewsFeedApp;
            })();
            exports_1("NewsFeedApp", NewsFeedApp);
        }
    }
});
//# sourceMappingURL=NewsFeedApp.js.map