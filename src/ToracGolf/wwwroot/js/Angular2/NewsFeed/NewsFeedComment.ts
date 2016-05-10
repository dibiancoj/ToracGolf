import {Component, Input, Inject, Output, EventEmitter} from '@angular/core';
import {NewsFeedComment, NewsFeedTypeId, NewsFeedService} from './NewsFeedService';
import {CustomFormatterService} from '../Common/CustomFormatterService';

@Component({
    selector: 'NewsFeedCommentItem',
    templateUrl: 'NewsFeedCommentItemClientView'
})
export class NewsFeedItemComment {

    @Input() Comment: NewsFeedComment;
    @Output() LikeCommentEvent = new EventEmitter();

    NewsFeedSvc: NewsFeedService;
    CustomFormatterSvc: CustomFormatterService;
   
    constructor(newsFeedService: NewsFeedService, customFormatter: CustomFormatterService) {

        //set the properties
        this.NewsFeedSvc = newsFeedService;

        this.CustomFormatterSvc = customFormatter;
    };

    LikeComment(commentId: number) {
        //pass this back to the parent component.
        this.LikeCommentEvent.emit(commentId);
    }

}