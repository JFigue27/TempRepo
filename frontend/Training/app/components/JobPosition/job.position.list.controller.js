'use strict';

/**
 * @ngdoc function
 * @name Training.controller:JobPositionListController
 * @description
 * # JobPositionListController
 * Controller of the Training
 */
angular.module('Training').controller('JobPositionListController', function($scope, listController, JobPositionService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'JobPosition',
        baseService: JobPositionService,
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
