(function () {

    appToracGolf.controller('CourseListingController', ['$scope', 'ValidationService', 'CourseHttp', function ($scope, ValidationService, CourseHttp) {

        $scope.init = function (courseViewModel) {

            //set the initial page
            $scope.CurrentPageId = 0;

            //go grab the records to display
            CourseHttp.CourseListing($scope.CurrentPageId)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData;

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        };

    }]);

})();