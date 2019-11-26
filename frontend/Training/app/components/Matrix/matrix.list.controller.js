'use strict';

/**
 * @ngdoc function
 * @name Training.controller:MatrixListController
 * @description
 * # MatrixListController
 * Controller of the Training
 */
angular.module('Training').controller('MatrixListController', function($scope, listController, MatrixService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'Matrix',
        baseService: MatrixService,
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
