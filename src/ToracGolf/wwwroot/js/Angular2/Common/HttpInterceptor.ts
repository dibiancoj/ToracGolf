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

        //show the spinner
        this.ShowAjaxSpinner(true);

        //grab the query so we can fork this
        var ajaxCall = this.HttpModule.post(url, body, { headers: this.CustomHeaderSelect() })
            .map(res => res.json())

        //fork this so we can remove the ajax waiting panel
        ajaxCall.subscribe(res => this.ShowAjaxSpinner(false),
            err => this.ErrorHandler(err));

        //return the ajax call
        return ajaxCall;
    }

    private ErrorHandler(err) {
        alert('Ajax Angular 2 Error');
        alert(JSON.stringify(err));
    }

    private ShowAjaxSpinner(showSpinner: boolean) {
        if (showSpinner) {
            $("#loadingDiv").show();
        }
        else {
            $("#loadingDiv").hide();
        }
    }

    private CustomHeaderSelect() {
        var customHeaders = new Headers();
        customHeaders.append('RequestVerificationToken', $('#__RequestVerificationToken').val());
        customHeaders.append('Content-Type', 'application/json');

        return customHeaders;
    }

}