(function () {

    appToracGolf.controller('CourseAddController', ['$scope', '$http', 'ValidationFactory', function ($scope, $http, ValidationFactory) {

        $scope.init = function (courseAddModel) {
            $scope.model = courseAddModel;
            $scope.ViewMode = 'AddCourse';
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

                   var s = response;

                   //$scope.model.Location = 'teststest';



               }, function (response) {

                   ValidationFactory.ShowValidationErrors($scope, response);

               });
        },

        $scope.EditTeeLocation = function () {
            $scope.ViewMode = 'EditTeeLocation';

            //add a new tee location model
            $scope.TempTeeLocation = {
                Description: '',
                Front9Par: 0,
                Back9Par: 0,
                TotalYardage: 0,
                Slope: 0,
                Rating: 0
            };
        },

        $scope.CancelTeeLocation = function () {
            $scope.ViewMode = 'AddCourse';
        },

        $scope.SaveTeeLocation = function () {

            //if we have 0 tee locations create a new array
            if ($scope.model.TeeLocations == null) {
                $scope.model.TeeLocations = [];
            }

            //put the temp tee location into the main model
            $scope.model.TeeLocations.push($scope.TempTeeLocation);

            $scope.ViewMode = 'AddCourse';
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