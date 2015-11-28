var CondensedStats = React.createClass({

    getInitialState: function () {
        return {
            TeeBoxCount: 'N/A',
            RoundCount: 'N/A',
            BestScore: 'N/A',
            AverageScore: 'N/A'
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

function RunQuery() {
    //ajax call

    RunAjax('CourseStatQuery', { CourseId: $('#CourseId').val(), SeasonId: $('#SeasonSelect').val(), TeeBoxLocationId: $('#TeeBoxSelect').val() })
          .done(function (response) {

              var quickStats = response.QuickStats;
             
              condensedStats.setState({
                  TeeBoxCount: quickStats.TeeBoxCount,
                  RoundCount: quickStats.RoundCount,
                  BestScore: quickStats.BestScore,
                  AverageScore: quickStats.AverageScore
              });

          })
          .fail(function (err) {
              alert('Error Getting Data');
          });
}

$(document).ready(function () {

    condensedStats = ReactDOM.render(
          <CondensedStats />,
          document.getElementById('CondensedStats'));

    //go load the initial data
    RunQuery();

    //add hooks into the select combo box to re-run the query
    $('.ReRunQuery').change(RunQuery);
});