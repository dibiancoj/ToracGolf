(function () {
    'use strict';

    appToracGolf.factory('PagerFactory', function () {

        return {
            BuildPagerModel: function (totalNumberOfPages) {

                //go add the pager
                var pagerElements = [];

                //loop through the pages and add the specific page
                for (var i = 0; i < totalNumberOfPages; i++) {
                    pagerElements.push(i);
                }

                //return the array
                return pagerElements;
            }
        }
    });

})();