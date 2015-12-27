import { Injectable, Inject } from 'angular2/core';
import { Http, Headers, Response } from 'angular2/http';

import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class HttpInterceptor {

    constructor(http: Http) {
        this.HttpModule = http;
    }

    private HttpModule: Http;

    Post(url: string, body: string): Observable<Response> {
        return this.HttpModule.post(url, body, { headers: this.CustomHeaderSelect() }).map(res => res.json());
    }

    private CustomHeaderSelect() {
        var customHeaders = new Headers();
        customHeaders.append('RequestVerificationToken', $('#__RequestVerificationToken').val());

        return customHeaders;
    }

}