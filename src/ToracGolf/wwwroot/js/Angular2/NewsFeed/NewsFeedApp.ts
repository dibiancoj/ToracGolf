import {Component, Inject, NgZone} from 'angular2/core';
import {NewsFeedService, NewsFeedItem} from './NewsFeedService';

@Component({
    selector: 'NewsFeedPostContainer',
    templateUrl: 'js/Angular2/NewsFeed/NewsFeedPostView.html',
    bindings: [NewsFeedService]
})
export class NewsFeedApp {

    NewsFeedSvc: NewsFeedService;
    NgZoneSvc: NgZone;

    constructor(newsFeedService: NewsFeedService, ngZone: NgZone) {
        this.NewsFeedSvc = newsFeedService;
        this.NgZoneSvc = ngZone;

        var t = this;

        setTimeout(function () {

            //run outside of the angular zone for performance
            t.NgZoneSvc.runOutsideAngular(() => {

                //we have some data (get this from ajax return 
                var data = [{ Month: 'January', Day: 5 }, { Month: 'Feb', Day: 10 }];

                //now re-enter the zone so we can update the ui
                t.NgZoneSvc.run(() => {

                    data.forEach(x => t.NewsFeedSvc.NewsFeeds.push(x));
                });

            });
        }, 3000);
    };

    //Test() {
       
    //    this.NewsFeedSvc.NewsFeeds.push({ Month: 'January', Day: 5 });
    //};

}