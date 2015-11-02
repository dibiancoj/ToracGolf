(function () {

    appToracGolf.controller('DashboardController', ['$scope', 'DashboardHttp', function ($scope, DashboardHttp) {

        $scope.init = function (dashboardView) {

            //set the dashboard view
            $scope.DashboardView = dashboardView;

            //add a watch on the dashboard view
            $scope.$watch(function (scope) { return scope.DashboardView; },
                          function () {
                              $scope.LoadDashboardView($scope.DashboardView)
                          });

        },

        $scope.LoadDashboardView = function (viewToLoad) {
            DashboardHttp.ViewTypeHasChanged(viewToLoad)
            .then(function (result) {
                $scope.Last5Rounds = result.data.Last5Rounds;
            });
        }

    }]);

})();