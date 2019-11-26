'use strict';

/**
 * @ngdoc function
 * @name Training.controller:WorkstationListController
 * @description
 * # WorkstationListController
 * Controller of the Training
 */
angular.module('Training').controller('WorkstationListController', function($scope, listController, WorkstationService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Workstation',
        baseService: WorkstationService,
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
