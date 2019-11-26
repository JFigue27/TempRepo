'use strict';

/**
 * @ngdoc function
 * @name Training.controller:ShiftController
 * @description
 * # ShiftController
 * Controller of the Training
 */
angular.module('Training').controller('ShiftController', function($scope, formController, ShiftService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Shift',
        baseService: ShiftService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
