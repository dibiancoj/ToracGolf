var PasswordInput = React.createClass({
    render: function () {
        return <input onKeyPress={this.props.onKeyPressEvent} onChange={this.props.onChangeEvent} type="password" maxLength="100"></input>
    }
});

var ChangePasswordForm = React.createClass({
    getInitialState: function () {
        return { Errors: [] }
    },
    handleSubmit: function () {

        //go run validation with submit clicked
        var result = this.validation(true);

        var _this = this;

        //go make an ajax call if we have 0 errors
        if (result.length == 0) {

     
            alert('need to add antiforgery token');

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
                        <fieldset>
                                    <section>
                                        <label className="label">Current Password</label>
                                        <label className="input">
                                            <i className="icon-append fa fa-edit"></i>
                                           <PasswordInput ref="CurrentPassword" onChangeEvent={this.handleChange}></PasswordInput>
                                        </label>
                                    </section>
                                    <section>
                                        <label className="label">New Password</label>
                                        <label className="input">
                                            <i className="icon-append fa fa-edit"></i>
                                             <PasswordInput ref="NewPassword1" onChangeEvent={this.handleChange}></PasswordInput>
                                        </label>
                                    </section>
                                    <section>
                                        <label className="label">Retype New Password</label>
                                        <label className="input">
                                            <i className="icon-append fa fa-edit"></i>
                                            <PasswordInput ref="NewPassword2" onKeyPressEvent={this.enterKeyCheck} onChangeEvent={this.handleChange}></PasswordInput>
                                        </label>
                                    </section>
                                    <section>
                                        <div className="ValidationSummary form-group ValidationMinHeight">
                                            <div id="ValidationError" className="text-danger validationErrors">
                                                <ErrorList ErrorItems={this.state.Errors} />
                                            </div>
                                        </div>
                                    </section>
                                    <section>
                                        <button id="ChangePassword" onClick={this.handleSubmit} className="btn btn-base btn-icon btn-icon-right btn-sign-in pull-right" type="button">
                                            <span>Change Password</span>
                                        </button>
                                    </section>
                            </fieldset>);
    }
});


        $(document).ready(function () {

            //go render the form
            ReactDOM.render(
          <ChangePasswordForm />,
          document.getElementById('ChangePwForm'));

        })