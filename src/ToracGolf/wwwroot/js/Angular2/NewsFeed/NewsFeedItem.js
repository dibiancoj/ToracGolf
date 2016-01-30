System.register(['angular2/core', './NewsFeedService', './NewsFeedComment', 'rxjs/add/operator/map'], function(exports_1) {
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
            },
            function (_1) {}],
        execute: function() {
            NewsFeedItemPost = (function () {
                function NewsFeedItemPost(newsFeedService, ngZone) {
                    this.LikeEvent = new core_1.EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
                    this.CommentSaveEvent = new core_1.EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
                    this.ShowHideCommentEvent = new core_1.EventEmitter();
                    //set the properties
                    this.NewsFeedSvc = newsFeedService;
                    this.NgZoneSvc = ngZone;
                }
                ;
                NewsFeedItemPost.prototype.LikeClick = function (id, newsFeedTypeId) {
                    //pass this back to the parent component.
                    this.LikeEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
                };
                NewsFeedItemPost.prototype.SaveComment = function (id, newsFeedTypeId, comment, InputElement) {
                    //pass this back to the parent component
                    this.CommentSaveEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId, Comment: comment, TextBoxElement: InputElement });
                };
                NewsFeedItemPost.prototype.ShowHideComments = function (id, newsFeedTypeId, comment) {
                    //pass this back to the parent component
                    this.ShowHideCommentEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
                };
                NewsFeedItemPost.prototype.LikeCommentBubble = function (commentId) {
                    //closure
                    var _thisClass = this;
                    //go grab the record
                    var recordToUpdate = this.Post.Comments.First(function (x) { return x.CommentId == commentId; });
                    //go save the comment
                    this.NewsFeedSvc.CommentLikeClick(commentId).subscribe(function (likeCount) {
                        //go run this so angular can update the new records
                        _thisClass.NgZoneSvc.run(function () {
                            recordToUpdate.NumberOfLikes = likeCount;
                        });
                    });
                };
                NewsFeedItemPost.prototype.DateTimePipeHack = function (dateTimeIsoValue, isMonth) {
                    var dateToFormat = new Date(dateTimeIsoValue);
                    if (isMonth) {
                        return this.NewsFeedSvc.MonthNameSelect(dateToFormat.getMonth());
                    }
                    return dateToFormat.getDate().toString();
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
                __decorate([
                    //declared on NewsFeedPostClientView. its the event we bind with the parent component.
                    core_1.Output(), 
                    __metadata('design:type', Object)
                ], NewsFeedItemPost.prototype, "ShowHideCommentEvent", void 0);
                NewsFeedItemPost = __decorate([
                    core_1.Component({
                        selector: 'NewsFeedPostItem',
                        templateUrl: 'NewsFeedItemClientView',
                        directives: [NewsFeedComment_1.NewsFeedItemComment]
                    }), 
                    __metadata('design:paramtypes', [NewsFeedService_1.NewsFeedService, core_1.NgZone])
                ], NewsFeedItemPost);
                return NewsFeedItemPost;
            })();
            exports_1("NewsFeedItemPost", NewsFeedItemPost);
        }
    }
});
//# sourceMappingURL=NewsFeedItem.js.map