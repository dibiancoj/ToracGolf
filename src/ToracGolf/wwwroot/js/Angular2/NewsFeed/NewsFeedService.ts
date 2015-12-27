import { Injectable } from 'angular2/core';
import { Http, Response } from 'angular2/http';
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
        return this.HttpModule.get('NewsFeedsGetPost').map(res => res.json());
    }

}

export class NewsFeedItem {
    PostDate: Date;
}