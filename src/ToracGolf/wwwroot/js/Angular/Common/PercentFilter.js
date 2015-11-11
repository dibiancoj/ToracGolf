(function () {
    'use strict';

    //number should be in a decimal form ie. .17 = 17 %
    //{{FairwaysHitOutput(round.FairwaysHit,round.FairwaysHitAttempted) | percent: 0}}

    appToracGolf.filter('percent', ['$filter', function ($filter) {
        return function (input, decimals) {

            return $filter('number')(input * 100, decimals) + '%';
        };
    }]);

})();