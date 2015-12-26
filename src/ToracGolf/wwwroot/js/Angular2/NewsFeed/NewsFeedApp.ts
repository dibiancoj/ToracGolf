import {Component, Inject} from 'angular2/core';
import {NewsFeedService, NewsFeedItem} from './NewsFeedService';

@Component({
    selector: 'NewsFeedPostContainer',
    templateUrl: 'js/Angular2/NewsFeed/NewsFeedPostView.html',
    bindings: [NewsFeedService]
})
export class NewsFeedApp {

    //Posts: Array<NewsFeedItem>;
    NewsFeedSvc: NewsFeedService;
    Posts: Array<NewsFeedItem>;

    constructor(newsFeedService: NewsFeedService) {
        this.NewsFeedSvc = newsFeedService;
        this.Posts = this.NewsFeedSvc.NewsFeeds;

       // var postClosure = this.Posts;

        //setTimeout(function () {
        //    debugger;
        //    postClosure.push({ Month: 'January', Day: 5 });
        //}, 4000);
    }

}