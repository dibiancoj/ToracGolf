(function () {
    'use strict';

    appToracGolf.directive('datePicker', function () {

        //<input id="RoundDate" date-picker name="RoundDate" maxlength="100" ng-model="model.RoundDate | date:'shortDate'" />

        return {
            require: '?ngModel', //this way we can push stuff to the model
            restrict: 'A',
            //scope: {
            //    ngModel: '=' // set up bi-directional binding between a local scope property and the parent scope property 
            //},
            link: function (scope, element, attrs, ngModelCtrl) {

                element.datepicker({
                    onSelect: function (dateText) {

                        scope.$apply(function () {

                            scope.model.RoundDate = dateText;

                        });
                    }
                });
            }

        }
    })

})();