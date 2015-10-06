(function () {

    appToracGolf.controller('CourseListingController', ['$scope', 'ValidationService', 'CourseHttp', function ($scope, ValidationService, CourseHttp) {

        $scope.init = function (totalNumberOfPages, courseSortOrder) {

            //set the drop down items
            $scope.SortByLookup = courseSortOrder;

            //add a sort by model (to sort the grid)
            $scope.SortBy = courseSortOrder[0].Value;

            //set the initial page
            $scope.CurrentPageId = 0;

            //total number of pages
            $scope.TotalNumberOfPages = totalNumberOfPages;

            //go add the pager
            var pagerElements = [];

            //loop through the pages and add the specific page
            for (var i = 0; i < totalNumberOfPages; i++) {
                pagerElements.push(i);
            }

            //set the pager elements
            $scope.PagerElements = pagerElements;

            //go fetch the page now
            $scope.FetchAPageOfData(false);
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
            CourseHttp.CourseListing($scope.CurrentPageId, $scope.SortBy)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData;

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        }

    }]);

})();