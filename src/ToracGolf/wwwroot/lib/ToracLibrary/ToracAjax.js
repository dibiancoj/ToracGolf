/// <reference path="../typescriptdefinitions/jquery.d.ts" />
var ToracTechnologies;
(function (ToracTechnologies) {
    //holds specific functionality to make ajax calls using jquery and promises
    var Ajax;
    (function (Ajax) {
        //Make an ajax call. 
        //Url: Url to call.
        //AjaxParameters: The parameters that will be passed into the server side method. This should be in object form (not JSON.stringify)
        //RunAsynchronously: Run with a non blocking call ie. Ajax. If false, the gui will be blocked until the has completed.
        //ShowWaitSpinner: Do you want to show the wait spinner? (this causes the ajax start / end event not be raised) - this was setup in the SetupAjaxWaitSpinner Method
        //returns a promise
        function RunAjaxCall(Url, AjaxParameters, RunAsynchronously) {
            //if we don't have a url blowup
            if (Url == null || Url.length == 0) {
                alert('RunAjaxCall: Url Is Mandatory');
            }
            //this will handle blank strings or nulls. Null will get built saying "null" and the controller will ignore that param if its not what its expecting
            var json = JSON.stringify(AjaxParameters);
            return $.ajax({
                type: "POST",
                url: Url,
                data: json,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: RunAsynchronously
            });
        }
        Ajax.RunAjaxCall = RunAjaxCall;
    })(Ajax = ToracTechnologies.Ajax || (ToracTechnologies.Ajax = {}));
})(ToracTechnologies || (ToracTechnologies = {}));
