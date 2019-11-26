'use strict';

/**
 * @ngdoc function
 * @name Training.controller:JobPositionController
 * @description
 * # JobPositionController
 * Controller of the Training
 */
angular.module('Training').controller('JobPositionController', function($scope, formController, JobPositionService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'JobPosition',
        baseService: JobPositionService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
