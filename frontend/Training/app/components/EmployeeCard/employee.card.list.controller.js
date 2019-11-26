'use strict';

/**
 * @ngdoc function
 * @name Training.controller:EmployeeCardListController
 * @description
 * # EmployeeCardListController
 * Controller of the Training
 */
angular.module('Training').controller('EmployeeCardListController', function($scope, listController, EmployeeCardService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'EmployeeCard',
        baseService: EmployeeCardService,
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
