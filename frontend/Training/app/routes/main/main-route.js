'use strict';

/**
 * @ngdoc function
 * @name Training.controller:MainController
 * @description
 * # MainController
 * Controller of the Training
 */
angular.module('Training').controller('MainController', function($scope, $mdSidenav, $timeout) {

    $scope.toggleMainSideNav = buildToggler('main-sidenav');

    function buildToggler(componentId) {
        return function() {
            $mdSidenav(componentId).toggle();
        };
    }

    $timeout(function() {
        $('#mainSection').css('visibility', 'visible');
    });

});
