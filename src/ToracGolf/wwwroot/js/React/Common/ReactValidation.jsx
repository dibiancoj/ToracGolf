//removing var so other modules can pick this up on the global namespace

//gets called in the form using <ErrorList ErrorItems={this.state.Errors} />
//this is the error validation <ul>
ErrorList = React.createClass({
    render: function () {

        var createItem = function (itemText, index) {
            return <li key={index}>{itemText}</li>;
        };

        //props.ErrorItems comes from the attribute when we specify <ErrorList ErrorItems
        return <ul>{this.props.ErrorItems.map(createItem)}</ul>;
    }
});