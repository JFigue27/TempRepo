'use strict';

/**
 * @ngdoc function
 * @name Training.controller:EmployeeListController
 * @description
 * # EmployeeListController
 * Controller of the Training
 */
angular.module('Training').controller('EmployeeListController', function($scope, listController, EmployeeService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Employee',
        baseService: EmployeeService,
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
