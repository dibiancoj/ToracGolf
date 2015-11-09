(function () {
    'use strict';

    //create the factory
    appToracGolf.service('DashboardHttp', ['$http', function ($http) {

        //set the default http headers, so we don't need to keep setting it everytime we make an ajax call
        //$http.defaults.headers.common.RequestVerificationToken = $('[name=__RequestVerificationToken]').val();

        return {

            //for some reason the home controll because of default path is having issues with background image and http path...so we will put the root path in. (only when publishing its an issue)
            //background image is put into css.
            ViewTypeHasChanged: function (viewType, rootPath) {
                return $http.post(rootPath + 'DashboardViewTypeChange', { ViewType: viewType });
            }

        }

    }]);
})();