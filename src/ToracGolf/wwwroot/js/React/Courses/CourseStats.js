var CondensedStats = React.createClass({displayName: "CondensedStats",

    getInitialState: function () {
        return {
            TeeBoxCount: this.props.InitData.TeeBoxCount,
            RoundCount: this.props.InitData.RoundCount,
            BestScore: this.props.InitData.BestScore,
            AverageScore: this.props.InitData.AverageScore
        };
    },

    render: function () {
        return  React.createElement("ul", {className: "aux-info"}, 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-building"}), "Tee Boxes: ", this.state.TeeBoxCount), 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-user"}), "Rounds: ", React.createElement(FormatNumber, {valueToSet: this.state.RoundCount})), 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-tint"}), "Best Score: ", this.state.BestScore), 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-car"}), "Avg Score: ", React.createElement(ToDecimalPoints, {decimals: 2, valueToSet: this.state.AverageScore}))
        )
    }
});

var TeeBoxInformation = React.createClass({displayName: "TeeBoxInformation",

    render: function () {

        var createRow = function (rowValue, index) {

            return React.createElement("tr", {key: rowValue.TeeLocationId}, 
                        React.createElement("td", null, rowValue.Name), 
                        React.createElement("td", null, React.createElement(FormatNumber, {valueToSet: rowValue.Yardage})), 
                        React.createElement("td", null, rowValue.Par), 
                        React.createElement("td", null, rowValue.Slope), 
                        React.createElement("td", null, rowValue.Rating)
            )
        };

        return  React.createElement("table", {className: "table table-bordered table-striped table-hover table-responsive"}, 
                                                   React.createElement("tbody", null, 
                                                       React.createElement("tr", null, 
                                                           React.createElement("td", null, React.createElement("strong", null, "Tee Box")), 
                                                           React.createElement("td", null, React.createElement("strong", null, "Yardage")), 
                                                           React.createElement("td", null, React.createElement("strong", null, "Par")), 
                                                           React.createElement("td", null, React.createElement("strong", null, "Slope")), 
                                                           React.createElement("td", null, React.createElement("strong", null, "Rating"))
                                                       ), 
                                                       this.props.TeeBoxInfo.map(createRow)
                                                   )
        )
    }
});

