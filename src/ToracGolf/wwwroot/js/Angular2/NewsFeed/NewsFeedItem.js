System.register(['angular2/core', './NewsFeedService', './NewsFeedComment'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, NewsFeedService_1, NewsFeedComment_1;
    var NewsFeedItemPost;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (NewsFeedService_1_1) {
                NewsFeedService_1 = NewsFeedService_1_1;
            },
            function (NewsFeedComment_1_1) {
                NewsFeedComment_1 = NewsFeedComment_1_1;
            }],
        execute: function() {
            NewsFeedItemPost = (function () {
                function NewsFeedItemPost() {
                    this.LikeEvent = new core_1.EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
                    this.CommentSaveEvent = new core_1.EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
                }
                NewsFeedItemPost.prototype.LikeClick = function (id, newsFeedTypeId) {
                    //pass this back to the parent component.
                    this.LikeEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
                };
                NewsFeedItemPost.prototype.SaveComment = function (id, newsFeedTypeId, comment) {
                    //pass this back to the parent component
                    this.CommentSaveEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId, Comment: comment });
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', NewsFeedService_1.NewsFeedItem)
                ], NewsFeedItemPost.prototype, "Post", void 0);
                __decorate([
                    //declared on NewsFeedPostClientView. it's the attribute name where we pass in the object
                    core_1.Output(), 
                    __metadata('design:type', Object)
                ], NewsFeedItemPost.prototype, "LikeEvent", void 0);
                __decorate([
                    //declared on NewsFeedPostClientView. its the event we bind with the parent component.
                    core_1.Output(), 
                    __metadata('design:type', Object)
                ], NewsFeedItemPost.prototype, "CommentSaveEvent", void 0);
                NewsFeedItemPost = __decorate([
                    core_1.Component({
                        selector: 'NewsFeedPostItem',
                        templateUrl: 'NewsFeedItemClientView',
                        directives: [NewsFeedComment_1.NewsFeedItemComment]
                    }), 
                    __metadata('design:paramtypes', [])
                ], NewsFeedItemPost);
                return NewsFeedItemPost;
            })();
            exports_1("NewsFeedItemPost", NewsFeedItemPost);
        }
    }
});
//# sourceMappingURL=NewsFeedItem.js.map