(function () {
    'use strict';

    //create the factory
    appToracGolf.service('DashboardHttp', ['$http', function ($http) {

        //set the default http headers, so we don't need to keep setting it everytime we make an ajax call
        //$http.defaults.headers.common.RequestVerificationToken = $('[name=__RequestVerificationToken]').val();

        return {

            ViewTypeHasChanged: function (viewType) {
                return $http.post('DashboardViewTypeChange', { ViewType: viewType });
            }

        }

    }]);
})();