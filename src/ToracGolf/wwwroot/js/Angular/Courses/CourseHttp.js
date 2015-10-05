(function () {
    'use strict';

    //create the factory
    appToracGolf.service('CourseHttp', ['$http', function ($http) {

        //set the default http headers, so we don't need to keep setting it everytime we make an ajax call
        $http.defaults.headers.common.RequestVerificationToken = $('[name=__RequestVerificationToken]').val();

        return {

            AddCourse: function (model, validationService) {
                return $http.post('AddACourse', model, validationService);
            },

            AddTeeLocation: function (model, validationService) {
                return $http.post('ValidateTeeLocation', model, validationService);
            },

            CourseListing: function (pageId) {
                return $http.post('CourseListingSelectPage', { pageIndexId: pageId });
            }
        }

    }]);
})();