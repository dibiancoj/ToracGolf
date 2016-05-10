import {Component, Input, Inject, Output, EventEmitter, NgZone} from '@angular/core';
import {NewsFeedService, NewsFeedItem, NewsFeedTypeId} from './NewsFeedService';
import {CustomFormatterService} from '../Common/CustomFormatterService';
import {NewsFeedItemComment} from './NewsFeedComment';

import { Http, Response } from '@angular/http';
import 'rxjs/add/operator/map';

@Component({
    selector: 'NewsFeedPostItem',
    templateUrl: 'NewsFeedItemClientView',
    directives: [NewsFeedItemComment]
})
export class NewsFeedItemPost {

    @Input() Post: NewsFeedItem; //declared on NewsFeedPostClientView. it's the attribute name where we pass in the object
    @Output() LikeEvent = new EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
    @Output() CommentSaveEvent = new EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
    @Output() ShowHideCommentEvent = new EventEmitter();

    NewsFeedSvc: NewsFeedService;
    NgZoneSvc: NgZone;
    CustomFormatterSvc: CustomFormatterService;

    constructor(newsFeedService: NewsFeedService, ngZone: NgZone, customFormatter: CustomFormatterService) {

        //set the properties
        this.NewsFeedSvc = newsFeedService;
        this.NgZoneSvc = ngZone;
        this.CustomFormatterSvc = customFormatter;
    };

    LikeClick(id: number, newsFeedTypeId: NewsFeedTypeId) {
    
        //pass this back to the parent component.
        this.LikeEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
    }

    SaveComment(id: number, newsFeedTypeId: NewsFeedTypeId, comment: string, InputElement: HTMLInputElement) {

        //pass this back to the parent component
        this.CommentSaveEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId, Comment: comment, TextBoxElement: InputElement });
    }

    ShowHideComments(id: number, newsFeedTypeId: NewsFeedTypeId, comment: string) {

        //pass this back to the parent component
        this.ShowHideCommentEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
    }

    LikeCommentBubble(commentId: number) {

        //closure
        var _thisClass = this;

        //go grab the record
        var recordToUpdate = this.Post.Comments.First(x=> x.CommentId == commentId);

        //go save the comment
        this.NewsFeedSvc.CommentLikeClick<number>(commentId).subscribe((likeCount: number) => {
         
            //go run this so angular can update the new records
            _thisClass.NgZoneSvc.run(() => {

                recordToUpdate.NumberOfLikes = likeCount;
            });
        });
    }

}