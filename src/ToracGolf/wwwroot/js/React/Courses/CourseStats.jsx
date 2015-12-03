var CondensedStats = React.createClass({

    getInitialState: function () {
        return {
            TeeBoxCount: this.props.InitData.TeeBoxCount,
            RoundCount: this.props.InitData.RoundCount,
            BestScore: this.props.InitData.BestScore,
            AverageScore: this.props.InitData.AverageScore
        };
    },

    render: function () {
        return  <ul className="aux-info">
                          <li><i className="fa fa-building"></i>Tee Boxes: {this.state.TeeBoxCount}</li>
                          <li><i className="fa fa-user"></i>Rounds: <FormatNumber valueToSet={this.state.RoundCount}></FormatNumber></li>
                          <li><i className="fa fa-tint"></i>Best Score: {this.state.BestScore}</li>
                          <li><i className="fa fa-car"></i>Avg Score: {this.state.AverageScore}</li>
        </ul>
    }
});

var TeeBoxInformation = React.createClass({

    render: function () {

        var createRow = function (rowValue, index) {

            return <tr key={rowValue.TeeLocationId}>
                        <td>{rowValue.Name}</td>
                        <td><FormatNumber valueToSet={rowValue.Yardage}></FormatNumber></td>
                        <td>{rowValue.Par}</td>
                        <td>{rowValue.Slope}</td>
                        <td>{rowValue.Rating}</td>
            </tr>
        };

        return  <table className="table table-bordered table-striped table-hover table-responsive">
                                                   <tbody>
                                                       <tr>
                                                           <td><strong>Tee Box</strong></td>
                                                           <td><strong>Yardage</strong></td>
                                                           <td><strong>Par</strong></td>
                                                           <td><strong>Slope</strong></td>
                                                           <td><strong>Rating</strong></td>
                                                       </tr>
        {this.props.TeeBoxInfo.map(createRow)}
        </tbody>
</table>
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
              
              //go build the score chart
              BuildScoreChart(response.ScoreGraphData);
          })
          .fail(function (err) {
              alert('Error Getting Data: ' + err);
          });
}

function BuildScoreChart(dataSet) {

    var rounds = [];
    var handicap = [];

    if (dataSet != null)
    {
        for (var i = 0; i < dataSet.length; i++) {

            var item = dataSet[i];

            //subtract for month because javascript uses a 0 base index. .net is 1 base index ie. jan = 1
            rounds.push([Date.UTC(item.Year, item.Month - 1, item.Day), item.Score])
            handicap.push([Date.UTC(item.Year, item.Month - 1, item.Day), item.Handicap])
        }
    }

    $('#ScoreLineChart').highcharts({
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
}

function InitReact(teeBoxData, quickStats, scoreGraphData) {

    condensedStats = ReactDOM.render(
       <CondensedStats InitData={quickStats} />,
document.getElementById('CondensedStats'));

teeBoxTabData = ReactDOM.render(
    <TeeBoxInformation TeeBoxInfo={teeBoxData} />,
        document.getElementById('TabDataTeeBox'));

    BuildScoreChart(scoreGraphData);

//add hooks into the select combo box to re-run the query
    $('.ReRunQuery').change(RunQuery);
}