'use strict';

/**
 * @ngdoc function
 * @name Training.controller:TrainingListController
 * @description
 * # TrainingListController
 * Controller of the Training
 */
angular.module('Training').controller('TrainingListController', function($scope, listController, TrainingService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Training',
        baseService: TrainingService,
        afterCreate: function(oInstance) {

        },
        afterLoad: function() {

        },
        onOpenItem: function(oItem) {

        },
        filters: {
            
        }
    });

    ctrl.load();

});
