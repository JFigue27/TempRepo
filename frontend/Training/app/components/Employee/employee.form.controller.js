'use strict';

/**
 * @ngdoc function
 * @name Training.controller:EmployeeController
 * @description
 * # EmployeeController
 * Controller of the Training
 */
angular.module('Training').controller('EmployeeController', function($scope, formController, EmployeeService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Employee',
        baseService: EmployeeService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
