'use strict';

/**
 * @ngdoc function
 * @name Training.controller:ProductionLineController
 * @description
 * # ProductionLineController
 * Controller of the Training
 */
angular.module('Training').controller('ProductionLineController', function($scope, formController, ProductionLineService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'ProductionLine',
        baseService: ProductionLineService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
