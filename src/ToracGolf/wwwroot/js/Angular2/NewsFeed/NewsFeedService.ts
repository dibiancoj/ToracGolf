import { Injectable } from 'angular2/core';
import { Http, Headers, Response } from 'angular2/http';
//import 'rxjs/operators/map';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class NewsFeedService {

    constructor(http: Http) {
        this.HttpModule = http;
    }

    HttpModule: Http;

    NewFeedGet(): Observable<Response> {

        var customHeaders = new Headers();
        customHeaders.append('RequestVerificationToken', $('#__RequestVerificationToken').val());

        return this.HttpModule.post('NewsFeedsGetPost', '', { headers: customHeaders }).map(res => res.json());
    }

}

export class NewsFeedItem {
    PostDate: Date;
}