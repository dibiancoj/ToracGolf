import { Injectable } from 'angular2/core';
import { Http, Headers, Response } from 'angular2/http';
//import 'rxjs/operators/map';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { HttpInterceptor } from '../Common/httpinterceptor';

@Injectable()
export class NewsFeedService {

    constructor(httpInterceptor: HttpInterceptor) {
        this.HttpInterceptorSvc = httpInterceptor;
    }

    HttpInterceptorSvc: HttpInterceptor;

    NewFeedGet(newsFeedTypeId: NewsFeedTypeId, searchFilterText: string): Observable<Response> {
        return this.HttpInterceptorSvc.Post('NewsFeedsGetPost', JSON.stringify({ NewsFeedTypeIdFilter: newsFeedTypeId, SearchFilterText: searchFilterText }));
    }

    LikePost(id: number, newsFeedTypeId: NewsFeedTypeId): Observable<Response> {
        return this.HttpInterceptorSvc.Post('NewsFeedsLike', JSON.stringify({ Id: id, NewsFeedTypeId: newsFeedTypeId }));
    }

    NewsFeedCommentSave(id: number, newsFeedTypeId: NewsFeedTypeId, commentToAdd: string): Observable<Response> {
        return this.HttpInterceptorSvc.Post('NewsFeedCommentSave', JSON.stringify({ Id: id, NewsFeedTypeId: newsFeedTypeId, CommentToAdd: commentToAdd }));
    }

    NewsFeedCommentSelect(id: number, newsFeedTypeId: NewsFeedTypeId): Observable<Response> {
        return this.HttpInterceptorSvc.Post('NewsFeedsCommentSelect', JSON.stringify({ Id: id, NewsFeedTypeId: newsFeedTypeId }));
    }

    CommentLikeClick(commentId: number): Observable<Response> {
        return this.HttpInterceptorSvc.Post('NewsFeedsCommentLike', JSON.stringify({ CommentId: commentId }));
    }

}

export class NewsFeedQueryResult {
    Results: NewsFeedItem[];
    UnFilteredRoundCount: number;
    UnFilteredCourseCount: number;
}

export class NewsFeedItem {
    Id: number;
    FeedTypeId: NewsFeedTypeId;
    PostDate: Date;
    CommentCount: number;
    LikeCount: number;
    YouLikedItem: boolean;
    Comments: Array<NewsFeedComment>;
}

export class NewsFeedComment {
    CommentId: number;
    User: string;
    CommentText: string;
    NumberOfLikes: number;
    //UserLikesThisComment: boolean;
}

export enum NewsFeedTypeId {
    NewRound,
    NewCourse,
    FeedTypeId
}