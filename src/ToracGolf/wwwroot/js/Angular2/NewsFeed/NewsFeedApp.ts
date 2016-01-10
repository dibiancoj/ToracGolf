/// <reference path="../../../lib/jlinq/jlinq.ts" />
import {Component, Inject, NgZone} from 'angular2/core';
import {NewsFeedService, NewsFeedItem, NewsFeedQueryResult, NewsFeedTypeId} from './NewsFeedService';
import {NewsFeedItemPost} from './NewsFeedItem';
import {NgClass} from 'angular2/common';

import { Http, Response } from 'angular2/http';
import 'rxjs/add/operator/map';

@Component({
    selector: 'NewsFeedPostContainer',
    templateUrl: 'NewsFeedClientView',
    bindings: [NewsFeedService],
    directives: [NgClass, NewsFeedItemPost]
})
export class NewsFeedApp {

    NewsFeedSvc: NewsFeedService;
    NgZoneSvc: NgZone;
    Posts: Array<NewsFeedItem>;
    NewCoursePostCount: number;
    NewRoundPostCount: number;
    ActiveFeedTypeId: number;
    SearchFilterText: string;

    constructor(newsFeedService: NewsFeedService, ngZone: NgZone) {

        //set the properties
        this.NewsFeedSvc = newsFeedService;
        this.NgZoneSvc = ngZone;

        //initial property setting
        this.SearchFilterText = '';

        //go load the posts
        this.LoadPosts(null, null);
    };

    LoadPosts(newsFeedTypeId: NewsFeedTypeId, searchFilterText: string) {
        
        //closure
        var _thisClass = this;

        //go grab the data
        this.NewsFeedSvc.NewFeedGet(newsFeedTypeId, searchFilterText).subscribe((queryResult: NewsFeedQueryResult) => {

            //go run this so angular can update the new records
            _thisClass.NgZoneSvc.run(() => {

                //the pipe for date time format doesn't support iso string's right now. so flip it to a date it can handle
                queryResult.Results.forEach(x => x.PostDate = new Date(x.PostDate.toString()));

                //set the working posts
                this.Posts = queryResult.Results;

                //go set the counts
                this.NewCoursePostCount = queryResult.UnFilteredCourseCount;

                //set the count for new rounds
                this.NewRoundPostCount = queryResult.UnFilteredRoundCount;

                //what is the active nav menu
                this.ActiveFeedTypeId = newsFeedTypeId;
            });
        });
    };

    Like(likeConfig: { Id: number, NewsFeedTypeId: NewsFeedTypeId }) {
        
        //go grab the record
        var recordToUpdate = this.Posts.First(function (x) { return x.Id == likeConfig.Id && x.FeedTypeId == likeConfig.NewsFeedTypeId });

        //if you already like the item, you want to unlike the item
        var youLikeTheItemAlready = recordToUpdate.YouLikedItem;

        //closure
        var _thisClass = this;

        this.NewsFeedSvc.LikePost(likeConfig.Id, likeConfig.NewsFeedTypeId).subscribe((posts: Array<NewsFeedItem>) => {
         
            //go run this so angular can update the new records
            _thisClass.NgZoneSvc.run(() => {

                //go find the post and increment by 1 (instead of reloading the data)
                if (youLikeTheItemAlready) {
                    recordToUpdate.LikeCount--;
                }
                else {
                    recordToUpdate.LikeCount++;
                }

                //set that you liked it
                recordToUpdate.YouLikedItem = !youLikeTheItemAlready;
            });
        });
    };

    FilterByType(newsFeedTypeId: number, element: any) {

        //set the active news feed type id
        this.ActiveFeedTypeId = newsFeedTypeId;
       
        //go reload the posts
        this.LoadPosts(newsFeedTypeId, this.SearchFilterText);
    };

    ReRunSearch() {
        //they want to filter backed on text box
        this.LoadPosts(this.ActiveFeedTypeId, this.SearchFilterText);
    }

}