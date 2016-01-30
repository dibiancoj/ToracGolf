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

    DateTimeHack(commentDate: string): string {
        //compensate for angular 2 not handling iso string dates

        var dateToUse = new Date(commentDate);

        //grab the hour
        var hour: number = dateToUse.getHours();
        var pmOrAm: string;

        if (hour > 12) {
            pmOrAm = 'PM';
            hour = hour - 12;
        }
        else {
            pmOrAm = 'AM';
        }

        //minutes
        var minutes: string = dateToUse.getMinutes().toString();

        if (minutes.length == 1) {
            minutes = '0' + minutes;
        }

        return (dateToUse.getMonth() + 1) + "/" + dateToUse.getDate() + "/" + dateToUse.getFullYear() + "  " +
            hour + ":" + minutes + ' ' + pmOrAm;
    }

}