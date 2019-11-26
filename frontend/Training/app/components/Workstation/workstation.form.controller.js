'use strict';

/**
 * @ngdoc function
 * @name Training.controller:WorkstationController
 * @description
 * # WorkstationController
 * Controller of the Training
 */
angular.module('Training').controller('WorkstationController', function($scope, formController, WorkstationService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Workstation',
        baseService: WorkstationService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
