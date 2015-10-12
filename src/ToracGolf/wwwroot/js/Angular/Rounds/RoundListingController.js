(function () {

    appToracGolf.controller('RoundListingController', ['$scope', 'ValidationService', 'RoundHttp', 'PagerFactory', function ($scope, ValidationService, RoundHttp, PagerFactory) {

        $scope.$watch('SearchByRoundDateStart', function (newValue) {
            //we need the date picker to run first, now we can go fetch the data
            $scope.FetchAPageOfData(true);
        });

        $scope.$watch('SearchByRoundDateEnd', function (newValue) {
            //we need the date picker to run first, now we can go fetch the data
            $scope.FetchAPageOfData(true);
        });

        $scope.init = function (totalNumberOfPages, roundSortOrder, defaultRoundsPerPage, roundsPerPageLookup) {

            //set the drop down items
            $scope.SortByLookup = roundSortOrder;

            //set the rounds per page lookup
            $scope.RoundsPerPageLookup = roundsPerPageLookup;

            //add a sort by model (to sort the grid)
            $scope.SortBy = roundSortOrder[0].Value;

            //set the default courses per page
            $scope.RoundsPerPage = defaultRoundsPerPage;

            //set the seach by course
            $scope.SearchByCourseName = '';

            //set the search by date
            $scope.SearchByRoundDateStart = '';
            $scope.SearchByRoundDateEnd = '';

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

            //set the pager elements
            $scope.PagerElements = PagerFactory.BuildPagerModel(totalNumberOfPages);
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
            RoundHttp.RoundListing(resetPagerToPage1, $scope.CurrentPageId, $scope.SortBy, $scope.SearchByCourseName, $scope.SeasonFilter, $scope.RoundsPerPage, $scope.SearchByRoundDateStart, $scope.SearchByRoundDateEnd)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData.ListingData;

                    //set the course image lookup
                    $scope.CourseImageLookup = result.data.PagedData.CourseImages;

                    //do we need to rebuild the pager?
                    if (result.data.TotalNumberOfPages != null) {
                        $scope.BuildPager(result.data.TotalNumberOfPages)
                    }

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        },

        $scope.FindCourseImageFromLookup = function (courseId) {
            return $scope.CourseImageLookup[courseId];
        }

    }]);

})();