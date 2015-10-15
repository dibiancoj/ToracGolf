(function () {

    appToracGolf.controller('SeasonListingController', ['$scope', 'ValidationService', 'RoundHttp', function ($scope, ValidationService, SeasonHttp) {

        $scope.init = function () {

            //go fetch the page now
            $scope.FetchAPageOfData(false);
        },

        $scope.FetchAPageOfData = function () {

            //go grab the records to display
            SeasonHttp.CourseListing()
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData;

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        },

        $scope.ArchiveASeason = function (seasonId) {
            alert('need to implement');
        }

    }]);

})();