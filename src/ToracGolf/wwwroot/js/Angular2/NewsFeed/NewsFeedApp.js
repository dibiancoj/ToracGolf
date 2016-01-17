System.register(['angular2/core', './NewsFeedService', './NewsFeedItem', './NewsFeedComment', 'angular2/common', 'rxjs/add/operator/map'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, NewsFeedService_1, NewsFeedItem_1, NewsFeedComment_1, common_1;
    var NewsFeedApp;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (NewsFeedService_1_1) {
                NewsFeedService_1 = NewsFeedService_1_1;
            },
            function (NewsFeedItem_1_1) {
                NewsFeedItem_1 = NewsFeedItem_1_1;
            },
            function (NewsFeedComment_1_1) {
                NewsFeedComment_1 = NewsFeedComment_1_1;
            },
            function (common_1_1) {
                common_1 = common_1_1;
            },
            function (_1) {}],
        execute: function() {
            NewsFeedApp = (function () {
                function NewsFeedApp(newsFeedService, ngZone) {
                    //set the properties
                    this.NewsFeedSvc = newsFeedService;
                    this.NgZoneSvc = ngZone;
                    //initial property setting
                    this.SearchFilterText = '';
                    //go load the posts
                    this.LoadPosts(null, null);
                }
                ;
                NewsFeedApp.prototype.LoadPosts = function (newsFeedTypeId, searchFilterText) {
                    var _this = this;
                    //closure
                    var _thisClass = this;
                    //go grab the data
                    this.NewsFeedSvc.NewFeedGet(newsFeedTypeId, searchFilterText).subscribe(function (queryResult) {
                        //go run this so angular can update the new records
                        _thisClass.NgZoneSvc.run(function () {
                            //the pipe for date time format doesn't support iso string's right now. so flip it to a date it can handle
                            queryResult.Results.forEach(function (x) { return x.PostDate = new Date(x.PostDate.toString()); });
                            //set the working posts
                            _this.Posts = queryResult.Results;
                            //go set the counts
                            _this.NewCoursePostCount = queryResult.UnFilteredCourseCount;
                            //set the count for new rounds
                            _this.NewRoundPostCount = queryResult.UnFilteredRoundCount;
                            //what is the active nav menu
                            _this.ActiveFeedTypeId = newsFeedTypeId;
                        });
                    });
                };
                ;
                NewsFeedApp.prototype.Like = function (likeConfig) {
                    //go grab the record
                    var recordToUpdate = this.Posts.First(function (x) { return x.Id == likeConfig.Id && x.FeedTypeId == likeConfig.NewsFeedTypeId; });
                    //if you already like the item, you want to unlike the item
                    var youLikeTheItemAlready = recordToUpdate.YouLikedItem;
                    //closure
                    var _thisClass = this;
                    this.NewsFeedSvc.LikePost(likeConfig.Id, likeConfig.NewsFeedTypeId).subscribe(function (posts) {
                        //go run this so angular can update the new records
                        _thisClass.NgZoneSvc.run(function () {
                            //go find the post and increment by 1 (instead of reloading the data)
                            if (youLikeTheItemAlready) {
                                recordToUpdate.LikeCount--;
                            }
                            else {
                                recordToUpdate.LikeCount++;
                            }
                            //set that you liked it
                            recordToUpdate.YouLikedItem = !youLikeTheItemAlready;
                        });
                    });
                };
                ;
                NewsFeedApp.prototype.FilterByType = function (newsFeedTypeId, element) {
                    //set the active news feed type id
                    this.ActiveFeedTypeId = newsFeedTypeId;
                    //go reload the posts
                    this.LoadPosts(newsFeedTypeId, this.SearchFilterText);
                };
                ;
                NewsFeedApp.prototype.ReRunSearch = function () {
                    //they want to filter backed on text box
                    this.LoadPosts(this.ActiveFeedTypeId, this.SearchFilterText);
                };
                NewsFeedApp.prototype.SaveComment = function (commentConfig) {
                    //closure
                    var _thisClass = this;
                    //go grab the record
                    var recordToUpdate = this.Posts.First(function (x) { return x.Id == commentConfig.Id && x.FeedTypeId == commentConfig.NewsFeedTypeId; });
                    //go save the comment
                    this.NewsFeedSvc.NewsFeedCommentSave(commentConfig.Id, commentConfig.NewsFeedTypeId, commentConfig.Comment).subscribe(function (comments) {
                        //go run this so angular can update the new records
                        _thisClass.NgZoneSvc.run(function () {
                            //increase the tally of comments
                            recordToUpdate.CommentCount++;
                            //go add it to the list of comments
                            recordToUpdate.Comments = comments;
                        });
                    });
                };
                NewsFeedApp.prototype.ShowHideComment = function (showHideConfig) {
                    //closure
                    var _thisClass = this;
                    //try to find out if we have any comments
                    var recordToUpdate = this.Posts.First(function (x) { return x.Id == showHideConfig.Id && x.FeedTypeId == showHideConfig.NewsFeedTypeId; });
                    //if we have 0 comments, then just set it to null and don't go to the server
                    if (recordToUpdate.Comments != null || recordToUpdate.CommentCount == 0) {
                        recordToUpdate.Comments = null; //set it back to null
                    }
                    else {
                        //go get the comments
                        this.NewsFeedSvc.NewsFeedCommentSelect(showHideConfig.Id, showHideConfig.NewsFeedTypeId).subscribe(function (comments) {
                            //go run this so angular can update the new records
                            _thisClass.NgZoneSvc.run(function () {
                                //go add it to the list of comments
                                recordToUpdate.Comments = comments;
                            });
                        });
                    }
                };
                NewsFeedApp = __decorate([
                    core_1.Component({
                        selector: 'NewsFeedPostContainer',
                        templateUrl: 'NewsFeedClientView',
                        bindings: [NewsFeedService_1.NewsFeedService],
                        directives: [common_1.NgClass, NewsFeedItem_1.NewsFeedItemPost, NewsFeedComment_1.NewsFeedItemComment]
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