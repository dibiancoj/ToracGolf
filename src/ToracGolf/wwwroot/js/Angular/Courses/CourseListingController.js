(function () {

    appToracGolf.controller('CourseListingController', ['$scope', 'ValidationService', 'CourseHttp', function ($scope, ValidationService, CourseHttp) {

        $scope.init = function (totalNumberOfPages, courseSortOrder, userStatePreference) {

            //set the drop down items
            $scope.SortByLookup = courseSortOrder;

            //add a sort by model (to sort the grid)
            $scope.SortBy = courseSortOrder[0].Value;

            //state filter
            $scope.StateFilter = userStatePreference;

            //set the seach by course
            $scope.SearchByCourseName = '';

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
            CourseHttp.CourseListing(resetPagerToPage1, $scope.CurrentPageId, $scope.SortBy, $scope.SearchByCourseName, $scope.StateFilter)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData;

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