/// <reference path="../typescriptdefinitions/jquery.d.ts" />

module ToracTechnologies {

    //holds specific functionality to make ajax calls using jquery and promises
    export module Ajax {

        /*
        ToracTechnologies.Ajax.RunAjaxCall('ChangePassword', { oldPw: form.CurrentPW, newPw: form.NewPw1 }, true)
            .done(function (result) {
                debugger;
                alert('done');

                //go make ajax call here
                this.setState({ Errors: ['controller error on mvc side after ajax call'] });
            })
            .fail(function (err) {
                debugger;
                alert('err');

                //go make ajax call here
                this.setState({ Errors: ['Error Trying To Change New Password'] });
            });
      */

        //if you want to add a custom header on every ajax request using the following (ie: custom antiforgergy token)
        //$.ajaxSetup({
        //    headers: { 'x-my-custom-header': 'some value' }
        //});

        //to add a waiting div
        //  ToracTechnologies.Ajax.SetupAjaxWaitSpinner($('#loadingDiv'), ToracTechnologies.Ajax.FadeInOutSpeedEnum.fast);

        //#region Enums

        //holds the enum for the speed at which the await spinner fades in and out.
        export enum FadeInOutSpeedEnum {
            fast,
            slow
        }

        //#endregion

        //#region Methods

        //Make an ajax call. 
        //Url: Url to call.
        //AjaxParameters: The parameters that will be passed into the server side method. This should be in object form (not JSON.stringify)
        //RunAsynchronously: Run with a non blocking call ie. Ajax. If false, the gui will be blocked until the has completed.
        //ShowWaitSpinner: Do you want to show the wait spinner? (this causes the ajax start / end event not be raised) - this was setup in the SetupAjaxWaitSpinner Method
        //returns a promise
        export function RunAjaxCall(Url: string, AjaxParameters: any, RunAsynchronously: boolean, ShowWaitSpinner: boolean): JQueryXHR {

            //if we don't have a url blowup
            if (Url == null || Url.length == 0) {
                alert('RunAjaxCall: Url Is Mandatory');
            }

            return $.ajax({
                type: "POST",
                url: Url,
                data: AjaxParameters,
                async: RunAsynchronously,
                global: ShowWaitSpinner
            });
        }

        //setup the wait spinner
        export function SetupAjaxWaitSpinner(JQueryElements: JQuery, FadeInOutSpeed: FadeInOutSpeedEnum) {

            //do we wan't to fade in fast or slow (always default to fast)
            var thisspeedToFadeInOrOut: FadeInOutSpeedEnum = FadeInOutSpeed;

            //loop through each of the elements
            for (var i = 0; i < JQueryElements.length; i++) {

                //grab the item into a jquery element
                var thisItem: JQuery = $(JQueryElements[i]);

                //add the ajax start method
                $(document).ajaxStart(function () {
                    thisItem.fadeIn(thisspeedToFadeInOrOut);
                });

                //add the ajax stop method
                $(document).ajaxStop(function () {
                    thisItem.fadeOut(thisspeedToFadeInOrOut);
                });
            }
        }

        //#endregion

    }
}