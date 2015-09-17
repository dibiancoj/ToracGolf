(function () {

    appToracGolf.controller('CourseAddController', ['$scope', '$http', 'ValidationFactory', function ($scope, $http, ValidationFactory) {

        $scope.init = function (courseAddModel) {
            $scope.model = courseAddModel;
        },

        $scope.processForm = function () {

            var token = $('[name=__RequestVerificationToken]').val();

            var config = {
                headers: {
                    'RequestVerificationToken': token
                }
            };

            $http.post('AddACourse', $scope.model, config, ValidationFactory)
               .then(function (response) {
                   debugger;
                   var s = response;

                   $scope.model.Location = 'teststest';



               }, function (response) {

                   ValidationFactory.ShowValidationErrors($scope, response);


               });
        }

        //$scope.showValidationErrors = function ($scope, error) {
        //    $scope.validationErrors = [];
        //    if (error.data && angular.isObject(error.data)) {
        //        for (var key in error.data) {
        //            $scope.validationErrors.push(error.data[key][0]);
        //        }
        //    } else {
        //        $scope.validationErrors.push('Could not add the course.');
        //    };
        //}

    }]);

})();