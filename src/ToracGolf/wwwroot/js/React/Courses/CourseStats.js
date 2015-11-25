var CourseStats = React.createClass({displayName: "CourseStats",
    getInitialState: function () {
        return {};
    },
    handleSubmit: function () {

        alert('thow new notimplemented');

            RunAjax('ChangePassword', this.getFormData())
            .done(function (response) {

                if (response.result) {
                    $('#SaveSuccessfulDialog').modal('show');

                    //reset the fields
                    React.findDOMNode(_this.refs.CurrentPassword).value = '';
                    React.findDOMNode(_this.refs.NewPassword1).value = '';
                    React.findDOMNode(_this.refs.NewPassword2).value = '';
                }
            })
            .fail(function (err) {
                //go set the validation items and show the error
                _this.setState({ Errors: err.responseJSON[''] });
            });
        
    },
    handleChange: function (e) {

        //something has changed, go run the form validation
        
    },
    getFormData: function () {
        return {
            
        };
    },
    enterKeyCheck: function (evt) {

        //if (evt.key == 'Enter') {
        //    $('#ChangePassword').trigger('click');
        //}
    },
    render: function () {
        return (
                        React.createElement("div", null, "jason")

                        );
    }
});


$(document).ready(function () {

    //go render the form
    ReactDOM.render(
          React.createElement(CourseStats, null),
          document.getElementById('CourseStatsContainer'));

})