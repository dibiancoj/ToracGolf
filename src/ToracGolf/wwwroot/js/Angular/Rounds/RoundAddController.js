(function () {

    appToracGolf.controller('RoundAddController', ['$scope', 'ValidationService', 'RoundHttp', function ($scope, ValidationService, RoundHttp) {

        $scope.init = function (roundAddModel) {

            //set the model
            $scope.model = roundAddModel;
        },

        $scope.SaveARound = function () {

            //go make the http call using the service
            CourseHttp.AddRound($scope.model, ValidationService)
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

        $scope.SaveRound = function () {
            $scope.model.RoundDate = '10/7/2015';
        }

    }]);

})();