(function () {

    appToracGolf.controller('RoundListingController', ['$scope', 'ValidationService', 'RoundHttp', function ($scope, ValidationService, RoundHttp) {

        $scope.init = function (totalNumberOfPages, courseSortOrder, userStatePreference, defaultCoursesPerPage, coursesPerPageLookup) {

            //set the drop down items
            $scope.SortByLookup = courseSortOrder;

            //set the courses per page lookup
            $scope.CoursesPerPageLookup = coursesPerPageLookup;

            //add a sort by model (to sort the grid)
            $scope.SortBy = courseSortOrder[0].Value;

            //set the default courses per page
            $scope.CoursesPerPage = defaultCoursesPerPage;

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
            CourseHttp.CourseListing(resetPagerToPage1, $scope.CurrentPageId, $scope.SortBy, $scope.SearchByCourseName, $scope.StateFilter, $scope.CoursesPerPage)
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