'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Level
 * @description
 * # Level
 */
angular.module('Training').directive('levelList', function() {
    return {
        templateUrl: 'components/Level/level.list.html',
        restrict: 'E',
        controller: 'LevelListController'
    };
});
