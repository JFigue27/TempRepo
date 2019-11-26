'use strict';

/**
 * @ngdoc function
 * @name Training.controller:EmployeeCardController
 * @description
 * # EmployeeCardController
 * Controller of the Training
 */
angular.module('Training').controller('EmployeeCardController', function($scope, formController, EmployeeCardService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'EmployeeCard',
        baseService: EmployeeCardService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
