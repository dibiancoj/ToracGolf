//removing var so other modules can pick this up on the global namespace

FormatNumber = React.createClass({displayName: "FormatNumber",

    numberWithCommas: function () {
        if (this.props.valueToSet == null)
        {
            return '';
        }

        return this.props.valueToSet.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    },

    render: function () {
        return React.createElement("span", null, this.numberWithCommas())
    }
});

ToDecimalPoints = React.createClass({displayName: "ToDecimalPoints",

    numberToDecimals: function (){
        return parseFloat(Math.round(this.props.valueToSet * 100) / 100).toFixed(this.props.decimals)
    },

    render: function () {
        return React.createElement("span", null, this.numberToDecimals())
    }

})