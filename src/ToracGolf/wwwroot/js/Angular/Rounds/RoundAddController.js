(function () {

    appToracGolf.controller('RoundAddController', ['$scope', 'ValidationService', 'RoundHttp', '$filter', function ($scope, ValidationService, RoundHttp, $filter) {

        $scope.init = function (roundAddModel) {

            //set the model
            $scope.model = roundAddModel;
        },

        $scope.$watch('model.RoundDate', function (newValue) {
            $scope.model.RoundDate = $filter('date')(newValue, 'MM/dd/yyyy');
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
             },
             function (errResponse) {
                 alert('Error Calling FetchCoursesForState');
             });
        }

    }]);

})();