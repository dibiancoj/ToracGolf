var CondensedStats = React.createClass({displayName: "CondensedStats",

    getInitialState: function () {
        return {
            TeeBoxCount: 'N/A',
            RoundCount: 'N/A',
            BestScore: 'N/A',
            AverageScore: 'N/A'
        };
    },
    render: function () {
        return  React.createElement("ul", {className: "aux-info"}, 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-building"}), "Tee Boxes: ", this.state.TeeBoxCount), 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-user"}), "Rounds: ", this.state.RoundCount), 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-tint"}), "Best Score: ", this.state.BestScore), 
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-car"}), "Avg Score: ", this.state.AverageScore)
        )
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
          React.createElement(CondensedStats, null),
          document.getElementById('CondensedStats'));

    //go load the initial data
    RunQuery();

    //add hooks into the select combo box to re-run the query
    $('.ReRunQuery').change(RunQuery);
});