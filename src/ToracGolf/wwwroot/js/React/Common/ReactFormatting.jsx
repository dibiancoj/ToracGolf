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

ToDecimalPoints = React.createClass({

    numberToDecimals: function (){
        return parseFloat(Math.round(this.props.valueToSet * 100) / 100).toFixed(this.props.decimals)
    },

    render: function () {
        return <span>{this.numberToDecimals()}</span>
    }

})

ToDate = React.createClass({
    
    toDateFormat: function(){
        return new Date(this.props.dateValue).toLocaleDateString();
    },

    render: function () {
        return <span>{this.toDateFormat()}</span>
    }
});