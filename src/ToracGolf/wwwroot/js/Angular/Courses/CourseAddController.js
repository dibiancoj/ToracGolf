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

        $scope.AddTeeLocation = function () {

            //pass in a new tee location
            $scope.InitTeeLocationView({
                Description: '',
                Front9Par: 0,
                Back9Par: 0,
                TotalYardage: 0,
                Slope: 0,
                Rating: 0
            });
        },

        $scope.EditTeeLocation = function (index) {

            //grab the tee location and pass it in
            $scope.InitTeeLocationView($scope.model.TeeLocations[index]);
        },

        $scope.InitTeeLocationView = function (teeLocationToAddOrEdit) {

            //clear out the validation errors
            $scope.validationErrors = [];

            //set the edit mode
            $scope.ViewMode = 'EditTeeLocation';

            //add a new tee location model
            $scope.TempTeeLocation = teeLocationToAddOrEdit;
        },

        $scope.CancelTeeLocation = function () {

            //so we are ok with this tee location, clear out the validation errors
            $scope.validationErrors = [];

            //switch the view mode
            $scope.ViewMode = 'AddCourse';
        },

        $scope.SaveTeeLocation = function () {

            //let's just make sure the name is unique
            if ($scope.model.TeeLocations.Any(function (x) { return x.Description === $scope.TempTeeLocation.Description; })) {
                //we have 1 with the name name, let's throw up an error messsage
                ValidationFactory.ShowValidationErrors($scope, { data: 'There is already a tee box with the same name' });

                //exit the method
                return;
            }

            //put the temp tee location into the main model
            $scope.model.TeeLocations.push($scope.TempTeeLocation);

            $scope.ViewMode = 'AddCourse';

            //so we are ok with this tee location, clear out the validation errors
            $scope.validationErrors = [];
        },

        $scope.DeleteTeeLocation = function (index) {

            //remove the guy from the collection
            $scope.model.TeeLocations.splice(index, 1);
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