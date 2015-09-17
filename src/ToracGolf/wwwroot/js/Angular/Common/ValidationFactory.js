﻿(function () {
    'use strict';

    //create the factory
    appToracGolf.service('ValidationFactory', function () {

        return {

            ShowValidationErrors: function ($scope, error) {

                $scope.validationErrors = [];
                if (error.data && angular.isObject(error.data)) {
                    for (var key in error.data) {
                        $scope.validationErrors.push(error.data[key][0]);
                    }
                } else {
                    $scope.validationErrors.push('Could not add the course.');
                };

            }
        }

    });

    //ValidationFactory.$inject = ['$http'];


})();