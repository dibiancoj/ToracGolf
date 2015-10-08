(function () {
    'use strict';

    appToracGolf.directive('datePicker', function () {

        //<input id="RoundDate" date-picker maxDate="0" name="RoundDate" maxlength="100" ng-model="model.RoundDate | date:'shortDate'" />
        //max date takes the jquery max date parameters. 0 is today. 1 is tomorrow .You can do +1m = 1 month

        return {
            require: '?ngModel', //this way we can push stuff to the model
            restrict: 'A',
            //scope: {
            //    ngModel: '=' // set up bi-directional binding between a local scope property and the parent scope property 
            //},
            link: function (scope, element, attrs, ngModelCtrl) {

                element.datepicker({
                    maxDate: attrs.maxdate,
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