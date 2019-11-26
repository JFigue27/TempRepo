'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Level
 * @description
 * # Level
 */
angular.module('Training').directive('levelForm', function() {
    return {
        templateUrl: 'components/Level/level.form.html',
        restrict: 'E',
        controller: 'LevelController'
    };
});
