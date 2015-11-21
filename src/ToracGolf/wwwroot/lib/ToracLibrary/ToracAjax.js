/// <reference path="../typescriptdefinitions/jquery.d.ts" />
var ToracTechnologies;
(function (ToracTechnologies) {
    //holds specific functionality to make ajax calls using jquery and promises
    var Ajax;
    (function (Ajax) {
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
            return $.ajax({
                type: "POST",
                url: Url,
                data: AjaxParameters,
                async: RunAsynchronously
            });
        }
        Ajax.RunAjaxCall = RunAjaxCall;
    })(Ajax = ToracTechnologies.Ajax || (ToracTechnologies.Ajax = {}));
})(ToracTechnologies || (ToracTechnologies = {}));
