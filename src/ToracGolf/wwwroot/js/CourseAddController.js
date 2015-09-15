(function () {

    var app = angular.module('toracApp', []);

    app.controller('CourseAddController', function ($scope, $http) { 

        $scope.init = function (courseAddModel) {

            $scope.model = courseAddModel;
        },

        $scope.processForm = function () {

            
            var token = $('[name=__RequestVerificationToken]').val();

            debugger;
            var req = {
                method: 'POST',
                url: 'AddACourse',
                headers: { "__RequestVerificationToken": token },
                data: JSON.stringify($scope.model)
            }

            debugger;
            $http(req)
            .then(function (response) {
                debugger;
                $scope.names = response.records;
            }), function (errResponse) {
                debugger;
            };
        }

    });

})();