'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Dc3
 * @description
 * # Dc3
 */
angular.module('Training').directive('dc3List', function() {
    return {
        templateUrl: 'components/Dc3/dc.3.list.html',
        restrict: 'E',
        controller: 'Dc3ListController'
    };
});
