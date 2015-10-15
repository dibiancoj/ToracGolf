(function () {

    appToracGolf.controller('SeasonAddController', ['$scope', 'ValidationService', 'SeasonHttp', '$filter', function ($scope, ValidationService, SeasonHttp, $filter) {

        $scope.init = function (seasonAddModel) {

            //set the model
            $scope.model = seasonAddModel;
        },

        $scope.SaveASeason = function () {

            //go make the http call using the service
            SeasonHttp.AddSeason($scope.model, ValidationService)
              .then(function (response) {

                  //go show the save dialog
                  $scope.ShowSavedSuccessfulModal = true;
              },
              function (errResponse) {
                  ValidationService.ShowValidationErrors($scope, errResponse);
              });
        },

        $scope.SaveASeasonDialogOkEvent = function () {
            alert('finish');
        }

    }]);

})();