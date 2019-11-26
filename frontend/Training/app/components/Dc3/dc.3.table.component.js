'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Dc3
 * @description
 * # Dc3
 */
angular.module('Training').directive('dc3Table', function() {
    return {
        templateUrl: 'components/Dc3/dc.3.table.html',
        restrict: 'E',
        controller: 'Dc3ListController'
    };
});
