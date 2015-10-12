(function () {
    'use strict';

    //create the factory
    appToracGolf.service('RoundHttp', ['$http', function ($http) {

        //set the default http headers, so we don't need to keep setting it everytime we make an ajax call
        //$http.defaults.headers.common.RequestVerificationToken = $('[name=__RequestVerificationToken]').val();

        return {

            AddRound: function (model, validationService) {
                return $http.post('AddARound', model, validationService);
            },

            FetchCoursesForState: function (stateId) {
                return $http.post('CourseSelectByState', { StateId: stateId });
            },

            FetchTeeBoxForCourseId: function (courseId) {
                return $http.post('TeeLocationSelectForCourseId', { CourseId: courseId });
            },

            RoundListing: function (resetPager, pageId, sortBy, courseNameFilter, seasonFilter, roundsPerPage, roundDateStartFilter, roundDateEndFilter) {
                return $http.post('RoundListingSelectPage',
                    {
                        ResetPager: resetPager,
                        PageIndexId: pageId,
                        SortBy: sortBy,
                        CourseNameFilter: courseNameFilter,
                        SeasonFilter: seasonFilter,
                        RoundsPerPage: roundsPerPage,
                        RoundDateStartFilter: isNaN(Date.parse(roundDateStartFilter)) ? null : roundDateStartFilter,
                        RoundDateEndFilter: isNaN(Date.parse(roundDateEndFilter)) ? null : roundDateEndFilter
                    });
            }
        }

    }]);
})();