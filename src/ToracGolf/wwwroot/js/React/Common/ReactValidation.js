//removing var so other modules can pick this up on the global namespace

//gets called in the form using <ErrorList ErrorItems={this.state.Errors} />
//this is the error validation <ul>
ErrorList = React.createClass({displayName: "ErrorList",
    render: function () {

        var createItem = function (itemText, index) {
            return React.createElement("li", {key: index}, itemText);
        };

        //props.ErrorItems comes from the attribute when we specify <ErrorList ErrorItems
        return React.createElement("ul", null, this.props.ErrorItems.map(createItem));
    }
});