'use strict';

/**
 * @ngdoc function
 * @name Training.controller:FormatDc3ListController
 * @description
 * # FormatDc3ListController
 * Controller of the Training
 */
angular.module('Training').controller('FormatDc3ListController', function($scope, listController, FormatDc3Service) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'FormatDc3',
        baseService: FormatDc3Service,
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
