'use strict';

/**
 * @ngdoc function
 * @name Training.controller:MatrixController
 * @description
 * # MatrixController
 * Controller of the Training
 */
angular.module('Training').controller('MatrixController', function($scope, formController, MatrixService) {
    
    var ctrl = this;

    formController.call(this, {
        scope: $scope,
        entityName: 'Matrix',
        baseService: MatrixService,
        afterCreate: function(oEntity) {

        },
        afterLoad: function(oEntity) {

        }
    });
	
	ctrl.load();
});
