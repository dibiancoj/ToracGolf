System.register(['@angular/core'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1;
    var CustomFormatterService;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            }],
        execute: function() {
            CustomFormatterService = (function () {
                function CustomFormatterService() {
                    //build the month name array
                    this.MonthNameArray = ["January", "February", "March", "April", "May", "June",
                        "July", "August", "September", "October", "November", "December"
                    ];
                }
                //work around the number pipe issue on ipad. Hopefully they fix this
                CustomFormatterService.prototype.NumberPipe = function (numberToFormat) {
                    if (isNaN(numberToFormat)) {
                        return '';
                    }
                    //format with ,
                    return numberToFormat.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                };
                //work around the date pipe issue on ipad. Hopefully they fix this
                CustomFormatterService.prototype.DatePipe = function (valueToFormat, format) {
                    //current formats
                    //'M' = month name
                    //'D' = day of the month
                    //'DT' = date and time
                    //parse the date
                    var dateToFormat;
                    if (!(valueToFormat instanceof Date)) {
                        dateToFormat = new Date(valueToFormat);
                    }
                    if (format == 'M') {
                        return this.MonthNameArray[dateToFormat.getMonth()];
                    }
                    if (format == 'D') {
                        return dateToFormat.getDate().toString();
                    }
                    if (format = 'DT') {
                        //grab the hour
                        var hour = dateToFormat.getHours();
                        var pmOrAm;
                        if (hour > 12) {
                            pmOrAm = 'PM';
                            hour = hour - 12;
                        }
                        else {
                            pmOrAm = 'AM';
                        }
                        //minutes
                        var minutes = dateToFormat.getMinutes().toString();
                        if (minutes.length == 1) {
                            minutes = '0' + minutes;
                        }
                        return (dateToFormat.getMonth() + 1) + "/" + dateToFormat.getDate() + "/" + dateToFormat.getFullYear() + "  " +
                            hour + ":" + minutes + ' ' + pmOrAm;
                    }
                    //fall back to whatever was passed in
                    return valueToFormat;
                };
                CustomFormatterService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [])
                ], CustomFormatterService);
                return CustomFormatterService;
            }());
            exports_1("CustomFormatterService", CustomFormatterService);
        }
    }
});
//# sourceMappingURL=CustomFormatterService.js.map