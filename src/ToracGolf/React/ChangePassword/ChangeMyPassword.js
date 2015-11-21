//gets called in the form using <ErrorList ErrorItems={this.state.Errors} />
//this is the error validation <ul>
var ErrorList = React.createClass({displayName: "ErrorList",
    render: function() {

        var createItem = function(itemText, index) {
            return React.createElement("li", {key: index}, itemText);
        };

        //props.ErrorItems comes from the attribute when we specify <ErrorList ErrorItems
        return React.createElement("ul", null, this.props.ErrorItems.map(createItem));
    }
}); 

var ChangePasswordForm = React.createClass({displayName: "ChangePasswordForm",
    getInitialState: function() {
        return { Errors:[] }
    },
    handleSubmit: function() {

        //go run validation with submit clicked
        var result = this.validation(true);

        //go make an ajax call if we have 0 errors
        if (result.length == 0)
        {
            //go make ajax call here
            this.setState({Errors: ['controller error on mvc side after ajax call']});
        } 
    },
    handleChange: function(field, e) {

        //something has changed, go run the form validation
        this.validation(false);
    },
    getFormData: function() {
        return { 
            CurrentPW: $('#CurrentPassword').val(),
            NewPw1: $('#NewPassword1').val(),
            NewPw2: $('#NewPassword2').val(),
        };
    },
    validation: function(onFormSubmit){
        var errors = [];

        //grab the form data
        var formData = this.getFormData();

        if (onFormSubmit)
        {
            if (formData.CurrentPW.length == 0 || formData.NewPw1.length == 0 || formData.NewPw2.length == 0)
            {
                errors.push("Please Fill In All The Fields.");
            }
        }

        if ((formData.NewPw1.length > 0 || formData.NewPw2.length > 0) && (formData.NewPw1 != formData.NewPw2))
        {
            errors.push("New Password Fields Don't Match.");
        }
        
        this.setState({Errors: errors});

        return errors;
    },
    render: function() {
        return (            
                        React.createElement("fieldset", null, 
                                    React.createElement("section", null, 
                                        React.createElement("label", {className: "label"}, "Current Password"), 
                                        React.createElement("label", {className: "input"}, 
                                            React.createElement("i", {className: "icon-append fa fa-edit"}), 
                                            React.createElement("input", {id: "CurrentPassword", type: "password", onChange: this.handleChange.bind(this, 'CurrentPW'), maxLength: "100"})
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("label", {className: "label"}, "New Password"), 
                                        React.createElement("label", {className: "input"}, 
                                            React.createElement("i", {className: "icon-append fa fa-edit"}), 
                                            React.createElement("input", {id: "NewPassword1", type: "password", onChange: this.handleChange.bind(this, 'NewPw1'), maxLength: "100"})
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("label", {className: "label"}, "Retype New Password"), 
                                        React.createElement("label", {className: "input"}, 
                                            React.createElement("i", {className: "icon-append fa fa-edit"}), 
                                            React.createElement("input", {id: "NewPassword2", type: "password", onChange: this.handleChange.bind(this, 'NewPw2'), maxLength: "100"})
                                        )
                                    ), 
                                    React.createElement("section", null, 
                                        React.createElement("div", {className: "ValidationSummary form-group"}, 
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

});