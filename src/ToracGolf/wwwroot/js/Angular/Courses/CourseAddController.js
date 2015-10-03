(function () {

    appToracGolf.controller('CourseAddController', ['$scope', '$http', 'ValidationFactory', function ($scope, $http, ValidationFactory) {

        //set the default http headers, so we don't need to keep setting it everytime we make an ajax call
        $http.defaults.headers.common.RequestVerificationToken = $('[name=__RequestVerificationToken]').val();

        $scope.init = function (courseAddModel) {

            $scope.model = courseAddModel;
            $scope.ViewMode = 'AddCourse';

            //$scope.ShowSavedSuccessfulModal = true;
        },

        $scope.SaveACourse = function () {

            //let's go grab the upload file and put it into the model
            var file = document.getElementById('CoursePicture').files[0]; //Files[0] = 1st file

            //do they have a file
            if (file == null) {

                //no file, go save the rest
                $scope.SaveACourseToServer(null);
            }
            else {

                //we have a file, go read it
                var reader = new FileReader();

                //create the event so when it's done we can go save the record (file reader is async)
                reader.onload = function (e) {
                    $scope.SaveACourseToServer(e.target.result);
                }

                //go read the file now
                reader.readAsDataURL(file);
            }
        },

        $scope.SaveACourseToServer = function (imageData) {

            //set the image data on the model
            $scope.model.CourseImage = imageData;

            //go to the server and try to save this
            $http.post('AddACourse', $scope.model, ValidationFactory)
               .then(function (response) {

                   //go show the save dialog
                   $scope.ShowSavedSuccessfulModal = true;

               }, function (response) {

                   ValidationFactory.ShowValidationErrors($scope, response);

               });
        }

        //event when the user clicks ok on the dialog
        $scope.SaveACourseDialogOkEvent = function () {
            //we are all set...course has been saved, push them over to the home page
            window.location.href = '/jason';
        },

        $scope.AddTeeLocation = function () {

            //pass in a new tee location
            $scope.InitTeeLocationView(null, {
                Description: '',
                Front9Par: 0,
                Back9Par: 0,
                Yardage: 0,
                Slope: 0,
                Rating: 0,
                Par: function () {
                    return parseInt(this.Front9Par) + parseInt(this.Back9Par);
                }
            });
        },

        $scope.EditTeeLocation = function (index) {

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
                ValidationFactory.ShowValidationErrors($scope, { data: 'There is already a tee box with the same name' });

                //exit the method
                return;
            }

            //let's do a full validation now on the server
            $http.post('ValidateTeeLocation', $scope.TempTeeLocation, ValidationFactory)
                .then(function (response) {

                    //add this to the collection of tee boxes
                    $scope.model.TeeLocations.push($scope.TempTeeLocation);

                    //set the view mode
                    $scope.ViewMode = 'AddCourse';

                    //so we are ok with this tee location, clear out the validation errors
                    $scope.validationErrors = [];

                }, function (response) {

                    //we have an error, so raise the validation error and show it
                    ValidationFactory.ShowValidationErrors($scope, response);
                });
        },

        $scope.DeleteTeeLocation = function (index) {

            //remove the guy from the collection
            $scope.model.TeeLocations.splice(index, 1);
        }

    }]);

})();