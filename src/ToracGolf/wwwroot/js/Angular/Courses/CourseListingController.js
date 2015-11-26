(function () {

    appToracGolf.controller('CourseListingController', ['$scope', 'ValidationService', 'CourseHttp', 'PagerFactory', function ($scope, ValidationService, CourseHttp, PagerFactory) {

        $scope.init = function (totalNumberOfPages, courseSortOrder, userStatePreference, defaultCoursesPerPage, coursesPerPageLookup, courseNameFilter) {

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
            $scope.SearchByCourseName = courseNameFilter;

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
        },

        $scope.ArchiveACourse = function (courseId) {

            //set the course id to delete
            $scope.CourseIdToDelete = courseId;

            //show the dialog
            $scope.DeleteCourseModalShow = true;
        },

        $scope.DeleteACourseConfirm = function () {

            //let's go delete that round
            CourseHttp.CourseDelete($scope.CourseIdToDelete)
                .then(function (result) {

                    //flip the variable so the modal will popup again
                    $scope.DeleteCourseModalShow = false;

                    //we want to reload the page now we can refresh everything
                    $scope.FetchAPageOfData(true);
                },
                function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        }

    }]);

})();