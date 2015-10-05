(function () {
    'use strict';

    appToracGolf.directive('numbersOnly', function () {

        //<input type="text" numbers-only numbers-only-allowOnlyInts="false" name="Total Yardage" ng-model="TempTeeLocation.Yardage" ng-keyup="DefaultTeeButton($event)" maxlength="4" />

        return {
            require: 'ngModel', //this way we can push stuff to the model
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs, modelCtrl) {

                function IsValidNumber(inputValue, onlyAllowInts) {

                    // this next if is necessary for when using ng-required on your input. 
                    // In such cases, when a letter is typed first, this parser will be called
                    // again, and the 2nd time, the value will be undefined
                    if (inputValue == undefined) {
                        return ''
                    }

                    //this will strip out anything that we don't accept. so if user has 0t...it will remove the t
                    var transformedInput;

                    //do they only want int's?
                    if (onlyAllowInts) {
                        transformedInput = inputValue.replace(/[^0-9]/g, '');
                    }
                    else {
                        transformedInput = inputValue.replace(/[^0-9.]/g, '');
                    }

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
                }

                //set the scope variable if we want only ints
                scope.allowInts = attrs.numbersOnlyAllowonlyints.toLowerCase() === 'true';

                //add the new parser
                modelCtrl.$parsers.push(function (inputValue) {

                    //grab the only allow ints
                    var onlyAllowInts = scope.allowInts;

                    //go set the validity
                    return IsValidNumber(inputValue, onlyAllowInts);
                });
            },
        }

    })

})();