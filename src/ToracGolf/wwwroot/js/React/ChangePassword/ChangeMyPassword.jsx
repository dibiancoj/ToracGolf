﻿//gets called in the form using <ErrorList ErrorItems={this.state.Errors} />
//this is the error validation <ul>
var ErrorList = React.createClass({
    render: function () {

        var createItem = function (itemText, index) {
            return <li key={index }>{itemText}</li>;
        };

        //props.ErrorItems comes from the attribute when we specify <ErrorList ErrorItems
        return <ul>{this.props.ErrorItems.map(createItem)}</ul>;
    }
});

var PasswordInput = React.createClass({
    render: function () {
        return <input id={this.props.IdToUse} onChange={this.props.onChangeEvent} type="password" maxLength="100"></input>
    }
});

var ChangePasswordForm = React.createClass({
    getInitialState: function () {
        return { Errors: [] }
    },
    handleSubmit: function () {

        //go run validation with submit clicked
        var result = this.validation(true);

        //go make an ajax call if we have 0 errors
        if (result.length == 0) {

            alert('test');

            ToracTechnologies.Ajax.RunAjaxCall('ChangePw', this.getFormData().NewPw1, true)
            .done(function (result) {
                alert('done');
            })
            .fail(function (err) {
                alert('err');
            });

            //go make ajax call here
            this.setState({ Errors: ['controller error on mvc side after ajax call'] });
        }
    },
    handleChange: function (e) {

        //something has changed, go run the form validation
        this.validation(false);
    },
    getFormData: function () {
        return {
            CurrentPW: $('#CurrentPassword').val(),
            NewPw1: $('#NewPassword1').val(),
            NewPw2: $('#NewPassword2').val(),
        };
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
                                           <PasswordInput IdToUse="CurrentPassword" onChangeEvent={this.handleChange}></PasswordInput>
                                        </label>
                                    </section>
                                    <section>
                                        <label className="label">New Password</label>
                                        <label className="input">
                                            <i className="icon-append fa fa-edit"></i>
                                             <PasswordInput IdToUse="NewPassword1" onChangeEvent={this.handleChange}></PasswordInput>
                                        </label>
                                    </section>
                                    <section>
                                        <label className="label">Retype New Password</label>
                                        <label className="input">
                                            <i className="icon-append fa fa-edit"></i>
                                            <PasswordInput IdToUse="NewPassword2" onChangeEvent={this.handleChange}></PasswordInput>
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