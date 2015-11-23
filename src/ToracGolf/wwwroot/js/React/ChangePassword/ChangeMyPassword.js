var PasswordInput = React.createClass({displayName: "PasswordInput",
    render: function () {
        return React.createElement("input", {onKeyPress: this.props.onKeyPressEvent, onChange: this.props.onChangeEvent, type: "password", maxLength: "100"})
    }
});

var ChangePasswordForm = React.createClass({displayName: "ChangePasswordForm",
    getInitialState: function () {
        return { Errors: [] }
    },
    handleSubmit: function () {

        //go make an ajax call if we have 0 errors
        if (this.validation(true).length == 0) {

            //grab this for closure
            var _this = this;

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
                //go make ajax call here
                _this.setState({ Errors: err.responseJSON[''] });
            });
        }
    },
    handleChange: function (e) {

        //something has changed, go run the form validation
        this.validation(false);
    },
    getFormData: function () {
        return {
            CurrentPW: React.findDOMNode(this.refs.CurrentPassword).value,
            NewPw1: React.findDOMNode(this.refs.NewPassword1).value,
            NewPw2: React.findDOMNode(this.refs.NewPassword2).value
        };
    },
    enterKeyCheck: function (evt) {

        if (evt.key == 'Enter') {
            $('#ChangePassword').trigger('click');
        }
    },
    validation: function (onFormSubmit) {
        var errors = [];

        //grab the form data
        var formData = this.getFormData();

        if (onFormSubmit) {
            if (formData.CurrentPW.length == 0 || formData.NewPw1.length == 0 || formData.NewPw2.length == 0) {
                errors.push("Please Fill In All The Fields.");
            }
        }

        if ((formData.NewPw1.length > 0 || formData.NewPw2.length > 0) && (formData.NewPw1 != formData.NewPw2)) {
            errors.push("New Password Fields Don't Match.");
        }

        this.setState({ Errors: errors });

        return errors;
    },
    render: function () {
        return (
                        React.createElement("fieldset", null, 
                                    React.createElement("section", null, 
                                        React.createElement("label", {className: "label"}, "Current Password"), 
                                        React.createElement("label", {className: "input"}, 
                                            React.createElement("i", {className: "icon-append fa fa-edit"}), 
                                           React.createElement(PasswordInput, {ref: "CurrentPassword", onChangeEvent: this.handleChange})
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("label", {className: "label"}, "New Password"), 
                                        React.createElement("label", {className: "input"}, 
                                            React.createElement("i", {className: "icon-append fa fa-edit"}), 
                                             React.createElement(PasswordInput, {ref: "NewPassword1", onChangeEvent: this.handleChange})
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("label", {className: "label"}, "Retype New Password"), 
                                        React.createElement("label", {className: "input"}, 
                                            React.createElement("i", {className: "icon-append fa fa-edit"}), 
                                            React.createElement(PasswordInput, {ref: "NewPassword2", onKeyPressEvent: this.enterKeyCheck, onChangeEvent: this.handleChange})
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("div", {className: "ValidationSummary form-group ValidationMinHeight"}, 
                                            React.createElement("div", {id: "ValidationError", className: "text-danger validationErrors"}, 
                                                React.createElement(ErrorList, {ErrorItems: this.state.Errors})
                                            )
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("button", {id: "ChangePassword", onClick: this.handleSubmit, className: "btn btn-base btn-icon btn-icon-right btn-sign-in pull-right", type: "button"}, 
                                            React.createElement("span", null, "Change Password")
                                        )
                                    )
                            ));
    }
});


        $(document).ready(function () {

            //go render the form
            ReactDOM.render(
          React.createElement(ChangePasswordForm, null),
          document.getElementById('ChangePwForm'));

        })