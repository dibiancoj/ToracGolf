$(document).ready(function () {

    //add the await spinner
    ToracTechnologies.Ajax.SetupAjaxWaitSpinner($('#loadingDiv'), ToracTechnologies.Ajax.FadeInOutSpeedEnum.fast);
});

function RunAjax(Url, AjaxParameters) {
    return ToracTechnologies.Ajax.RunAjaxCall(Url, AjaxParameters, true, true);
}