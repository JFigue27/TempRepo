'use strict';

/**
 * @ngdoc function
 * @name Training.controller:FormatDc3Controller
 * @description
 * # FormatDc3Controller
 * Controller of the Training
 */
angular.module('Training').controller('FormatDc3Controller', function($scope, formController, FormatDc3Service) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'FormatDc3',
        baseService: FormatDc3Service,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
