export class NewsFeedService {

    constructor() {
        this.NewsFeeds = [];

        this.NewsFeeds.push({ Month: 'January', Day: 5 }, { Month: 'December', Day: 10 });
    }

    NewsFeeds: Array<NewsFeedItem>;

    NewFeedGet(): Array<NewsFeedItem> {
        return this.NewsFeeds;
    }

}

export class NewsFeedItem {
    Month: string;
    Day: number;
}