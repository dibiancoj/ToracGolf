(function () {

    appToracGolf.controller('RoundListingController', ['$scope', 'ValidationService', 'RoundHttp', function ($scope, ValidationService, RoundHttp) {

        $scope.init = function (totalNumberOfPages, roundSortOrder, userStatePreference, defaultRoundsPerPage, roundsPerPageLookup) {

            //set the drop down items
            $scope.SortByLookup = roundSortOrder;

            //set the rounds per page lookup
            $scope.RoundsPerPageLookup = roundsPerPageLookup;

            //add a sort by model (to sort the grid)
            $scope.SortBy = roundSortOrder[0].Value;

            //set the default courses per page
            $scope.RoundsPerPage = defaultRoundsPerPage;

            //state filter
            $scope.StateFilter = userStatePreference;

            //set the seach by course
            $scope.SearchByRoundName = '';

            //set the initial page
            $scope.CurrentPageId = 0;

            //go build the pager 
            $scope.BuildPager(totalNumberOfPages);

            //go fetch the page now
            $scope.FetchAPageOfData(false);
        },

        $scope.BuildPager = function (totalNumberOfPages) {

            //total number of pages (set the scope)
            $scope.TotalNumberOfPages = totalNumberOfPages;

            //go add the pager
            var pagerElements = [];

            //loop through the pages and add the specific page
            for (var i = 0; i < totalNumberOfPages; i++) {
                pagerElements.push(i);
            }

            //set the pager elements
            $scope.PagerElements = pagerElements;
        },

        $scope.PagerClick = function (index) {

            //increment / decrement the page
            $scope.CurrentPageId = index;

            //go fetch the page now
            $scope.FetchAPageOfData(false);
        };

        $scope.FetchAPageOfData = function (resetPagerToPage1) {

            //do we want to go back to page 1? (coming from the sort by drop down?)
            if (resetPagerToPage1) {

                //go back to pager 1
                $scope.CurrentPageId = 0;
            }

            //go grab the records to display
            RoundHttp.RoundListing(resetPagerToPage1, $scope.CurrentPageId, $scope.SortBy, $scope.SearchByRoundName, $scope.StateFilter, $scope.RoundsPerPage)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData.ListingData;

                    //do we need to rebuild the pager?
                    if (result.data.TotalNumberOfPages != null) {
                        $scope.BuildPager(result.data.TotalNumberOfPages)
                    }

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        }

    }]);

})();