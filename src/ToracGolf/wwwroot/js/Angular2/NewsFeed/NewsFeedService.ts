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

    NewFeedGet(): Observable<Response> {
        return this.HttpInterceptorSvc.Post('NewsFeedsGetPost', '');
    }

}

export class NewsFeedItem {
    PostDate: Date;
    CommentCount: number;
}