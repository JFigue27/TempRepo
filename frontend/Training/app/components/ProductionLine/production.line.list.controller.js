'use strict';

/**
 * @ngdoc function
 * @name Training.controller:ProductionLineListController
 * @description
 * # ProductionLineListController
 * Controller of the Training
 */
angular.module('Training').controller('ProductionLineListController', function($scope, listController, ProductionLineService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'ProductionLine',
        baseService: ProductionLineService,
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
