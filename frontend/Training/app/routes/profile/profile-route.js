'use strict';

/**
 * @ngdoc function
 * @name Training.controller:profileController
 * @description
 * # profileController
 * Controller of the Training
 */
angular.module('Training').controller('ProfileController', function($scope, formController, userService, $rootScope, $timeout) {

    var ctrl = new formController({
        scope: $scope,
        entityName: 'User',
        baseService: userService
    });

    $timeout(function() {
        ctrl.load($rootScope.currentUser);
    }, 500);

    $scope.sendTestEmail = function(oUser) {
        userService.sendTestEmail(oUser).then(function() {
            alertify.success('Email sent successfully.');
        });
    };
});
