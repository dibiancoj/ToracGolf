System.register(['@angular/core', './NewsFeedService', '../Common/CustomFormatterService'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, NewsFeedService_1, CustomFormatterService_1;
    var NewsFeedItemComment;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (NewsFeedService_1_1) {
                NewsFeedService_1 = NewsFeedService_1_1;
            },
            function (CustomFormatterService_1_1) {
                CustomFormatterService_1 = CustomFormatterService_1_1;
            }],
        execute: function() {
            NewsFeedItemComment = (function () {
                function NewsFeedItemComment(newsFeedService, customFormatter) {
                    this.LikeCommentEvent = new core_1.EventEmitter();
                    //set the properties
                    this.NewsFeedSvc = newsFeedService;
                    this.CustomFormatterSvc = customFormatter;
                }
                ;
                NewsFeedItemComment.prototype.LikeComment = function (commentId) {
                    //pass this back to the parent component.
                    this.LikeCommentEvent.emit(commentId);
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', NewsFeedService_1.NewsFeedComment)
                ], NewsFeedItemComment.prototype, "Comment", void 0);
                __decorate([
                    core_1.Output(), 
                    __metadata('design:type', Object)
                ], NewsFeedItemComment.prototype, "LikeCommentEvent", void 0);
                NewsFeedItemComment = __decorate([
                    core_1.Component({
                        selector: 'NewsFeedCommentItem',
                        templateUrl: 'NewsFeedCommentItemClientView'
                    }), 
                    __metadata('design:paramtypes', [NewsFeedService_1.NewsFeedService, CustomFormatterService_1.CustomFormatterService])
                ], NewsFeedItemComment);
                return NewsFeedItemComment;
            }());
            exports_1("NewsFeedItemComment", NewsFeedItemComment);
        }
    }
});
//# sourceMappingURL=NewsFeedComment.js.map