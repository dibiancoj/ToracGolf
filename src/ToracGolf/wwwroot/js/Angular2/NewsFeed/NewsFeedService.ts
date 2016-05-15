import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
//import 'rxjs/operators/map';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { HttpInterceptor } from '../Common/httpinterceptor';

@Injectable()
export class NewsFeedService {

    constructor(httpInterceptor: HttpInterceptor) {
        this.HttpInterceptorSvc = httpInterceptor;

        //build the month name array
        this.MonthNameArray = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
    }

    private HttpInterceptorSvc: HttpInterceptor;
    private MonthNameArray: string[];

    NewFeedGet<T>(newsFeedTypeId: NewsFeedTypeId, searchFilterText: string): Observable<T> {
        return this.HttpInterceptorSvc.Post<T>('NewsFeedsGetPost', JSON.stringify({ NewsFeedTypeIdFilter: newsFeedTypeId, SearchFilterText: searchFilterText }));
    }

    LikePost<T>(id: number, newsFeedTypeId: NewsFeedTypeId): Observable<T> {
        return this.HttpInterceptorSvc.Post<T>('NewsFeedsLike', JSON.stringify({ Id: id, NewsFeedTypeId: newsFeedTypeId }));
    }

    NewsFeedCommentSave<T>(id: number, newsFeedTypeId: NewsFeedTypeId, commentToAdd: string): Observable<T> {
        return this.HttpInterceptorSvc.Post<T>('NewsFeedCommentSave', JSON.stringify({ Id: id, NewsFeedTypeId: newsFeedTypeId, CommentToAdd: commentToAdd }));
    }

    NewsFeedCommentSelect<T>(id: number, newsFeedTypeId: NewsFeedTypeId): Observable<T> {
        return this.HttpInterceptorSvc.Post<T>('NewsFeedsCommentSelect', JSON.stringify({ Id: id, NewsFeedTypeId: newsFeedTypeId }));
    }

    CommentLikeClick<T>(commentId: number): Observable<T> {
        return this.HttpInterceptorSvc.Post<T>('NewsFeedsCommentLike', JSON.stringify({ CommentId: commentId }));
    }

}

export class NewsFeedQueryResult {
    Results: NewsFeedItem[];
    UnFilteredRoundCount: number;
    UnFilteredCourseCount: number;
    FriendCount: number;
}

export class NewsFeedItem {
    Id: number;
    FeedTypeId: NewsFeedTypeId;
    PostDate: Date;
    CommentCount: number;
    LikeCount: number;
    YouLikedItem: boolean;
    CourseId: number;
    Comments: Array<NewsFeedComment>;
}

export class NewsFeedComment {
    CommentId: number;
    User: string;
    CommentText: string;
    NumberOfLikes: number;
    CommentDate: Date;
    //UserLikesThisComment: boolean;
}

export enum NewsFeedTypeId {
    NewRound,
    NewCourse,
    FeedTypeId
}