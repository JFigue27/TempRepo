'use strict';

/**
 * @ngdoc function
 * @name Training.controller:TrainingProfileController
 * @description
 * # TrainingProfileController
 * Controller of the Training
 */
angular.module('Training').controller('TrainingProfileController', function($scope, formController, TrainingProfileService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'TrainingProfile',
        baseService: TrainingProfileService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
