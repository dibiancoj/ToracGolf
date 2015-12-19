(function (app) {
    app.AppComponent = ng.core
      .Component({
          //directives: [FORM_DIRECTIVES],
          selector: 'my-app',
          //template: '<h1>My First Angular 2 App {{bla}}</h1>' +
          //          '<input id="txt" type="text" (keyup)="myControllerMethod()" />' +
          //          '<button (click)="testClick()">button text</button>' +
          //          '<span>{{test}}</span>' +

          //          '<br /><br />'
          templateUrl: 'js/Angular2/NewsFeed/Test.html'

      })
      .Class({
          constructor: function () {
              this.test = '';
          },

          testClick: function () {
              this.test = $('#txt').val() + 'jason';
              //alert(this.test);
          },

          myControllerMethod: function () {
              this.test = $('#txt').val();
          }
      });
})(window.app || (window.app = {}));