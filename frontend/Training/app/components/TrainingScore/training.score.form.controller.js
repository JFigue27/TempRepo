'use strict';

/**
 * @ngdoc function
 * @name Training.controller:TrainingScoreController
 * @description
 * # TrainingScoreController
 * Controller of the Training
 */
angular.module('Training').controller('TrainingScoreController', function($scope, formController, TrainingScoreService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'TrainingScore',
        baseService: TrainingScoreService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
