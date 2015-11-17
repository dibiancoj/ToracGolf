var validationApplication = React.createClass({
    render: function () {

         return React.DOM.li(null, this.props.ErrorMessage);
    }
});

var validateFactory = React.createFactory(validationApplication);




$(document).ready(function () {
    $('#ChangePassword').click(function () {
        //go save something
        ReactDOM.render(validateFactory({ ErrorMessage: 'Invalid Jason' }), document.getElementById('ValidationError'));
    });
});