(function (app) {

    var PostSvc = function PostService() {
        this.Items = [{ Month: 'Jan', Day: 5 }, { Month: 'Feb', Day: 6 }];
    };

    var postService = PostSvc();

    app.NewsFeedPage = ng.core
    .Component({
        selector: 'NewsFeed',
        templateUrl: 'js/Angular2/NewsFeed/NewsFeedPostView.html',
        //viewInjector: PostService //<-- pass in the service 
        //directives: [FooComponent, BarComponent] --> then i can use other compon by doing <FooComponent>
    }).Class({
        constructor: function () {

            debugger;
            this.test = '';
            this.Posts = postService; //[{ Month: 'Jan', Day: 5 }, { Month: 'Feb', Day: 6 }];
        },
        AddItem(item) {
            this.Posts.push(item);
        }
    });

    //app.NewsFeedPage.parameters = [[PostService]];

})(window.app || (window.app = {}));


//setTimeout(function () {

//    new app.NewsFeedPage().AddItem({ Month: 'Dec', Day: 12 });
//}, 2000);