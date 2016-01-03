import {Component, Inject, NgZone} from 'angular2/core';
import {NewsFeedService, NewsFeedItem, NewsFeedTypeId} from './NewsFeedService';

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

        //set the properties
        this.NewsFeedSvc = newsFeedService;
        this.NgZoneSvc = ngZone;

        //closure
        var _thisClass = this;

        //go grab the data
        this.NewsFeedSvc.NewFeedGet().subscribe((posts: Array<NewsFeedItem>) => {

            //go run this so angular can update the new records
            _thisClass.NgZoneSvc.run(() => {

                //the pipe for date time format doesn't support iso string's right now. so flip it to a date it can handle
                posts.forEach(x => x.PostDate = new Date(x.PostDate.toString()));

                this.Posts = posts;
            });
        });
    };

    Like(id: number, newsFeedTypeId: NewsFeedTypeId) {
        
        alert('need to make sure they havent already liked it');

        //closure 
        var _thisClass = this;

        this.NewsFeedSvc.LikePost(id, newsFeedTypeId).subscribe((posts: Array<NewsFeedItem>) => {
            debugger;
            //go run this so angular can update the new records
            _thisClass.NgZoneSvc.run(() => {

                debugger;
            });
        });
    };

}