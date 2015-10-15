(function () {

    appToracGolf.controller('SeasonAddController', ['$scope', 'ValidationService', 'SeasonHttp', '$filter', function ($scope, ValidationService, SeasonHttp, $filter) {

        $scope.init = function (seasonAddModel) {

            //set the model
            $scope.model = seasonAddModel;
        }

    }]);

})();