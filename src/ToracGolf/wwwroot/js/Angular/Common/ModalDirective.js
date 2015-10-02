(function () {
    'use strict';


    appToracGolf.directive('modal', function () {

        //sample html
        //<modal title="Saved Succesfully" showdialog="ShowSavedSuccessfulModal" bodyhtml="Your Course Has Been Saved Successfully." closebuttontext="Ok" onmodalcloseevent="SaveACourseDialogOkEvent()">
        //</modal>

        return {
            template: '<div class="modal fade">' +
                       '<div class="modal-dialog">' +
                         '<div class="modal-content">' +
                           '<div class="modal-header">' +
                             '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                             '<h4 class="modal-title">{{title}}</h4>' +
                           '</div>' +
                           '<div class="modal-body">{{bodyhtml}}</div>' +
                           '<div class="modal-footer">' +
                                '<button type="button" class="btn btn-default btn-primary" data-dismiss="modal">{{closebuttontext}}</button>' +
                           '</div>' +
                         '</div>' +
                       '</div>' +
                     '</div>',
            restrict: 'EA',
            replace: true,
            transclude: true,
            //scope: true,
            scope: {
                onmodalcloseevent: '&',
                ShowSavedSuccessfulModal: '='
            },
            link: function postLink(scope, element, attrs) {
                scope.title = attrs.title;
                scope.bodyhtml = attrs.bodyhtml;
                scope.closebuttontext = attrs.closebuttontext;

                //since this directive has it's own scope (it doesn't share the parent's). we need to add the watch to the parent which has our variable
                scope.$parent.$watch(attrs.showdialog, function (value) {
                    if (value)
                        $(element).modal('show');
                    else
                        $(element).modal('hide');
                });

                $(element).on('shown.bs.modal', function () {
                    scope.$apply(function () {
                        scope.$parent[attrs.visible] = true;
                    });
                });

                $(element).on('hidden.bs.modal', function () {
                    scope.$apply(function () {
                        scope.$parent[attrs.visible] = false;

                        //call the close method
                        scope.onmodalcloseevent();
                    });
                });
            }
        };
    });
})();