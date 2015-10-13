(function () {
    'use strict';

    appToracGolf.directive('modalevent', function () {

        //sample html (important is the "modalevent" tag which says use this directive. Then the "showdialog" which is the boolean binding to show the dialog
        //    <div class="modal fade" modalevent showdialog="DeleteRoundModalShow">
        //    <div class="modal-dialog">
        //        <div class="modal-content">
        //            <div class="modal-header">
        //                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
        //                <h4 class="modal-title">Delete Round?</h4>
        //            </div>
        //            <div class="modal-body">Are You Sure You Want To Delete Your Round?</div>
        //            <div class="modal-footer">
        //                <button type="button" class="btn btn-default btn-danger" data-dismiss="modal">Delete</button>
        //            </div>
        //        </div>
        //    </div>
        //</div>

        return {
            restrict: 'EA',
            replace: false,
            //transclude: true,
            scope: {
                showdialog: '='
            },
            link: function postLink(scope, element, attrs) {

                //since this directive has it's own scope (it doesn't share the parent's). we need to add the watch to the parent which has our variable
                scope.$parent.$watch(attrs.showdialog, function (value) {
                    if (value) {
                        $(element).modal('show');
                    }
                    else {
                        $(element).modal('hide');
                    }
                });

                $(element).on('shown.bs.modal', function () {
                    scope.$apply(function () {
                        scope.showdialog = true;
                    });
                });

                $(element).on('hidden.bs.modal', function () {
                    scope.$apply(function () {
                        scope.showdialog = false;
                    });
                });
            }
        };
    });
})();