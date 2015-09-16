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
                    'RequestVerificationToken': token
                }
            };

            //$scope.model 

            $http.post('AddACourse', $scope.model, config)
               .then(function (response) {
                   debugger;
                   var s = response;

                   $scope.model.Location = 'teststest';

                   // this callback will be called asynchronously
                   // when the response is available
               }, function (response) {
                   var s = response;
                   // called asynchronously if an error occurs
                   // or server returns response with an error status.
               });
        }

    });

})();