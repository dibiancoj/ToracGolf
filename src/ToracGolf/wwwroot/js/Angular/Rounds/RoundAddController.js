(function () {

    appToracGolf.controller('RoundAddController', ['$scope', 'ValidationService', 'RoundHttp', '$filter', function ($scope, ValidationService, RoundHttp, $filter) {

        $scope.init = function (roundAddModel) {

            //set the model
            $scope.model = roundAddModel;

            //go grab the course for the selected states
            $scope.FetchCoursesForState();
        },

        $scope.$watch('model.RoundDate', function (newValue) {
            $scope.model.RoundDate = $filter('date')(newValue, 'MM/dd/yyyy');
        });

        $scope.$watch('model.TeeLocationId', function (newValue) {

            //go set the yardage,par, scope, rating
   
            //grab the selected tee box
            var selectedTeeBox = null;

            //if we have a new value, then go try to find it
            if (newValue != null && $scope.TeeBoxLookup != null) {
                selectedTeeBox = $scope.TeeBoxLookup.FirstOrDefault(function (x) { return x.CourseTeeLocationId == newValue });
            }

            //func to select the items
            var selector = function (selectedTee, propertyToReturn) {
                if (selectedTee == null) {
                    return null;
                }
                else {
                    return selectedTee[propertyToReturn];
                }
            }

            $scope.TeeBoxSelectedInfo = {
                Yardage: selector(selectedTeeBox, 'Yardage'),
                Par: selectedTeeBox == null ? null : selectedTeeBox.Front9Par + selectedTeeBox.Back9Par,
                Slope: selector(selectedTeeBox, 'Slope'),
                Rating: selector(selectedTeeBox, 'Rating'),
                MaxScorePerHole: selector(selectedTeeBox, 'MaxScorePerHole'),
                CourseTeeBoxHandicap: selector(selectedTeeBox, 'CourseTeeBoxHandicap')
            };
        });

        $scope.SaveARound = function () {

            //go make the http call using the service
            RoundHttp.AddRound($scope.model, ValidationService)
              .then(function (response) {
                  //go show the save dialog
                  $scope.ShowSavedSuccessfulModal = true;
              },
              function (errResponse) {
                  ValidationService.ShowValidationErrors($scope, errResponse);
              });
        },

        //event when the user clicks ok on the dialog
        $scope.SaveARoundDialogOkEvent = function () {
            //we are all set...course has been saved, push them over to the home page
            window.location.href = 'ViewRounds';
        },

        $scope.FetchCoursesForState = function () {

            RoundHttp.FetchCoursesForState($scope.model.StateId)
             .then(function (response) {

                 //set the courses for this state
                 $scope.CourseLookup = response.data.CourseData;

                 //if we have records, then we want to set the course as the first course is in the list
                 if ($scope.CourseLookup != null && $scope.CourseLookup.length > 0) {

                     //set the course id to the first item in the list (just so something is selected)
                     $scope.model.CourseId = $scope.CourseLookup[0].CourseId;

                     //go grab the tee box location
                     $scope.FetchTeeBoxesForCourse();
                 }
             },
             function (errResponse) {
                 alert('Error Calling FetchCoursesForState');
             });
        },

        $scope.FetchTeeBoxesForCourse = function () {
 
            //if we change states, then we need to wait until we have a course selected
            if ($scope.model.CourseId == null)
            {
                $scope.model.TeeLocationId = null;
                $scope.TeeBoxLookup = null;

                return;
            }

            RoundHttp.FetchTeeBoxForCourseId($scope.model.CourseId)
            .then(function (response) {

                //set the tee box to the first one
                $scope.model.TeeLocationId = response.data.TeeBoxData[0].CourseTeeLocationId;

                //set the courses for this state
                $scope.TeeBoxLookup = response.data.TeeBoxData;
            },
            function (errResponse) {
                alert('Error Calling FetchTeeBoxesForCourse');
            });

        }

    }]);

})();