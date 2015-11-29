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
                          React.createElement("li", null, React.createElement("i", {className: "fa fa-car"}), "Avg Score: ", this.state.AverageScore)
        )
    }
});

var TeeBoxInformation = React.createClass({displayName: "TeeBoxInformation",

    //getInitialState: function () {
    //    //return { TeeBoxInfo: [{ TeeBoxName: '', Yardage: '', Par: '', Slope: '', Rating: '' }] };
    //    return { TeeBoxInfo: [] };
    //},

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

    condensedStats = ReactDOM.render(
       React.createElement(CondensedStats, {InitData: quickStats}),
       document.getElementById('CondensedStats'));

    teeBoxTabData = ReactDOM.render(
        React.createElement(TeeBoxInformation, {TeeBoxInfo: teeBoxData}),
        document.getElementById('TabDataTeeBox'));

    //add hooks into the select combo box to re-run the query
    $('.ReRunQuery').change(RunQuery);
}