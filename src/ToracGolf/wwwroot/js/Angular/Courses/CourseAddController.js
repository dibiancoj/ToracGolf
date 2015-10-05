(function () {

    appToracGolf.controller('CourseAddController', ['$scope', 'ValidationService', 'FileReader', 'CourseHttp', function ($scope, ValidationService, FileReader, CourseHttp) {

        $scope.init = function (courseAddModel) {

            $scope.model = courseAddModel;
            $scope.ViewMode = 'AddCourse';

            //$scope.ShowSavedSuccessfulModal = true;
        },

        $scope.SaveACourse = function () {

            //let's go grab the image
            FileReader.ReadAsDataURL(document.getElementById('CoursePicture').files[0], $scope)
                      .then(function (result) {

                          //set the course image
                          $scope.model.CourseImage = result;

                          //go make the http call using the service
                          CourseHttp.AddCourse($scope.model, ValidationService)
                            .then(function (response) {
                                //go show the save dialog
                                $scope.ShowSavedSuccessfulModal = true;
                            },
                            function (errResponse) {
                                ValidationService.ShowValidationErrors($scope, errResponse);
                            });
                      });
        },

        //event when the user clicks ok on the dialog
        $scope.SaveACourseDialogOkEvent = function () {
            //we are all set...course has been saved, push them over to the home page
            window.location.href = 'ViewCourses';
        },

        $scope.AddTeeLocation = function () {

            //set the tee location title
            $scope.TeeLocationTitle = 'Add A New Tee Location';

            //pass in a new tee location
            $scope.InitTeeLocationView(null, {
                Description: '',
                Front9Par: null,
                Back9Par: null,
                Yardage: null,
                Slope: null,
                Rating: null,
                Par: function () {
                    return parseInt(this.Front9Par) + parseInt(this.Back9Par);
                }
            });
        },

        $scope.EditTeeLocation = function (index) {

            //set the tee location title
            $scope.TeeLocationTitle = 'Edit Tee Location';

            //we are going to not clone the object.

            //grab the tee location and pass it in
            $scope.InitTeeLocationView(index, $scope.model.TeeLocations[index]);
        },

        $scope.InitTeeLocationView = function (indexToUpdateOrAdd, teeLocationToAddOrEdit) {

            //clear out the validation errors
            $scope.validationErrors = [];

            //set the index 
            $scope.TempTeeBeforeModifying = { Index: indexToUpdateOrAdd, Model: angular.copy(teeLocationToAddOrEdit) };

            //set the edit mode
            $scope.ViewMode = 'EditTeeLocation';

            //add a new tee location model
            $scope.TempTeeLocation = angular.copy(teeLocationToAddOrEdit);

            //we are going to remove this item from tee location list...this way it makes validation a tad easier
            if (indexToUpdateOrAdd != null) {

                //we are in edit mode
                $scope.model.TeeLocations.splice(indexToUpdateOrAdd, 1); //at the index, remove 1 element (the element we are editing)
            }
        },

        $scope.CancelTeeLocation = function () {

            //so we are ok with this tee location, clear out the validation errors
            $scope.validationErrors = [];

            //if we are in edit mode, then put the guy we started with back into the collection
            if ($scope.TempTeeBeforeModifying.Index != null) {

                //we are in edit, put this tee box back
                $scope.model.TeeLocations.splice($scope.TempTeeBeforeModifying.Index, 0, $scope.TempTeeBeforeModifying.Model);
            }

            //switch the view mode
            $scope.ViewMode = 'AddCourse';
        },

        $scope.SaveTeeLocation = function () {

            //let's just make sure the name is unique
            if ($scope.model.TeeLocations.Any(function (x) { return x.Description === $scope.TempTeeLocation.Description; })) {

                //we have 1 with the name name, let's throw up an error messsage
                ValidationService.ShowValidationErrors($scope, { data: 'There is already a tee box with the same name' });

                //exit the method
                return;
            }

            //go make the http call using the service
            CourseHttp.AddTeeLocation($scope.TempTeeLocation, ValidationService)
                    .then(function (response) {

                        //add this to the collection of tee boxes
                        $scope.model.TeeLocations.push($scope.TempTeeLocation);

                        //set the view mode
                        $scope.ViewMode = 'AddCourse';

                        //so we are ok with this tee location, clear out the validation errors
                        $scope.validationErrors = [];

                    }, function (errResponse) {

                        //we have an error, so raise the validation error and show it
                        ValidationService.ShowValidationErrors($scope, errResponse);
                    });
        },

        $scope.DeleteTeeLocation = function (index) {

            //remove the guy from the collection
            $scope.model.TeeLocations.splice(index, 1);
        },

        $scope.DefaultTeeButton = function (event) {

            //if they hit the enter key then trigger the save button
            if ((event.which && event.which == 13) || (event.keyCode && event.keyCode == 13)) {
                $scope.SaveTeeLocation();
            }
        }

    }]);

})();