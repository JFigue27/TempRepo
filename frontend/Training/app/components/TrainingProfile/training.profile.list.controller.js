'use strict';

/**
 * @ngdoc function
 * @name Training.controller:TrainingProfileListController
 * @description
 * # TrainingProfileListController
 * Controller of the Training
 */
angular.module('Training').controller('TrainingProfileListController', function($scope, listController, TrainingProfileService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'TrainingProfile',
        baseService: TrainingProfileService,
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
