import {Component, Input, Inject, Output, EventEmitter} from 'angular2/core';
import {NewsFeedItem, NewsFeedTypeId} from './NewsFeedService';

@Component({
    selector: 'NewsFeedPostItem',
    templateUrl: 'NewsFeedItemClientView',
    inputs: ['Post']
})
export class NewsFeedItemPost {
    Post: NewsFeedItem;

    @Output() LikeEvent = new EventEmitter();

    LikeClick(id: number, newsFeedTypeId: NewsFeedTypeId) {
        //pass this back to the parent component.
        this.LikeEvent.emit({ Id: id, NewsFeedTypeId: newsFeedTypeId });
    }
}