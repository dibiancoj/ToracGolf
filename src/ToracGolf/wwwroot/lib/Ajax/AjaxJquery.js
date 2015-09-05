/// <reference path="jquery.d.ts" />

//#endregion
var ToracTechnologies;
(function (ToracTechnologies) {
    //holds specific functionality to make ajax calls
    (function (Ajax) {
        //#region Enums
        //holds the enum for the speed at which the await spinner fades in and out.
        (function (FadeInOutSpeedEnum) {
            FadeInOutSpeedEnum[FadeInOutSpeedEnum["fast"] = 0] = "fast";
            FadeInOutSpeedEnum[FadeInOutSpeedEnum["slow"] = 1] = "slow";
        })(Ajax.FadeInOutSpeedEnum || (Ajax.FadeInOutSpeedEnum = {}));
        var FadeInOutSpeedEnum = Ajax.FadeInOutSpeedEnum;

        //#endregion
        //#region Methods
        //Make an ajax call.
        //Url: Url to call.
        //AjaxParameters: The parameters that will be passed into the server side method. This should be in object form (not JSON.stringify)
        //CallBackFunction: Method that is called after the ajax completes successfully
        //ErrorCallBackFunction: Method that is called after the ajax call has failed to completed because of an error
        //RunAsynchronously: Run with a non blocking call ie. Ajax. If false, the gui will be blocked until the has completed.
        //ShowWaitSpinner: Do you want to show the wait spinner? (this causes the ajax start / end event not be raised) - this was setup in the SetupAjaxWaitSpinner Method
        function RunAjaxCall(Url, AjaxParameters, CallBackFunction, ErrorCallBackFunction, RunAsynchronously, ShowWaitSpinner) {
            //if we don't have a url blowup
            if (Url == null || Url.length == 0) {
                alert('RunAjaxCall: Url Is Mandatory');
            }

            //this will handle blank strings or nulls. Null will get built saying "null" and the controller will ignore that param if its not what its expecting
            var json = JSON.stringify(AjaxParameters);

            $.ajax({
                type: "POST",
                url: Url,
                data: json,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: CallBackFunction,
                error: ErrorCallBackFunction,
                async: RunAsynchronously,
                global: ShowWaitSpinner
            });
        }
        Ajax.RunAjaxCall = RunAjaxCall;

        //For each ajax call this item will be displayed until the ajax is completed / or resulted in an error. This is mainly used in a please wait icon
        function SetupAjaxWaitSpinner(JQueryElements, FadeInOutSpeed) {
            //do we wan't to fade in fast or slow (always default to fast)
            var thisspeedToFadeInOrOut = FadeInOutSpeed;

            for (var i = 0; i < JQueryElements.length; i++) {
                //grab the item into a jquery element
                var thisItem = $(JQueryElements[i]);

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
        Ajax.SetupAjaxWaitSpinner = SetupAjaxWaitSpinner;

        //if you want ot use the abort ajax calls you need to call this first. Otherwise we have too much overhead, this way the user can opt in
        function ImplementAbortAjaxCalls(ImplementFunctionality) {
            //set the property
            ImplementAbortAjaxCallFunctionality = ImplementFunctionality;

            //if we want to use this functionality then attach the handlers
            if (ImplementFunctionality) {
                //attach the ajax send so we can add to the array
                $(document).ajaxSend(AjaxSender);

                //on each ajax complete...remove it from the array
                $(document).ajaxComplete(AjaxComplete);
            }
        }
        Ajax.ImplementAbortAjaxCalls = ImplementAbortAjaxCalls;

        //aborts all the ajax calls
        //**note this will throw an error in the errorfn (ajaxCall) callback make sure handle the abort like below
        //if (statustext.toLowerCase() == "abort")
        function AbortAllAjaxCalls() {
            //let's make sure the set the flag
            if (!ImplementAbortAjaxCallFunctionality) {
                alert("Please Call ImplementAbortAjaxCalls If You Want To Use The Abort AJAX Functionality");

                //exit out of the method
                return;
            }

            //going to leave it like i had in the plugin and just use $each.
            $.each(q, function (i, jqx) {
                //abort the ajax call
                jqx.abort();

                //remove it from the array which holds the pending ajax calls
                delete q[jqx._id];

                //chrome / firefox add's another item after abort...so we need to loop through another level. Don't need to recurse through it though
                $.each(q, function (ii, jqxx) {
                    //abort this XmlHttpRequest object
                    jqxx.abort();

                    //delete the XmlHttpRequest from the array
                    delete q[jqxx._id];
                });
            });
        }
        Ajax.AbortAllAjaxCalls = AbortAllAjaxCalls;

        //#region Abort Functionality Helper Methods
        //holds the boolean if we want the functionality to abort ajax calls
        var ImplementAbortAjaxCallFunctionality;

        //start int of the id
        var id = 0;

        //holds the XmlHttpRequest call object (list of objects builds up as we make calls. After each call it should be removed from the array)
        //this is a dictionary q[Id] = value is the XMLHttpRequest
        var q = {};

        //gets set on the document.ajaxSend
        function AjaxSender(e, jqx) {
            //increment the id
            jqx._id = ++id;

            //add the XmlHttpRequest Object)
            q[jqx._id] = jqx;
        }

        //gets set on the document.ajaxComplete
        function AjaxComplete(e, jqx) {
            //remove the XmlHttpRequest from the array
            delete q[jqx._id];
        }
    })(ToracTechnologies.Ajax || (ToracTechnologies.Ajax = {}));
    var Ajax = ToracTechnologies.Ajax;
})(ToracTechnologies || (ToracTechnologies = {}));
