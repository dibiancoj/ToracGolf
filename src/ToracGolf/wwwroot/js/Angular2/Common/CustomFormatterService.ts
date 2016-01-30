/* this is mainly to handle the issues with piping on the ipad. Hopefully they fix all this once they go live */
import { Injectable } from 'angular2/core';

@Injectable()
export class CustomFormatterService {

    constructor() {
        //build the month name array
        this.MonthNameArray = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
    }

    private MonthNameArray: string[];

    //work around the number pipe issue on ipad. Hopefully they fix this
    NumberPipe(numberToFormat: number): string {

        if (isNaN(numberToFormat)) {
            return '';
        }

        //format with ,
        return numberToFormat.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    //work around the date pipe issue on ipad. Hopefully they fix this
    DatePipe(valueToFormat: any, format: string): string {
        //current formats
        //'M' = month name
        //'D' = day of the month
        //'DT' = date and time


        //parse the date
        var dateToFormat: Date;

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
            var hour: number = dateToFormat.getHours();
            var pmOrAm: string;

            if (hour > 12) {
                pmOrAm = 'PM';
                hour = hour - 12;
            }
            else {
                pmOrAm = 'AM';
            }

            //minutes
            var minutes: string = dateToFormat.getMinutes().toString();

            if (minutes.length == 1) {
                minutes = '0' + minutes;
            }

            return (dateToFormat.getMonth() + 1) + "/" + dateToFormat.getDate() + "/" + dateToFormat.getFullYear() + "  " +
                hour + ":" + minutes + ' ' + pmOrAm;
        }

        //fall back to whatever was passed in
        return valueToFormat;
    }

}