var ScoreChart = React.createClass({displayName: "ScoreChart",

    getInitialState: function () {
        return {
            DataSet: this.props.DataSetToUse
        };
    },

    render: function () {

        return React.createElement("div", {ref: "ScoreLineChart"})
    },

    componentDidMount: function () {

        //only gets called 1 time on first render
        this.initializeChart();
    },

    componentDidUpdate: function () {

        //does not get called on initial render
        this.initializeChart();
    },

    initializeChart: function () {

        var rounds = [];
        var handicap = [];

        if (this.state.DataSet != null) {
            for (var i = 0; i < this.state.DataSet.length; i++) {

                var item = this.state.DataSet[i];

                //subtract for month because javascript uses a 0 base index. .net is 1 base index ie. jan = 1
                rounds.push([Date.UTC(item.Year, item.Month - 1, item.Day), item.Score])
                handicap.push([Date.UTC(item.Year, item.Month - 1, item.Day), item.Handicap])
            }
        }

        $(this.refs.ScoreLineChart).highcharts({
            credits: {
                enabled: false
            },
            chart: {
                type: 'spline',
                zoomType: 'x'
            },
            title: {
                text: 'Scores'
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
                name: 'Round Score',
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
    }

});

var StandardChart = React.createClass({displayName: "StandardChart",

    getInitialState: function () {
        return {
            DataSet: this.props.DataSetToUse
        };
    },

    render: function () {
        return React.createElement("div", {ref: this.props.IdToUse})
    },

    componentDidMount: function () {
        //only gets called 1 time on first render
        this.initializeChart();
    },

    componentDidUpdate: function () {
        //does not get called on initial render
        this.initializeChart();
    },

    initializeChart: function () {
      
        var dataSetToRender = [];

        if (this.state.DataSet != null) {
            for (var i = 0; i < this.state.DataSet.length; i++) {

                var item = this.state.DataSet[i];

                //subtract for month because javascript uses a 0 base index. .net is 1 base index ie. jan = 1
                dataSetToRender.push([Date.UTC(item.Year, item.Month - 1, item.Day), item[this.props.PropertyOfData]])
            }
        }
       
        $(this.refs[this.props.IdToUse]).highcharts({
            credits: {
                enabled: false
            },
            chart: {
                type: 'spline',
                zoomType: 'x'
            },
            title: {
                text: this.props.TitleOfChart
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
                    text: 'Putts',
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
            }],

            plotOptions: {
                spline: {
                    marker: {
                        enabled: true
                    }
                }
            },

            series: [{
                name: this.props.TitleOfSeries,
                yAxis: 0,
                // Define the data points. All series have a dummy year
                // of 1970/71 in order to be compared on the same x axis. Note
                // that in JavaScript, months start at 0 for January, 1 for February etc.
                data: dataSetToRender
            }]
        });
    }

});

function RunQuery() {
    //ajax call

    RunAjax('CourseStatQuery', { CourseId: $('#CourseId').val(), SeasonId: $('#SeasonSelect').val(), TeeBoxLocationId: $('#TeeBoxSelect').val() })
          .done(function (response) {

              //go set the quick stats
              condensedStats.setState({
                  TeeBoxCount: response.QuickStats.TeeBoxCount,
                  RoundCount: response.QuickStats.RoundCount,
                  BestScore: response.QuickStats.BestScore,
                  AverageScore: response.QuickStats.AverageScore
              });

              //we basically so need to re-render the entire grid since it's not really html
              roundScoreGraph.setState({ DataSet: response.ScoreGraphData });

              puttGraph.setState({ DataSet: response.PuttsGraphData });

              girGraph.setState({ DataSet: response.GIRGraphData });

              fairwaysHitGraph.setState({ DataSet: response.FairwaysGraphData });
          })
          .fail(function (err) {
              alert('Error Getting Data: ' + err);
          });
}

function InitReact(teeBoxData, quickStats, scoreGraphData, puttGraphData, girGraphData, fairwaysHitGraphData) {

    condensedStats = ReactDOM.render(
       React.createElement(CondensedStats, {InitData: quickStats}),
        document.getElementById('CondensedStats'));

    teeBoxTabData = ReactDOM.render(
        React.createElement(TeeBoxInformation, {TeeBoxInfo: teeBoxData}),
        document.getElementById('TabDataTeeBox'));

    roundScoreGraph = ReactDOM.render(
        React.createElement(ScoreChart, {DataSetToUse: scoreGraphData }),
        document.getElementById('ScoreDivContainer'));

    puttGraph = ReactDOM.render(
        React.createElement(StandardChart, {IdToUse: "PuttsChart", PropertyOfData: "Putts", TitleOfChart: "Putts Per Round", TitleOfSeries: "Putts Per Round", DataSetToUse: puttGraphData}),
        document.getElementById('PuttDivContainer'));

    girGraph = ReactDOM.render(
       React.createElement(StandardChart, {IdToUse: "GIRChart", PropertyOfData: "GreensHit", TitleOfChart: "Greens In Regulation (GIR)", TitleOfSeries: "Greens In Regulation (GIR)", DataSetToUse: girGraphData }),
        document.getElementById('GIRDivContainer'));

    fairwaysHitGraph = ReactDOM.render(
       React.createElement(StandardChart, {IdToUse: "FairwaysHitChart", PropertyOfData: "FairwaysHit", TitleOfChart: "Fairways In Regulation", TitleOfSeries: "Fairways In Regulation", DataSetToUse: fairwaysHitGraphData }),
        document.getElementById('FaiwaysHitDivContainer'));

    //add hooks into the select combo box to re-run the query
    $('.ReRunQuery').change(RunQuery);
}