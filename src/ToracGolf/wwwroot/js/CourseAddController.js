(function () {

    var app = angular.module('toracApp', []);

    app.controller('CourseAddController', function ($scope, $http) {

        $scope.init = function (courseAddModel) {

            $scope.model = courseAddModel;
        },

        $scope.processForm = function () {


            var token = $('[name=__RequestVerificationToken]').val();

            var config = {
                headers: {
                    '__RequestVerificationToken': token
                }
            };



            $http.post('AddACourse', $scope.model, config)
               .then(function (response) {
                   debugger;
                   var s = response;
                   // this callback will be called asynchronously
                   // when the response is available
               }, function (response) {
                   var s = response;
                   // called asynchronously if an error occurs
                   // or server returns response with an error status.
               });






            //debugger;
            //var req = {
            //    method: 'POST',
            //    url: 'AddACourse',
            //    headers: { "RequestVerificationToken": token },
            //    data: JSON.stringify($scope.model)
            //}

            //debugger;
            //$http(req)
            //.then(function (response) {
            //    debugger;
            //    $scope.names = response.records;
            //}), function (errResponse) {
            //    var s = errResponse;
            //    debugger;
            //};
        }

    });

})();