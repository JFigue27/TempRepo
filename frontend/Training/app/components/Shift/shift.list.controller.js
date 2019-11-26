'use strict';

/**
 * @ngdoc function
 * @name Training.controller:ShiftListController
 * @description
 * # ShiftListController
 * Controller of the Training
 */
angular.module('Training').controller('ShiftListController', function($scope, listController, ShiftService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Shift',
        baseService: ShiftService,
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
