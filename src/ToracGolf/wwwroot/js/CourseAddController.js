(function () {

    var app = angular.module('toracApp', []);

    app.controller('CourseAddController', function ($scope, $http) {

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

            //$scope.model 

            $http.post('AddACourse', $scope.model, config)
               .then(function (response) {
                   debugger;
                   var s = response;

                   $scope.model.Location = 'teststest';

                   // this callback will be called asynchronously
                   // when the response is available
               }, function (response) {
      
                   $scope.showValidationErrors($scope, response);
                   // called asynchronously if an error occurs
                   // or server returns response with an error status.
               });
        }

        $scope.showValidationErrors = function ($scope, error) {
            $scope.validationErrors = [];
            if (error.data && angular.isObject(error.data)) {
                for (var key in error.data) {
                    $scope.validationErrors.push(error.data[key][0]);
                }
            } else {
                $scope.validationErrors.push('Could not add the course.');
            };
        }

    });

})();