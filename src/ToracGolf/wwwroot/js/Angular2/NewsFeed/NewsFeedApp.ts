﻿/// <reference path="../../../lib/jlinq/jlinq.ts" />
import {Component, Inject, NgZone} from 'angular2/core';
import {NewsFeedService, NewsFeedItem, NewsFeedTypeId} from './NewsFeedService';
import {NgClass} from 'angular2/common';

import { Http, Response } from 'angular2/http';
import 'rxjs/add/operator/map';

@Component({
    selector: 'NewsFeedPostContainer',
    templateUrl: 'NewsFeedClientView',
    bindings: [NewsFeedService],
    directives: [NgClass]
})
export class NewsFeedApp {

    NewsFeedSvc: NewsFeedService;
    NgZoneSvc: NgZone;
    Posts: Array<NewsFeedItem>;
    NewCoursePostCount: number;
    NewRoundPostCount: number;
    ActiveFeedTypeId: number;

    constructor(newsFeedService: NewsFeedService, ngZone: NgZone) {

        //set the properties
        this.NewsFeedSvc = newsFeedService;
        this.NgZoneSvc = ngZone;

        //go load the posts
        this.LoadPosts(null, true);
    };

    LoadPosts(newsFeedTypeId: NewsFeedTypeId, setPostCount: boolean) {

        //closure
        var _thisClass = this;

        var resetPostCount = setPostCount;

        //go grab the data
        this.NewsFeedSvc.NewFeedGet(newsFeedTypeId).subscribe((posts: Array<NewsFeedItem>) => {

            //go run this so angular can update the new records
            _thisClass.NgZoneSvc.run(() => {

                //the pipe for date time format doesn't support iso string's right now. so flip it to a date it can handle
                posts.forEach(x => x.PostDate = new Date(x.PostDate.toString()));

                //set the working posts
                this.Posts = posts;

                //set the counts in the nav menu?
                if (resetPostCount) {

                    //go set the counts
                    this.NewCoursePostCount = this.Posts.Count(x => x.FeedTypeId == NewsFeedTypeId.NewCourse);

                    //set the count for new rounds
                    this.NewRoundPostCount = this.Posts.Count(x => x.FeedTypeId == NewsFeedTypeId.NewRound);
                }

                //what is the active nav menu
                this.ActiveFeedTypeId = newsFeedTypeId;
            });
        });
    };

    Like(id: number, newsFeedTypeId: NewsFeedTypeId) {

        //go grab the record
        var recordToUpdate = this.Posts.First(function (x) { return x.Id == id && x.FeedTypeId == newsFeedTypeId });

        //if you already like the item, you want to unlike the item
        var youLikeTheItemAlready = recordToUpdate.YouLikedItem;

        //closure
        var _thisClass = this;

        this.NewsFeedSvc.LikePost(id, newsFeedTypeId).subscribe((posts: Array<NewsFeedItem>) => {
         
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
        this.LoadPosts(newsFeedTypeId, false);
    };

}