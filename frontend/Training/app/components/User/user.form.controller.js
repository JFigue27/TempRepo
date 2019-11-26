'use strict';

/**
 * @ngdoc function
 * @name Training.controller:UserController
 * @description
 * # UserController
 * Controller of the Training
 */
angular.module('Training').controller('UserController', function($scope, formController, UserService) {
    var ctrl = new formController({
        scope: $scope,
        entityName: 'User',
        baseService: UserService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
