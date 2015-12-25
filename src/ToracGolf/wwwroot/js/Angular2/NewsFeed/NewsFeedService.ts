export class NewsFeedService {

    constructor() {
        this.NewsFeeds = [];

        this.NewsFeeds.push({ PostDate: new Date() });
    }

    NewsFeeds: Array<NewsFeedItem>;

    NewFeedGet(): Array<NewsFeedItem> {
        return this.NewsFeeds;
    }

}

export class NewsFeedItem {
    PostDate: Date;
}