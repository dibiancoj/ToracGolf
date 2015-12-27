import {Component, Inject, NgZone} from 'angular2/core';
import {NewsFeedService, NewsFeedItem} from './NewsFeedService';

import { Http, Response } from 'angular2/http';
import 'rxjs/add/operator/map';

@Component({
    selector: 'NewsFeedPostContainer',
    templateUrl: 'js/Angular2/NewsFeed/NewsFeedPostView.html',
    bindings: [NewsFeedService]
})
export class NewsFeedApp {

    NewsFeedSvc: NewsFeedService;
    NgZoneSvc: NgZone;
    Posts: Array<NewsFeedItem>;

    constructor(newsFeedService: NewsFeedService, ngZone: NgZone) {
        this.Posts = [];
        this.NewsFeedSvc = newsFeedService;
        this.NgZoneSvc = ngZone;

        //closure
        var _thisClass = this;

        //go grab the data
        this.NewsFeedSvc.NewFeedGet().subscribe((posts: Array<NewsFeedItem>) => {

            _thisClass.NgZoneSvc.run(() => {
                debugger;
                this.Posts = posts;
            });
        });
    };

    //Test() {
       
    //    this.NewsFeedSvc.NewsFeeds.push({ Month: 'January', Day: 5 });
    //};

}