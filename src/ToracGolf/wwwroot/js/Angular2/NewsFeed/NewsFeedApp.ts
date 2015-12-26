import {Component, Inject} from 'angular2/core';
import {NewsFeedService, NewsFeedItem} from './NewsFeedService';

@Component({
    selector: 'NewsFeedPostContainer',
    templateUrl: 'js/Angular2/NewsFeed/NewsFeedPostView.html',
    bindings: [NewsFeedService]
})
export class NewsFeedApp {

    NewsFeedSvc: NewsFeedService;

    constructor(newsFeedService: NewsFeedService) {
        this.NewsFeedSvc = newsFeedService;
    };

    //Test() {
       
    //    this.NewsFeedSvc.NewsFeeds.push({ Month: 'January', Day: 5 });
    //};

}