(function () {
    'use strict';

    appToracGolf.factory('FileReader', ['$q', '$log', function ($q, $log) {

        //let's build up our object. this way we can create public and private variables

        var onLoad = function (reader, deferred, scope) {
            return function () {
                scope.$apply(function () {
                    deferred.resolve(reader.result);
                });
            };
        };

        var onError = function (reader, deferred, scope) {
            return function () {
                scope.$apply(function () {
                    deferred.reject(reader.result);
                });
            };
        };

        var onProgress = function (reader, scope) {
            return function (event) {
                scope.$broadcast("fileProgress",
                    {
                        total: event.total,
                        loaded: event.loaded
                    });
            };
        };

        var getReader = function (deferred, scope) {

            //create the file reader
            var reader = new FileReader();

            //add our events
            reader.onload = onLoad(reader, deferred, scope);
            reader.onerror = onError(reader, deferred, scope);
            reader.onprogress = onProgress(reader, scope);

            //now return the reader
            return reader;
        };

        return {

            //our only public method
            ReadAsDataURL: function (file, scope) {
                //grab our promise
                var deferred = $q.defer();

                //if we don't have a file, just return the promise
                if (file == null)
                {
                    //resolve a null value
                    deferred.resolve(null);

                    //return the promise
                    return deferred.promise;
                }

                //go grab the reader
                var reader = getReader(deferred, scope);

                //now go reader the data
                reader.readAsDataURL(file);

                //return the promise
                return deferred.promise;
            }
        };

    }]);

})();