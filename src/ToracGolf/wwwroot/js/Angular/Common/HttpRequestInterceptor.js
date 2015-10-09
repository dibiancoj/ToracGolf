
//factory will add the http request interceptor to the $httpProvider
appToracGolf.factory('httpRequestInterceptor', ['$q', '$location', function ($q, $location) {
    return {
        request: function (config) {

            //show the spinner
            $("#loadingDiv").show();

            //return the config
            return config;
        },

        requestError: function (rejection) {

            //hide the spinner
            $("#loadingDiv").hide();

            //return the rejected promise
            return $q.reject(rejection);
        },

        response: function (response) {

            //hide the spinner
            $("#loadingDiv").hide();

            //return the response
            return response;
        },

        responseError: function (rejection) {
            
            //hide the spinner
            $("#loadingDiv").hide();

            //return the rejected promise
            return $q.reject(rejection);
        }
    }
}]);

//Http Intercpetor to check auth failures for xhr requests
appToracGolf.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('httpRequestInterceptor');
}]);