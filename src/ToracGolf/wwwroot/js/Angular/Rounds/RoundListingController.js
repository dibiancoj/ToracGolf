﻿(function () {

    appToracGolf.controller('RoundListingController', ['$scope', 'ValidationService', 'RoundHttp', 'PagerFactory', function ($scope, ValidationService, RoundHttp, PagerFactory) {

        $scope.$watch('SearchByRoundDateEnd', function (newValue, oldValue) {
            //we need the date picker to run first, now we can go fetch the data
            $scope.WatchRoundDateFilter(newValue, oldValue);
        });

        $scope.$watch('SearchByRoundDateStart', function (newValue, oldValue) {
            //we need the date picker to run first, now we can go fetch the data
            $scope.WatchRoundDateFilter(newValue, oldValue);
        });

        $scope.WatchRoundDateFilter = function (newValue, oldValue) {

            //if we have different values then run the method
            if (newValue != oldValue) {
                //go refresh the data
                $scope.FetchAPageOfData(true);
            }
        },

        $scope.init = function (roundSortOrder, defaultRoundsPerPage, roundsPerPageLookup) {

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

            //handicap rounds only
            $scope.SearchByHandicapRound = false;

            //set the search by date
            $scope.SearchByRoundDateStart = '';
            $scope.SearchByRoundDateEnd = '';

            //set the initial page
            $scope.CurrentPageId = 0;

            //go build the pager 
            //$scope.BuildPager(totalNumberOfPages, numberOfRecords);

            //go fetch the page now
            $scope.FetchAPageOfData(true);
        },

        $scope.BuildPager = function (totalNumberOfPages, numberOfRecords) {

            //total number of pages (set the scope)
            $scope.TotalNumberOfPages = totalNumberOfPages;

            //total number of records
            $scope.TotalNumberOfRecords = numberOfRecords;

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
            RoundHttp.RoundListing(resetPagerToPage1, $scope.CurrentPageId, $scope.SortBy, $scope.SearchByCourseName, $scope.SeasonFilter, $scope.RoundsPerPage, $scope.SearchByRoundDateStart, $scope.SearchByRoundDateEnd, $scope.SearchByHandicapRound)
                .then(function (result) {

                    //set the paged data
                    $scope.PagedData = result.data.PagedData.ListingData;

                    //do we need to rebuild the pager?
                    if (result.data.TotalNumberOfPages != null && result.data.TotalNumberOfRecords != null) {
                        $scope.BuildPager(result.data.TotalNumberOfPages, result.data.TotalNumberOfRecords);
                    }

                }, function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        },

        $scope.DeleteARound = function (roundId) {

            //set the round id to delete
            $scope.RoundIdToDelete = roundId;

            //show the dialog
            $scope.DeleteRoundModalShow = true;
        },

        $scope.DeleteARoundConfirm = function () {

            //let's go delete that round
            RoundHttp.RoundDelete($scope.RoundIdToDelete)
                .then(function (result) {

                    //flip the variable so the modal will popup again
                    $scope.DeleteRoundModalShow = false;

                    //we want to reload the page now we can refresh everything
                    $scope.FetchAPageOfData(true);

                    //let's go reset the handicap stuff
                    $('#SeasonHandicap').text(result.data.NewHandicap.SeasonHandicap == null ? '' : result.data.NewHandicap.SeasonHandicap);
                    $('#CareerHandicap').text(result.data.NewHandicap.CareerHandicap == null ? '' : result.data.NewHandicap.CareerHandicap);

                    $('#SeasonProgression').text(result.data.NewHandicap.SeasonHandicap == null ? '' : result.data.NewHandicap.SeasonProgression);
                    $('#CareerProgression').text(result.data.NewHandicap.CareerHandicap == null ? '' : result.data.NewHandicap.CareerProgression);


                    //window.location.reload();
                },
                function (errResponse) {
                    alert('Error: ' + JSON.stringify(errResponse));
                });
        },

        $scope.RootPath = function () {
            return _rootDir;
        }

    }]);

})();