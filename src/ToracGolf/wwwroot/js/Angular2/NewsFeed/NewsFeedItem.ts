import {Component, Input, Inject, Output, EventEmitter} from 'angular2/core';
import {NewsFeedItem, NewsFeedTypeId} from './NewsFeedService';

@Component({
    selector: 'NewsFeedPostItem',
    templateUrl: 'NewsFeedItemClientView'
})
export class NewsFeedItemPost {

    @Input() Post: NewsFeedItem; //declared on NewsFeedPostClientView. it's the attribute name where we pass in the object
    @Output() LikeEvent = new EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.
    @Output() CommentSaveEvent = new EventEmitter(); //declared on NewsFeedPostClientView. its the event we bind with the parent component.

    LikeClick(id: number, newsFeedTypeId: NewsFeedTypeId) {
    
        //pass this back to the parent component.
        this.LikeEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
    }

    SaveComment(id: number, newsFeedTypeId: NewsFeedTypeId, comment: string) {
        
        //pass this back to the parent component
        this.CommentSaveEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId, Comment: comment });
    }
}