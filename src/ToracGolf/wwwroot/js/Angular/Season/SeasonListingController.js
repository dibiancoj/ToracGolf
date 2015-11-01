(function () {

    appToracGolf.controller('SeasonListingController', ['$scope', 'ValidationService', 'SeasonHttp', function ($scope, ValidationService, SeasonHttp) {

        $scope.init = function (currentSeasonId) {

            //grab the current season id
            $scope.CurrentSeasonId = currentSeasonId;

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

        $scope.ArchiveASeason = function (seasonId, numberOfRoundsAssociatedWithThisSeason) {

            //set the season variable
            $scope.SeasonIdToDelete = seasonId;

            //set the number of rounds associated with this season for the dialog
            $scope.NumberOfRoundsAssociatedWithThisSeason = numberOfRoundsAssociatedWithThisSeason;

            //show the dialog
            $scope.DeleteSeasonModalShow = true;
        },

        $scope.ArchiveASeasonConfirm = function () {

            //go archive the season
            SeasonHttp.ArchiveASeason($scope.SeasonIdToDelete)
            .then(function (result) {

                //method will pass back the listing data...so just set the scope variable
                $scope.PagedData = result.data.PagedData;

                //hide the dialog
                $scope.DeleteSeasonModalShow = false;
            },
             function (errResponse) {
                 alert('Error: ' + JSON.stringify(errResponse));
             });
        },

        $scope.MakeCurrentSeason = function (seasonId) {

            SeasonHttp.SeasonMakeCurrent(seasonId)
            .then(function (result) {

                //if it completed successfully, then just reload the page so we can adjust the header and everything. This is a rare call, so we don't need to optimize it.
                if (result.data.Result) {
                    window.location.reload();
                }

            },
           function (errResponse) {
               alert('Error: ' + JSON.stringify(errResponse));
           });
        }

    }]);

})();