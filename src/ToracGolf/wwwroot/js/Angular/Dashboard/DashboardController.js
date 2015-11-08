(function () {

    appToracGolf.controller('DashboardController', ['$scope', 'DashboardHttp', function ($scope, DashboardHttp) {

        $scope.init = function (dashboardView) {

            //set the dashboard view
            $scope.DashboardView = dashboardView;

            //add a watch on the dashboard view
            $scope.$watch(function (scope) { return scope.DashboardView; },
                          function () {

                              //go load the data
                              $scope.LoadDashboardView($scope.DashboardView)
                          });
        },

        $scope.LoadDashboardView = function (viewToLoad) {

            DashboardHttp.ViewTypeHasChanged(viewToLoad)
            .then(function (result) {

                //load the last 5 rounds
                $scope.Last5Rounds = result.data.Last5Rounds;

                //load the top 5 rounds
                $scope.Top5Rounds = result.data.Top5Rounds;

                //load the handicapgrid
                $scope.LoadHandicapGrid(viewToLoad, result.data.HandicapScoreSplitGrid);

                //load the round pie chart
                $scope.RoundPieChart(result.data.RoundPieChart);

                //resize the window so highcharts adjust correcty
                $(window).trigger('resize');
            });
        },

        $scope.LoadHandicapGrid = function (viewToLoad, dataSet) {

            //format as follows
            //[Date.UTC(1970, 9, 21), 70],

            var rounds = [];
            var handicap = [];

            for (var i = 0; i < dataSet.length; i++) {

                var item = dataSet[i];

                //subtract for month because javascript uses a 0 base index. .net is 1 base index ie. jan = 1
                rounds.push([Date.UTC(item.Year, item.Month - 1, item.Day), item.Score])
                handicap.push([Date.UTC(item.Year, item.Month - 1, item.Day), item.Handicap])
            }

            $('#HandicapProgression').highcharts({
                credits: {
                    enabled: false
                },
                chart: {
                    type: 'spline',
                    zoomType: 'x'
                },
                title: {
                    text: viewToLoad + ' Progression'
                },
                xAxis: {
                    type: 'datetime',
                    dateTimeLabelFormats: { // don't display the dummy year
                        month: '%e. %b',
                        year: '%b'
                    },
                    title: {
                        text: 'Date'
                    }
                },
                yAxis: [{ // primary yaxis
                    title: {
                        text: 'Round Score',
                        style: {
                            color: Highcharts.getOptions().colors[0]
                        }
                    },
                    labels: {
                        format: '{value}',
                        style: {
                            color: Highcharts.getOptions().colors[0]
                        }
                    },
                    opposite: false
                },
                { // secondary yAxis
                    labels: {
                        format: '{value}',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    },
                    title: {
                        text: 'Handicap',
                        style: {
                            color: Highcharts.getOptions().colors[1]
                        }
                    }
                }],

                plotOptions: {
                    spline: {
                        marker: {
                            enabled: true
                        }
                    }
                },

                series: [{
                    name: 'Round',
                    yAxis: 0,
                    // Define the data points. All series have a dummy year
                    // of 1970/71 in order to be compared on the same x axis. Note
                    // that in JavaScript, months start at 0 for January, 1 for February etc.
                    data: rounds
                }, {
                    name: 'Handicap',
                    yAxis: 1,
                    data: handicap
                }]
            });
        },

        $scope.RoundPieChart = function (dataSet) {

            $('#ScorePieChart').highcharts({
                credits: false,
                chart: {
                    type: 'pie',
                    options3d: {
                        enabled: true,
                        alpha: 45,
                        beta: 0
                    }
                },
                title: {
                    text: 'Round Scores'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}% [{point.y} Rounds]</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        depth: 35,
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: 'Round Breakdown',
                    data: dataSet
                }]
            });
        }

    }]);

})();