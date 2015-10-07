(function () {
    'use strict';

    appToracGolf.directive('select2', function () {

        //<select id="CourseSelect" class="Select2DropDown" select2   <-- that attribute

        return {
            require: '?ngModel', //this way we can push stuff to the model
            restrict: 'A',
            scope: {
                ngModel: '=' // set up bi-directional binding between a local scope property and the parent scope property 
            },
            link: function (scope, element, attrs, ngModelCtrl) {

                scope.$watch('ngModel', function (newValue) {

                    //do we have a value?
                    if (newValue != null) {
                        scope.$$postDigest(function () {
                            //should be 'number:38' (38 is the value to set)
                            element.val(typeof scope.ngModel + ':' + newValue).trigger('change');
                        });
                    }
                });

                element.select2({
                    placeholder: "Select a state"
                });
            }

        }
    })

})();