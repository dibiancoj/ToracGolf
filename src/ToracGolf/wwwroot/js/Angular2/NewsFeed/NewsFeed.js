(function (app) {
    app.AppComponent = ng.core
      .Component({
          directives: [FORM_DIRECTIVES],
          selector: 'my-app',
          template: '<h1>My First Angular 2 App {{bla}}</h1>' +
                    '<input id="txt" type="text" />' +
                    '<button (click)="testClick()">button text</button>' +
                    '<span>{{test}}</span>' +

                    '<br /><br />' + 
                    '<input [(ng-model)]="todoModel"/> + <span>{{todoModel}}'

      })
      .Class({
          constructor: function () {
              this.bla = 'Jason';
              this.test = '';
              this.todoModel = 'test';
          },

          testClick: function () {
              this.test = $('#txt').val();
              //alert(this.test);
          }
      });
})(window.app || (window.app = {}));