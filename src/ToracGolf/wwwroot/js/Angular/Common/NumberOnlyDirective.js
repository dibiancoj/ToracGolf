(function () {
    'use strict';

    appToracGolf.directive('numbersOnly', function () {

        //<input type="text" numbers-only numbers-only-allowOnlyInts="false" name="Total Yardage" ng-model="TempTeeLocation.Yardage" ng-keyup="DefaultTeeButton($event)" maxlength="4" />

        return {
            require: 'ngModel', //this way we can push stuff to the model
            restrict: 'A',
            link: function (scope, element, attrs, modelCtrl) {

                scope.allowInts = attrs.onlyint;

                //when we push a value
                modelCtrl.$parsers.push(function (inputValue) {

                    // this next if is necessary for when using ng-required on your input. 
                    // In such cases, when a letter is typed first, this parser will be called
                    // again, and the 2nd time, the value will be undefined
                    if (inputValue == undefined) {
                        return ''
                    }

                    NEED TO FIGURE OUT HOW TO GRAB THE ATTRIBUTE numbers-only-allowOnlyInts and use it below to remove the '.' in teh regex expression

                    //this will strip out anything that we don't accept. so if user has 0t...it will remove the t
                    var transformedInput = inputValue.replace(/[^0-9.]/g, '');

                    //make sure it's a number
                    if (isNaN(transformedInput)) {

                        //rollback to the last commited value
                        transformedInput = modelCtrl.$modelValue;
                    }

                    //is the value the same as whats on the text field
                    if (transformedInput != inputValue) {

                        //we need to set the view value
                        modelCtrl.$setViewValue(transformedInput);

                        //go render that change
                        modelCtrl.$render();
                    }

                    //return the transformed input value
                    return transformedInput;
                });
            },

        }

    })

})();