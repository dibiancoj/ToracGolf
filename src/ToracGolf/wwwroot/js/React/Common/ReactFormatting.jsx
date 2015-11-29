//removing var so other modules can pick this up on the global namespace

FormatNumber = React.createClass({

    numberWithCommas: function () {
        if (this.props.valueToSet == null)
        {
            return '';
        }

        return this.props.valueToSet.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    },

    render: function () {
        return <span>{this.numberWithCommas()}</span>
    }
});