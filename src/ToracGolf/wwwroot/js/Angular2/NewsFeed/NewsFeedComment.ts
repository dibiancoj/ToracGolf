import {Component, Input, Inject, Output, EventEmitter} from 'angular2/core';
import {NewsFeedComment, NewsFeedTypeId} from './NewsFeedService';

@Component({
    selector: 'NewsFeedCommentItem',
    templateUrl: 'NewsFeedCommentItemClientView'
})
export class NewsFeedItemComment {

    @Input() Comment: NewsFeedComment;
    @Output() LikeCommentEvent = new EventEmitter();

    LikeComment(commentId: number) {
        //pass this back to the parent component.
        this.LikeCommentEvent.emit(commentId);
    }

}