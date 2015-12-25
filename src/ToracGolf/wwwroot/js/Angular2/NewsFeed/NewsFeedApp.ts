import {Component, Inject} from 'angular2/core';
import {NewsFeedService} from './NewsFeedService';

@Component({
    selector: 'my-app',
    template: '<h1>Jason</h1>',
    bindings: [NewsFeedService]
})
export class NewsFeedApp {

    service: NewsFeedService;

    constructor(newsFeedService: NewsFeedService) {
        this.service = newsFeedService;
    }

}