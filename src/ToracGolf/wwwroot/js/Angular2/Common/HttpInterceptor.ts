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

    Post<T>(url: string, body: string): Observable<T> {

        //show the spinner
        this.ShowAjaxSpinner(true);

        //grab the query so we can fork this (i'm hooking into the map to hide the show dialog)...using multiple subscribe caused 2 ajax calls to be made.
        //now sure if this is the best way to do this. It works though. Couldn't find much documentation on how to do this. Basically fork and run multiple methods
        //off an observable. 
        return this.HttpModule.post(url, body, { headers: this.CustomHeaderSelect() })
            .map(res => res.json())
            .map(x => {
                this.ShowAjaxSpinner(false);
                return x;
            });
    }

    //private ErrorHandler(err) {
    //    alert('Ajax Angular 2 Error');
    //    alert(JSON.stringify(err));
    //}

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