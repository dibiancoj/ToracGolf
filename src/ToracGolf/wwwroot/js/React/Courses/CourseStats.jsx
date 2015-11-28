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
                          <li><i className="fa fa-user"></i>Rounds: {this.state.RoundCount}</li>
                          <li><i className="fa fa-tint"></i>Best Score: {this.state.BestScore}</li>
                          <li><i className="fa fa-car"></i>Avg Score: {this.state.AverageScore}</li>
        </ul>
    }
});

var TeeBoxInformation = React.createClass({

    //getInitialState: function () {
    //    //return { TeeBoxInfo: [{ TeeBoxName: '', Yardage: '', Par: '', Slope: '', Rating: '' }] };
    //    return { TeeBoxInfo: [] };
    //},

    render: function () {

        var createRow = function (rowValue, index) {

            return <tr key={rowValue.TeeLocationId}>
                        <td>{rowValue.Name}</td>
                        <td>{rowValue.Yardage}</td>
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
          })
          .fail(function (err) {
              alert('Error Getting Data: ' + err);
          });
}

function InitReact(teeBoxData, quickStats) {

    debugger;

    condensedStats = ReactDOM.render(
       <CondensedStats InitData={quickStats} />,
       document.getElementById('CondensedStats'));

    teeBoxTabData = ReactDOM.render(
        <TeeBoxInformation TeeBoxInfo={teeBoxData} />,
        document.getElementById('TabDataTeeBox'));

    //add hooks into the select combo box to re-run the query
    $('.ReRunQuery').change(RunQuery);
}