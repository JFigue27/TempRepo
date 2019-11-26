'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Dc3
 * @description
 * # Dc3
 */
angular.module('Training').directive('dc3ListReport', function() {
    return {
        templateUrl: 'components/Dc3/dc.3.list.report.html',
        restrict: 'E',
        controller: function($scope, $routeParams) {
            $scope.ids = $routeParams.ids.split(',');
            $scope.print = function() {
                window.open('reports/DC-3-list.html');
            };
        }
    };
});
