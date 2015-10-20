(function () {

    appToracGolf.controller('SeasonListingController', ['$scope', 'ValidationService', 'SeasonHttp', function ($scope, ValidationService, SeasonHttp) {

        $scope.init = function () {

            alert('need to finish');
            alert('Let them switch season to current season');
            alert('Dont let them delete there current season');

            //init the dialog
            $scope.DeleteSeasonModalShow = false;

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
        }

    }]);

})();