(function () {

    appToracGolf.controller('SeasonListingController', ['$scope', 'ValidationService', 'SeasonHttp', function ($scope, ValidationService, SeasonHttp) {

        $scope.init = function () {

            //go fetch the page now
            $scope.FetchAPageOfData(false);
        },

        $scope.FetchAPageOfData = function () {

            //go grab the records to display
            SeasonHttp.SeasonListingFetchData()
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData;

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        },

        $scope.ArchiveASeason = function (seasonId) {
            SeasonHttp.ArchiveASeason(seasonId)
             .then(function(result){

                 alert('season is archived');

                 //go fetch the new data
                 $scope.FetchAPageOfData();
             },
              function (errResponse) {
                  alert('Error: ' + JSON.stringify(errResponse));
              });
        }

    }]);

})();