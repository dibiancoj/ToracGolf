(function () {

    appToracGolf.controller('CourseListingController', ['$scope', 'ValidationService', 'CourseHttp', function ($scope, ValidationService, CourseHttp) {

        $scope.init = function (totalNumberOfPages, courseSortOrder) {

            //set the drop down items
            $scope.SortByLookup = courseSortOrder;

            //add a sort by model (to sort the grid)
            $scope.SortBy = courseSortOrder[0].Value;

            //add a watch to the current page id
            $scope.$watch('CurrentPageId', $scope.FetchAPageOfData);

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
        },

        $scope.PagerClick = function (index) {

            //increment / decrement the page
            $scope.CurrentPageId = index;
        };

        $scope.FetchAPageOfData = function () {

            //go grab the records to display
            CourseHttp.CourseListing($scope.CurrentPageId)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData;

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        }

    }]);

})();